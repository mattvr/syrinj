using System.Reflection;

namespace Syrinj.Provision
{
    public class PropertyProvider : IProvider
    {
        public System.Type Type { get; set; }
        public string Tag { get; set; }

        private PropertyInfo info;
        private object instance;

        public PropertyProvider(PropertyInfo info, object instance, string tag)
        {
            this.info = info;
            this.Type = info.PropertyType;
            this.Tag = tag;
            this.instance = instance;
        }

        public object Get()
        {
            return info.GetValue(instance, null);
        }
    }
}
