using GraphQL;
using GraphQL.Types;
using RealEstate.API.Mutations;
using RealEstate.API.Queries;
using RealEstate.Types;
using System;

namespace RealEstate.API.Schema
{
    public class RealEstateSchema : GraphQL.Types.Schema
    {
        public RealEstateSchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<PropertyQuery>();
            Mutation = resolver.Resolve<PropertyMutation>();

            RegisterType<FilmGraphType>();
            //RegisterType<PeopleGraphType>();
            //RegisterType<PlanetGraphType>();
            //RegisterType<SpeciesGraphType>();
            //RegisterType<StarshipGraphType>();
            //RegisterType<VehicleGraphType>();
        }

        //public RealEstateSchema(Func<Type, object> resolveType)
        //    : base(type => (GraphType)resolveType(type))
        //{
        //    var obj = resolveType(typeof(StarWarsQuery));
        //    Query = obj as StarWarsQuery;

        //    RegisterType<FilmGraphType>();
        //    RegisterType<PeopleGraphType>();
        //    RegisterType<PlanetGraphType>();
        //    RegisterType<SpeciesGraphType>();
        //    RegisterType<StarshipGraphType>();
        //    RegisterType<VehicleGraphType>();
        //}
    }
}
