using System.Collections.Generic;
using GraphQL.Types;
using RealEstate.Types.Property;
using RealEstate.DataAccess.Repositories.Contracts;
using System.Threading.Tasks;
using GraphQL.Builders;
using RealEstate.Database.Models;
using System;
using System.Threading;
using GraphQL.Types.Relay.DataObjects;
using System.Linq;
using RealEstate.Types;
using GraphQL.Relay.Types;

namespace RealEstate.API.Queries
{
    public class PropertyQuery : ObjectGraphType
    {
        //private readonly Swapi _api;
        public PropertyQuery(IPropertyRepository propertyRepository, Swapi api)
        {
            Field<ListGraphType<PropertyType>>(
                "properties",
                resolve: context => propertyRepository.GetAll());

            Field<PropertyType>(
                "property",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> { Name = "id" }),
                resolve: context => propertyRepository.GetById(context.GetArgument<int>("id")));


            this.Name = "Query";
            this.Description = "The query type, represents all of the entry points into our object graph.";


            Connection<PropertyType>()
               .Name("properties")
               .Resolve(ctx => propertyRepository.GetAllProperties()
                   .GetConnection(ctx.GetConnectionArguments())
                   .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
               );

            Connection<FilmGraphType>()
                .Name("films")
                .Resolve(ctx => api
                    .GetConnection<Films>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<PeopleGraphType>()
                .Name("people")
                .Resolve(ctx => api
                    .GetConnection<People>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<PlanetGraphType>()
                .Name("planets")
                .Resolve(ctx => api
                    .GetConnection<Planets>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<SpeciesGraphType>()
                .Name("species")
                .Resolve(ctx => api
                    .GetConnection<Species>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<StarshipGraphType>()
                .Name("starships")
                .Resolve(ctx => api
                    .GetConnection<Starships>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<VehicleGraphType>()
                .Name("vehicles")
                .Resolve(ctx => api
                    .GetConnection<Vehicles>(ctx.GetConnectionArguments())
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );
            //Connection<FilmGraphType>()
            //   .Name("films")
            //   .Resolve(ctx => api
            //       .GetConnection<Films>(ctx.GetConnectionArguments())
            //       .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
            //   );

            //this.Connection<PropertyType>()
            //    .Name("propertiesList")
            //   .Unidirectional()
            //   .Resolve(ctx => )
        }       

    }
}