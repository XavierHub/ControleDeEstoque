using AutoMapper;
using InventoryControl.Application.Commands.Products;
using InventoryControl.Application.Commands.Stocks;
using InventoryControl.Application.Queries.Products;
using InventoryControl.Domain;
using InventoryControl.WebApi.Controllers;
using InventoryControl.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace InventoryUnitTests.Controllers
{
    public class ProductControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<ProductController>> _loggerMock;
        private readonly Mock<IUrlHelper> _urlHelperMock;
        private readonly ProductController _controller;

        public ProductControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<ProductController>>();
            _urlHelperMock = new Mock<IUrlHelper>();

            _controller = new ProductController(_mediatorMock.Object, _mapperMock.Object, _loggerMock.Object)
            {
                Url = _urlHelperMock.Object
            };
        }

        [Fact]
        public async Task GetProductById_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", PartNumber = "123" };
            var productModel = new ProductModel { Id = productId, Name = "Test Product", PartNumber = "123" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(product);
            _mapperMock.Setup(m => m.Map<ProductModel>(It.IsAny<Product>()))
                .Returns(productModel);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<ProductModel>(okResult.Value);
            Assert.Equal(productModel.Id, returnValue.Id);
        }

        [Fact]
        public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetProductByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Product)null);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1, Name = "Test Product 1", PartNumber = "123" } };
            var productModels = new List<ProductModel> { new ProductModel { Id = 1, Name = "Test Product 1", PartNumber = "123" } };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllProductsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(products);
            _mapperMock.Setup(m => m.Map<IEnumerable<ProductModel>>(It.IsAny<IEnumerable<Product>>()))
                .Returns(productModels);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnValue = Assert.IsType<List<ProductModel>>(okResult.Value);
            Assert.Single(returnValue);
        }

        [Fact]
        public async Task CreateProduct_ReturnsCreatedResult_WhenProductIsCreated()
        {
            // Arrange
            var productModel = new ProductModel { Name = "New Product", PartNumber = "123" };
            var createProductCommand = new CreateProductCommand { Name = "New Product", PartNumber = "123" };
            var productId = 1;

            _mapperMock.Setup(m => m.Map<CreateProductCommand>(It.IsAny<ProductModel>()))
                .Returns(createProductCommand);
            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(productId);

            _urlHelperMock.Setup(x => x.Action(It.IsAny<UrlActionContext>()))
                .Returns("http://localhost/api/products/1");

            // Act
            var result = await _controller.CreateProduct(productModel);

            // Assert
            var createdResult = Assert.IsType<CreatedResult>(result.Result);
            Assert.Equal("http://localhost/api/products/1", createdResult.Location);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContent_WhenProductIsUpdated()
        {
            // Arrange
            var productId = 1;
            var productModel = new ProductModel { Id = productId, Name = "Updated Product", PartNumber = "123" };
            var updateProductCommand = new UpdateProductCommand { Id = productId, Name = "Updated Product", PartNumber = "123" };

            _mapperMock.Setup(m => m.Map<UpdateProductCommand>(It.IsAny<ProductModel>()))
                .Returns(updateProductCommand);
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateProduct(productId, productModel);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsBadRequest_WhenProductIdMismatch()
        {
            // Arrange
            var productId = 1;
            var productModel = new ProductModel { Id = 2, Name = "Updated Product", PartNumber = "123" };

            // Act
            var result = await _controller.UpdateProduct(productId, productModel);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Product ID mismatch", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            var productModel = new ProductModel { Id = productId, Name = "Updated Product", PartNumber = "123" };
            var updateProductCommand = new UpdateProductCommand { Id = productId, Name = "Updated Product", PartNumber = "123" };

            _mapperMock.Setup(m => m.Map<UpdateProductCommand>(It.IsAny<ProductModel>()))
                .Returns(updateProductCommand);
            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateProduct(productId, productModel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNoContent_WhenProductIsDeleted()
        {
            // Arrange
            var productId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteProduct_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteProductCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
