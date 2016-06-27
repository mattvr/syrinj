using System;
using System.Collections.Generic;
using Syrinj.Injection;
using Syrinj.Providers;
using Syrinj.Resolvers;

namespace Syrinj.Graph
{
    public class DependencyMap
    {
        protected class InjectionKey
        {
            public readonly Type Type;
            public readonly string Tag;

            public InjectionKey(Type type)
            {
                this.Type = type;
            }

            public InjectionKey(Type type, string tag)
            {
                this.Type = type;
                this.Tag = tag;
            }

            public override bool Equals(object obj)
            {
                var binding = obj as InjectionKey;
                if (binding == null)
                {
                    return false;
                }
                return binding.Type == this.Type && binding.Tag == this.Tag;
            }

            public override int GetHashCode()
            {
                return 7 * Type.GetHashCode() + (Tag != null ? 23 * Tag.GetHashCode() : 29);
            }
        }

        private Dictionary<Type, IResolver> resolverDependencies;
        private Dictionary<InjectionKey, Provider> providerDependencies;
        private List<Injectable> resolvableDependents;
        private List<Injectable> providableDependents;

        public DependencyMap()
        {
            resolverDependencies = new Dictionary<Type, IResolver>();
            providerDependencies = new Dictionary<InjectionKey, Provider>();
            resolvableDependents = new List<Injectable>();
            providableDependents = new List<Injectable>();
        }

        public List<Injectable> UnloadResolvableDependents()
        {
            var temp = resolvableDependents;
            resolvableDependents = new List<Injectable>();
            return temp;
        }

        public List<Injectable> UnloadProvidableDependents()
        {
            var temp = providableDependents;
            providableDependents = new List<Injectable>();
            return temp;
        }

        public void RegisterProvidableDependent(Injectable injectable)
        {
            providableDependents.Add(injectable);
        }

        public void RegisterResolvableDependent(Injectable injectable)
        {
            resolvableDependents.Add(injectable);
        }

        public IResolver GetResolverForDependency(Injectable injectable)
        {
            var type = injectable.Attribute.GetType();
            if (resolverDependencies.ContainsKey(type))
            {
                return resolverDependencies[type];
            }
            return null;
        }

        public Provider GetProviderForDependency(Injectable injectable)
        {
            var key = new InjectionKey(injectable.Type, injectable.Tag);
            if (providerDependencies.ContainsKey(key))
            {
                return providerDependencies[key];
            }
            return null;
        }

        public void RegisterResolver(Type type, IResolver resolver)
        {
            RegisterBindingResolver(type, resolver);
        }

        public void RegisterProvider(Type type, string tag, Provider provider)
        {
            RegisterBindingProvider(new InjectionKey(type, tag), provider);
        }

        private void RegisterBindingResolver(Type type, IResolver resolver)
        {
            if (!resolverDependencies.ContainsKey(type))
            {
                resolverDependencies.Add(type, resolver);
            }
        }

        private void RegisterBindingProvider(InjectionKey injectionKey, Provider provider)
        {
            if (!providerDependencies.ContainsKey(injectionKey))
            {
                providerDependencies.Add(injectionKey, provider);
            }
        }
    }
}
