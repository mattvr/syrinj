using System;

namespace Syrinj.Providers
{
    public abstract class Provider
    {
        public Type Type;
        public object Instance;

        protected Provider(object instance)
        {
            this.Instance = instance;
        }

        public abstract object Get();
    }
}
