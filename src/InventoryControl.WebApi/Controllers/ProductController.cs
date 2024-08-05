using AutoMapper;
using InventoryControl.Application.Commands.Products;
using InventoryControl.Application.Commands.Stocks;
using InventoryControl.Application.Queries.Products;
using InventoryControl.Application.Queries.Stocks;
using InventoryControl.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace InventoryControl.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Produces(MediaTypeNames.Application.Json)]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IMediator mediator, IMapper mapper, ILogger<ProductController> logger)
        {
            _mediator = mediator;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Gets a product by ID", Description = "Returns the product with the specified ID.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductModel))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductModel>> GetProductById(int id)
        {
            var query = new GetProductByIdQuery { Id = id };
            var product = await _mediator.Send(query);

            if (product == null)
                return NotFound();

            var productModel = _mapper.Map<ProductModel>(product);
            return Ok(productModel);
        }

        [HttpGet]
        [SwaggerOperation(Summary = "Gets all products", Description = "Returns a list of all products.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ProductModel>))]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetAllProducts()
        {
            var query = new GetAllProductsQuery();
            var products = await _mediator.Send(query);
            var productModels = _mapper.Map<IEnumerable<ProductModel>>(products);
            return Ok(productModels);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Creates a new product", Description = "Creates a new product and returns the ID of the created product.")]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        public async Task<ActionResult<int>> CreateProduct([FromBody] ProductModel model)
        {
            var command = _mapper.Map<CreateProductCommand>(model);
            var productId = await _mediator.Send(command);

            var uri = Url.Action("GetProductById", new { id = productId });
            return Created(uri, null);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Updates a product", Description = "Updates the product with the specified ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductModel model)
        {
            if (id != model.Id)
                return BadRequest("Product ID mismatch");

            var command = _mapper.Map<UpdateProductCommand>(model);
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Deletes a product", Description = "Deletes the product with the specified ID.")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var command = new DeleteProductCommand { Id = id };
            var result = await _mediator.Send(command);

            if (!result)
                return NotFound();

            return NoContent();
        }

        [HttpPost("{id}/stock")]
        [SwaggerOperation(Summary = "Adiciona ao estoque do produto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddStock(int id, [FromBody] AddStockModel stockModel)
        {
            if (stockModel.Quantity < 0)
            {
                return BadRequest("Quantity must be a positive value for addition.");
            }

            var command = new StockCommand(id, stockModel.Quantity, stockModel.UnitPrice);
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpPatch("{id}/stock")]
        [SwaggerOperation(Summary = "Remove do estoque do produto")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> RemoveStock(int id, [FromBody] StockModel stockModel)
        {
            if (stockModel.Quantity < 0)
            {
                return BadRequest("Quantity must be a positive value for subtraction.");
            }

            var command = new StockCommand(id, -stockModel.Quantity);
            await _mediator.Send(command);

            return NoContent();
        }

        [HttpGet("{id}/stock")]
        [SwaggerOperation(Summary = "Gets the stock quantity of a product", Description = "Returns the stock quantity of the product with the specified ID.")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<int>> GetProductStock(int id)
        {
            var query = new GetProductStockQuery(id);
            var quantity = await _mediator.Send(query);

            if (quantity == -1)
                return NotFound();

            return Ok(quantity);
        }
    }
}
