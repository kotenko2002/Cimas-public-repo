using Cimas.Api.Contracts.Halls;
using Cimas.Domain.Entities.Halls;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;

namespace Cimas.IntegrationTests.ControllersTests
{
    public class HallControllerTest : BaseControllerTest
    {
        private const string _baseUrl = "halls";

        #region CreateHall
        [Test]
        public Task HallController_CreateHall_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                var requestModel = new CreateHallRequest("Hall #created", 5, 5);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{cinema1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task HallController_CreateHall_ShouldReturnNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                var requestModel = new CreateHallRequest("Hall #created", 5, 5);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{Guid.NewGuid()}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task HallController_CreateHall_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner2UserName);

                var requestModel = new CreateHallRequest("Hall #created", 5, 5);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{cinema1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region GetHallsByCinemaId
        [Test]
        public Task HallController_GetHallsByCinemaId_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/{cinema1Id}");

                var halls = await GetResponseContent<List<HallResponse>>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(halls.Count, Is.EqualTo(2));
                foreach (var hall in halls)
                    Assert.That(hall.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }

        [Test]
        public Task HallController_GetHallsByCinemaId_ShouldReturnNotFound()
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
        public Task HallController_GetHallsByCinemaId_ShouldReturnForbidden()
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

        #region GetSeatsByHallId
        [Test]
        public Task HallController_GetSeatsByHallId_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/seats/{hall1Id}");

                var seats = await GetResponseContent<List<SeatResponse>>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(seats.Count, Is.EqualTo(4));
                foreach (var seat in seats)
                    Assert.That(seat.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }

        [Test]
        public Task HallController_GetSeatsByHallId_ShouldReturnNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/seats/{Guid.NewGuid()}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task HallController_GetSeatsByHallId_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner2UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/seats/{hall1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region UpdateHallSeats
        [Test]
        public Task HallController_UpdateHallSeats_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                var requestModel = new UpdateHallSeatsRequst([
                    new (seat1Id, HallSeatStatus.NotExists),
                    new (seat2Id, HallSeatStatus.Unavailable)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/{hall1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task HallController_UpdateHallSeats_ShouldReturnHallNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                var requestModel = new UpdateHallSeatsRequst([
                    new (seat1Id, HallSeatStatus.NotExists),
                    new (seat2Id, HallSeatStatus.Unavailable)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/{Guid.NewGuid()}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task HallController_UpdateHallSeats_ShouldReturnSeatNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                var requestModel = new UpdateHallSeatsRequst([
                    new (Guid.NewGuid(), HallSeatStatus.NotExists),
                    new (Guid.NewGuid(), HallSeatStatus.Unavailable)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/{hall1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task HallController_UpdateHallSeats_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner2UserName);

                var requestModel = new UpdateHallSeatsRequst([
                    new (seat1Id, HallSeatStatus.NotExists),
                    new (seat2Id, HallSeatStatus.Unavailable)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/{hall1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region DeleteHall
        [Test]
        public Task HallController_DeleteHall_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{hall1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task HallController_DeleteHall_ShouldReturnNotFound()
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
        public Task HallController_DeleteHall_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner2UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{hall1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion
    }
}
