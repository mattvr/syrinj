using System.Reflection;

namespace Syrinj.Providers
{
    public class ProviderField : Provider
    {
        private FieldInfo info;

        public ProviderField(FieldInfo info, object instance, string tag) : base(instance, tag)
        {
            this.info = info;
            this.Type = info.FieldType;
        }

        public override object Get()
        {
            return info.GetValue(Instance);
        }
    }
}
