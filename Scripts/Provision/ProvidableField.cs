using System.Reflection;

namespace Syrinj.Provision
{
    public class ProvidableField : Providable
    {
        private FieldInfo info;

        public ProvidableField(FieldInfo info, object instance, string tag) : base(instance, tag)
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
