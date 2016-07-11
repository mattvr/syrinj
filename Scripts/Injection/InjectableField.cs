using System;
using System.Reflection;
using UnityEngine;

namespace Syrinj.Injection
{
    public class InjectableField : Injectable
    {
        private FieldInfo _fieldInfo;

        public InjectableField(FieldInfo fieldInfo, Type type, object obj, string tag, Attribute attribute) : base(type, obj, tag, attribute)
        {
            _fieldInfo = fieldInfo;
        }

        public override void Inject(object dependency)
        {
            _fieldInfo.SetValue(Object, dependency);
        }

        public override string ToString()
        {
            return _fieldInfo.Name;
        }
    }
}
