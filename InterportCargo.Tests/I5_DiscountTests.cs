using InterportCargo.BusinessLogic.Entities;
using InterportCargo.BusinessLogic.Services;
using InterportCargo.DataAccess.Repositories;
using Xunit;

namespace InterportCargo.Tests
{
    [Trait("UserStory", "I5")]
    [Collection("E2E-Sqlite")]
    public class I5_DiscountTests
    {
        private readonly SqliteTestDb _db;
        public I5_DiscountTests(SqliteTestDb db) { _db = db; }

        [Fact]
        public void Offers_Correct_Discount()
        {
            using var ctx = _db.CreateContext();
            var repo = new EFQuotationRequestRepository(ctx);
            var req = new QuotationRequest
            {
                NumberOfContainers = 11,
                IsQuarantineRequired = true,
                IsFumigationRequired = true,
                RequestId = "R5",
                CustomerId = 1,
                CustomerName = "Test",
                CustomerEmail = "t@x.com",
                Source = "A",
                Destination = "B",
                NatureOfPackage = "General",
                PackageWidth = 1,
                PackageHeight = 1,
                ImportOrExport = "Import",
                PackingOrUnpacking = "Packing"
            };
            repo.Add(req);
            var percent = new DiscountService().CalculateDiscount(req);
            Assert.Equal(10m, percent);
        }
    }
}
