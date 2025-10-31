using InterportCargo.Application.Services;
using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Interfaces;
using InterportCargo.BusinessLogic.Services;
using InterportCargo.DataAccess.Repositories;
using InterportCargo.DataAccess.Data;
using Moq;
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

        [Theory]
        [InlineData(" ")]
        [InlineData("bad-email")]
        public async Task RegisterEmployee_AppService_Fails_On_Invalid_Email(string email)
        {
            var svc = new EmployeeAppService(Mock.Of<IEmployeeService>());
            var result = await svc.RegisterEmployeeAsync("A", "B", email, "04", "QuotationOfficer", "Addr", "Secret123");
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task RegisterEmployee_AppService_Fails_On_Blank_EmployeeType()
        {
            var svc = new EmployeeAppService(Mock.Of<IEmployeeService>());
            var result = await svc.RegisterEmployeeAsync("A", "B", "x@y.com", "04", " ", "Addr", "Secret123");
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task RegisterEmployee_AppService_Fails_On_Short_Password()
        {
            var svc = new EmployeeAppService(Mock.Of<IEmployeeService>());
            var result = await svc.RegisterEmployeeAsync("A", "B", "x@y.com", "04", "QuotationOfficer", "Addr", "12345");
            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task RegisterEmployee_AppService_Calls_Business_Service_On_Valid_Input()
        {
            var business = new Mock<IEmployeeService>();
            business
                .Setup(b => b.RegisterEmployeeAsync(It.IsAny<Employee>(), It.IsAny<string>()))
                .ReturnsAsync(RegistrationResult.Success(new Employee { Id = 1, Email = "officer@x.com", EmployeeType = "QuotationOfficer" }));

            var app = new EmployeeAppService(business.Object);
            var result = await app.RegisterEmployeeAsync("A", "B", "officer@x.com", "04", "QuotationOfficer", "Addr", "Secret123");

            Assert.True(result.IsSuccess);
            business.Verify(b => b.RegisterEmployeeAsync(It.Is<Employee>(e => e.Email == "officer@x.com" && e.EmployeeType == "QuotationOfficer"), "Secret123"), Times.Once);
        }
    }
}


