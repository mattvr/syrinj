using Syrinj.Attributes;

namespace Syrinj
{
    public class FindAttribute : UnityConvenienceAttribute
    {
        public readonly string GameObjectName;

        public FindAttribute(string gameObjectName)
        {
            GameObjectName = gameObjectName;
        }
    }
}
