using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Syrinj.Provision;

namespace Syrinj.Tests.Utility
{
    internal class MockProviderFactory
    {
        public static Providable Create(Type type, string tag)
        {
            return new ProvidableSingleton(type, tag);
        }

        public static ProvidableField Create(FieldInfo info, object instance, string tag) {
            return new ProvidableField(info, instance, tag);
        }
    }
}
