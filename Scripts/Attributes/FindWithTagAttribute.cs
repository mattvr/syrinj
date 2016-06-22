using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class FindWithTagAttribute : Attribute
{
    public string Tag { get; private set; }

    public FindWithTagAttribute(string tag)
    {
        Tag = tag;
    }
}
