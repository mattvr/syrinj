using System;

namespace Syrinj.Provision
{
    public class SingletonProvider : IProvider
    {
        public System.Type Type { get; set; }
        public string Tag { get; set; }

        private object instance;

        public SingletonProvider(Type type, string tag)
        {
            this.Type = type;
            this.Tag = tag;
        }

        public object Get()
        {
            if (instance == null)
            {
                instance = Activator.CreateInstance(Type);
                DependencyContainer.Instance.Inject(instance);
            }

            return instance;
        }
    }
}

