using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Syrinj;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Tests.Utility;
using UnityEngine;
using Object = System.Object;

namespace Syrinj.Tests.Integration.DependencyInjection
{
    internal class InjectTagged
    {
        private const string TAG = "TEST_TAG_A";
        private const string TAG_B = "TEST_TAG_B";

        private const string STR = "abc";
        private const string STR_B = "def";
        private const string STR_C = "123";

        internal class MockTaggedProvider : MonoBehaviour
        {
            [Provides]
            public MockDependency<string> untaggedProvider;

            [Provides(TAG)]
            public MockDependency<string> taggedProviderA;

            [Provides(TAG_B)]
            public MockDependency<string> taggedProviderB;
        }

        internal class MockInjectTagged : MonoBehaviour
        {
            [Inject]
            public MockDependency<string> UntaggedDependency;

            [Inject(TAG)]
            public MockDependency<string> TaggedDependencyA;

            [Inject(TAG_B)]
            public MockDependency<string> TaggedDependencyB;
        }

        private MockTaggedProvider mockProvider;
        private MockDependency<string> untaggedDependency;
        private MockDependency<string> taggedDependencyA;
        private MockDependency<string> taggedDependencyB;

        private MockInjectTagged mockInjected;

        private GameObject gameObject;

        [SetUp]
        public void SetUp()
        {
            DependencyContainer.Instance.Reset();

            gameObject = new GameObject();

            mockProvider = gameObject.AddComponent<MockTaggedProvider>();
            mockInjected = gameObject.AddComponent<MockInjectTagged>();

            untaggedDependency = new MockDependency<string>(STR);
            taggedDependencyA = new MockDependency<string>(STR_B);
            taggedDependencyB = new MockDependency<string>(STR_C);
        }

        [Test]
        public void TestTaggedDependenciesMet()
        {
            mockProvider.untaggedProvider = untaggedDependency;
            mockProvider.taggedProviderA = taggedDependencyA;
            mockProvider.taggedProviderB = taggedDependencyB;

            new GameObjectInjector(gameObject).Inject();

            Assert.AreEqual(untaggedDependency, mockInjected.UntaggedDependency);
            Assert.AreEqual(taggedDependencyA, mockInjected.TaggedDependencyA);
            Assert.AreEqual(taggedDependencyB, mockInjected.TaggedDependencyB);
        }

        [Test]
        [ExpectedException(typeof(InjectionException))]
        public void TestSomeTaggedDependenciesNotMet()
        {
            mockProvider.untaggedProvider = untaggedDependency;
            mockProvider.taggedProviderB = taggedDependencyB;

            new GameObjectInjector(gameObject).Inject();
        }

        [Test]
        [ExpectedException(typeof(InjectionException))]
        public void TestTaggedDependenciesNotMet()
        {
            new GameObjectInjector(gameObject).Inject();
        }

        [TearDown]
        public void TearDown()
        {
            if (gameObject != null) GameObject.DestroyImmediate(gameObject);
        }
    }
}
