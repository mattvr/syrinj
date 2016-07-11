using System;
using UnityEngine;

namespace Syrinj.Exceptions
{
    public class InjectionException : Exception
    {
        public InjectionException(object obj, string message) : base(string.Format("[{0}] {1}", obj, message))
        {

        }
    }
}
