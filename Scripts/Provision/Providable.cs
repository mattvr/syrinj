using System;

namespace Syrinj.Provision
{
    public abstract class Providable
    {
        public Type Type;
        public string Tag;
        public object Instance;

        protected Providable(object instance, string tag)
        {
            this.Instance = instance;
            this.Tag = tag;
        }

        public abstract object Get();
    }
}
