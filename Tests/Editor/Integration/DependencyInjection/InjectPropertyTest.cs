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
    internal class InjectPropertyTest
    {
        private const string TAG = "TEST_TAG";
        private const int NUM = 31337;

        internal class MockPropertyProvider : MonoBehaviour
        {
            [Provides] private MockDependency<string> stringProvider  { get; set; }

            [Provides]
            public int randomNumber
            {
                get { return new Random(seed).Next(); }
            }

            private int seed = 0;

            public void Initialize(MockDependency<string> stringProvider, int seed)
            {
                this.stringProvider = stringProvider;
                this.seed = seed;
            }
        }

        internal class MockInjectProperty : MonoBehaviour
        {
            [Inject] public MockDependency<string> stringDependency { get; set; }
            [Inject] public int randomNumber { get; set; }
        }

        internal class InvalidInjectorProvider : MonoBehaviour {

            [Inject] [Provides]
            public int number
            {
                get;
                set;
            }
        }
        
        private MockPropertyProvider mockPropertyProvider;
        private MockDependency<string> stringDependency;
        private int randomNumber;
        
        private MockInjectProperty mockInjectProperty;

        private GameObject gameObject;

        [SetUp]
        public void SetUp()
        {
            DependencyContainer.Instance.Reset();

            gameObject = new GameObject();

            mockPropertyProvider = gameObject.AddComponent<MockPropertyProvider>();

            mockInjectProperty = gameObject.AddComponent<MockInjectProperty>();
        }

        [Test]
        public void DependencyMet()
        {
            stringDependency = new MockDependency<string>(TAG);
            randomNumber = new Random(NUM).Next();

            mockPropertyProvider.Initialize(stringDependency, NUM);

            new GameObjectInjector(gameObject).Inject();

            Assert.AreEqual(randomNumber, mockInjectProperty.randomNumber);
            Assert.AreEqual(stringDependency, mockInjectProperty.stringDependency);
        }

        [Test]
        public void DependencyNotMet()
        {
            new GameObjectInjector(gameObject).Inject();

            Assert.IsNull(mockInjectProperty.stringDependency);
        }

        [Test]
        [ExpectedException(typeof(InjectionException))]
        public void BothInjectAndProvideIsInvalid() {
            gameObject.AddComponent<InvalidInjectorProvider>();

            new GameObjectInjector(gameObject).Inject();
        }

        [TearDown]
        public void TearDown()
        {
            if (gameObject != null) Object.DestroyImmediate(gameObject);
        }
    }
}

