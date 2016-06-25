using System;
using System.Reflection;
using UnityEngine;

namespace Syrinj.Injection
{
    public class InjectableField : Injectable
    {
        private FieldInfo _fieldInfo;

        public InjectableField(FieldInfo fieldInfo, Type type, Attribute attribute, MonoBehaviour monoBehaviour) : base(type, attribute, monoBehaviour)
        {
            _fieldInfo = fieldInfo;
        }

        public override void Inject(object dependency)
        {
            _fieldInfo.SetValue(MonoBehaviour, dependency);
        }
    }
}
