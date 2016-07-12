using System;

namespace Syrinj.Provision
{
    public class ProvidableInstance : Providable
    {
        public ProvidableInstance(Type type, string tag) : base(null, tag)
        {
            Type = type;
        }

        public override object Get()
        {
            return Activator.CreateInstance(Type);
        }
    }
}

