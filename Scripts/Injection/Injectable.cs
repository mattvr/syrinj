using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syrinj.Resolvers;
using UnityEngine;

namespace Syrinj.Injection
{
    public abstract class Injectable
    {
        public Type Type;
        public MonoBehaviour MonoBehaviour;
        public string Tag;
        public Attribute Attribute;

        protected Injectable(Type type, MonoBehaviour monoBehaviour, string tag, Attribute attribute)
        {
            Type = type;
            Tag = tag;
            Attribute = attribute;
            MonoBehaviour = monoBehaviour;
        }

        public abstract void Inject(object dependency);

        public abstract override string ToString();
    }
}
