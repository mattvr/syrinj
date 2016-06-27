using System.Reflection;

namespace Syrinj.Providers
{
    public class ProviderProperty : Provider
    {
        private PropertyInfo info;

        public ProviderProperty(PropertyInfo info, object instance, string tag) : base(instance, tag)
        {
            this.info = info;
            this.Type = info.PropertyType;
        }

        public override object Get()
        {
            return info.GetValue(Instance, null);
        }
    }
}
