using System;

namespace Syrinj.Provision
{
    public class ProvidableSingleton : Providable
    {
        private object Instance;

        public ProvidableSingleton(Type type, string tag) : base(null, tag)
        {
            Type = type;
        }

        public override object Get()
        {
            if (Instance == null)
            {
                Instance = Activator.CreateInstance(Type);
            }

            return Instance;
        }
    }
}

