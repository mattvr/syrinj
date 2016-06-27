namespace Syrinj.Attributes
{
    public class FindWithTagAttribute : UnityConvenienceAttribute
    {
        public readonly string Tag;

        public FindWithTagAttribute(string tag)
        {
            Tag = tag;
        }
    }
}
