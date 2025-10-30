using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Services;
using InterportCargo.DataAccess.Repositories;
using InterportCargo.DataAccess.Data;
using System.Security.Cryptography;
using System.Text;
using Xunit;

namespace InterportCargo.Tests
{
    [Trait("UserStory", "T2")]
    [Collection("E2E-Sqlite")]
    public class T2_CustomerRegistrationTests
    {
        private readonly SqliteTestDb _db;
        public T2_CustomerRegistrationTests(SqliteTestDb db) { _db = db; }
        [Fact]
        public async Task RegisterCustomer_Succeeds_HashesPassword_ActivatesAndPersists()
        {
            using var ctx = _db.CreateContext();
            // Ensure a clean slate for this known email
            var existing = ctx.Customers.Where(c => c.Email == "new@user.com").ToList();
            if (existing.Any()) { ctx.Customers.RemoveRange(existing); ctx.SaveChanges(); }
            var repo = new EFCustomerRepository(ctx);
            var service = new CustomerService(repo);

            var customer = new Customer
            {
                FirstName = "New",
                FamilyName = "User",
                Email = "new@user.com",
                PhoneNumber = "0400000000",
                Address = "1 Test St"
            };

            var result = await service.RegisterCustomerAsync(customer, "Secret123");

            Assert.True(result.IsSuccess);
            var persisted = ctx.Customers.Single(c => c.Email == "new@user.com");
            Assert.True(persisted.IsActive);
            Assert.False(string.IsNullOrWhiteSpace(persisted.PasswordHash));

            // Verify password hashed with SHA256 (as used by AuthenticationService/CustomerService)
            using var sha = SHA256.Create();
            var expectedHash = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes("Secret123")));
            Assert.Equal(expectedHash, persisted.PasswordHash);
        }

        [Fact]
        public async Task RegisterCustomer_Fails_WhenEmailExists()
        {
            using var ctx = _db.CreateContext();
            ctx.Customers.Add(new Customer { FirstName = "X", FamilyName = "Y", Email = "dup@user.com", PhoneNumber = "04", Address = "A", PasswordHash = "h", IsActive = true });
            ctx.SaveChanges();
            var service = new CustomerService(new EFCustomerRepository(ctx));
            var customer = new Customer { FirstName = "Dup", FamilyName = "User", Email = "dup@user.com", PhoneNumber = "04", Address = "A" };

            var result = await service.RegisterCustomerAsync(customer, "Secret123");

            Assert.False(result.IsSuccess);
            Assert.Equal(1, ctx.Customers.Count(c => c.Email == "dup@user.com"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task RegisterCustomer_Fails_WhenPasswordMissing(string? password)
        {
            using var ctx = _db.CreateContext();
            var service = new CustomerService(new EFCustomerRepository(ctx));
            var customer = new Customer { FirstName = "A", FamilyName = "B", Email = "x@y.com", PhoneNumber = "04", Address = "A" };

            var result = await service.RegisterCustomerAsync(customer, password ?? string.Empty);

            Assert.False(result.IsSuccess);
            Assert.Empty(ctx.Customers.Where(c => c.Email == "x@y.com"));
        }
    }
}


