using UnityEngine;
using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Injection;

namespace Syrinj.Tests.Resolvers
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

            new MonoBehaviourInjector(behaviour).Inject();
        }

        [Test]
        public void InjectIsExpectedObject()
        {
            SetUpWithDependencyAndInject();

            Assert.AreEqual(behaviour.GetComponentInChildren<AudioSource>(), behaviour.audioSource);
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
            behaviour = obj.AddComponent<GetComponentInChildrenTestClass>();

            var child = new GameObject();
            child.transform.SetParent(obj.transform);

            new MonoBehaviourInjector(behaviour).Inject();
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

            new MonoBehaviourInjector(specificBehaviour).Inject();
        }

        [Test]
        public void InjectSpecificIsExpectedObject()
        {
            SetUpWithSpecificDependencyAndInject();

            Assert.AreEqual(specificBehaviour.GetComponentInChildren<BoxCollider>(), specificBehaviour.collider);
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
            specificBehaviour = obj.AddComponent<SpecificGetComponentInChildrenTestClass>();

            new MonoBehaviourInjector(specificBehaviour).Inject();
        }

        [TearDown]
        public void TearDown()
        {
            if (behaviour != null) GameObject.DestroyImmediate(behaviour.gameObject);
            if (specificBehaviour != null) GameObject.DestroyImmediate(specificBehaviour.gameObject);
        }
    }
}

