using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Syrinj.Providers;

namespace Syrinj.Tests.Utility
{
    internal class MockProviderFactory
    {
        public static Provider Create(Type type, string tag)
        {
            return new ProviderSingleton(type, tag);
        }

        public static ProviderField Create(FieldInfo info, object instance, string tag) {
            return new ProviderField(info, instance, tag);
        }
    }
}
