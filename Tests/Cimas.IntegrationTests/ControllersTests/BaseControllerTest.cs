using Cimas.Api;
using Cimas.Infrastructure.Common;
using Cimas.Infrastructure.Auth;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Data;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Cimas.Domain.Entities.Users;
using Cimas.Domain.Entities.Companies;
using Cimas.Domain.Entities.Cinemas;
using Cimas.Domain.Entities.Halls;
using Cimas.Domain.Entities.Films;
using Cimas.Domain.Entities.Sessions;
using Cimas.Domain.Entities.Tickets;
using Cimas.Domain.Entities.WorkDays;
using Cimas.Domain.Entities.Products;
using Cimas.Domain.Entities.Reports;

namespace Cimas.IntegrationTests.ControllersTests
{
    public class BaseControllerTest
    {
        private HttpClient _client;
        private WebApplicationFactory<Program> _factory;

        #region HardcodedInfo
        protected readonly string owner1UserName = "owner1";
        protected readonly string owner2UserName = "owner2";
        protected readonly string worker1UserName = "worker1";
        protected readonly string worker11UserName = "worker11";
        protected readonly string worker2UserName = "worker2";
        protected readonly string reviewer1UserName = "reviewer1";
        protected readonly string reviewer2UserName = "reviewer2";

        protected readonly string owner1FisrtRefreshToken =
            "VbrQY4xob1rIK68PK79rc7tyflGtdlwCfU7IitKKkHYZvi84DijfZcwgT29KLUZzFUdBEL8ybVPCkKPfcXwjNQ==";

        protected readonly Guid cinema1Id = Guid.NewGuid();
        protected readonly Guid cinema2Id = Guid.NewGuid();
        protected readonly Guid cinema3Id = Guid.NewGuid();
        protected readonly Guid hall1Id = Guid.NewGuid();
        protected readonly Guid seat1Id = Guid.NewGuid();
        protected readonly Guid seat2Id = Guid.NewGuid();
        protected readonly Guid seat3Id = Guid.NewGuid();
        protected readonly Guid seat4Id = Guid.NewGuid();
        protected readonly Guid seat5Id = Guid.NewGuid();
        protected readonly Guid film1Id = Guid.NewGuid();
        protected readonly Guid film3Id = Guid.NewGuid();
        protected readonly Guid session1Id = Guid.NewGuid();
        protected readonly Guid session2Id = Guid.NewGuid();
        protected readonly Guid ticket1Id = Guid.NewGuid();
        protected readonly Guid ticket2Id = Guid.NewGuid();
        protected readonly Guid ticket3Id = Guid.NewGuid();
        protected readonly Guid worker1Id = Guid.NewGuid();
        protected readonly Guid worker11Id = Guid.NewGuid();
        protected readonly Guid product1Id = Guid.NewGuid();
        protected readonly Guid product2Id = Guid.NewGuid();
        protected readonly Guid product3Id = Guid.NewGuid();
        protected readonly Guid report1Id = Guid.NewGuid();
        #endregion

        public async Task PerformTest(Func<HttpClient, Task> testFunc, Action<IServiceCollection> configureServices = null)
        {
            _factory = new WebApplicationFactory<Program>().WithWebHostBuilder(builder =>
            {
                builder.ConfigureTestServices(services =>
                {
                    SetUpInMemoryDb(services);
                    configureServices?.Invoke(services);
                });
            });
            await SeedData();
            _client = _factory.CreateClient();

            await testFunc(_client);

            _client.Dispose();
            _factory.Dispose();
        }

        public async Task<string> GenerateTokenAndSetAsHeader(string username, bool setTikenAsHeader = true)
        {
            using var scope = _factory.Services.CreateScope();
            var config = scope.ServiceProvider.GetRequiredService<IOptions<JwtConfig>>().Value;
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();

            User user = await userManager.FindByNameAsync(username);

            var authClaims = new List<Claim>
            {
                new("userId", user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            };
            IList<string> userRoles = await userManager.GetRolesAsync(user);

            authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Secret));

