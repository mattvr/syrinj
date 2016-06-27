using System;

namespace Syrinj.Providers
{
    public abstract class Provider
    {
        public Type Type;
        public string Tag;
        public object Instance;

        protected Provider(object instance, string tag)
        {
            this.Instance = instance;
            this.Tag = tag;
        }

        public abstract object Get();
    }
}
