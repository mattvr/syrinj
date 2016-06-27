using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Syrinj.Attributes;

namespace Syrinj.Resolvers
{
    public static class ResolverGroups
    {
        public static readonly Dictionary<Type, IResolver> Default = new Dictionary<Type, IResolver>()
        {
            {typeof (GetComponentAttribute),            new GetComponentResolver()},
            {typeof (GetComponentInChildrenAttribute),  new GetComponentInChildrenResolver()},
            {typeof (FindAttribute),                    new FindResolver()},
            {typeof (FindWithTagAttribute),             new FindWithTagResolver()},
            {typeof (FindObjectOfTypeAttribute),        new FindObjectOfTypeResolver()},
        };

        public static readonly Dictionary<Type, IResolver> GetComponentResolvers = new Dictionary<Type, IResolver>()
        {
            {typeof (GetComponentAttribute),            new GetComponentResolver()},
            {typeof (GetComponentInChildrenAttribute),  new GetComponentInChildrenResolver()},
        };

        public static readonly Dictionary<Type, IResolver> Empty = new Dictionary<Type, IResolver>();
    }
}
