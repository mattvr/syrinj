using System;
using UnityEngine;

namespace Syrinj.Exceptions
{
    public class DependencyException : Exception
    {
        public DependencyException(MonoBehaviour monoBehaviour, string message) : base(string.Format("[{0}] {1}", monoBehaviour, message))
        {

        }
    }
}
