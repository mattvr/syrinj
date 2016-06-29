using System;
using System.Collections.Generic;
using System.Linq;
using Syrinj.Reflection;
using Syrinj.Exceptions;
using Syrinj.Graph;
using Syrinj.Injection;
using Syrinj.Resolvers;
using UnityEngine;

namespace Syrinj
{
    public class DependencyContainer
    {
        public static DependencyContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DependencyContainer();
                }
                return instance;
            }
            set { instance = value; }
        }

        private static DependencyContainer instance;

        private static readonly Dictionary<Type, IResolver> defaultResolvers = ResolverGroups.Default;

        private DependencyMap dependencyMap;
        private AttributeCache attributeCache;
        private MemberEvaluator memberEvaluator;

        public DependencyContainer()
        {
            Instance = this;
            attributeCache = new AttributeCache();
            Reset();
        }

        public void Reset()
        {
            dependencyMap = new DependencyMap();
            memberEvaluator = new MemberEvaluator(attributeCache, dependencyMap);

            RegisterDefaultDependencyResolvers();
            GameObjectInjector.ResetVisited();
        }

        private void RegisterDefaultDependencyResolvers()
        {
            defaultResolvers.ToList().ForEach(kvp => dependencyMap.RegisterResolver(kvp.Key, kvp.Value));
        }

        public void Inject(IList<MonoBehaviour> monoBehaviours)
        {
            for (int i = 0; i < monoBehaviours.Count; i++)
            {
                memberEvaluator.EvaluateMembers(monoBehaviours[i]);
            }

            TryInjectAll();
        }

        public void Inject(MonoBehaviour monoBehaviour)
        {
            memberEvaluator.EvaluateMembers(monoBehaviour);

            TryInjectAll();
        }

        private void TryInjectAll()
        {
            var resolvables = dependencyMap.UnloadResolvableDependents();
            for (int i = 0; i < resolvables.Count; i++)
            {
                TryInjectResolvable(resolvables[i]);
            }

            var providables = dependencyMap.UnloadProvidableDependents();
            for (int i = 0; i < providables.Count; i++)
            {
                TryInjectProvidable(providables[i]);
            }
        }

        private void TryInjectResolvable(Injectable injectable)
        {
            var resolver = dependencyMap.GetResolverForDependency(injectable);
            if (resolver == null)
            {
                throw new InjectionException(injectable.MonoBehaviour, "Could not find resolver for " + injectable);
            }
            var dependency = resolver.Resolve(injectable);
            TryInject(injectable, dependency);
        }

        private void TryInjectProvidable(Injectable injectable)
        {
            var provider = dependencyMap.GetProviderForDependency(injectable);
            if (provider == null)
            {
                throw new InjectionException(injectable.MonoBehaviour, "Could not find provider for " + injectable);
            }

            var dependency = provider.Get();
            TryInject(injectable, dependency);
        }

        private void TryInject(Injectable injectable, object dependency)
        {
            if (!IsValidDependency(injectable, dependency))
            {
                throw new InjectionException(injectable.MonoBehaviour, "Could not get dependency for " + injectable);
            }

            injectable.Inject(dependency);
        }

        private static bool IsValidDependency(Injectable injectable, object dependency)
        {
            return (dependency != null && !dependency.Equals(null) && injectable.Type.IsInstanceOfType(dependency));
        }
    }
}
