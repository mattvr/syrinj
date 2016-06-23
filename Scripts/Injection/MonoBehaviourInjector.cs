using System;
using System.Collections.Generic;
using System.Reflection;
using Syrinj.Attributes;
using Syrinj.Resolvers;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Syrinj.Injection
{
    public class MonoBehaviourInjector
    {
        private readonly MonoBehaviour _monoBehaviour;
        private readonly Type _derivedType;

        private List<Injectable> _injectables;

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
            var allMembers = _derivedType.GetMembers(GetBindingFlags());

            for (int i = 0; i < allMembers.Length; i++)
            {
                LoadInjectableMemberAttributes(allMembers[i]);
            }
        }

        private static BindingFlags GetBindingFlags()
        {
            return BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        }

        private void LoadInjectableMemberAttributes(MemberInfo info)
        {
            var attributes = info.GetCustomAttributes(typeof (UnityHelperAttribute), false);
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
                injectable.Inject(dependency);
            }
        }
    }
}