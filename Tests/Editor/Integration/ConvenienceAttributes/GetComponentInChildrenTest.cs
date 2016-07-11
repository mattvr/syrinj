using UnityEngine;
using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Injection;

namespace Syrinj.Tests.Integration.ConvenienceAttributes
{
    [TestFixture]
    internal class GetComponentInChildrenTest
    {
        internal class GetComponentInChildrenTestClass : MonoBehaviour
        {
            [GetComponentInChildren] public AudioSource audioSource;
        }

        internal class SpecificGetComponentInChildrenTestClass : MonoBehaviour
        {
            [GetComponentInChildren(typeof(BoxCollider))]
            public new Collider collider;
        }

        private GetComponentInChildrenTestClass behaviour;
        private SpecificGetComponentInChildrenTestClass specificBehaviour;

		[SetUp]
		public void SetUp()
		{
			DependencyContainer.Instance.Reset();
		}

        [Test]
        public void InjectNotNull()
        {
            SetUpWithDependencyAndInject();

            Assert.NotNull(behaviour.audioSource);
        }

        private void SetUpWithDependencyAndInject()
        {
            var obj = new GameObject();
            behaviour = obj.AddComponent<GetComponentInChildrenTestClass>();

            var child = new GameObject();
            child.transform.SetParent(obj.transform);
            child.AddComponent<AudioSource>();

            new GameObjectInjector(behaviour.gameObject).Inject();
        }

        [Test]
        public void InjectIsExpectedObject()
        {
            SetUpWithDependencyAndInject();

            Assert.AreEqual(behaviour.GetComponentInChildren<AudioSource>(), behaviour.audioSource);
        }

        [Test]
        public void InjectWithoutDependency()
        {
            SetUpWithoutDependencyAndInject();

			Assert.IsNull(behaviour.audioSource);
        }

        private void SetUpWithoutDependencyAndInject()
        {
            var obj = new GameObject();
            behaviour = obj.AddComponent<GetComponentInChildrenTestClass>();

            var child = new GameObject();
            child.transform.SetParent(obj.transform);

            new GameObjectInjector(behaviour.gameObject).Inject();
        }

        [Test]
        public void InjectSpecificNotNull()
        {
            SetUpWithSpecificDependencyAndInject();

            Assert.NotNull(specificBehaviour.collider);
        }

        private void SetUpWithSpecificDependencyAndInject()
        {
            var obj = new GameObject();
            specificBehaviour = obj.AddComponent<SpecificGetComponentInChildrenTestClass>();

            obj.AddComponent<BoxCollider>();

            new GameObjectInjector(specificBehaviour.gameObject).Inject();
        }

        [Test]
        public void InjectSpecificIsExpectedObject()
        {
            SetUpWithSpecificDependencyAndInject();

            Assert.AreEqual(specificBehaviour.GetComponentInChildren<BoxCollider>(), specificBehaviour.collider);
        }

        [Test]
        public void InjectSpecificWithoutDependency()
        {
            SetUpWithoutSpecificDependencyAndInject();

			Assert.IsNull(specificBehaviour.collider);
        }

        private void SetUpWithoutSpecificDependencyAndInject()
        {
            var obj = new GameObject();
            specificBehaviour = obj.AddComponent<SpecificGetComponentInChildrenTestClass>();

            new GameObjectInjector(specificBehaviour.gameObject).Inject();
        }

        [TearDown]
        public void TearDown()
        {
            if (behaviour != null) GameObject.DestroyImmediate(behaviour.gameObject);
            if (specificBehaviour != null) GameObject.DestroyImmediate(specificBehaviour.gameObject);
        }
    }
}

