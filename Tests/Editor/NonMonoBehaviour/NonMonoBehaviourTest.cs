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
        private const int Number = 5;

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

        public class AltProviderTestClass {
            [Provides(InstanceTag)] [Instance]
            public AltMockClass Instance;

            [Provides(SingletonTag)] [Singleton]
            public AltMockClass Singleton;
        }

        public class AltInjectableTestClass {
            [Inject(InstanceTag)]
            public AltMockClass Instance;

            [Inject(SingletonTag)]
            public AltMockClass Singleton;
        }

        public class AltMockClass {
            [Inject]
            public float Number;
        }

        public class MockProviderClass {
            [Provides]
            public float ANumber = Number;
        }

        private ProviderTestClass provider;
        private InjectableTestClass injected;

        private AltProviderTestClass altProvider;
        private MockProviderClass mockProvider;
        private AltInjectableTestClass altInjected;

        public void SetUpBasic() {
            provider = new ProviderTestClass();
            injected = new InjectableTestClass();

            DependencyContainer.Instance.Inject(provider);
            DependencyContainer.Instance.Inject(injected);
        }

        [Test]
        public void InstanceInjectionsDifferent() {
            SetUpBasic();
            Assert.AreNotEqual(injected.InstanceA.GetHashCode(), injected.InstanceB.GetHashCode());
        }

        [Test]
        public void SingletonInjectionsSame() {
            SetUpBasic();
            Assert.AreEqual(injected.SingletonA.GetHashCode(), injected.SingletonB.GetHashCode());
        }

        public void SetUpComplex() {
            altProvider = new AltProviderTestClass();
            mockProvider = new MockProviderClass();
            altInjected = new AltInjectableTestClass();

            DependencyContainer.Instance.Inject(altProvider);
            DependencyContainer.Instance.Inject(mockProvider);
            DependencyContainer.Instance.Inject(altInjected);
        }

        [Test]
        public void CheckInjectedDepthTwo() {
            SetUpComplex();

            Assert.AreEqual(Number, altInjected.Instance.Number);
            Assert.AreEqual(Number, altInjected.Singleton.Number);
        }
    }
}

