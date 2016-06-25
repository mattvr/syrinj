using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syrinj.Attributes;

namespace Syrinj.Providers
{
    public static class ProviderMaps
    {
        public static readonly Dictionary<Type, IProvider> Default = new Dictionary<Type, IProvider>()
        {
            {typeof (GetComponentAttribute),            new GetComponentProvider()},
            {typeof (GetComponentInChildrenAttribute),  new GetComponentInChildrenProvider()},
            {typeof (FindAttribute),                    new FindProvider()},
            {typeof (FindWithTagAttribute),             new FindWithTagProvider()},
            {typeof (FindObjectOfTypeAttribute),        new FindObjectOfTypeProvider()},
        };
    }
}
