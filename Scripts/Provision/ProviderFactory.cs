using System;
using System.Reflection;
using System.Collections.Generic;

using Syrinj.Attributes;

namespace Syrinj.Provision
{
    public class ProviderFactory
    {
        protected class ProviderData
        {
            public string Tag;
            public bool IsValid;
            public bool IsSingleton;
            public bool IsInstance;
        }

        public static Provider CreateGeneric(Type type) {
            var providerType = typeof(Provider<>).MakeGenericType(type);
            return (Provider) Activator.CreateInstance(providerType);
        }

        public static IProvider Create(MemberInfo info, object instance, IList<UnityProviderAttribute> attributes)
        {
            var data = ParseAttributes(attributes);

            if (!data.IsValid)
            {
                return null;
            }
            else if (info.MemberType == MemberTypes.Property)
            {
                return GetProviderFromProperty(info, instance, data);
            }
            else if (info.MemberType == MemberTypes.Field)
            {
                return GetProviderFromField(info, instance, data);
            }
            return null;
        }

        private static ProviderData ParseAttributes(IList<UnityProviderAttribute> attributes) 
        {
            var data = new ProviderData();

            for (int i = 0; i < attributes.Count; i++)
            {
                var attribute = attributes[i];

                var provides = AsProvidesAttribute(attribute);
                if (provides != null)
                {
                    data.Tag = provides.Tag;
                    data.IsValid = true;
                    continue;
                }

                var isSingleton = IsSingleton(attribute);
                if (isSingleton)
                {
                    data.IsSingleton = true;
                    continue;
                }  

                var isInstance = IsInstance(attribute);
                if (isInstance)
                {
                    data.IsInstance = true;
                    continue;
                }
            }

            return data;
        }

        private static ProvidesAttribute AsProvidesAttribute(UnityProviderAttribute attribute) {
            return attribute as ProvidesAttribute;
        }

        private static bool IsSingleton(UnityProviderAttribute attribute)
        {
            return attribute is SingletonAttribute;
        }

        private static bool IsInstance(UnityProviderAttribute attribute)
        {
            return attribute is InstanceAttribute;
        }

        private static IProvider GetProviderFromProperty(MemberInfo info, object instance, ProviderData data) {
            var pInfo = (PropertyInfo)info;

            var provider = GetInstantiatingProvider(pInfo.PropertyType, data);
            if (provider != null)
            {
                return provider;
            }
            else
            {
                return new PropertyProvider(pInfo, instance, data.Tag);
            }
        }

        private static IProvider GetProviderFromField(MemberInfo info, object instance, ProviderData data) {
            var fInfo = (FieldInfo)info;

            var provider = GetInstantiatingProvider(fInfo.FieldType, data);
            if (provider != null)
            {
                return provider;
            }
            else
            {
                return new FieldProvider(fInfo, instance, data.Tag);
            }
        }

        private static IProvider GetInstantiatingProvider(Type type, ProviderData data) {
            if (data.IsSingleton)
            {
                return new SingletonProvider(type, data.Tag);
            }
            else if (data.IsInstance)
            {
                return new InstanceProvider(type, data.Tag);
            }
            else
            {
                return null;
            }
        }
    }
}
