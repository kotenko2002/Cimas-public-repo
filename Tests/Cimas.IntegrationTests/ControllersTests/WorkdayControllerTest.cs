using Cimas.Api.Contracts.Workdays;
using Cimas.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System.Net;

namespace Cimas.IntegrationTests.ControllersTests
{
    public class WorkdayControllerTest : BaseControllerTest
    {
        private const string _baseUrl = "workdays";

        #region StartWorkday
        [Test]
        public Task WorkdayController_StartWorkday_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.PostAsync($"{_baseUrl}/start/{cinema1Id}", null);

                //// Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            });
        }

        [Test]
        public Task WorkdayController_StartWorkday_ShouldReturnBadRequest_WhenUserHasUnfinishedWorkday()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker11UserName);

                // Act
                var response = await client.PostAsync($"{_baseUrl}/start/{cinema1Id}", null);

                //// Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }

        [Test]
        public Task WorkdayController_StartWorkday_ShouldReturnNotFound_WhenCinemaWithSuchIdDoesNotExist()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.PostAsync($"{_baseUrl}/start/{Guid.NewGuid()}", null);

                //// Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task WorkdayController_StartWorkday_ShouldReturn_WhenUserIsFromAnotherCompany()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.PostAsync($"{_baseUrl}/start/{cinema2Id}", null);

                //// Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region GetCurrentWorkday
        [Test]
        public Task WorkdayController_GetCurrentWorkday_ShouldReturnOk_WhenWorkdayExists()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/current");

                var cinema = await GetResponseContent<WorkdayResponse>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(cinema.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }

        [Test]
        public Task WorkdayController_GetCurrentWorkday_ShouldReturnOk_WhenWorkdayDoesNotExist()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/current");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }
        #endregion

        #region FinishWorkday
        [Test]
        public Task WorkdayController_FinishWorkday_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker2UserName);

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/finish", null);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            }, configureServices: service => {
                var fileStorageService = new Mock<IFileStorageService>();

                fileStorageService.Setup(service => service.UploadFileAsync(It.IsAny<MemoryStream>(), "Report", "application/pdf"))
                    .ReturnsAsync("fileId");

                service.AddSingleton(fileStorageService.Object);
            });
        }

        [Test]
        public Task WorkdayController_FinishWorkday_ShouldReturnBadRequest_WhenUserDoesNotHaveUnfinishedWorkday()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: worker1UserName);

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/finish", null);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            });
        }
        #endregion
    }
}
