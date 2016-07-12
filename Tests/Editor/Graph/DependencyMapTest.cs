using System;
using System.Reflection;
using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Graph;
using Syrinj.Injection;
using Syrinj.Provision;
using Syrinj.Resolvers;
using Syrinj.Tests.Utility;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Syrinj.Tests.Graph
{
    [TestFixture]
    internal class DependencyMapTest
    {
        internal class MockProvider
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
        public void ResolverRegistered()
        {
            var resolver = new GetComponentResolver();
            var injectable = MockInjectableFactory.Create(new GetComponentAttribute());

            map.RegisterResolver(typeof(GetComponentAttribute), resolver);

            Assert.AreEqual(resolver, map.GetResolverForDependency(injectable));
        }

        [Test]
        public void ResolverUnregistered()
        {
            var injectable = MockInjectableFactory.Create(new GetComponentAttribute());

            Assert.Null(map.GetResolverForDependency(injectable));
        }

        [Test]
        public void ProviderRegistered()
        {
            var provider = MockProviderFactory.Create(providerObject.GetType().GetField("audioSourceProvide"), providerObject, null);
            var injectable = MockInjectableFactory.Create(providerObject.GetType().GetField("audioSourceInject"), typeof(AudioSource));

            map.RegisterProvider(typeof(AudioSource), null, provider);

            Assert.AreEqual(provider, map.GetProviderForDependency(injectable));
        }

        [Test]
        public void ProviderUnregistered()
        {
            var injectable = MockInjectableFactory.Create(providerObject.GetType().GetField("audioSourceInject"), typeof(AudioSource));

            Assert.Null(map.GetProviderForDependency(injectable));
        }

        [Test]
        public void ProvidableDependentsRegistered()
        {
            var injectable = MockInjectableFactory.Create();
            map.RegisterProvidableDependent(injectable);

            Assert.AreEqual(injectable, map.UnloadProvidableDependents()[0]);
        }

        [Test]
        public void ResolvableDependentsRegistered()
        {
            var injectable = MockInjectableFactory.Create();
            map.RegisterResolvableDependent(injectable);

            Assert.AreEqual(injectable, map.UnloadResolvableDependents()[0]);
        }

        [Test]
        public void ProvidableDependentsUnloaded()
        {
            var injectable = MockInjectableFactory.Create();
            map.RegisterProvidableDependent(injectable);

            map.UnloadProvidableDependents();
            Assert.AreEqual(0, map.UnloadProvidableDependents().Count);
        }

        [Test]
        public void ResolvableDependentsUnloaded()
        {
            var injectable = MockInjectableFactory.Create();
            map.RegisterResolvableDependent(injectable);

            map.UnloadResolvableDependents();
            Assert.AreEqual(0, map.UnloadResolvableDependents().Count);
        }
    }
}
