using System;

namespace Syrinj.Providers
{
    public class ProviderSingleton : Provider
    {
        private object Instance;

        public ProviderSingleton(Type type, string tag) : base(null, tag)
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

