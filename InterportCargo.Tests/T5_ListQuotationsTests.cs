using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Repositories;
using Xunit;

namespace InterportCargo.Tests
{
    [Trait("UserStory", "T5")]
    [Collection("E2E-Sqlite")]
    public class T5_ListQuotationsTests
    {
        private readonly SqliteTestDb _db;
        public T5_ListQuotationsTests(SqliteTestDb db) { _db = db; }
        [Fact]
        public void Officer_Sees_All_Quotations_InDb()
        {
            using var ctx = _db.CreateContext();
            var custRepo = new EFCustomerRepository(ctx);
            var reqRepo = new EFQuotationRequestRepository(ctx);
            var cust = custRepo.GetByEmail("t5@x.com") ?? new Customer{FirstName="t5c",FamilyName="c",Email="t5@x.com",PhoneNumber="0",Address="a",PasswordHash="h",IsActive=true};
            if (cust.Id == 0) custRepo.Add(cust);
            if (!ctx.QuotationRequests.Any(r => r.CustomerId==cust.Id))
            {
                reqRepo.Add(new QuotationRequest{CustomerId=cust.Id,CustomerName=cust.FirstName,CustomerEmail=cust.Email,Source="s1",Destination="d1",NumberOfContainers=1,NatureOfPackage="g",PackageWidth=1,PackageHeight=1,ImportOrExport="Import",PackingOrUnpacking="Packing"});
                reqRepo.Add(new QuotationRequest{CustomerId=cust.Id,CustomerName=cust.FirstName,CustomerEmail=cust.Email,Source="s2",Destination="d2",NumberOfContainers=2,NatureOfPackage="g",PackageWidth=2,PackageHeight=2,ImportOrExport="Import",PackingOrUnpacking="Packing"});
            }
            var all = reqRepo.GetAll();
            Assert.True(all.Count >= 2);
        }
    }
}
