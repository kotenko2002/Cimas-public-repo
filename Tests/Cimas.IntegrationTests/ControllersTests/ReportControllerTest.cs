using Cimas.Api.Contracts.Reports;
using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Reports;
using Cimas.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Net;
using System.Text;

namespace Cimas.IntegrationTests.ControllersTests
{
    public class ReportControllerTest : BaseControllerTest
    {
        private const string _baseUrl = "reports";

        #region GetAllCompanyReports
        [Test]
        public Task ReportController_GetAllCompanyReports_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: reviewer2UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}");
                var reports = await GetResponseContent<List<ReportResponse>>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(reports.Count, Is.EqualTo(2));
                foreach (var report in reports)
                    Assert.That(report.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }
        #endregion

        #region GetReportById
        [Test]
        public Task ReportController_GetReportById_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange 
                await GenerateTokenAndSetAsHeader(username: reviewer2UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/{report1Id}");
                var report = await GetResponseContent<ReportResponse>(response);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
                Assert.That(report.Id.ToString(), Is.Not.EqualTo("00000000-0000-0000-0000-000000000000"));
            });
        }

        [Test]
        public Task ReportController_GetReportById_ShouldReturnNotFound_WhenReportWithSuchIdDoesNotExist()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: reviewer2UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/{Guid.NewGuid()}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task ReportController_GetReportById_ShouldReturnForbidden_WhenUserIsFromAnotherCompany()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: reviewer1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/{report1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region GetReportFile
        [Test]
        public Task ReportController_GetReportFile_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: reviewer2UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/file/{report1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            }, configureServices: service => {
                var fileStorageService = new Mock<IFileStorageService>();

                QuestPDF.Settings.License = LicenseType.Community;

                fileStorageService.Setup(service => service.DownloadFileAsync("fileId"))
                    .ReturnsAsync(() =>
                    {
                        var stream = new MemoryStream();

                        Document.Create(container =>
                        {
                            container.Page(page => {});
                        }).GeneratePdf(stream);

                        stream.Seek(0, SeekOrigin.Begin);

                        return new FileDownloadResult()
                        {
                            Stream = stream,
                            FileName = "Report.pdf",
                            ContentType = "application/pdf"
                        };
                    });

                service.AddSingleton(fileStorageService.Object);
            });
        }

        [Test]
        public Task ReportController_GetReportFile_ShouldReturnNotFound_WhenReportWithSuchIdDoesNotExist()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: reviewer2UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/file/{Guid.NewGuid()}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task ReportController_GetReportFile_ShouldReturnForbidden_WhenUserIsFromAnotherCompany()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: reviewer1UserName);

                // Act
                var response = await client.GetAsync($"{_baseUrl}/file/{report1Id}");

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion

        #region UpdateReportStatus
        [Test]
        public Task ReportController_UpdateReportStatus_ShouldReturnOk()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: reviewer2UserName);

                var requestModel = new UpdateReportStatusRequest(ReportStatus.Approved);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/{report1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NoContent));
            });
        }

        [Test]
        public Task ReportController_UpdateReportStatus_ShouldReturnNotFound_WhenReportWithSuchIdDoesNotExist()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: reviewer2UserName);

                var requestModel = new UpdateReportStatusRequest(ReportStatus.Approved);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/{Guid.NewGuid()}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            });
        }

        [Test]
        public Task ReportController_UpdateReportStatus_ShouldReturnForbidden_WhenUserIsFromAnotherCompany()
        {
            return PerformTest(async (client) =>
            {
                // Arrange
                await GenerateTokenAndSetAsHeader(username: reviewer1UserName);

                var requestModel = new UpdateReportStatusRequest(ReportStatus.Approved);
                var content = new StringContent(JsonConvert.SerializeObject(requestModel), Encoding.UTF8, "application/json");

                // Act
                var response = await client.PatchAsync($"{_baseUrl}/{report1Id}", content);

                // Assert
                Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
            });
        }
        #endregion
    }
}
