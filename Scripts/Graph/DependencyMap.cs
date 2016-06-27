using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private Dictionary<InjectionKey, IResolver> resolverDependencies;
        private Dictionary<InjectionKey, Provider> providerDependencies;
        private List<Injectable> resolvableDependents;
        private List<Injectable> providableDependents;

        public DependencyMap()
        {
            resolverDependencies = new Dictionary<InjectionKey, IResolver>();
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

        public void RegisterInjectionDependent(Injectable injectable)
        {
            providableDependents.Add(injectable);
        }

        public void RegisterConvenienceDependent(Injectable injectable)
        {
            resolvableDependents.Add(injectable);
        }

        public object ResolveDependency(Injectable injectable)
        {
            var key = new InjectionKey(injectable.Type);
            if (resolverDependencies.ContainsKey(key))
            {
                return resolverDependencies[key].Resolve(injectable);
            }
            return null;
        }

        public object ProvideDependency(Injectable injectable)
        {
            var key = new InjectionKey(injectable.Type, injectable.Tag);
            if (providerDependencies.ContainsKey(key))
            {
                return providerDependencies[key].Get();
            }
            return null;
        }

        public void RegisterResolver(Type type, IResolver resolver)
        {
            RegisterBindingResolver(new InjectionKey(type), resolver);
        }

        public void RegisterProvider(Type type, string tag, Provider provider)
        {
            RegisterBindingProvider(new InjectionKey(type, tag), provider);
        }

        private void RegisterBindingResolver(InjectionKey injectionKey, IResolver resolver)
        {
            if (!providerDependencies.ContainsKey(injectionKey))
            {
                resolverDependencies.Add(injectionKey, resolver);
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
