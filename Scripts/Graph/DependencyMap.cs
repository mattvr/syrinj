 using System;
using System.Collections.Generic;
using System.Diagnostics;
using Syrinj.Injection;
using Syrinj.Provision;
using Syrinj.Resolvers;

using Debug = UnityEngine.Debug;
namespace Syrinj.Graph
{
    public class DependencyMap
    {
        protected class InjectionKey
        {
            public readonly Type Type;
            public readonly string Tag;

            public InjectionKey(Type type, string tag)
            {
                this.Type = type;
                this.Tag = tag;
            }

            public override bool Equals(object obj)
            {
                var binding = obj as InjectionKey;
                if (binding != null)
                {
                    return this.Tag == binding.Tag && this.Type == binding.Type;
                }
                return false;
            }

            public override int GetHashCode()
            {
                return 7 * Type.GetHashCode() + (Tag != null ? 23 * Tag.GetHashCode() : 29);
            }
        }

        private Dictionary<Type, IResolver> resolvers;
        private Dictionary<InjectionKey, IProvider> providers;
        private List<Injectable> resolvableDependents;
        private List<Injectable> providableDependents;

        public DependencyMap()
        {
            resolvers = new Dictionary<Type, IResolver>();
            providers = new Dictionary<InjectionKey, IProvider>();
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
            TryRegisterProvider(injectable);
        }

        private void TryRegisterProvider(Injectable injectable)
        {
            if (injectable.Type != null && injectable.Type.IsSubclassOf(typeof(Provider)) && injectable.Tag == null)
            {
                var key = new InjectionKey(injectable.Type, null);
                var provider = ProviderFactory.CreateGeneric(injectable.Type);
                providers.Add(key, provider);
            }
        }

        public void RegisterResolvableDependent(Injectable injectable)
        {
            resolvableDependents.Add(injectable);
        }

        public IResolver GetResolverForDependency(Injectable injectable)
        {
            var type = injectable.Attribute.GetType();
            if (resolvers.ContainsKey(type))
            {
                return resolvers[type];
            }
            return null;
        }

        public IProvider GetProviderForDependency(Injectable injectable)
        {
            var key = new InjectionKey(injectable.Type, injectable.Tag);
            if (providers.ContainsKey(key))
            {
                return providers[key];
            }
            return null;
        }

        public void RegisterResolver(Type type, IResolver resolver)
        {
            RegisterBindingResolver(type, resolver);
        }

        public void RegisterProvider(Type type, string tag, IProvider provider)
        {
            RegisterBindingProvider(new InjectionKey(type, tag), provider);
        }

        private void RegisterBindingResolver(Type type, IResolver resolver)
        {
            if (!resolvers.ContainsKey(type))
            {
                resolvers.Add(type, resolver);
            }
        }

        private void RegisterBindingProvider(InjectionKey injectionKey, IProvider provider)
        {
            if (!providers.ContainsKey(injectionKey))
            {
                providers.Add(injectionKey, provider);
            }
        }
    }
}
