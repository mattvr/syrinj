using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FindAttribute : Attribute
{
    public string GameObjectName { get; private set; }

    public FindAttribute(string gameObjectName)
    {
        GameObjectName = gameObjectName;
    }
}
