using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Tests.Integration.ConvenienceAttributes
{
    [TestFixture]
    internal class FindWithTagTest
    {
        private const string TAG = "EditorOnly";

        internal class FindWithTagTestClass : MonoBehaviour
        {
            [FindWithTag(TAG)] public GameObject dependency;
        }

        private GameObject dependency;
        private FindWithTagTestClass behaviour;

		[SetUp]
		public void SetUp()
		{
			DependencyContainer.Instance.Reset();
		}

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

            dependency = new GameObject();
            dependency.tag = TAG;

            new GameObjectInjector(behaviour.gameObject).Inject();
        }

        [Test]
        public void InjectIsExpectedDependency()
        {
            SetUpBehaviourAndInject();

            Assert.AreEqual(dependency, behaviour.dependency);
        }

        [Test]
        public void InjectNull()
        {
            SetUpBehaviourWithoutTagAndInject();

			Assert.IsNull(behaviour.dependency);
        }

        private void SetUpBehaviourWithoutTagAndInject()
        {
            var obj = new GameObject();
            behaviour = obj.AddComponent<FindWithTagTestClass>();

            dependency = new GameObject();

            new GameObjectInjector(behaviour.gameObject).Inject();
        }

        [TearDown]
        public void TearDown()
        {
            if (dependency != null) GameObject.DestroyImmediate(dependency);
            if (behaviour != null) GameObject.DestroyImmediate(behaviour.gameObject);
        }
    }
}

