using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Repositories;
using Xunit;

namespace InterportCargo.Tests
{
    [Trait("UserStory", "I6")]
    [Collection("E2E-Sqlite")]
    public class I6_CustomerDecisionStatusTests
    {
        private readonly SqliteTestDb _db;
        public I6_CustomerDecisionStatusTests(SqliteTestDb db) { _db = db; }
        [Theory]
        [InlineData("Accepted")]
        [InlineData("Rejected")]
        public void Customer_Response_Sets_Status(string status)
        {
            using var ctx = _db.CreateContext();
            var custRepo = new EFCustomerRepository(ctx);
            var reqRepo = new EFQuotationRequestRepository(ctx);
            var detailsRepo = new EFQuotationDetailsRepository(ctx);
            var respRepo = new EFQuotationResponseRepository(ctx);
            var cust = custRepo.GetByEmail("i6@x.com") ?? new Customer { FirstName = "I6C", FamilyName = "Q", Email = "i6@x.com", PhoneNumber = "0", Address = "A", PasswordHash = "h", IsActive = true };
            if (cust.Id == 0) custRepo.Add(cust);
            var req = ctx.QuotationRequests.FirstOrDefault(r => r.CustomerId == cust.Id) ??
                new QuotationRequest { CustomerId = cust.Id, CustomerName = cust.FirstName, CustomerEmail = cust.Email, Source = "a", Destination = "b", NumberOfContainers = 1, NatureOfPackage = "General", PackageWidth = 1, PackageHeight = 1, ImportOrExport = "Import", PackingOrUnpacking = "Packing" };
            if (req.Id == 0) reqRepo.Add(req);
            var details = ctx.QuotationDetails.FirstOrDefault(d => d.QuotationRequestId == req.Id) ??
                new QuotationDetails { QuotationNumber = "I6-E2E", QuotationRequestId = req.Id, OfficerId = 1, OfficerName = "QO", DateIssued = DateTime.UtcNow, ContainerType = "20ft", Scope = "x", Subtotal = 1000m, DiscountPercentage = 0, DiscountAmount = 0, AmountAfterDiscount = 1000m, GST = 100m, TotalAmount = 1100m, Status = "Pending", CreatedDate = DateTime.UtcNow };
            if (details.Id == 0) detailsRepo.Add(details);
            var resp = new QuotationResponse { QuotationRequestId = req.Id, CustomerId = cust.Id, CustomerName = cust.FirstName, ResponseType = "Customer", Status = status, Message = status, CreatedDate = DateTime.UtcNow, IsRead = false };
            respRepo.Add(resp);
            var found = ctx.QuotationResponses.Where(r => r.QuotationRequestId == req.Id && r.Status == status).ToList();
            Assert.Single(found);
        }
    }
}
