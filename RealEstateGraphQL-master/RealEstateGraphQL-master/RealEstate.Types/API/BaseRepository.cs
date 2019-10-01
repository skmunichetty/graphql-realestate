using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using RealEstate.Database;
using RealEstate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.DataAccess.Repositories
{
    public abstract class BaseRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly RealEstateContext AppDbContext;

        protected BaseRepository(RealEstateContext appDbContext)
        {
            AppDbContext = appDbContext;
        }

        public virtual async Task<T> GetById(int id)
        {
            return await AppDbContext.Set<T>().FindAsync(id);
        }

        public async Task<List<T>> ListAll()
        {
            return await AppDbContext.Set<T>().ToListAsync();
        }

        public async Task<T> GetSingleBySpec(ISpecification<T> spec)
        {
            var result = await List(spec);
            return result.FirstOrDefault();
        }

        public async Task<List<T>> List(ISpecification<T> spec)
        {
            // fetch a Queryable that includes all expression-based includes
            var queryableResultWithIncludes = spec.Includes
                .Aggregate(AppDbContext.Set<T>().AsQueryable(),
                    (current, include) => current.Include(include));

            // modify the IQueryable to include any string-based include statements
            var secondaryResult = spec.IncludeStrings
                .Aggregate(queryableResultWithIncludes,
                    (current, include) => current.Include(include));

            // return the result of the query using the specification's criteria expression
            return await secondaryResult
                            .Where(spec.Criteria)
                            .ToListAsync();
        }


        public async Task<T> Add(T entity)
        {
            AppDbContext.Set<T>().Add(entity);
            await AppDbContext.SaveChangesAsync();
            return entity;
        }

        public async Task Update(T entity)
        {
            AppDbContext.Entry(entity).State = EntityState.Modified;
            await AppDbContext.SaveChangesAsync();
        }

        public async Task Delete(T entity)
        {
            AppDbContext.Set<T>().Remove(entity);
            await AppDbContext.SaveChangesAsync();
        }



        private string GetResource<T>() where T : BaseEntity
        {
            return typeof(T).Name.ToLower();
        }


        private readonly HttpClient _client;

        //private string _apiBase = "http://swapi.co/api";
        private ResponseCache _cache = new ResponseCache();

        public BaseRepository(ResponseCache cache)
        {
            _cache = cache;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
        }

        public Task<T> Fetch<T>(Uri url) where T : BaseEntity
        {
            return _cache.GetOrAdd(url, async u =>
            {
                var result = await _client.GetAsync(u);
                return DeserializeObject<T>(
                    await result.Content.ReadAsStringAsync()
                );
            }).ContinueWith(
                t => (T)t.Result,
                TaskContinuationOptions.OnlyOnRanToCompletion |
                TaskContinuationOptions.ExecuteSynchronously
            );
        }

        public async Task<IEnumerable<T>> FetchMany<T>(IEnumerable<Uri> urls)
            where T : Entity
        {
            var entities = await Task.WhenAll(urls.Select(Fetch<T>));
            return entities.AsEnumerable();
        }

        public async Task<T> GetEntity<T>(string id) where T : BaseEntity
        {
            var name = GetResource<T>();
            var entity = await GetEntity<T>(new Uri($"{_apiBase}/{name}/{id}"));

            return entity;
        }

        public Task<T> GetEntity<T>(Uri url) where T : BaseEntity =>
            Fetch<T>(url);


        public Task<IEnumerable<T>> GetMany<T>(IEnumerable<Uri> urls) where T : Entity =>
            FetchMany<T>(urls);


        private bool DoneFetching(int count, ConnectionArguments args)
        {
            if (args.After != null || args.Before != null || args.Last != null || args.First == null)
                return false;
            return count >= args.First.Value;
        }
        public async Task<List<T>> GetConnection<T>(ConnectionArguments args)
            where T : Entity
        {
            var nextUrl = new Uri($"{_apiBase}/{typeof(T).Name.ToLower()}");
            var entities = new List<T>();
            var canStopEarly =
                args.After != null ||
                args.Before != null ||
                args.Last != null ||
                args.First == null;

            EntityList<T> page;
            while (nextUrl != null && !DoneFetching(entities.Count, args))
            {
                page = await Fetch<EntityList<T>>(nextUrl);
                entities.AddRange(page.Results);
                nextUrl = page.Next;
            }

            return entities;
        }

        private T DeserializeObject<T>(string payload)
        {
            return JsonConvert.DeserializeObject<T>(
                payload,
                new JsonSerializerSettings
                {
                    ContractResolver = new DefaultContractResolver
                    {
                        NamingStrategy = new SnakeCaseNamingStrategy()
                    },
                    Converters = new List<JsonConverter> {
                            new NumberConverter()
                    }
                }
            );
        }

    }
}
