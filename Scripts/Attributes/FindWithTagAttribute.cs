using Syrinj.Attributes;

namespace Syrinj
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
