using System;

namespace Syrinj
{
    public class InjectFactory<T> where T : new()
    {
        public T Create()
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
}

