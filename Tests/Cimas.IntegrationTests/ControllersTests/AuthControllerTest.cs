using Cimas.Api.Contracts.Auth;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;

namespace Cimas.IntegrationTests.ControllersTests
{
    public class AuthControllerTest : BaseControllerTest
    {
        private const string _baseUrl = "auth";

        #region RegisterOwner
        [Test]
        public Task AuthController_RegisterOwner_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange

                var requestModel = new RegisterOwnerRequest(
                    "Company #created",
                    "FirstName #created",
                    "LastName #created",
                    "testUserName41",
                    "Qwerty123!"
                );
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/register/owner", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }
        #endregion

        #region Login
        [Test]
        public Task AuthController_Login_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: owner1UserName);

                var requestModel = new LoginRequest(
                    owner1UserName,
                    "Qwerty123!"
                );
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/login", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
        #endregion

        #region RefreshTokens
        [Test]
        public Task AuthController_RefreshTokens_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                string accessToken = await GenerateTokenAndSetAsHeader(username: owner1UserName, setTikenAsHeader: false);
                AddCookieToRequest("refreshToken", owner1FisrtRefreshToken);

                var requestModel = new RefreshTokensRequest(
                    accessToken
                );
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PostAsync($"{_baseUrl}/refresh-tokens", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }
        #endregion
    }
}
