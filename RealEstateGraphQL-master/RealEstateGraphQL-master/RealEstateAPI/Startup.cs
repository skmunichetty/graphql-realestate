using GraphQL;
using GraphQL.Relay.Types;
using GraphQL.Server.Ui.Playground;
using GraphQL.Types;
using GraphQL.Types.Relay;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstate.API.Models;
using RealEstate.API.Mutations;
using RealEstate.API.Queries;
using RealEstate.API.Schema;
using RealEstate.DataAccess.Repositories;
using RealEstate.DataAccess.Repositories.Contracts;
using RealEstate.Database;
using RealEstate.Types;
using RealEstate.Types.Payment;
using RealEstate.Types.Property;
using System;

namespace RealEstate.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddTransient<IPropertyRepository, PropertyRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();

            
            services.AddDbContext<RealEstateContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Default"]));
            services.AddSingleton<IDocumentExecuter, DocumentExecuter>();
            services.AddScoped<GraphQL.Relay.Http.RequestExecutor>();


            services.AddSingleton<PropertyQuery>();
            services.AddSingleton<PropertyMutation>();
            services.AddSingleton<PropertyType>();
            services.AddSingleton<PropertyInputType>();
            services.AddSingleton<PaymentType>();

            services.AddTransient(typeof(ConnectionType<>));
            services.AddTransient(typeof(EdgeType<>));
            //services.AddTransient<NodeInterface>();
            services.AddTransient<PageInfoType>();

            services.AddTransient<EntityInterface>();

            services.AddScoped<Swapi>();
            services.AddSingleton<ResponseCache>();
            services.AddScoped<FilmGraphType>();
            services.AddScoped<PeopleGraphType>();
            services.AddScoped<PlanetGraphType>();
            services.AddScoped<SpeciesGraphType>();
            services.AddScoped<StarshipGraphType>();
            services.AddScoped<VehicleGraphType>();

            var sp = services.BuildServiceProvider();
            //services.AddScoped<RealEstateSchema>(_ =>
            //    new RealEstateSchema(graphType => {
            //        var t = _.GetService(graphType);
            //        return t ?? Activator.CreateInstance(graphType);
            //    })
            //);
            services.AddSingleton<ISchema>(new RealEstateSchema(new FuncDependencyResolver(type => sp.GetService(type))));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RealEstateContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions
            {
                Path ="/ui/playground"
            });
            app.UseGraphiQl();
            app.UseMvc();
            db.EnsureSeedData();
        }
    }
}
