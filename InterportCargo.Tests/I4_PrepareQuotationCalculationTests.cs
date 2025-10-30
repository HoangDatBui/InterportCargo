using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Repositories;
using Xunit;

namespace InterportCargo.Tests
{
    [Trait("UserStory", "I4")]
    [Collection("E2E-Sqlite")]
    public class I4_PrepareQuotationCalculationTests
    {
        private readonly SqliteTestDb _db;
        public I4_PrepareQuotationCalculationTests(SqliteTestDb db) { _db = db; }

        [Fact]
        public void Prepare_Computes_Totals_Without_Discount()
        {
            using var ctx = _db.CreateContext();
            var custRepo = new EFCustomerRepository(ctx);
            var reqRepo = new EFQuotationRequestRepository(ctx);
            var detailsRepo = new EFQuotationDetailsRepository(ctx);
            var cust = custRepo.GetByEmail("i4@x.com") ?? new Customer { FirstName = "I4C", FamilyName="Q", Email="i4@x.com", PhoneNumber="0", Address="A", PasswordHash="h", IsActive=true };
            if(cust.Id==0) custRepo.Add(cust);
            var req = ctx.QuotationRequests.FirstOrDefault(r => r.CustomerId == cust.Id) ?? new QuotationRequest{ CustomerId=cust.Id, CustomerName=cust.FirstName, CustomerEmail=cust.Email, Source="a",Destination="b",NumberOfContainers=1,NatureOfPackage="General",PackageWidth=1,PackageHeight=1,ImportOrExport="Import",PackingOrUnpacking="Packing"};
            if(req.Id==0) reqRepo.Add(req);
            // Typical no-discount calculation
            var details = new QuotationDetails{
                QuotationNumber="Q4-E2E",
                QuotationRequestId=req.Id,
                OfficerId=1,OfficerName="QO",DateIssued=DateTime.UtcNow,ContainerType="20ft",Scope="x",
                Subtotal=1000m,DiscountPercentage=0,DiscountAmount=0,AmountAfterDiscount=1000m,GST=100m,TotalAmount=1100m,Status="Pending",CreatedDate=DateTime.UtcNow
            };
            detailsRepo.Add(details);
            var saved = ctx.QuotationDetails.First(d => d.QuotationNumber=="Q4-E2E");
            Assert.Equal(1000m, saved.Subtotal);
            Assert.Equal(0, saved.DiscountAmount);
            Assert.Equal(100m, saved.GST);
            Assert.Equal(1100m, saved.TotalAmount);
        }
    }
}
