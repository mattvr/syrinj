using System.Collections.Generic;
using System.Reflection;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Graph;
using Syrinj.Injection;
using Syrinj.Provision;
using UnityEngine;

namespace Syrinj.Reflection
{
    public class MemberEvaluator
    {
        private Dictionary<MemberInfo, IProvider> providers;
        private Dictionary<MemberInfo, Injectable> dependentProviders;

        private Dictionary<MemberInfo, Injectable> dependencies;

        private AttributeCache attributeCache;
        private DependencyMap dependencyMap;

        public MemberEvaluator(AttributeCache attributeCache, DependencyMap dependencyMap)
        {
            this.attributeCache = attributeCache;
            this.dependencyMap = dependencyMap;
        }

        public void EvaluateMembers(object obj)
        {
            if (obj == null)
            {
                return;
            }

            var allMembers = attributeCache.GetMembersForType(obj.GetType());

            for (int i = 0; i < allMembers.Count; i++)
            {
                EvaluateMemberAttributes(allMembers[i], obj);
            }
        }

        private void EvaluateMemberAttributes(MemberInfo info, object obj)
        {
            var injectable = GetInjectableAttribute(info, obj);
            var provider = GetProviderAttribute(info, obj);

            EvaluateAttributes(info, injectable, provider);
        }

        private Injectable GetInjectableAttribute(MemberInfo info, object obj)
        {
            var attributes = attributeCache.GetInjectorAttributesForMember(info);

            if (attributes == null || attributes.Count == 0) return null;

            return InjectableFactory.Create(info, obj, attributes[0]);
        }

        private IProvider GetProviderAttribute(MemberInfo info, object obj)
        {
            var attributes = attributeCache.GetProviderAttributesForMember(info);

            if (attributes == null || attributes.Count == 0) return null;

            return ProviderFactory.Create(info, obj, attributes);
        }

        private void EvaluateAttributes(MemberInfo info, Injectable injectable, IProvider provider)
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

        private void EvaluateInjectableProvider(Injectable injectable, IProvider provider)
        {
            dependencyMap.RegisterProvider(injectable.Type, injectable.Tag, provider);

            if (injectable.Attribute is UnityConvenienceAttribute)
            {
                dependencyMap.RegisterResolvableDependent(injectable);
            }
            else
            {
                throw new InjectionException(injectable.Object, "A member cannot be annotated both with [Inject] and [Provides]" + injectable.Type);
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

        private void EvaluateProvider(IProvider provider)
        {
            dependencyMap.RegisterProvider(provider.Type, provider.Tag, provider);
        }
    }
}
