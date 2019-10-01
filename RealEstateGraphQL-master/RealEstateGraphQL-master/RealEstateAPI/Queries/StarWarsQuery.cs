﻿using GraphQL.Relay.Types;
using RealEstate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.API.Queries
{
    public class StarWarsQuery : GraphQL.Relay.Types.QueryGraphType
    {
        public StarWarsQuery(Swapi api)
        {
            Name = "StarWarsQuery";

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
        }
    }
}
