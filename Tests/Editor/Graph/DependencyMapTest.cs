using System;
using System.Reflection;
using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Graph;
using Syrinj.Injection;
using Syrinj.Providers;
using Syrinj.Resolvers;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Syrinj.Tests.Graph
{
    [TestFixture]
    public class DependencyMapTest
    {
        public class MockProvider
        {
            [Provides] public AudioSource audioSourceProvide;
            [Inject] public AudioSource audioSourceInject;
        }

        private DependencyMap map;
        private MockProvider providerObject;

        [SetUp]
        public void SetUp()
        {
            map = new DependencyMap();
            providerObject = new MockProvider();
        }

        [Test]
        public void TestRegisterResolver()
        {
            var resolver = new GetComponentResolver();
            var injectable = new InjectableField(null, null, null, null, new GetComponentAttribute());

            map.RegisterResolver(typeof(GetComponentAttribute), resolver);

            Assert.AreEqual(resolver, map.GetResolverForDependency(injectable));
        }

        [Test]
        public void TestUnregisteredResolver()
        {
            var injectable = new InjectableField(null, null, null, null, new GetComponentAttribute());

            Assert.Null(map.GetResolverForDependency(injectable));
        }

        [Test]
        public void TestRegisterProvider()
        {
            var provider = ProviderFactory.Create(providerObject.GetType().GetField("audioSourceProvide"), providerObject, null);
            var injectable = InjectableFactory.Create(providerObject.GetType().GetField("audioSourceInject"),
                null, null, null);

            map.RegisterProvider(typeof(AudioSource), null, provider);

            Assert.AreEqual(provider, map.GetProviderForDependency(injectable));
        }

        [Test]
        public void TestUnregisteredProvider()
        {
            var injectable = InjectableFactory.Create(providerObject.GetType().GetField("audioSourceInject"),
                null, null, null);

            Assert.Null(map.GetProviderForDependency(injectable));
        }

        [Test]
        public void TestRegisterProvidableDependents()
        {
            var injectable = new InjectableField(null, null, null, null, null);
            map.RegisterProvidableDependent(injectable);

            Assert.AreEqual(injectable, map.UnloadProvidableDependents()[0]);
        }

        [Test]
        public void TestRegisterResolvableDependents()
        {
            var injectable = new InjectableField(null, null, null, null, null);
            map.RegisterResolvableDependent(injectable);

            Assert.AreEqual(injectable, map.UnloadResolvableDependents()[0]);
        }

        [Test]
        public void TestUnloadProvidableDependents()
        {
            var injectable = new InjectableField(null, null, null, null, null);
            map.RegisterProvidableDependent(injectable);

            map.UnloadProvidableDependents();
            Assert.AreEqual(0, map.UnloadProvidableDependents().Count);
        }

        [Test]
        public void TestUnloadResolvableDependents()
        {
            var injectable = new InjectableField(null, null, null, null, null);
            map.RegisterResolvableDependent(injectable);

            map.UnloadResolvableDependents();
            Assert.AreEqual(0, map.UnloadResolvableDependents().Count);
        }
    }
}
