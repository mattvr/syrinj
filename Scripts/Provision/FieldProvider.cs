using System.Reflection;

namespace Syrinj.Provision
{
    public class FieldProvider : IProvider
    {
        public System.Type Type { get; set; }
        public string Tag { get; set; }

        private object instance;
        private FieldInfo info;

        public FieldProvider(FieldInfo info, object instance, string tag)
        {
            this.info = info;
            this.Tag = tag;
            this.instance = instance;
            this.Type = info.FieldType;
        }

        public object Get()
        {
            return info.GetValue(instance);
        }
    }
}
