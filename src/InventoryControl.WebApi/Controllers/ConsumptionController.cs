using InventoryControl.Application.Queries.Consumptions;
using InventoryControl.Domain;
using InventoryControl.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace InventoryControl.WebApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ConsumptionController : ControllerBase
    {
        private readonly IMediator _mediator;


        public ConsumptionController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}/consumption/{date?}")]
        [SwaggerOperation(Summary = "Gets daily consumption for a product", Description = "Returns the consumptions for a specific product and date.")]
        [SwaggerOperationFilter(typeof(OptionalRouteParameterFilter))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Consumption))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Consumption>> GetDailyConsumption(int id, DateTime? date = null)
        {
            var queryDate = date ?? DateTime.Now.Date;
            var query = new GetProductDailyConsumptionQuery(id, queryDate);
            var consumptions = await _mediator.Send(query);

            if (consumptions==null || consumptions?.Id == 0)
                return NotFound();

            return Ok(consumptions);
        }
    }
}
