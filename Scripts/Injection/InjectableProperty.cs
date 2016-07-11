using System;
using System.Reflection;
using UnityEngine;

namespace Syrinj.Injection
{
    public class InjectableProperty : Injectable
    {
        private PropertyInfo _propertyInfo;

        public InjectableProperty(PropertyInfo propertyInfo, Type type, object obj, string tag, Attribute attribute) : base(type, obj, tag, attribute)
        {
            _propertyInfo = propertyInfo;
        }

        public override void Inject(object dependency)
        {
            _propertyInfo.SetValue(Object, dependency, null);
        }

        public override string ToString()
        {
            return _propertyInfo.Name;
        }
    }
}
