using System;
using Syrinj.Attributes;

namespace Syrinj
{
    public class ProvidesAttribute : UnityProviderAttribute
    {
        public readonly string Tag;

        public ProvidesAttribute()
        {

        }

        public ProvidesAttribute(string tag)
        {
            this.Tag = tag;
        }
    }
}