            var accessToken = new JwtSecurityToken(
                issuer: config.ValidIssuer,
                audience: config.ValidAudience,
                expires: DateTime.UtcNow.AddMinutes(config.TokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            string token = new JwtSecurityTokenHandler().WriteToken(accessToken);
            if (setTikenAsHeader)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }

            return token;
        }

        public void AddCookieToRequest(string cookieName, string cookieValue)
        {
            _client.DefaultRequestHeaders.Add("Cookie", $"{cookieName}={cookieValue}");
        }

        public async Task<T> GetResponseContent<T>(HttpResponseMessage response)
        {
            string responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(responseContent);
        }

        private void SetUpInMemoryDb(IServiceCollection services)
        {
            string databaseName = Guid.NewGuid().ToString();

            var dbContextDescriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<CimasDbContext>));
            services.Remove(dbContextDescriptor);
            services.AddDbContext<CimasDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName);
            });
        }

        private async Task SeedData()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<CimasDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            foreach (var role in Roles.GetRoles())
                    await roleManager.CreateAsync(new IdentityRole<Guid>(role));

            Company company1 = new() { Id = Guid.NewGuid(), Name = "Company #1" };
            Company company2 = new() { Id = Guid.NewGuid(), Name = "Company #2" };
            await context.Companies.AddRangeAsync(company1, company2);

            User owner1 = await AddUser(userManager, company1, owner1UserName, Roles.Owner, "O1FirstName", "O1LastName", refreshToken: owner1FisrtRefreshToken);
            User owner2 = await AddUser(userManager, company2, owner2UserName, Roles.Owner, "O2FirstName", "O2LastName");
            User worker1 = await AddUser(userManager, company1, worker1UserName, Roles.Worker, "W1FirstName", "W1LastName", worker1Id);
            User worker11 = await AddUser(userManager, company1, worker11UserName, Roles.Worker, "W11FirstName", "W11LastName", worker11Id);
            User worker2 = await AddUser(userManager, company2, worker2UserName, Roles.Worker, "W2FirstName", "W2LastName");
            User reviewer1 = await AddUser(userManager, company1, reviewer1UserName, Roles.Reviewer, "R1FirstName", "R1LastName");
            User reviewer2 = await AddUser(userManager, company2, reviewer2UserName, Roles.Reviewer, "R2FirstName", "R2LastName");

            Cinema cinema1_c1 = new() { Id = cinema1Id, Company = company1, Name = "Cinema #1", Address = "1 street" };
            Cinema cinema2_c2 = new() { Id = cinema2Id, Company = company2, Name = "Cinema #2", Address = "2 street" };
            Cinema cinema3_c1 = new() { Id = cinema3Id, Company = company1, Name = "Cinema #3", Address = "3 street" };
            await context.Cinemas.AddRangeAsync(cinema1_c1, cinema2_c2, cinema3_c1);

            Product product1 = new() { Id = product1Id, Cinema = cinema1_c1, Name = "Product #1", Price = 100, Amount = 5, SoldAmount = 4, IncomeAmount = 10};
            Product product2 = new() { Id = product2Id, Cinema = cinema1_c1, Name = "Product #2", Price = 50, Amount = 3, SoldAmount = 8, IncomeAmount = 15 };
            Product product3 = new() { Id = product3Id, Cinema = cinema2_c2, Name = "Product #3", Price = 25, Amount = 4, SoldAmount = 7, IncomeAmount = 5 };
            Product product4 = new() { Id = Guid.NewGuid(), Cinema = cinema2_c2, Name = "Product #4", Price = 75, Amount = 6, SoldAmount = 15, IncomeAmount = 20 };
            Product product5 = new() { Id = Guid.NewGuid(), Cinema = cinema2_c2, Name = "Product #5", Price = 125, Amount = 8, SoldAmount = 3, IncomeAmount = 0 };
            await context.Products.AddRangeAsync(product1, product2, product3, product4, product5);

            Hall hall1_c1 = new() { Id = hall1Id, Cinema = cinema1_c1, Name = "Hall #1" };
            Hall hall2_c1 = new() { Id = Guid.NewGuid(), Cinema = cinema1_c1, Name = "Hall #2" };
            Hall hall3_c2 = new() { Id = Guid.NewGuid(), Cinema = cinema2_c2, Name = "Hall #3" };
            await context.Halls.AddRangeAsync(hall1_c1, hall2_c1, hall3_c2);

            HallSeat seat1_c1 = new() { Id = seat1Id, Hall = hall1_c1, Row = 0, Column = 0, Status = HallSeatStatus.Available };
            HallSeat seat2_c1 = new() { Id = seat2Id, Hall = hall1_c1, Row = 0, Column = 1, Status = HallSeatStatus.Available };
            HallSeat seat3_c1 = new() { Id = seat3Id, Hall = hall1_c1, Row = 1, Column = 0, Status = HallSeatStatus.Available };
            HallSeat seat4_c1 = new() { Id = seat4Id, Hall = hall1_c1, Row = 1, Column = 1, Status = HallSeatStatus.Available };
            HallSeat seat5_c2 = new() { Id = seat5Id, Hall = hall3_c2, Row = 0, Column = 0, Status = HallSeatStatus.Available };
            HallSeat seat6_c2 = new() { Id = Guid.NewGuid(), Hall = hall3_c2, Row = 0, Column = 1, Status = HallSeatStatus.Available };
            HallSeat seat7_c2 = new() { Id = Guid.NewGuid(), Hall = hall3_c2, Row = 1, Column = 0, Status = HallSeatStatus.Available };
            HallSeat seat8_c2 = new() { Id = Guid.NewGuid(), Hall = hall3_c2, Row = 1, Column = 1, Status = HallSeatStatus.Available };
            await context.Seats.AddRangeAsync(seat1_c1, seat2_c1, seat3_c1, seat4_c1, seat5_c2, seat6_c2, seat7_c2, seat8_c2);

            Film film1_c1 = new() { Id = film1Id, Cinema = cinema1_c1, Name = "Film #1", Duration = new TimeSpan(1, 0, 0) };
            Film film2_c1 = new() { Id = Guid.NewGuid(), Cinema = cinema1_c1, Name = "Film #2", Duration = new TimeSpan(1, 0, 0), IsDeleted = true };
            Film film3_c2 = new() { Id = film3Id, Cinema = cinema2_c2, Name = "Film #3", Duration = new TimeSpan(1, 0, 0), };
            Film film4_c2 = new() { Id = Guid.NewGuid(), Cinema = cinema2_c2, Name = "Film #4", Duration = new TimeSpan(2, 0, 0), };
            await context.Films.AddRangeAsync(film1_c1, film2_c1, film3_c2);

            Session session1_c1 = new() { Id = session1Id, Film = film1_c1, Hall = hall1_c1, Price = 100, StartDateTime = DateTime.UtcNow };
            Session session2_c1 = new() { Id = session2Id, Film = film2_c1, Hall = hall1_c1, Price = 200, StartDateTime = DateTime.UtcNow.AddMinutes(15) + film1_c1.Duration };
            Session session3_c2 = new() { Id = Guid.NewGuid(), Film = film3_c2, Hall = hall3_c2, Price = 250, StartDateTime = DateTime.UtcNow.AddDays(1) };
            Session session4_c1 = new() { Id = Guid.NewGuid(), Film = film1_c1, Hall = hall1_c1, Price = 125, StartDateTime = DateTime.UtcNow.AddDays(2) };
            Session session5_c1 = new() { Id = Guid.NewGuid(), Film = film2_c1, Hall = hall1_c1, Price = 275, StartDateTime = DateTime.UtcNow.AddDays(3) };
            Session session6_c2 = new() { Id = Guid.NewGuid(), Film = film4_c2, Hall = hall3_c2, Price = 75, StartDateTime = DateTime.UtcNow.AddDays(4) };
            await context.Sessions.AddRangeAsync(session1_c1, session2_c1, session3_c2, session4_c1, session5_c1, session6_c2);

            Ticket ticket1_c1 = new() { Id = ticket1Id, Seat = seat1_c1, Session = session1_c1, Status = TicketStatus.Booked, CreationTime = DateTime.UtcNow };
            Ticket ticket2_c1 = new() { Id = ticket2Id, Seat = seat2_c1, Session = session1_c1, Status = TicketStatus.Sold, CreationTime = DateTime.UtcNow };
            Ticket ticket3_c2 = new() { Id = ticket3Id, Seat = seat5_c2, Session = session3_c2, Status = TicketStatus.Sold, CreationTime = DateTime.UtcNow };
            Ticket ticket4_c2 = new() { Id = Guid.NewGuid(), Seat = seat6_c2, Session = session3_c2, Status = TicketStatus.Sold, CreationTime = DateTime.UtcNow };
            Ticket ticket5_c2 = new() { Id = Guid.NewGuid(), Seat = seat7_c2, Session = session3_c2, Status = TicketStatus.Booked, CreationTime = DateTime.UtcNow };
            Ticket ticket6_c2 = new() { Id = Guid.NewGuid(), Seat = seat5_c2, Session = session6_c2, Status = TicketStatus.Sold, CreationTime = DateTime.UtcNow };
            Ticket ticket7_c2 = new() { Id = Guid.NewGuid(), Seat = seat7_c2, Session = session6_c2, Status = TicketStatus.Sold, CreationTime = DateTime.UtcNow };
            await context.Tickets.AddRangeAsync(ticket1_c1, ticket2_c1, ticket3_c2, ticket4_c2, ticket5_c2, ticket6_c2, ticket7_c2);

            Workday workday1_c2 = new() { Id = Guid.NewGuid(), Cinema = cinema2_c2, User = worker2, StartDateTime = DateTime.UtcNow.AddMinutes(-15) };
            Workday workday2_c2 = new() { Id = Guid.NewGuid(), Cinema = cinema2_c2, User = worker2, StartDateTime = DateTime.UtcNow.AddHours(-50), EndDateTime = DateTime.UtcNow.AddHours(-42) };
            Workday workday3_c2 = new() { Id = Guid.NewGuid(), Cinema = cinema2_c2, User = worker2, StartDateTime = DateTime.UtcNow.AddHours(-28), EndDateTime = DateTime.UtcNow.AddHours(-20) };
            Workday workday4_c1 = new() { Id = Guid.NewGuid(), Cinema = cinema3_c1, User = worker11, StartDateTime = DateTime.UtcNow.AddMinutes(-15) };
            await context.Workdays.AddRangeAsync(workday1_c2, workday2_c2, workday3_c2, workday4_c1);

            Report report1_c2 = new() { Id = report1Id, WorkDay = workday2_c2, Status = ReportStatus.NotReviewed, FileId = "fileId" };
            Report report2_c2 = new() { Id = Guid.NewGuid(), WorkDay = workday3_c2, Status = ReportStatus.NotReviewed, FileId = "fileId" };
            await context.Reports.AddRangeAsync(report1_c2, report2_c2);

            await context.SaveChangesAsync();
        }

        private async Task<User> AddUser(
            UserManager<User> userManager,
            Company company,
            string username,
            string role,
            string firstName,
            string lastName,
            Guid? id = null,
            string refreshToken = null)
        {
            var user = new User()
            {
                Id = id ?? Guid.NewGuid(),
                Company = company,
                FirstName = firstName,
                LastName = lastName,
                UserName = username,
                RefreshToken = refreshToken ?? "refresh_token",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1)
            };

            await userManager.CreateAsync(user, "Qwerty123!");
            await userManager.AddToRoleAsync(user, role);

            return user;
        }
    }
}
