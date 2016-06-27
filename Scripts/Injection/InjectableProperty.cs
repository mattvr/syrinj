using System;
using System.Reflection;
using UnityEngine;

namespace Syrinj.Injection
{
    public class InjectableProperty : Injectable
    {
        private PropertyInfo _propertyInfo;

        public InjectableProperty(PropertyInfo propertyInfo, Type type, MonoBehaviour monoBehaviour, string tag, Attribute attribute) : base(type, monoBehaviour, tag, attribute)
        {
            _propertyInfo = propertyInfo;
        }

        public override void Inject(object dependency)
        {
            _propertyInfo.SetValue(MonoBehaviour, dependency, null);
        }

        public override string ToString()
        {
            return _propertyInfo.Name;
        }
    }
}
