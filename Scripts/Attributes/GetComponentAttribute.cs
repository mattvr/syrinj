
using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class GetComponentAttribute : Attribute
{
    public Type ComponentType { get; private set; }

    public GetComponentAttribute()
    {
        
    }

    public GetComponentAttribute(Type componentType)
    {
        ComponentType = componentType;
    }
}
