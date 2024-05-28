using Cimas.Api.Contracts.Cinemas;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;

namespace Cimas.IntegrationTests.ControllersTests
{
    public class CinemaControllerTest : BaseControllerTest
    {
        private const string _baseUrl = "cinemas";

        #region CreateCinema
        [Test]
        public Task CinemaController_CreateCinema_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                var requestModel = new CreateCinemaRequest("Cinema #created", "created street");
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}", content);

                var cinema = await GetResponseContent<CinemaResponse>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(cinema.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }
        #endregion

        #region GetCinemaById
        [Test]
        public Task CinemaController_GetCinemaById_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/{cinema1Id}");

                var cinema = await GetResponseContent<CinemaResponse>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(cinema.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }

        [Test]
        public Task CinemaController_GetCinemaById_ShouldReturnNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/{Guid.NewGuid()}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task CinemaController_GetCinemaById_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner2UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/{cinema1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region GetAllCinemas
        [Test]
        public Task CinemaController_GetAllCinemas_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}");

                var cinemas = await GetResponseContent<List<CinemaResponse>>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(cinemas.Count, Is.EqualTo(2));
                foreach (var cinema in cinemas)
                    Assert.That(cinema.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }
        #endregion

        #region UpdateCinema
        [Test]
        public Task CinemaController_UpdateCinema_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                var requestModel = new UpdateCinemaRequest("Cinema #updated", "updated street");
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/{cinema1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task CinemaController_UpdateCinema_ShouldReturnNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                var requestModel = new UpdateCinemaRequest("Cinema #updated", "updated street");
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/{Guid.NewGuid()}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task CinemaController_UpdateCinema_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner2UserName);

                var requestModel = new UpdateCinemaRequest("Cinema #updated", "updated street");
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/{cinema1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region DeleteCinema
        [Test]
        public Task CinemaController_DeleteCinema_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{cinema1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task CinemaController_DeleteCinema_ShouldReturnNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{Guid.NewGuid()}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task CinemaController_DeleteCinema_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner2UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{cinema1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion
    }
}
