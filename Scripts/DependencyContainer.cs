using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Syrinj.Attributes;
using Syrinj.Caching;
using Syrinj.Exceptions;
using Syrinj.Graph;
using Syrinj.Injection;
using Syrinj.Providers;
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
            dependencyMap = new DependencyMap();
            attributeCache = new AttributeCache();
            memberEvaluator = new MemberEvaluator(attributeCache, dependencyMap);

            RegisterDefaultDependencyResolvers();
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
            resolvables.ForEach(TryInjectResolvable);

            var providables = dependencyMap.UnloadProvidableDependents();
            providables.ForEach(TryInjectProvidable);
        }

        private void TryInjectResolvable(Injectable injectable)
        {
            var dependency = dependencyMap.ResolveDependency(injectable);
            TryInject(injectable, dependency);
        }

        private void TryInjectProvidable(Injectable injectable)
        {
            var dependency = dependencyMap.ProvideDependency(injectable);
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
