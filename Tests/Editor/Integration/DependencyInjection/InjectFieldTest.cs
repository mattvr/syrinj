using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Injection;
using Syrinj.Tests.Utility;
using UnityEngine;
using Random = System.Random;

namespace Syrinj.Tests.Integration.DependencyInjection
{
    [TestFixture]
    internal class InjectFieldTest
    {
        private const string TAG = "TEST_TAG";
        private const int NUM = 31337;

        internal class MockFieldProvider : MonoBehaviour
        {
            [Provides] public MockDependency<string> stringProvider = null;
            [Provides] public MockDependency<int> intProvider = null;
        }

        internal class MockInjectField : MonoBehaviour
        {
            [Inject] public MockDependency<string> stringField;
            [Inject] public MockDependency<int> intField;
        }

        private MockFieldProvider mockFieldProvider;
        private MockDependency<string> stringDependency;
        private MockDependency<int> intDependency;
        
        private MockInjectField mockInjectField;

        private GameObject gameObject;

        [SetUp]
        public void SetUp()
        {
            DependencyContainer.Instance.Reset();

            gameObject = new GameObject();

            mockFieldProvider = gameObject.AddComponent<MockFieldProvider>();

            mockInjectField = gameObject.AddComponent<MockInjectField>();
        }

        [Test]
        public void DependenciesMet()
        {
            stringDependency = mockFieldProvider.stringProvider = new MockDependency<string>(TAG);
            intDependency = mockFieldProvider.intProvider = new MockDependency<int>(NUM);
            
            new GameObjectInjector(gameObject).Inject();

            Assert.AreEqual(intDependency, mockInjectField.intField);
            Assert.AreEqual(stringDependency, mockInjectField.stringField);
        }

        [Test]
        public void DependenciesNotMet()
        {
            new GameObjectInjector(gameObject).Inject();

            Assert.IsNull(mockInjectField.intField);
            Assert.IsNull(mockInjectField.stringField);
        }

        [TearDown]
        public void TearDown()
        {
            if (gameObject != null) Object.DestroyImmediate(gameObject);
        }
    }
}

