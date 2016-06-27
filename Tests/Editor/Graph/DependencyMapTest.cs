using System;
using System.Reflection;
using NUnit.Framework;
using Syrinj.Graph;
using Syrinj.Providers;
using UnityEngine;

namespace Syrinj.Tests.Graph
{
    [TestFixture]
    public class DependencyMapTest
    {
        /*
        public class MockClass
        {
            public AudioSource AudioSourceA;
            public AudioSource AudioSourceB { get; set; }
            public BoxCollider BoxCollider;
            public Collider Collider { get; set; }
        }

        private const string TAG = "TEST_TAG";
        
        private DependencyMap map;
        private Provider providerA;
        private Provider providerB;
        private Provider providerC;

        [SetUp]
        public void Setup()
        {
            var mockClass = new MockClass();
            var audioSourceA = mockClass.GetType().GetField("AudioSourceA");
            var audioSourceB = mockClass.GetType().GetProperty("AudioSourceB");
            var boxCollider = mockClass.GetType().GetField("BoxCollider");

            providerA = new ProviderField(audioSourceA, mockClass);
            providerB = new ProviderProperty(audioSourceB, mockClass);
            providerC = new ProviderField(boxCollider, mockClass);

            map = new DependencyMap();
            map.RegisterProvider(typeof(AudioSource), providerA);
            map.RegisterProvider(typeof(AudioSource), TAG, providerB);

            map.RegisterProvider(typeof(BoxCollider), providerC);
            map.RegisterProvider(typeof(Collider), providerB);
        }

        [Test]
        public void TestProviderRegistered()
        {
            Assert.NotNull(map.Get(typeof(AudioSource)));
            Assert.AreEqual(providerA, map.Get(typeof(AudioSource)));
        }

        [Test]
        public void TestProviderNotRegistered()
        {
            Assert.IsNull(map.Get(typeof(Camera)));
        }

        [Test]
        public void TestTaggedProviderRegistered()
        {
            Assert.NotNull(map.Get(typeof(AudioSource), TAG));
            Assert.AreEqual(providerB, map.Get(typeof (AudioSource), TAG));
        }

        [Test]
        public void TestTaggedProviderNotRegistered()
        {
            Assert.IsNull(map.Get(typeof(AudioSource), TAG + "_SUFFIX"));
        }

        [Test]
        public void TestTaggedProviderDifferentThanUntagged()
        {
            Assert.AreNotEqual(map.Get(typeof(AudioSource)), map.Get(typeof(AudioSource), TAG));
        }

        [Test]
        public void TestRegisteredSubTypeDifferent()
        {
            Assert.AreNotEqual(map.Get(typeof(BoxCollider)), map.Get(typeof(Collider)));
        }
        */
    }
}
