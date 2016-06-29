using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Syrinj.Tests.Utility
{
    internal class MockDependency<T>
    {
        public readonly T Attribute;

        public MockDependency(T attribute)
        {
            this.Attribute = attribute;
        }

        public override bool Equals(object obj)
        {
            var dep = obj as MockDependency<T>;
            if (obj == null)
            {
                return false;
            }
            else
            {
                return this.Attribute.Equals(dep.Attribute);
            }
        }

        public override int GetHashCode()
        {
            return Attribute.GetHashCode();
        }

        public override string ToString()
        {
            return Attribute.ToString();
        }
    }
}
