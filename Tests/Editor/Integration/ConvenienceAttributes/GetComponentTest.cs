using UnityEngine;
using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Injection;

namespace Syrinj.Tests.ConvenienceAttributes
{
    [TestFixture]
    internal class GetComponentTest
    {
        internal class GetComponentTestClass : MonoBehaviour
        {
            [GetComponent] public AudioSource audioSource;
        }

        internal class SpecificGetComponentTestClass : MonoBehaviour
        {
            [GetComponent(typeof(BoxCollider))]
            public new Collider collider;
        }

        private GetComponentTestClass behaviour;
        private SpecificGetComponentTestClass specificBehaviour;

        [Test]
        public void InjectNotNull()
        {
            SetUpWithDependencyAndInject();

            Assert.NotNull(behaviour.audioSource);
        }

        private void SetUpWithDependencyAndInject()
        {
            var obj = new GameObject();
            behaviour = obj.AddComponent<GetComponentTestClass>();

            obj.AddComponent<AudioSource>();

            new GameObjectInjector(behaviour.gameObject).Inject();
        }

        [Test]
        public void InjectIsExpectedObject()
        {
            SetUpWithDependencyAndInject();

            Assert.AreEqual(behaviour.GetComponent<AudioSource>(), behaviour.audioSource);
        }

        [Test]
        [ExpectedException(typeof(InjectionException))]
        public void InjectWithoutDependency()
        {
            SetUpWithoutDependencyAndInject();
        }

        private void SetUpWithoutDependencyAndInject()
        {
            var obj = new GameObject();
            behaviour = obj.AddComponent<GetComponentTestClass>();

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
            specificBehaviour = obj.AddComponent<SpecificGetComponentTestClass>();

            obj.AddComponent<BoxCollider>();

            new GameObjectInjector(specificBehaviour.gameObject).Inject();
        }

        [Test]
        public void InjectSpecificIsExpectedObject()
        {
            SetUpWithSpecificDependencyAndInject();

            Assert.AreEqual(specificBehaviour.GetComponent<BoxCollider>(), specificBehaviour.collider);
        }

        [Test]
        [ExpectedException(typeof(InjectionException))]
        public void InjectSpecificWithoutDependency()
        {
            SetUpWithoutSpecificDependencyAndInject();
        }

        private void SetUpWithoutSpecificDependencyAndInject()
        {
            var obj = new GameObject();
            specificBehaviour = obj.AddComponent<SpecificGetComponentTestClass>();

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

