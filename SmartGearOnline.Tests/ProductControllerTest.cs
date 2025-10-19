using Xunit;
using Moq;
using SmartGearOnline.Controllers;
using SmartGearOnline.Models;
using SmartGearOnline.Services;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ProductsControllerTests
{
    [Fact]
    public async Task Index_ReturnsViewResult_WithProducts()
    {
        // Mocks
        var mockProductRepo = new Mock<IProductRepository>();
        var mockCategoryRepo = new Mock<ICategoryRepository>();
        var mockLogger = new Mock<ILogger<ProductsController>>();

        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Test Product A" },
            new Product { Id = 2, Name = "Test Product B" }
        };

        mockProductRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(products);

        var controller = new ProductsController(
            mockProductRepo.Object,
            mockCategoryRepo.Object,
            mockLogger.Object
        );

        var result = await controller.Index();

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Product>>(viewResult.Model);
        Assert.Equal(2, ((List<Product>)model).Count);
    }

    [Fact]
    public async Task Index_LogsError_WhenExceptionThrown()
    {
        var mockProductRepo = new Mock<IProductRepository>();
        var mockCategoryRepo = new Mock<ICategoryRepository>();
        var mockLogger = new Mock<ILogger<ProductsController>>();

        //Simulate a failure
        mockProductRepo.Setup(r => r.GetAllAsync()).ThrowsAsync(new Exception("DB error"));

        var controller = new ProductsController(
            mockProductRepo.Object,
            mockCategoryRepo.Object,
            mockLogger.Object
        );

        var result = await controller.Index();

        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);

        mockLogger.Verify(logger =>
            logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) =>
                    v.ToString().Contains("Error retrieving products in Index")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}
