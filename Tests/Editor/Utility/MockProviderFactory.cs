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
        public static IProvider Create(Type type, string tag)
        {
            return new SingletonProvider(type, tag);
        }

        public static FieldProvider Create(FieldInfo info, object instance, string tag) {
            return new FieldProvider(info, instance, tag);
        }
    }
}
