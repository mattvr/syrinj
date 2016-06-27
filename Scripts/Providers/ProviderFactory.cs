using System;
using System.Reflection;

namespace Syrinj.Providers
{
    public static class ProviderFactory
    {
        public static Provider Create(MemberInfo info, object instance, string tag)
        {
            if (info.MemberType == MemberTypes.Property)
            {
                return new ProviderProperty((PropertyInfo)info, instance, tag);
            }
            else if (info.MemberType == MemberTypes.Field)
            {
                return new ProviderField((FieldInfo)info, instance, tag);
            }
            return null;
        }
    }
}
