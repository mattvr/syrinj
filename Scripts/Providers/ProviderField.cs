using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Syrinj.Providers
{
    public class ProviderField : Provider
    {
        private FieldInfo info;

        public ProviderField(FieldInfo info, object instance) : base(instance)
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
