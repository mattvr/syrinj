namespace Syrinj.Attributes
{
    public class FindWithTagAttribute : UnityInjectorAttribute
    {
        public readonly string Tag;

        public FindWithTagAttribute(string tag)
        {
            Tag = tag;
        }
    }
}
