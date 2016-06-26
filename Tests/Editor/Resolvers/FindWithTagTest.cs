using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Tests.Resolvers
{
    [TestFixture]
    internal class FindWithTagTest
    {
        private const string TAG = "EditorOnly";

        internal class FindWithTagTestClass : MonoBehaviour
        {
            [FindWithTag(TAG)] public GameObject dependency;
        }

        private GameObject thing;
        private FindWithTagTestClass behaviour;

        [Test]
        public void InjectNotNull()
        {
            SetUpBehaviourAndInject();

            Assert.NotNull(behaviour.dependency);
        }

        private void SetUpBehaviourAndInject()
        {
            var obj = new GameObject();
            behaviour = obj.AddComponent<FindWithTagTestClass>();

            thing = new GameObject();
            thing.tag = TAG;

            new MonoBehaviourInjector(behaviour).Inject();
        }

        [Test]
        public void InjectIsExpectedDependency()
        {
            SetUpBehaviourAndInject();

            Assert.AreEqual(thing, behaviour.dependency);
        }

        [Test]
        [ExpectedException(typeof(InjectionException))]
        public void InjectNull()
        {
            SetUpBehaviourWithoutTagAndInject();

            Assert.Null(behaviour.dependency);
        }

        private void SetUpBehaviourWithoutTagAndInject()
        {
            var obj = new GameObject();
            behaviour = obj.AddComponent<FindWithTagTestClass>();

            thing = new GameObject();

            new MonoBehaviourInjector(behaviour).Inject();
        }

        [TearDown]
        public void TearDown()
        {
            if (thing != null) GameObject.DestroyImmediate(thing);
        }
    }
}

