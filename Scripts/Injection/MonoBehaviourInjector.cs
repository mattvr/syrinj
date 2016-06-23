using System;
using System.Collections.Generic;
using System.Reflection;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Resolvers;
using UnityEngine;

namespace Syrinj.Injection
{
    public class MonoBehaviourInjector
    {
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
            LoadInjectableMembers();

            for (int i = 0; i < _injectables.Count; i++)
            {
                InternalInject(_injectables[i]);
            }
        }

        private void LoadInjectableMembers()
        {
            var allMembers = GetCachedMembers(_derivedType);

            for (int i = 0; i < allMembers.Length; i++)
            {
                LoadInjectableMemberAttributes(allMembers[i]);
            }
        }

        private void LoadInjectableMemberAttributes(MemberInfo info)
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

        private void InternalInject(Injectable injectable)
        {
            var resolver = ResolverLookup.Get(injectable.Attribute.GetType());
            if (resolver != null)
            {
                var dependency = resolver.Resolve(_monoBehaviour, injectable);
                TryDoInject(injectable, dependency);
            }
        }

        private void TryDoInject(Injectable injectable, object dependency)
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
                cachedMembers.Add(type, type.GetMembers(GetBindingFlags()));
            }
            return cachedMembers[type];
        }

        private static bool IsValidType(MemberInfo info)
        {
            return (info.MemberType & GetMemberTypes()) != 0;
        }

        private static object[] GetCachedAttributes(MemberInfo info)
        {
            if (!cachedAttributes.ContainsKey(info))
            {
                cachedAttributes.Add(info, info.GetCustomAttributes(typeof(UnityHelperAttribute), false));
            }
            return cachedAttributes[info];
        }

        private static BindingFlags GetBindingFlags()
        {
            return BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        }

        private static MemberTypes GetMemberTypes()
        {
            return MemberTypes.Field | MemberTypes.Property;
        }
    }
}