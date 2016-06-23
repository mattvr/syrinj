namespace Syrinj.Attributes
{
    public class FindAttribute : UnityHelperAttribute
    {
        public string GameObjectName { get; private set; }

        public FindAttribute(string gameObjectName)
        {
            GameObjectName = gameObjectName;
        }
    }
}
