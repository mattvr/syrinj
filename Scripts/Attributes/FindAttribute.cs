namespace Syrinj.Attributes
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
