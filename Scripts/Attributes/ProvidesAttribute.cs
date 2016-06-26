using System;

namespace Syrinj.Attributes
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
