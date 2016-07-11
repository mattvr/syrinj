using UnityEngine;
using UnityEditor;
using Syrinj;
using Syrinj.Attributes;
using NUnit.Framework;

namespace Syrinj.Tests.NonMonoBehaviour
{
    internal class NonMonoBehaviourTest
    {
        private const string InstanceTag = "InstanceTag";
        private const string SingletonTag = "SingletonTag";

        public class ProviderTestClass
        {
            [Provides(InstanceTag)] [Instance]
            public MockClass Instance;

            [Provides(SingletonTag)] [Singleton]
            public MockClass Singleton;
        }

        public class InjectableTestClass
        {
            [Inject(InstanceTag)]
            public MockClass InstanceA;

            [Inject(InstanceTag)]
            public MockClass InstanceB;

            [Inject(SingletonTag)]
            public MockClass SingletonA;

            [Inject(SingletonTag)]
            public MockClass SingletonB;
        }

        public class MockClass {
            
        }

        private ProviderTestClass provider;
        private InjectableTestClass injected;


        [SetUp]
        public void SetUp() {
            provider = new ProviderTestClass();
            injected = new InjectableTestClass();

            DependencyContainer.Instance.Inject(provider);
            DependencyContainer.Instance.Inject(injected);
        }

        [Test]
        public void InstanceInjectionsDifferent() {
            Assert.AreNotEqual(injected.InstanceA.GetHashCode(), injected.InstanceB.GetHashCode());
        }

        [Test]
        public void SingletonInjectionsSame() {
            Assert.AreNotEqual(injected.SingletonA.GetHashCode(), injected.SingletonB.GetHashCode());
        }
    }
}

