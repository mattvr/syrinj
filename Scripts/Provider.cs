using System;
using Syrinj.Provision;

namespace Syrinj
{
    public class Provider<T> : Provider where T : new()
    {
        public override object Get()
        {
            return GetAsType();
        }

        public T GetAsType()
        {
            var obj = GetObject();
            DependencyContainer.Instance.Inject(obj);
            return obj;
        }

        protected virtual T GetObject()
        {
            return new T();
        }
    }

    public abstract class Provider : IProvider
    {
        public System.Type Type { get; set; }
        public string Tag { get; set; }
        public object Instance { get; set; }

        public abstract object Get();
    }
}

