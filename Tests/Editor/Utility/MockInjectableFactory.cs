using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Syrinj.Injection;

namespace Syrinj.Tests.Utility
{
    internal class MockInjectableFactory
    {
        public static Injectable Create()
        {
            return new InjectableField(null, null, null, null, null);
        }

        public static Injectable Create(FieldInfo info, Type type = null, string tag = null)
        {
            return new InjectableField(info, type, null, tag, null);
        }

        public static Injectable Create(PropertyInfo info, Type type = null, string tag = null)
        {
            return new InjectableProperty(info, type, null, tag, null);
        }

        public static Injectable Create(Attribute attribute)
        {
            return new InjectableField(null, null, null, null, attribute);
        }
    }
}
