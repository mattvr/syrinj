using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syrinj.Injection;
using UnityEngine;

namespace Syrinj.Providers
{
    public interface IProvider
    {
        object Provide(Injectable injectable);
    }
}