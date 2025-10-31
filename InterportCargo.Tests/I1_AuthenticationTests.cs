using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Services;
using InterportCargo.DataAccess.Repositories;
using InterportCargo.DataAccess.Data;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace InterportCargo.Tests
{
    [Trait("UserStory", "I1")]
    [Collection("E2E-Sqlite")]
    public class I1_AuthenticationTests
    {
        private readonly SqliteTestDb _db;
        public I1_AuthenticationTests(SqliteTestDb db) { _db = db; }
        [Fact]
        public void Correct_Credentials_Succeeds()
        {
            var email = "new@user.com";
            var password = "Secret123";
            var hash = Hash(password);

            using var ctx = _db.CreateContext();
            if (!ctx.Customers.Any(c => c.Email == email))
            {
                ctx.Customers.Add(new Customer { Email = email, FirstName = "A", FamilyName = "B", PasswordHash = hash, IsActive = true });
                ctx.SaveChanges();
            }

            var service = new AuthenticationService(new EFCustomerRepository(ctx), new EFEmployeeRepository(ctx));
            var result = service.AuthenticateUser(email, password);

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public void Wrong_Credentials_Returns_Error()
        {
            var email = "user2@x.com";
            using var ctx = _db.CreateContext();
            ctx.Customers.Add(new Customer { Email = email, FirstName = "A", FamilyName = "B", PasswordHash = Hash("correct"), IsActive = true });
            ctx.SaveChanges();

            var service = new AuthenticationService(new EFCustomerRepository(ctx), new EFEmployeeRepository(ctx));
            var result = service.AuthenticateUser(email, "wrong");

            Assert.False(result.IsSuccess);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData("", "password")]
        [InlineData("user@x.com", " ")]
        public void Empty_Inputs_Returns_Failure(string? email, string? password)
        {
            using var ctx = _db.CreateContext();
            var svc = new AuthenticationService(new EFCustomerRepository(ctx), new EFEmployeeRepository(ctx));
            var result = svc.AuthenticateUser(email ?? string.Empty, password ?? string.Empty);
            Assert.False(result.IsSuccess);
        }

        private static string Hash(string password)
        {
            using var sha = SHA256.Create();
            return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
        }
    }
}


