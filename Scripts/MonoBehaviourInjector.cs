using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Syrinj.Scripts.Graph;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Providers;
using Syrinj.Graph;
using UnityEngine;

namespace Syrinj.Injection
{
    public class MonoBehaviourInjector
    {
        private static IDependencyGraph graph = new DependencyMap();

        private static readonly Dictionary<Type, IProvider> defaultProviders = ProviderMaps.Default;

        private static readonly Dictionary<Type, MemberInfo[]> cachedMembers = new Dictionary<Type, MemberInfo[]>();
        private static readonly Dictionary<MemberInfo, object[]> cachedAttributes = new Dictionary<MemberInfo, object[]>();

        private readonly MonoBehaviour _monoBehaviour;
        private readonly Type _derivedType;

        private readonly List<Injectable> _injectables;

        public MonoBehaviourInjector(MonoBehaviour monoBehaviour)
        {
            _monoBehaviour = monoBehaviour;
            _derivedType = monoBehaviour.GetType();
            _injectables = new List<Injectable>();
        }

        public void Inject()
        {
            LoadMembers();

            EvaluateAllInjectables();
        }

        private void LoadMembers()
        {
            var allMembers = GetCachedMembers(_derivedType);

            for (int i = 0; i < allMembers.Length; i++)
            {
                LoadMemberAttributes(allMembers[i]);
            }
        }

        private void LoadMemberAttributes(MemberInfo info)
        {
            if (!IsValidType(info))
            {
                return;
            }

            var attributes = GetCachedAttributes(info);
            for (int i = 0; i < attributes.Length; i++)
            {
                var injectable = InjectableFactory.Create(info, (UnityHelperAttribute) attributes[i], _monoBehaviour);
                if (injectable != null)
                {
                    _injectables.Add(injectable);
                }
            }
        }

        private void EvaluateAllInjectables()
        {
            for (int i = 0; i < _injectables.Count; i++)
            {
                Evaluate(_injectables[i]);
            }
        }

        private void Evaluate(Injectable injectable)
        {
            var provider = defaultProviders[injectable.Attribute.GetType()];
            if (provider != null)
            {
                var dependency = provider.Provide(injectable);
                TryInject(injectable, dependency);
            }
        }

        private void TryInject(Injectable injectable, object dependency)
        {
            if (dependency == null || !injectable.Type.IsInstanceOfType(dependency))
            {
                throw new InjectionException(_monoBehaviour, "Could not find dependency for " + injectable.Type);
            }
            else
            {
                injectable.Inject(dependency);
            }
        }

        private static MemberInfo[] GetCachedMembers(Type type)
        {
            if (!cachedMembers.ContainsKey(type))
            {
                cachedMembers.Add(type, type.FindMembers(ValidMemberTypes(), ValidBindingFlags(), Filter, null));
            }
            return cachedMembers[type];
        }

        private static bool IsValidType(MemberInfo info)
        {
            return (info.MemberType & ValidMemberTypes()) != 0;
        }

        private static MemberTypes ValidMemberTypes()
        {
            return MemberTypes.Field | MemberTypes.Property;
        }

        private static BindingFlags ValidBindingFlags()
        {
            return BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        }

        private static object[] GetCachedAttributes(MemberInfo info)
        {
            if (!cachedAttributes.ContainsKey(info))
            {
                cachedAttributes.Add(info, info.GetCustomAttributes(typeof(UnityHelperAttribute), false));
            }
            return cachedAttributes[info];
        }

        private static bool Filter(MemberInfo m, object filtercriteria)
        {
            return m.IsDefined(typeof(UnityHelperAttribute), false);
        }
    }
}