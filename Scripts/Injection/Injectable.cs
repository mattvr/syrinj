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
        public object Object;
        public string Tag;
        public Attribute Attribute;

        protected Injectable(Type type, object obj, string tag, Attribute attribute)
        {
            Type = type;
            Tag = tag;
            Attribute = attribute;
            Object = obj;
        }

        public abstract void Inject(object dependency);

        public abstract override string ToString();
    }
}
