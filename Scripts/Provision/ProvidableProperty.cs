using System.Reflection;

namespace Syrinj.Provision
{
    public class ProvidableProperty : Providable
    {
        private PropertyInfo info;

        public ProvidableProperty(PropertyInfo info, object instance, string tag) : base(instance, tag)
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
