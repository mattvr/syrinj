using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FindObjectOfTypeAttribute : Attribute
{
    public Type ComponentType { get; private set; }

    public FindObjectOfTypeAttribute(Type componentType)
    {
        ComponentType = componentType;
    }
}
