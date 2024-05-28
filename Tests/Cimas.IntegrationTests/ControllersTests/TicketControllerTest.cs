using Cimas.Api.Contracts.Tickets;
using Cimas.Domain.Entities.Tickets;
using Cimas.IntegrationTests.Extensions;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;

namespace Cimas.IntegrationTests.ControllersTests
{
    public class TicketControllerTest : BaseControllerTest
    {
        private const string _baseUrl = "tickets";

        #region CreateTicket
        [Test]
        public Task TicketController_CreateTicketession_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);
                var requestModel = new CreateTicketsRequest([
                    new (seat3Id, TicketStatus.Booked),
                    new (seat4Id, TicketStatus.Sold)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{session1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public Task TicketController_CreateTicketession_ShouldReturnNotFound_WhenSessionNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);
                var requestModel = new CreateTicketsRequest([
                    new(seat3Id, TicketStatus.Booked),
                    new(seat4Id, TicketStatus.Sold)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{Guid.NewGuid()}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task TicketController_CreateTicketession_ShouldReturnNotFound_WhenSeatIdsNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);
                var requestModel = new CreateTicketsRequest([
                    new(Guid.NewGuid(), TicketStatus.Booked),
                    new(Guid.NewGuid(), TicketStatus.Sold)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{session1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task TicketController_CreateTicketession_ShouldReturnBadRequest_WhenSeatDoesNotBelongToSameHall()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);
                var requestModel = new CreateTicketsRequest([
                    new(seat3Id, TicketStatus.Booked),
                    new(seat5Id, TicketStatus.Sold)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{session2Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        public Task TicketController_CreateTicketession_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);
                var requestModel = new CreateTicketsRequest([
                    new(seat3Id, TicketStatus.Booked),
                    new(seat4Id, TicketStatus.Sold)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{session1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }

        [Test]
        public Task TicketController_CreateTicketession_ShouldReturnBadRequest_WhenTicketsAlreadyExists()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);
                var requestModel = new CreateTicketsRequest([
                    new(seat1Id, TicketStatus.Booked),
                    new(seat2Id, TicketStatus.Sold)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/{session1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }
        #endregion

        #region UpdateTickets
        [Test]
        public Task TicketController_UpdateTickets_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new UpdateTicketsRequest([
                    new (ticket1Id, TicketStatus.Sold),
                    new (ticket2Id, TicketStatus.Booked)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public Task TicketController_UpdateTickets_ShouldReturnNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new UpdateTicketsRequest([
                    new(Guid.NewGuid(), TicketStatus.Sold),
                    new(ticket2Id, TicketStatus.Booked)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task TicketController_UpdateTickets_ShouldReturnBadRequest()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new UpdateTicketsRequest([
                    new(ticket1Id, TicketStatus.Sold),
                    new(ticket3Id, TicketStatus.Booked)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        public Task TicketController_UpdateTickets_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                var requestModel = new UpdateTicketsRequest([
                    new(ticket1Id, TicketStatus.Sold),
                    new(ticket2Id, TicketStatus.Booked)
                ]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region DeleteTicket
        [Test]
        public Task TicketController_DeleteTicket_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new DeleteTicketsRequest([ticket1Id, ticket2Id]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task TicketController_DeleteTicket_ShouldReturnNotFound()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new DeleteTicketsRequest([Guid.NewGuid(), ticket2Id]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task TicketController_DeleteTicket_ShouldReturnBadRequest()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                var requestModel = new DeleteTicketsRequest([ticket1Id, ticket3Id]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        public Task TicketController_DeleteTicket_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                var requestModel = new DeleteTicketsRequest([ticket1Id, ticket2Id]);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion
    }
}
