using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Syrinj.Attributes;
using Syrinj.Tests.Utility;
using UnityEngine;

namespace Syrinj.Tests.Integration.DependencyInjection
{
    internal class InjectScene
    {
        private const int numInjectees = 100;
        private const string STRING = "test";
        private const int INT = 1237;

        internal class MockProvider : MonoBehaviour
        {
            [Provides] public MockDependency<string> stringProvider = null;
            [Provides] public MockDependency<int> intProvider = null;
        }

        internal class MockInjectee : MonoBehaviour
        {
            [Inject] public MockDependency<string> stringDependent = null;
            [Inject] public MockDependency<int> intDependent = null;
        }

        private MockProvider provider;
        private GameObject[] gameObjects;
        private MockInjectee[] injectees;
        private SceneInjector injector;

        private MockDependency<string> stringDependency;
        private MockDependency<int> intDependency;

        [SetUp]
        public void SetUp()
        {
            DependencyContainer.Instance.Reset();

            gameObjects = new GameObject[numInjectees + 2];
            injectees = new MockInjectee[numInjectees];

            stringDependency = new MockDependency<string>(STRING);
            intDependency = new MockDependency<int>(INT);

            for (int i = 0; i < numInjectees + 1; i++)
            {
                gameObjects[i] = new GameObject();

                if (i == 0)
                {
                    provider = gameObjects[i].AddComponent<MockProvider>();
                    provider.stringProvider = stringDependency;
                    provider.intProvider = intDependency;
                }
                else
                {
                    injectees[i - 1] = gameObjects[i].AddComponent<MockInjectee>();
                }
            }

            injector = provider.gameObject.AddComponent<SceneInjector>();
        }

        [Test]
        public void SceneInjected()
        {
            injector.InjectScene();

            Assert.AreEqual(stringDependency, injectees[7].stringDependent);
            Assert.AreEqual(intDependency, injectees[54].intDependent);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < numInjectees + 1; i++)
            {
                GameObject.DestroyImmediate(gameObjects[i]);
            }
        }
    }
}
