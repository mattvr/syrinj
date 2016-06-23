using System;

namespace Syrinj.Exceptions
{
    public class InjectionException : Exception
    {
        public InjectionException(string message) : base(message)
        {

        }
    }
}
