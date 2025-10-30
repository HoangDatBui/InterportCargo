using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Services;
using InterportCargo.DataAccess.Repositories;
using InterportCargo.DataAccess.Data;
using Xunit;

namespace InterportCargo.Tests
{
    [Trait("UserStory", "T3")]
    [Collection("E2E-Sqlite")]
    public class T3_EmployeeRegistrationTests
    {
        private readonly SqliteTestDb _db;
        public T3_EmployeeRegistrationTests(SqliteTestDb db) { _db = db; }
        [Fact]
        public async Task Create_QuotationOfficer_Succeeds()
        {
            using var ctx = _db.CreateContext();
            var repo = new EFEmployeeRepository(ctx);
            var service = new EmployeeService(repo);

            var employee = new Employee
            {
                FirstName = "Quo",
                FamilyName = "Officer",
                Email = "officer@x.com",
                EmployeeType = "QuotationOfficer",
                PhoneNumber = "04",
                Address = "A"
            };

            var result = await service.RegisterEmployeeAsync(employee, "Secret123");

            Assert.True(result.IsSuccess);
            var saved = ctx.Employees.Single(e => e.Email == "officer@x.com");
            Assert.True(saved.IsActive);
            Assert.Equal("QuotationOfficer", saved.EmployeeType);
        }
    }
}


