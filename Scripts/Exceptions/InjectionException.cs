using System;
using UnityEngine;

namespace Syrinj.Exceptions
{
    public class InjectionException : Exception
    {
        public InjectionException(MonoBehaviour monoBehaviour, string message) : base(string.Format("[{0}] {1}", monoBehaviour, message))
        {

        }
    }
}
