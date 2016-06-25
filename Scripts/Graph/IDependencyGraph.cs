using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syrinj.Graph;
using Syrinj.Providers;

namespace Assets.Syrinj.Scripts.Graph
{
    public abstract class IDependencyGraph
    {
        public abstract void RegisterProvider(Type binding, Provider provider);

        public abstract object Get(Type key);
    }
}
