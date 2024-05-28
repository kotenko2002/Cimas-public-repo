using Cimas.Api.Contracts.Products;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;

namespace Cimas.IntegrationTests.ControllersTests
{
    public class ProductControllerTest : BaseControllerTest
    {
        private const string _baseUrl = "products";

        #region CreateProduct
        [Test]
        public Task ProductController_CreateProduct_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new CreateProductRequest("ProductName #created", 80.99m);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{cinema1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task ProductController_CreateProduct_ShouldReturnNotFound_WhenCinemaWithSuchIdDoesNotExist()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new CreateProductRequest("ProductName #created", 80.99m);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{Guid.NewGuid()}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task ProductController_CreateProduct_ShouldReturnForbidden_WhenUserIsFromAnotherCompany()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                var requestModel = new CreateProductRequest("ProductName #created", 80.99m);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{cinema1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region GetProductsByCinemaId
        [Test]
        public Task ProductController_GetProductsByCinemaId_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/{cinema1Id}");
                var products = await GetResponseContent<List<ProductResponse>>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(products.Count, Is.EqualTo(2));
                foreach (var product in products)
                    Assert.That(product.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }

        [Test]
        public Task ProductController_GetProductsByCinemaId_ShouldReturnNotFound_WhenCinemaWithSuchIdDoesNotExist()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/{Guid.NewGuid()}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task ProductController_GetProductsByCinemaId_ShouldReturnForbidden_WhenUserIsFromAnotherCompany()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/{cinema1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region UpateProducts
        [Test]
        public Task HallController_UpdateHallSeats_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new UpdateProductsRequest([
                    new (product1Id, "Product #1 updated", 79.99m, 5, 5, 5),
                    new (product2Id, "Product #2 updated", 69.99m, 3, 3, 3),
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task HallController_UpdateHallSeats_ShouldReturnNotFound_WhenProductWithSuchIdDoesNotExist()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new UpdateProductsRequest([
                    new(product1Id, "Product #1 updated", 79.99m, 5, 5, 5),
                    new(Guid.NewGuid(), "Product #2 updated", 69.99m, 3, 3, 3),
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task HallController_UpdateHallSeats_ShouldReturnBadRequest_ProductsAreFromDifferentCinemas()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new UpdateProductsRequest([
                    new(product1Id, "Product #1 updated", 79.99m, 5, 5, 5),
                    new(product3Id, "Product #3 updated", 69.99m, 3, 3, 3),
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        public Task HallController_UpdateHallSeats_ShouldReturnForbidden_WhenUserIsFromAnotherCompany()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                var requestModel = new UpdateProductsRequest([
                    new(product1Id, "Product #1 updated", 79.99m, 5, 5, 5),
                    new(product2Id, "Product #2 updated", 69.99m, 3, 3, 3),
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region DeleteProduct
        [Test]
        public Task ProductController_DeleteProduct_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{product1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task ProductController_DeleteProduct_ShouldReturnNotFound_WhenProductWithSuchIdDoesNotExist()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{Guid.NewGuid()}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task ProductController_DeleteProduct_ShouldReturnForbidden_WhenUserIsFromAnotherCompany()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{product1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion
    }
}
