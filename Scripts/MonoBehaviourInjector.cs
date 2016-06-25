using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Syrinj.Scripts.Graph;
using Syrinj.Attributes;
using Syrinj.Caching;
using Syrinj.Exceptions;
using Syrinj.Resolvers;
using Syrinj.Graph;
using Syrinj.Providers;
using UnityEngine;

namespace Syrinj.Injection
{
    public class MonoBehaviourInjector
    {
        private static IDependencyGraph graph = new DependencyMap();

        private static readonly Dictionary<Type, IResolver> defaultResolvers = ResolverGroups.Default;

        private static readonly AttributeCache attributeCache = new AttributeCache();

        private static readonly HashSet<object> visited = new HashSet<object>(); // TODO: move to DependencyGraph

        private readonly MonoBehaviour _monoBehaviour;
        private readonly bool _injectChildren;
        private readonly List<Injectable> _injectables;

        public MonoBehaviourInjector(MonoBehaviour monoBehaviour) : this(monoBehaviour, false)
        {
        }

        public MonoBehaviourInjector(MonoBehaviour monoBehaviour, bool injectChildren) 
        {
            _monoBehaviour = monoBehaviour;
            _injectables = new List<Injectable>();
            _injectChildren = injectChildren;
        }

        public void Inject()
        {
            if (visited.Contains(this))
            {
                return;
            }

            ProcessAllComponents();

            InjectAllComponents();
        }

        private void ProcessAllComponents()
        {
            var behaviours = GetAttachedComponents();

            for (int i = 0; i < behaviours.Length; i++)
            {
                var behaviour = behaviours[i];
                if (!visited.Contains(behaviour))
                {
                    LoadMembers(behaviour);
                    visited.Add(behaviour);
                }
            }
        }

        private ExtendedMonoBehaviour[] GetAttachedComponents()
        {
            if (_injectChildren)
            {
                return _monoBehaviour.GetComponentsInChildren<ExtendedMonoBehaviour>();
            }
            else
            {
                return _monoBehaviour.GetComponents<ExtendedMonoBehaviour>();
            }
        }

        private void LoadMembers(MonoBehaviour behaviour)
        {
            var allMembers = attributeCache.GetMembersForType(behaviour.GetType());

            for (int i = 0; i < allMembers.Count; i++)
            {
                LoadMemberAttributes(allMembers[i], behaviour);
            }
        }

        private void LoadMemberAttributes(MemberInfo info, MonoBehaviour behaviour)
        {
            LoadInjectorAttributes(info, behaviour);
            LoadProviderAttributes(info, behaviour);
        }

        private void LoadInjectorAttributes(MemberInfo info, MonoBehaviour behaviour)
        {
            var attributes = attributeCache.GetInjectorAttributesForMember(info);
            if (attributes == null) return;

            for (int i = 0; i < attributes.Count; i++)
            {
                var injectable = InjectableFactory.Create(info, attributes[i], behaviour);
                if (injectable != null)
                {
                    _injectables.Add(injectable);
                }
            }
        }

        private void LoadProviderAttributes(MemberInfo info, MonoBehaviour behaviour)
        {
            var attributes = attributeCache.GetProviderAttributesForMember(info);
            if (attributes == null) return;

            for (int i = 0; i < attributes.Count; i++)
            {
                var provider = ProviderFactory.Create(info, behaviour);
                if (provider != null)
                {
                    graph.RegisterProvider(provider.Type, provider);
                }
            }
        }

        private void InjectAllComponents()
        {
            for (int i = 0; i < _injectables.Count; i++)
            {
                ResolveDependencyAndInject(_injectables[i]);
            }
        }

        private void ResolveDependencyAndInject(Injectable injectable)
        {
            var resolver = GetResolverOrNull(injectable);

            if (resolver != null)
            {
                var dependencyFromProvider = resolver.Resolve(injectable);
                TryInject(injectable, dependencyFromProvider);
            }
            else
            {
                var dependencyFromGraph = graph.Get(injectable.Type);
                TryInject(injectable, dependencyFromGraph);
            }
        }

        private IResolver GetResolverOrNull(Injectable injectable)
        {
            var type = injectable.Attribute.GetType();
            if (defaultResolvers.ContainsKey(type))
            {
                return defaultResolvers[type];
            }
            else
            {
                return null;
            }
        }

        private void TryInject(Injectable injectable, object dependency)
        {
            if (!ValidDependency(injectable, dependency))
            {
                throw new InjectionException(_monoBehaviour, "Could not find dependency for " + injectable);
            }

            injectable.Inject(dependency);
        }

        private bool ValidDependency(Injectable injectable, object dependency)
        {
            return (dependency != null && !dependency.Equals(null) && injectable.Type.IsInstanceOfType(dependency));
        }
    }
}