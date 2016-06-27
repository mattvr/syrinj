using System;
using System.Reflection;
using UnityEngine;

namespace Syrinj.Injection
{
    public class InjectableField : Injectable
    {
        private FieldInfo _fieldInfo;

        public InjectableField(FieldInfo fieldInfo, Type type, MonoBehaviour monoBehaviour, string tag, Attribute attribute) : base(type, monoBehaviour, tag, attribute)
        {
            _fieldInfo = fieldInfo;
        }

        public override void Inject(object dependency)
        {
            _fieldInfo.SetValue(MonoBehaviour, dependency);
        }

        public override string ToString()
        {
            return _fieldInfo.Name;
        }
    }
}
