using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syrinj.Graph;
using Syrinj.Providers;

namespace Assets.Syrinj.Scripts.Graph
{
    public abstract class IDependencyGraph
    {
        protected class Binding
        {
            public readonly Type Type;
            public readonly string Tag;

            public Binding(Type type)
            {
                this.Type = type;
            }

            public Binding(Type type, string tag)
            {
                this.Type = type;
                this.Tag = tag;
            }

            public override bool Equals(object obj)
            {
                var binding = obj as Binding;
                if (binding == null)
                {
                    return false;
                }
                return binding.Type == this.Type && binding.Tag == this.Tag;
            }

            public override int GetHashCode()
            {
                return 7 * Type.GetHashCode() + (Tag != null ? 23 * Tag.GetHashCode() : 29);
            }
        }

        public abstract void RegisterProvider(Type type, Provider provider);

        public abstract void RegisterProvider(Type type, string tag, Provider provider);

        public abstract object Get(Type key);

        public abstract object Get(Type key, string tag);
    }
}
