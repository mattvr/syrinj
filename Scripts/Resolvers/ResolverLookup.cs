using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syrinj.Attributes;

namespace Syrinj.Resolvers
{
    public static class ResolverLookup
    {
        private static readonly Dictionary<Type, IResolver> _resolvers = new Dictionary<Type, IResolver>()
        {
            {typeof (GetComponentAttribute),            new GetComponentResolver()},
            {typeof (GetComponentInChildrenAttribute),  new GetComponentInChildrenResolver()},
            {typeof (FindAttribute),                    new FindResolver()},
            {typeof (FindWithTagAttribute),             new FindWithTagResolver()},
            {typeof (FindObjectOfTypeAttribute),        new FindObjectOfTypeResolver()},

        };

        public static IResolver Get(Type t)
        {
            return _resolvers[t];
        }
    }
}
