using System;
using System.Reflection;
using UnityEngine;

namespace Syrinj.Injection
{
    public class InjectableProperty : Injectable
    {
        private PropertyInfo _propertyInfo;

        public InjectableProperty(PropertyInfo propertyInfo, Type type, Attribute attribute, MonoBehaviour monoBehaviour) : base(type, attribute, monoBehaviour)
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
