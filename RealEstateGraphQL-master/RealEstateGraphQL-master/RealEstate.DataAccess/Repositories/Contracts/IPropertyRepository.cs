using RealEstate.Database.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RealEstate.DataAccess.Repositories.Contracts
{
    public interface IPropertyRepository
    {
        IEnumerable<Property> GetAll();
        Property GetById(int id);
        Property Add(Property property);


        //==== Sample Driods methods 
        Task<List<Property>> GetDroids(
        int? first,
        DateTime? createdAfter,
        CancellationToken cancellationToken);

        Task<List<Property>> GetDroidsReverse(
            int? first,
            DateTime? createdAfter,
            CancellationToken cancellationToken);

        Task<bool> GetHasNextPage(
            int? first,
            DateTime? createdAfter,
            CancellationToken cancellationToken);

        Task<bool> GetHasPreviousPage(
            int? last,
            DateTime? createdBefore,
            CancellationToken cancellationToken);

        Task<int> GetTotalCount(CancellationToken cancellationToken);
        IEnumerable<Property> GetAllProperties();
    }
}
