using System;

namespace Syrinj.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public abstract class UnityDependencyAttribute : Attribute
    {

    }
}
