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
        public Attribute Attribute;
        public MonoBehaviour MonoBehaviour;

        protected Injectable(Type type, Attribute attribute, MonoBehaviour monoBehaviour)
        {
            Type = type;
            Attribute = attribute;
            MonoBehaviour = monoBehaviour;
        }

        public abstract void Inject(object dependency);

        public abstract override string ToString();
    }
}
