using Cimas.Api.Contracts.Sessions;
using Cimas.Domain.Models.Sessions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;

namespace Cimas.IntegrationTests.ControllersTests
{
    public class SessionControllerTest : BaseControllerTest
    {
        private const string _baseUrl = "sessions";

        #region CreateSession
        [Test]
        public Task SessionController_CreateSession_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new CreateSessionRequest(hall1Id, film1Id, DateTime.UtcNow.AddDays(1), 69.99m);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task SessionController_CreateSession_ShouldReturnHallNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new CreateSessionRequest(Guid.NewGuid(), film1Id, DateTime.UtcNow.AddDays(1), 69.99m);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task SessionController_CreateSession_ShouldReturnFilmNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new CreateSessionRequest(hall1Id, Guid.NewGuid(), DateTime.UtcNow.AddDays(1), 69.99m);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task SessionController_CreateSession_ShouldReturnBadRequest_WhenFilmNotShownInSameCinemaAsHall()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new CreateSessionRequest(hall1Id, film3Id, DateTime.UtcNow.AddDays(1), 69.99m);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        public Task SessionController_CreateSession_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                var requestModel = new CreateSessionRequest(hall1Id, film1Id, DateTime.UtcNow.AddDays(1), 69.99m);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }

        [Test]
        public Task SessionController_CreateSession_ShouldReturnBadRequest_WhenSessionCollisionDetected()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new CreateSessionRequest(hall1Id, film1Id, DateTime.UtcNow.AddMinutes(5), 69.99m);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }
        #endregion

        #region GetSessionsByRange
        [Test]
        public Task SessionController_GetSessionsByRange_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var req = new GetSessionsByRangeRequest(cinema1Id, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(3));
                string url = $"{_baseUrl}/byRange?CinemaId={req.CinemaId}&FromDateTime={req.FromDateTime}&ToDateTime={req.ToDateTime}";
                
                // Act
                var response = await client.GetAsync(url);
                var sessions = await GetResponseContent<List<SessionResponse>>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(sessions.Count, Is.EqualTo(3));
                foreach (var session in sessions)
                    Assert.That(session.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }

        [Test]
        public Task SessionController_GetSessionsByRange_ShouldReturnNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var req = new GetSessionsByRangeRequest(Guid.NewGuid(), DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(3));
                string url = $"{_baseUrl}/byRange?CinemaId={req.CinemaId}&FromDateTime={req.FromDateTime}&ToDateTime={req.ToDateTime}";

                // Act
                var response = await client.GetAsync(url);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task SessionController_GetSessionsByRange_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                var req = new GetSessionsByRangeRequest(cinema1Id, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(3));
                string url = $"{_baseUrl}/byRange?CinemaId={req.CinemaId}&FromDateTime={req.FromDateTime}&ToDateTime={req.ToDateTime}";

                // Act
                var response = await client.GetAsync(url);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region GetSeatsBySessionId
        [Test]
        public Task SessionController_GetSeatsBySessionId_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/seats/{session1Id}");

                var sessions = await GetResponseContent<List<SessionSeat>>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(sessions.Count, Is.EqualTo(4));
                foreach (var session in sessions)
                    Assert.That(session.TicketId.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }

        [Test]
        public Task SessionController_GetSeatsBySessionId_ShouldReturnNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/seats/{Guid.NewGuid()}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task SessionController_GetSeatsBySessionId_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/seats/{session1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region DeleteSession
        [Test]
        public Task SessionController_DeleteSession_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{session1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task SessionController_DeleteSession_ShouldReturnNotFound()
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
        public Task SessionController_DeleteSession_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{session1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion
    }
}
