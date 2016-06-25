using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Resolvers
{
    public interface IResolver
    {
        object Resolve(Injectable injectable);
    }
}