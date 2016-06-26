using System.Reflection;

namespace Syrinj.Providers
{
    public class ProviderProperty : Provider
    {
        private PropertyInfo info;

        public ProviderProperty(PropertyInfo info, object instance) : base(instance)
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
