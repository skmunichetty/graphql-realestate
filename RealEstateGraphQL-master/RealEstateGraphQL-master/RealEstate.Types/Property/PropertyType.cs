using System.Linq;
using System.Threading.Tasks;
using GraphQL.Relay.Types;
using GraphQL.Types;
using RealEstate.DataAccess.Repositories.Contracts;
using RealEstate.Database.Models;
using RealEstate.Types.Payment;

namespace RealEstate.Types.Property
{
    public class PropertyType : AsyncNodeGraphType<Database.Models.Property>
    {
        private readonly IPaymentRepository _paymentRepository;
        public PropertyType(IPaymentRepository paymentRepository)
        {
            Field(x => x.Id);
            Field(x => x.Name);
            Field(x => x.Value);
            Field(x => x.City);
            Field(x => x.Family);
            Field(x => x.Street);
            Field<ListGraphType<PaymentType>>("payments",
                arguments: new QueryArguments(new QueryArgument<IntGraphType> {Name = "last"}),
                resolve: context =>
                {
                    var lastItemsFilter = context.GetArgument<int?>("last");
                    return lastItemsFilter != null
                        ? paymentRepository.GetAllForProperty(context.Source.Id, lastItemsFilter.Value)
                        : paymentRepository.GetAllForProperty(context.Source.Id);
                });
        }

        public override Task<Database.Models.Property> GetById(string id)
        {
            return _paymentRepository.GetPropertyById(12);
        }
    }
}