using InterportCargo.BusinessLogic.Entities;
using InterportCargo.DataAccess.Repositories;
using Xunit;

namespace InterportCargo.Tests
{
    [Trait("UserStory", "I3")]
    [Collection("E2E-Sqlite")]
    public class I3_OfficerDecisionTests
    {
        private readonly SqliteTestDb _db;
        public I3_OfficerDecisionTests(SqliteTestDb db) { _db = db; }

        [Theory]
        [InlineData("Accepted")]
        [InlineData("Rejected")]
        public void Officer_Can_Accept_Or_Reject(string outcome)
        {
            using var ctx = _db.CreateContext();
            var customerRepo = new EFCustomerRepository(ctx);
            var reqRepo = new EFQuotationRequestRepository(ctx);
            var officerRepo = new EFEmployeeRepository(ctx);
            var email = "i3_customer@x.com";
            var cust = customerRepo.GetByEmail(email) ?? new Customer { FirstName="I3C", FamilyName="U", Email=email, PhoneNumber="123", Address="Q", PasswordHash="h", IsActive=true };
            if (cust.Id == 0) customerRepo.Add(cust);
            var request = ctx.QuotationRequests.FirstOrDefault(r => r.CustomerId == cust.Id) ?? 
                new QuotationRequest{ CustomerId=cust.Id, CustomerName=cust.FirstName, CustomerEmail=email, Source="Q1", Destination="Q2", NumberOfContainers=1, NatureOfPackage="N", PackageWidth=1, PackageHeight=1, ImportOrExport="Import", PackingOrUnpacking="Packing"};
            if(request.Id==0) reqRepo.Add(request);
            ctx.SaveChanges();
            var officerEmail = "i3_officer@x.com";
            var off = ctx.Employees.FirstOrDefault(e => e.Email == officerEmail) ?? new Employee{FirstName="QO",FamilyName="O", Email=officerEmail, EmployeeType="QuotationOfficer", PasswordHash="h", IsActive=true};
            if(off.Id==0) officerRepo.Add(off);
            request.Status = outcome;
            reqRepo.Update(request);
            var updated = reqRepo.GetById(request.Id);
            Assert.Equal(outcome, updated.Status);
        }
    }
}
