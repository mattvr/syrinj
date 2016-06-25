using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Syrinj.Attributes;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Syrinj.Caching
{
    public class AttributeCache
    {
        private readonly Dictionary<Type, List<MemberInfo>> cachedTypes;
        private readonly Dictionary<MemberInfo, List<UnityInjectorAttribute>> injectorAttributes;
        private readonly Dictionary<MemberInfo, List<UnityProviderAttribute>> providerAttributes;

        public AttributeCache()
        {
            cachedTypes = new Dictionary<Type, List<MemberInfo>>();
            injectorAttributes = new Dictionary<MemberInfo, List<UnityInjectorAttribute>>();
            providerAttributes = new Dictionary<MemberInfo, List<UnityProviderAttribute>>();
        }

        public List<UnityInjectorAttribute> GetInjectorAttributesForMember(MemberInfo info)
        {
            TryCacheType(info.ReflectedType);

            if (injectorAttributes.ContainsKey(info))
            {
                return injectorAttributes[info];
            }
            else
            {
                return null;
            }
        }

        public List<UnityProviderAttribute> GetProviderAttributesForMember(MemberInfo info)
        {
            TryCacheType(info.ReflectedType);

            if (providerAttributes.ContainsKey(info))
            {
                return providerAttributes[info];
            }
            else
            {
                return null;
            }
        }

        public List<MemberInfo> GetMembersForType(Type type)
        {
            TryCacheType(type);

            return cachedTypes[type];
        }

        private void TryCacheType(Type type)
        {
            if (!IsCached(type))
            {
                CacheAllMembers(type);
            }
        }

        private bool IsCached(Type type)
        {
            return cachedTypes.ContainsKey(type);
        }

        private void CacheAllMembers(Type type)
        {
            var members = type.FindMembers(ValidMemberTypes(), ValidBindingFlags(), Filter,
                typeof (UnityHelperAttribute));

            for (int i = 0; i < members.Length; i++)
            {
                CacheMember(members[i]);
            }

            cachedTypes.Add(type, members.ToList());
        }

        private static MemberTypes ValidMemberTypes()
        {
            return MemberTypes.Field | MemberTypes.Property;
        }

        private static BindingFlags ValidBindingFlags()
        {
            return BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        }

        private static bool Filter(MemberInfo m, object filtercriteria)
        {
            return m.IsDefined((Type)filtercriteria, false);
        }

        private void CacheMember(MemberInfo info)
        {
            if (!IsValidMember(info))
            {
                return;
            }

            var attributes = info.GetCustomAttributes(typeof(UnityHelperAttribute), false);

            for (int i = 0; i < attributes.Length; i++)
            {
                CacheAttribute(info, (UnityHelperAttribute)attributes[i]);
            }
        }

        private static bool IsValidMember(MemberInfo info)
        {
            return (info.MemberType & ValidMemberTypes()) != 0;
        }

        private void CacheAttribute(MemberInfo info, UnityHelperAttribute attribute)
        {
            if (IsInjectorAttribute(attribute))
            {
                CacheInjectorAttribute(info, (UnityInjectorAttribute) attribute);
            }
            else if (IsProviderAttribute(attribute))
            {
                CacheProviderAttribute(info, (UnityProviderAttribute) attribute);
            }
        }

        private static bool IsInjectorAttribute(Attribute attribute)
        {
            return attribute.GetType().IsSubclassOf(typeof(UnityInjectorAttribute));
        }

        private void CacheInjectorAttribute(MemberInfo info, UnityInjectorAttribute attribute)
        {
            if (!injectorAttributes.ContainsKey(info))
            {
                injectorAttributes.Add(info, new List<UnityInjectorAttribute>());
            }
            injectorAttributes[info].Add(attribute);
        }

        private static bool IsProviderAttribute(Attribute attribute)
        {
            return attribute.GetType().IsSubclassOf(typeof(UnityProviderAttribute));
        }

        private void CacheProviderAttribute(MemberInfo info, UnityProviderAttribute attribute)
        {
            if (!providerAttributes.ContainsKey(info))
            {
                providerAttributes.Add(info, new List<UnityProviderAttribute>());
            }
            providerAttributes[info].Add(attribute);
        }
    }
}
