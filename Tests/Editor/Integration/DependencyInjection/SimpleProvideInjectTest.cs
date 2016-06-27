using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Exceptions;
using Syrinj.Injection;
using UnityEngine;
using Random = System.Random;

namespace Syrinj.Tests.Integration
{
    [TestFixture]
    internal class SimpleProvideInjectTest
    {
        internal class MockFieldProvider : MonoBehaviour
        {
            [Provides] public AudioSource audioSource;
            [Provides] public BoxCollider boxCollider;
        }

        internal class MockPropertyProvider : MonoBehaviour
        {
            [Provides] private AudioSource audioSource { get; set; }

            [Provides]
            public int randomNumber
            {
                get { return new Random(seed).Next(); }
            }

            private int seed = 0;

            public void Initialize(AudioSource audioSource, int seed)
            {
                this.audioSource = audioSource;
                this.seed = seed;
            }
        }

        internal class MockHybridProvider : MonoBehaviour
        {
            [Provides] protected AudioSource audioSource;
            [Provides] private BoxCollider boxCollider { get; set; }

            public void Initialize(AudioSource audioSource, BoxCollider boxCollider)
            {
                this.audioSource = audioSource;
                this.boxCollider = boxCollider;
            }
        }

        internal class MockInjectField : MonoBehaviour
        {
            [Inject] public AudioSource audioSource;
            [Inject] public BoxCollider boxCollider;
        }

        internal class MockInjectProperty : MonoBehaviour
        {
            [Inject] public AudioSource audioSource { get; set; }
        }

        [SetUp]
        public void SetUp()
        {
            
        }
    }
}

