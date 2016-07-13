using System;
using Syrinj;
using NUnit.Framework;

namespace Syrinj.Tests.NonMonoBehaviour
{
    internal class ProviderTest
    {
        public class ProvidedClass
        {
            [Inject]
            public Provider<TestClass> TestProvider;
        }

        public class TestClass {
        }

        private ProvidedClass provided;

        [SetUp]
        public void SetUp() {
            DependencyContainer.Instance.Reset();

            provided = new ProvidedClass();
        }

        [Test]
        public void TestProviderInjected() {
            DependencyContainer.Instance.Inject(provided);

            Assert.NotNull(provided.TestProvider);
        }

        [Test]
        public void TestProviderCreated() {
            DependencyContainer.Instance.Inject(provided);

            Assert.NotNull(provided.TestProvider.Get());
        }

        [Test]
        public void TestProviderCreatedMultiple() {
            DependencyContainer.Instance.Inject(provided);

            var objA = provided.TestProvider.Get();
            var objB = provided.TestProvider.Get();

            Assert.NotNull(objA);
            Assert.NotNull(objB);

            Assert.AreNotEqual(objA, objB);
        }
    }
}

