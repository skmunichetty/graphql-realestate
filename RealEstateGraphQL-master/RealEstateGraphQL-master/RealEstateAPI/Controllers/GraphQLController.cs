using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Conversion;
using GraphQL.Relay.Http;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using RealEstate.API.Models;
using RealEstate.API.Schema;
using RealEstate.Types;

namespace RealEstate.API.Controllers
{
    [Route("[controller]")]
    public class GraphQLController : Controller
    {
        private readonly IDocumentExecuter _documentExecuter;
        private readonly RequestExecutor _executor;
        private readonly ISchema _schema;
        public RealEstateSchema Schema { get; }
        private readonly Swapi _api;

        public GraphQLController(IDocumentExecuter documentExecuter, ISchema schema)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
           
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            var inputs = query?.Variables?.ToInputs();
            var executionOptions = new ExecutionOptions
            {
                Schema = _schema,
                Query = query.Query,
                Inputs = inputs
            };

            var result = await _documentExecuter
                .ExecuteAsync(executionOptions);

            if (result.Errors?.Count > 0)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        //[HttpPost]
        //public async Task<IActionResult> Post()
        //{
        //    var response = await _executor
        //      .ExecuteAsync(Request.Body, Request.ContentType, (_, files) => {
        //          _.Schema = Schema;
        //          _.ExposeExceptions = true;
        //          _.UserContext = new GraphQLContext(_api);

        //          _.Root = new
        //          {
        //              Files = files,
        //          };
        //          _.FieldNameConverter = new CamelCaseFieldNameConverter();
        //      });


        //    return Content(response.Write(), "application/json", Encoding.UTF8);
        //}
    }
}
