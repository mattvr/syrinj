using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Syrinj
{
    public class SceneInjector : MonoBehaviour
    {
        public static SceneInjector Instance;

        void Awake()
        {
            Instance = this;
            DependencyContainer.Instance.Reset();
            InjectScene();

            SceneManager.sceneLoaded += delegate {
                InjectScene ();
            };
        }

        public void InjectScene()
        {
            var behaviours = GetAllBehavioursInScene();

            InjectBehaviours(behaviours);
        }

        private MonoBehaviour[] GetAllBehavioursInScene()
        {
            return GameObject.FindObjectsOfType<MonoBehaviour>();
        }

        private void InjectBehaviours(MonoBehaviour[] behaviours)
        {
            DependencyContainer.Instance.Inject(behaviours);
        }
    }
}
