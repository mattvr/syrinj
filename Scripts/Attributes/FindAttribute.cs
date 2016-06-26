namespace Syrinj.Attributes
{
    public class FindAttribute : UnityInjectorAttribute
    {
        public readonly string GameObjectName;

        public FindAttribute(string gameObjectName)
        {
            GameObjectName = gameObjectName;
        }
    }
}
