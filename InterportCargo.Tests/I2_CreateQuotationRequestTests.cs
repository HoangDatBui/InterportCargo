using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Repositories;
using Xunit;

namespace InterportCargo.Tests
{
    [Trait("UserStory", "I2")]
    [Collection("E2E-Sqlite")]
    public class I2_CreateQuotationRequestTests
    {
        private readonly SqliteTestDb _db;
        public I2_CreateQuotationRequestTests(SqliteTestDb db) { _db = db; }

        [Fact]
        public void Customer_Can_Create_Quotation_Request()
        {
            using var ctx = _db.CreateContext();
            var customerRepo = new EFCustomerRepository(ctx);
            var reqRepo = new EFQuotationRequestRepository(ctx);
            var email = "new@user.com";
            var cust = customerRepo.GetByEmail(email);
            if (cust == null)
            {
                cust = new Customer { FirstName="X", FamilyName="Y", Email=email, PhoneNumber="000", Address="A", PasswordHash="h", IsActive=true };
                customerRepo.Add(cust);
            }
            var req = new QuotationRequest
            {
                CustomerId = cust.Id, CustomerName = cust.FirstName, CustomerEmail = email,
                Source = "A", Destination = "B", NumberOfContainers = 1, NatureOfPackage = "General",
                PackageWidth = 1, PackageHeight = 1, ImportOrExport = "Import", PackingOrUnpacking = "Packing"
            };
            reqRepo.Add(req);
            Assert.Contains(ctx.QuotationRequests, r => r.CustomerId == cust.Id);
        }
    }
}
