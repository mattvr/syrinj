using System;
using UnityEngine;
using System.Collections;
using Syrinj.Attributes;

namespace Syrinj
{
    public class InjectAttribute : UnityInjectorAttribute
    {
        public readonly string Tag;

        public InjectAttribute()
        {
            
        }

        public InjectAttribute(string tag)
        {
            this.Tag = tag;
        }
    }
}
