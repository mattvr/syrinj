using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Syrinj.Scripts.Graph;

namespace Syrinj.Graph
{
    public class DependencyMap : IDependencyGraph
    {
        public Dictionary<Type, object> dictionary;

        public DependencyMap()
        {
            dictionary = new Dictionary<Type, object>();
        }

        public override void RegisterProvider(Type binding, object provider)
        {
            dictionary.Add(binding, provider);
        }

        public override object Get(Type key)
        {
            return dictionary[key];
        }
    }
}
