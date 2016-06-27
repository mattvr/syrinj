using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Syrinj.Injection
{
    public class SceneInjector : MonoBehaviour
    {
        public static SceneInjector Instance;

        void Awake()
        {
            Instance = this;
            InjectScene();
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
            for (int i = 0; i < behaviours.Length; i++)
            {
                new GameObjectInjector(behaviours[i].gameObject).Inject();
            }
        }
    }
}
