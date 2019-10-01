using GraphQL.Types;
using RealEstate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.API
{
    public class GraphQLContext
    {
        public Swapi Api { get; }
        public GraphQLContext(Swapi api)
        {
            Api = api;
        }
    }

    public static class GraphQLUserContextExtensions
    {
        public static Swapi Api<TSource>(this ResolveFieldContext<TSource> context)
        {
            return ((GraphQLContext)context.UserContext).Api;
        }

        // public static IDataLoader<int, TReturn> GetDataLoader<TSource, TReturn>(
        //     this ResolveFieldContext<TSource> context,
        //     Func<IEnumerable<int>, Task<ILookup<int, TReturn>>> fetchDelegate
        // ) {
        //     return ((GraphQLContext)context.UserContext).LoadContext.GetOrCreateLoader(context.FieldDefinition, fetchDelegate);
        // }
    }
}
