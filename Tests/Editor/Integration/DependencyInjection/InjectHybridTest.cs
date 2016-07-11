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
    internal class InjectHybridTest
    {
        private const string TAG = "TEST_TAG";
        private const int NUM = 31337;

        internal class MockHybridProvider : MonoBehaviour
        {
            [Provides] protected MockDependency<string> stringProvider;
            [Provides] private MockDependency<int> intProvider { get; set; }

            public void Initialize(MockDependency<string> stringProvider, MockDependency<int> intProvider)
            {
                this.stringProvider = stringProvider;
                this.intProvider = intProvider;
            }
        }

        internal class MockInjectHybrid : MonoBehaviour
        {
            [Inject] public MockDependency<string> stringField;
            [Inject] public MockDependency<int> intField { get; set; }
        }

        private MockHybridProvider mockHybridProvider;
        private MockDependency<string> stringDependency;
        private MockDependency<int> intDependency;
        
        private MockInjectHybrid mockInjectHybrid;

        private GameObject gameObject;

        [SetUp]
        public void SetUp()
        {
            DependencyContainer.Instance.Reset();

            gameObject = new GameObject();
            
            mockHybridProvider = gameObject.AddComponent<MockHybridProvider>();
            
            mockInjectHybrid = gameObject.AddComponent<MockInjectHybrid>();
        }


        [Test]
        public void TestDependenciesMet()
        {
            stringDependency = new MockDependency<string>(TAG);
            intDependency = new MockDependency<int>(NUM);

            mockHybridProvider.Initialize(stringDependency, intDependency);

            new GameObjectInjector(gameObject).Inject();

            Assert.AreEqual(intDependency, mockInjectHybrid.intField);
            Assert.AreEqual(stringDependency, mockInjectHybrid.stringField);
        }

        [Test]
        public void TestDependenciesNotMet()
        {
            new GameObjectInjector(gameObject).Inject();

            Assert.IsNull(mockInjectHybrid.intField);
            Assert.IsNull(mockInjectHybrid.stringField);
        }

        [TearDown]
        public void TearDown()
        {
            if (gameObject != null) Object.DestroyImmediate(gameObject);
        }
    }
}

