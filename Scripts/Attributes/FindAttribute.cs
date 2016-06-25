namespace Syrinj.Attributes
{
    public class FindAttribute : UnityInjectorAttribute
    {
        public string GameObjectName { get; private set; }

        public FindAttribute(string gameObjectName)
        {
            GameObjectName = gameObjectName;
        }
    }
}
