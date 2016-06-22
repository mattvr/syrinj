using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Object = UnityEngine.Object;

public class MonoBehaviourInjector
{
    private class InjectionParameters
    {
        public Type Type;
        public Attribute Attribute;

        public InjectionParameters(Type type, Attribute attribute)
        {
            Type = type;
            Attribute = attribute;
        }
    }

    private delegate object InjectDelegate(InjectionParameters attribute);

    private delegate Component GetComponentDelegate(Type type);

    private readonly MonoBehaviour _monoBehaviour;
    private readonly Type _derivedType;

    public MonoBehaviourInjector(MonoBehaviour monoBehaviour)
    {
        _monoBehaviour = monoBehaviour;
        _derivedType = monoBehaviour.GetType();
    }

    public void Inject()
    {
        InternalInject<FindAttribute>(p => 
            GameObject.Find((p.Attribute as FindAttribute).GameObjectName)
        );

        InternalInject<FindObjectOfTypeAttribute>(p =>
            Object.FindObjectOfType((p.Attribute as FindObjectOfTypeAttribute).ComponentType)
        );

        InternalInject<FindWithTagAttribute>(p =>
            GameObject.FindWithTag((p.Attribute as FindWithTagAttribute).Tag)
        );
        
        InternalInject<GetComponentAttribute>(p =>
            TryGetComponent(p, type => _monoBehaviour.GetComponent(type))
        );

        InternalInject<GetComponentInChildrenAttribute>(p =>
            TryGetComponent(p, type => _monoBehaviour.GetComponentInChildren(type))
        );
    }

    private object TryGetComponent(InjectionParameters parameters, GetComponentDelegate componentGetter)
    {
        var attribute = parameters.Attribute as GetComponentAttribute;
        if (attribute.ComponentType == null)
        {
            return componentGetter(parameters.Type);
        }
        else
        {
            return componentGetter(attribute.ComponentType);
        }
    }

    private void InternalInject<T>(InjectDelegate injectDelegate) where T : Attribute
    {
        Type attributeType = typeof(T);
        var members = GetMembersWithAttribute(attributeType);

        for (int i = 0; i < members.Length; i++)
        {
            var attribute = GetAttributeOfType<T>(members[i]);
            Inject<T>(members[i], injectDelegate, attribute);
        }
    }

    private MemberInfo[] GetMembersWithAttribute(Type t)
    {
        return _derivedType.FindMembers(
            GetMemberTypes(),
            GetBindingFlags(),
            MemberFilter,
            t);
    }
    
    private void Inject<T>(MemberInfo info, InjectDelegate injectDelegate, Attribute attribute)
    {
        if (info.MemberType == MemberTypes.Property)
        {
            var pInfo = info as PropertyInfo;
            var parameters = new InjectionParameters(pInfo.PropertyType, attribute);
            pInfo.SetValue(_monoBehaviour, injectDelegate(parameters), null);
        }
        else if (info.MemberType == MemberTypes.Field)
        {
            var fInfo = info as FieldInfo;
            var parameters = new InjectionParameters(fInfo.FieldType, attribute);
            fInfo.SetValue(_monoBehaviour, injectDelegate(parameters));
        }
    }

    private T GetAttributeOfType<T>(MemberInfo member) where T : Attribute
    {
        var attributes = member.GetCustomAttributes(typeof(T), true);
        if (attributes.Length > 1)
        {
            throw new InjectionException("Found multiple attributes of type " + typeof(T).Name);
        }
        else if (attributes.Length == 0)
        {
            throw new InjectionException("Could not find attributes of type " + typeof(T).Name);
        }
        else if (!(attributes[0] is T))
        {
            throw new InjectionException("Attribute is not of type " + typeof(T).Name);
        }
        return (T) attributes[0];
    }

    private static MemberTypes GetMemberTypes()
    {
        return MemberTypes.Property | MemberTypes.Field;
    }

    private static BindingFlags GetBindingFlags()
    {
        return BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
    }

    private static bool MemberFilter(MemberInfo memberInfo, object filterCriteria)
    {
        return memberInfo.IsDefined((Type)filterCriteria, true);
    }
}