using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Tests.ConvenienceAttributes
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

        private GameObject dependency;
        private FindObjectOfTypeTestClass behaviour;
        private FindObjectOfSpecificTypeTestClass specificBehaviour;

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

            dependency = new GameObject("Test");
            dependency.AddComponent<AudioSource>();

            new MonoBehaviourInjector(behaviour).Inject();
        }

        [Test]
        public void InjectIsExpectedDependency()
        {
            SetUpBehaviourAndSceneObjectAndInject();

            Assert.AreEqual(dependency.GetComponent<AudioSource>(), behaviour.dependency);
        }

        [Test]
        [ExpectedException(typeof(InjectionException))]
        public void InjectNull()
        {
            SetUpBehaviourWithoutSceneObjectAndInject();
        }

        private void SetUpBehaviourWithoutSceneObjectAndInject()
        {
            var obj = new GameObject();
            behaviour = obj.AddComponent<FindObjectOfTypeTestClass>();

            new MonoBehaviourInjector(behaviour).Inject();
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

            dependency = new GameObject();
            dependency.AddComponent<BoxCollider>();

            new MonoBehaviourInjector(specificBehaviour).Inject();
        }

        [Test]
        public void InjectSpecificIsExpectedDependency()
        {
            SetUpSpecificBehaviourAndSceneObjectAndInject();

            Assert.AreEqual(dependency.GetComponent<BoxCollider>(), specificBehaviour.dependency);
        }

        [Test]
        [ExpectedException(typeof(InjectionException))]
        public void InjectSpecificNull()
        {
            SetUpSpecificBehaviourWithoutSceneObjectAndInject();
        }

        private void SetUpSpecificBehaviourWithoutSceneObjectAndInject()
        {
            var obj = new GameObject();
            specificBehaviour = obj.AddComponent<FindObjectOfSpecificTypeTestClass>();

            dependency = new GameObject();

            new MonoBehaviourInjector(specificBehaviour).Inject();
        }

        [TearDown]
        public void TearDown()
        {
            if (dependency != null) GameObject.DestroyImmediate(dependency);
            if (behaviour != null) GameObject.DestroyImmediate(behaviour.gameObject);
            if (specificBehaviour != null) GameObject.DestroyImmediate(specificBehaviour.gameObject);
        }
    }
}

