using System.Collections.Generic;
using System.Reflection;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Graph;
using Syrinj.Injection;
using Syrinj.Providers;
using UnityEngine;

namespace Syrinj.Reflection
{
    public class MemberEvaluator
    {
        private Dictionary<MemberInfo, Provider> providers;
        private Dictionary<MemberInfo, Injectable> dependentProviders;

        private Dictionary<MemberInfo, Injectable> dependencies;
        private Dictionary<MemberInfo, Injectable> injectedDependencies;

        private AttributeCache attributeCache;
        private DependencyMap dependencyMap;

        public MemberEvaluator(AttributeCache attributeCache, DependencyMap dependencyMap)
        {
            this.attributeCache = attributeCache;
            this.dependencyMap = dependencyMap;
        }

        public void EvaluateMembers(MonoBehaviour behaviour)
        {
            if (behaviour == null)
            {
                return;
            }

            var allMembers = attributeCache.GetMembersForType(behaviour.GetType());

            for (int i = 0; i < allMembers.Count; i++)
            {
                EvaluateMemberAttributes(allMembers[i], behaviour);
            }
        }

        private void EvaluateMemberAttributes(MemberInfo info, MonoBehaviour behaviour)
        {
            var injectable = GetInjectableAttribute(info, behaviour);
            var provider = GetProviderAttribute(info, behaviour);

            EvaluateAttributes(info, injectable, provider);
        }

        private Injectable GetInjectableAttribute(MemberInfo info, MonoBehaviour behaviour)
        {
            var attributes = attributeCache.GetInjectorAttributesForMember(info);

            if (attributes == null || attributes.Count == 0) return null;

            return InjectableFactory.Create(info, behaviour, attributes[0]);
        }

        private Provider GetProviderAttribute(MemberInfo info, MonoBehaviour behaviour)
        {
            var attributes = attributeCache.GetProviderAttributesForMember(info);

            if (attributes == null || attributes.Count == 0) return null;

            return ProviderFactory.Create(info, behaviour, ((ProvidesAttribute)attributes[0]).Tag);
        }

        private void EvaluateAttributes(MemberInfo info, Injectable injectable, Provider provider)
        {
            if (injectable != null && provider != null)
            {
                EvaluateInjectableProvider(injectable, provider);
            }
            else if (injectable != null)
            {
                EvaluateInjectable(injectable);
            }
            else if (provider != null)
            {
                EvaluateProvider(provider);
            }
        }

        private void EvaluateInjectableProvider(Injectable injectable, Provider provider)
        {
            dependencyMap.RegisterProvider(injectable.Type, injectable.Tag, provider);

            if (injectable.Attribute is UnityConvenienceAttribute)
            {
                dependencyMap.RegisterResolvableDependent(injectable);
            }
            else
            {
                throw new InjectionException(injectable.MonoBehaviour, "A provider cannot be annotated with [Inject] " + injectable.Type);
            }
        }

        private void EvaluateInjectable(Injectable injectable)
        {
            if (injectable.Attribute is UnityConvenienceAttribute)
            {
                dependencyMap.RegisterResolvableDependent(injectable);
            }
            else if (injectable.Attribute is InjectAttribute)
            {
                dependencyMap.RegisterProvidableDependent(injectable);
            }
        }

        private void EvaluateProvider(Provider provider)
        {
            dependencyMap.RegisterProvider(provider.Type, provider.Tag, provider);
        }
    }
}
