using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Tests.Integration.ConvenienceAttributes
{
    [TestFixture]
    internal class FindObjectOfTypeTest
    {
        internal class FindObjectOfTypeTestClass : MonoBehaviour
        {
            [FindObjectOfType] public AudioSource dependency;
        }

        internal class FindObjectOfSpecificTypeTestClass : MonoBehaviour
        {
            [FindObjectOfType(typeof(BoxCollider))]
            public Collider dependency;
        }

        private GameObject dependent;
        private FindObjectOfTypeTestClass behaviour;
        private FindObjectOfSpecificTypeTestClass specificBehaviour;

		[SetUp]
		public void SetUp() 
		{
			DependencyContainer.Instance.Reset();
		}

        [Test]
        public void InjectNotNull()
        {
            SetUpBehaviourAndSceneObjectAndInject();

            Assert.NotNull(behaviour.dependency);
        }

        private void SetUpBehaviourAndSceneObjectAndInject()
        {
            var obj = new GameObject();
            behaviour = obj.AddComponent<FindObjectOfTypeTestClass>();

			dependent = new GameObject("Test");
            dependent.AddComponent<AudioSource>();

            new GameObjectInjector(behaviour.gameObject).Inject();
        }

        [Test]
        public void InjectIsExpectedDependency()
        {
            SetUpBehaviourAndSceneObjectAndInject();

            Assert.AreEqual(dependent.GetComponent<AudioSource>(), behaviour.dependency);
        }

        [Test]
        public void InjectNull()
        {
            SetUpBehaviourWithoutSceneObjectAndInject();

			Assert.IsNull (behaviour.dependency);
        }

        private void SetUpBehaviourWithoutSceneObjectAndInject()
        {
            var obj = new GameObject();
            behaviour = obj.AddComponent<FindObjectOfTypeTestClass>();

            new GameObjectInjector(behaviour.gameObject).Inject();
        }

        [Test]
        public void InjectSpecificNotNull()
        {
            SetUpSpecificBehaviourAndSceneObjectAndInject();

            Assert.NotNull(specificBehaviour.dependency);
        }

        private void SetUpSpecificBehaviourAndSceneObjectAndInject()
        {
            var obj = new GameObject();
            specificBehaviour = obj.AddComponent<FindObjectOfSpecificTypeTestClass>();

            dependent = new GameObject();
            dependent.AddComponent<BoxCollider>();

            new GameObjectInjector(specificBehaviour.gameObject).Inject();
        }

        [Test]
        public void InjectSpecificIsExpectedDependency()
        {
            SetUpSpecificBehaviourAndSceneObjectAndInject();

            Assert.AreEqual(dependent.GetComponent<BoxCollider>(), specificBehaviour.dependency);
        }

        [Test]
        public void InjectSpecificNull()
        {
			SetUpSpecificBehaviourWithoutSceneObjectAndInject();

			Assert.IsNull (specificBehaviour.dependency);
        }

        private void SetUpSpecificBehaviourWithoutSceneObjectAndInject()
        {
            var obj = new GameObject();
            specificBehaviour = obj.AddComponent<FindObjectOfSpecificTypeTestClass>();

            dependent = new GameObject();

            new GameObjectInjector(specificBehaviour.gameObject).Inject();
        }

        [TearDown]
        public void TearDown()
        {
            if (dependent != null) GameObject.DestroyImmediate(dependent);
            if (behaviour != null) GameObject.DestroyImmediate(behaviour.gameObject);
            if (specificBehaviour != null) GameObject.DestroyImmediate(specificBehaviour.gameObject);
        }
    }
}

