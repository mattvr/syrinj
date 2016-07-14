using System;

namespace Syrinj.Provision
{
    public class InstanceProvider : IProvider
    {
        public System.Type Type { get; set; }
        public string Tag { get; set; }

        public InstanceProvider(Type type, string tag)
        {
            this.Type = type;
            this.Tag = tag;
        }

        public object Get()
        {
            var instance = Activator.CreateInstance(Type);
            DependencyContainer.Instance.Inject(instance);
            return instance;
        }
    }
}

