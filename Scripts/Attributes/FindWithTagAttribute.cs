namespace Syrinj.Attributes
{
    public class FindWithTagAttribute : UnityHelperAttribute
    {
        public string Tag { get; private set; }

        public FindWithTagAttribute(string tag)
        {
            Tag = tag;
        }
    }
}
