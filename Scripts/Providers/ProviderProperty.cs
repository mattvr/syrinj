using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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
