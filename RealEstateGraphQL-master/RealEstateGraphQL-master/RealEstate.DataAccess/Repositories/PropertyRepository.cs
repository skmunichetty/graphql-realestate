using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RealEstate.DataAccess.Repositories.Contracts;
using RealEstate.Database;
using RealEstate.Database.Models;

namespace RealEstate.DataAccess.Repositories
{
    public class PropertyRepository : BaseRepository<Property>, IPropertyRepository
    {
        private readonly RealEstateContext _db;

        public PropertyRepository(RealEstateContext db): base(db)
        {
            _db = db;
        }

        public IEnumerable<Property> GetAll()
        {
            return _db.Properties;
        }

        public Property GetById(int id)
        {
            return _db.Properties.SingleOrDefault(x => x.Id == id);
        }

        public Property Add(Property property)
        {
            _db.Properties.Add(property);
            _db.SaveChanges();
            return property;
        }

        public IEnumerable<Property> GetAllProperties()
        {
            return _db.Properties.ToList();
        }

        public Task<List<Property>> GetDroids(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<List<Property>> GetDroidsReverse(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetHasNextPage(int? first, DateTime? createdAfter, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetHasPreviousPage(int? last, DateTime? createdBefore, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetTotalCount(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
