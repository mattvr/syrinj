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

        public void Inject(object obj)
        {
            memberEvaluator.EvaluateMembers(obj);

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
                Debug.LogError(string.Format("[{0}] {1} {2}", injectable.Object, "Could not find resolver for", injectable));
                return;
            }
            var dependency = resolver.Resolve(injectable);
            TryInject(injectable, dependency);
        }

        private void TryInjectProvidable(Injectable injectable)
        {
            var provider = dependencyMap.GetProviderForDependency(injectable);
            if (provider == null)
            {
                Debug.LogError(string.Format("[{0}] {1} {2}", injectable.Object, "Could not find provider for", injectable));
                return;
            }

            var dependency = provider.Get();
            TryInject(injectable, dependency);
        }

        private void TryInject(Injectable injectable, object dependency)
        {
            if (!IsValidDependency(injectable, dependency))
            {
                Debug.LogError(string.Format("[{0}] {1} {2}", injectable.Object, "Could not find dependency for", injectable));
                return;
            }

            injectable.Inject(dependency);
        }

        private static bool IsValidDependency(Injectable injectable, object dependency)
        {
            return (dependency != null && !dependency.Equals(null) && injectable.Type.IsInstanceOfType(dependency));
        }
    }
}
