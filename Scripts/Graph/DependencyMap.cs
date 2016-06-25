using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Syrinj.Scripts.Graph;
using Syrinj.Providers;

namespace Syrinj.Graph
{
    public class DependencyMap : IDependencyGraph
    {
        public Dictionary<Type, Provider> dictionary;

        public DependencyMap()
        {
            dictionary = new Dictionary<Type, Provider>();
        }

        public override void RegisterProvider(Type binding, Provider provider)
        {
            dictionary.Add(binding, provider);
        }

        public override object Get(Type key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key].Get();
            }
            else
            {
                return null;
            }
        }
    }
}
