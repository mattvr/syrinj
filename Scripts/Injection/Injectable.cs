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

        protected MonoBehaviour _monoBehaviour;

        protected Injectable(Type type, Attribute attribute, MonoBehaviour monoBehaviour)
        {
            Type = type;
            Attribute = attribute;
            _monoBehaviour = monoBehaviour;
        }

        public abstract void Inject(object dependency);
    }
}
