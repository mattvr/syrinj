using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Syrinj.Scripts.Graph;
using Syrinj.Providers;
using UnityEditor;

namespace Syrinj.Graph
{
    // TODO: Make proper dependency graph
    public class DependencyMap : IDependencyGraph
    {
        private Dictionary<Binding, Provider> dictionary;

        public DependencyMap()
        {
            dictionary = new Dictionary<Binding, Provider>();
        }

        public override void RegisterProvider(Type type, Provider provider)
        {
            RegisterBindingProvider(new Binding(type), provider);
        }

        public override void RegisterProvider(Type type, string tag, Provider provider)
        {
            RegisterBindingProvider(new Binding(type, tag), provider);
        }

        private void RegisterBindingProvider(Binding binding, Provider provider)
        {
            if (!dictionary.ContainsKey(binding))
            {
                dictionary.Add(binding, provider);
            }
            else
            {
                //dictionary[type] = provider;
            }
        }

        public override object Get(Type key)
        {
            return GetFromBinding(new Binding(key));
        }

        public override object Get(Type key, string tag)
        {
            return GetFromBinding(new Binding(key, tag));
        }

        private object GetFromBinding(Binding binding)
        {
            if (dictionary.ContainsKey(binding))
            {
                return dictionary[binding].Get();
            }
            else
            {
                return null;
            }
        }
    }
}
