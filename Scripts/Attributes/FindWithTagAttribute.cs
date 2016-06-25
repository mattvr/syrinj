namespace Syrinj.Attributes
{
    public class FindWithTagAttribute : UnityInjectorAttribute
    {
        public string Tag { get; private set; }

        public FindWithTagAttribute(string tag)
        {
            Tag = tag;
        }
    }
}
