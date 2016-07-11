using System;

namespace Syrinj.Providers
{
    public class ProviderInstance : Provider
    {
        public ProviderInstance(Type type, string tag) : base(null, tag)
        {
            Type = type;
        }

        public override object Get()
        {
            return Activator.CreateInstance(Type);
        }
    }
}

