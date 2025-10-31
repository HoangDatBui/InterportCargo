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

        private static QuotationRequest MakeReq(int containers, bool quarantine, bool fumigation)
        {
            return new QuotationRequest
            {
                NumberOfContainers = containers,
                IsQuarantineRequired = quarantine,
                IsFumigationRequired = fumigation,
                RequestId = "RB",
                CustomerId = 1,
                CustomerName = "T",
                CustomerEmail = "t@x.com",
                Source = "A",
                Destination = "B",
                NatureOfPackage = "General",
                PackageWidth = 1,
                PackageHeight = 1,
                ImportOrExport = "Import",
                PackingOrUnpacking = "Packing"
            };
        }

        [Theory]
        [InlineData(5, true, false, 0)]      // at threshold, single flag
        [InlineData(6, true, false, 2.5)]    // just above threshold, single flag
        [InlineData(6, true, true, 5)]       // both flags >5
        [InlineData(10, true, true, 5)]      // at 10, still 5%
        [InlineData(11, true, true, 10)]     // >10 and both flags
        [InlineData(4, false, false, 0)]     // no flags, low count
        public void CalculateDiscount_Boundaries(int containers, bool quarantine, bool fumigation, decimal expected)
        {
            var svc = new DiscountService();
            var percent = svc.CalculateDiscount(MakeReq(containers, quarantine, fumigation));
            Assert.Equal(expected, percent);
        }

        [Fact]
        public void CalculateDiscountAmount_Computes_Correct_Value()
        {
            var svc = new DiscountService();
            var amount = svc.CalculateDiscountAmount(2000m, 2.5m);
            Assert.Equal(50m, amount);
        }
    }
}
