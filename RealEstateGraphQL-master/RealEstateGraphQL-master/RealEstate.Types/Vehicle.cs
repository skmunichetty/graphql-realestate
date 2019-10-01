﻿using GraphQL.Relay.Types;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Types
{
    public class VehicleGraphType : NodeGraphType<Vehicles, Task<Vehicles>>
    {
        private readonly Swapi _api;
        public VehicleGraphType(Swapi api)
        {
            _api = api;

            Name = "Vehicle";

            Field(p => p.Id, nullable: true);
            Field(p => p.Name);
            Field(p => p.Model);
            Field(p => p.Manufacturer);
            Field(p => p.CostInCredits);
            Field(p => p.Length);
            Field(p => p.MaxAtmospheringSpeed);
            Field(p => p.Crew);
            Field(p => p.Passengers);
            Field(p => p.CargoCapacity);
            Field(p => p.VehicleClass);
            Field(p => p.Consumables);

            Connection<PeopleGraphType>()
                .Name("pilots")
                .Unidirectional()
                .Resolve(ctx => api
                    .GetMany<People>(ctx.Source.Pilots)
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx))
                );

            Connection<FilmGraphType>()
                .Name("films")
                .Unidirectional()
                .Resolve(ctx => api
                    .GetMany<Films>(ctx.Source.Films)
                    .ContinueWith(t => ConnectionUtils.ToConnection(t.Result, ctx, 1, 10))
                );
        }

        public override Task<Vehicles> GetById(string id) =>
            _api.GetEntity<Vehicles>(id);

    }
}
