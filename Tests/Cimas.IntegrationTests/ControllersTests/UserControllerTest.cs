using Cimas.Api.Contracts.Users;
using Cimas.Domain.Entities.Users;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;

namespace Cimas.IntegrationTests.ControllersTests
{
    public class UserControllerTest : BaseControllerTest
    {
        private const string _baseUrl = "users";

        #region RegisterNonOwner
        [TestCase(Roles.Worker)]
        [TestCase(Roles.Reviewer)]
        public Task AuthController_RegisterNonOwner_ShouldReturnOk(string role)
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                var requestModel = new RegisterNonOwnerRequest(
                    "FirstName #created",
                    "LastName #created",
                    "testUserName41",
                    "Qwerty123!",
                    role
                );
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/register/nonowner", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }
        #endregion

        #region GetCompanyUsers
        [Test]
        public Task UserController_GetCompanyUsers_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}");

                var sessions = await GetResponseContent<List<UserResponse>>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(sessions.Count, Is.EqualTo(3));
                foreach (var session in sessions)
                    Assert.That(session.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }
        #endregion

        #region FireUser
        [Test]
        public Task UserController_FireUser_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{worker1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task UserController_FireUser_ShouldReturnNotFound()
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
        public Task UserController_FireUser_ShouldReturnForbidden()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner2UserName);

                // Act
                var response = await client.DeleteAsync($"{_baseUrl}/{worker1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion
    }
}
