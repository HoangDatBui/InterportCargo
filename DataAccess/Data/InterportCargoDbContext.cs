using Microsoft.EntityFrameworkCore;
using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Data
{
    public class InterportCargoDbContext : DbContext
    {
        public InterportCargoDbContext(DbContextOptions<InterportCargoDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<QuotationRequest> QuotationRequests { get; set; }
        public DbSet<QuotationResponse> QuotationResponses { get; set; }
        public DbSet<RateSchedule> RateSchedules { get; set; }
        public DbSet<QuotationDetails> QuotationDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
            modelBuilder.Entity<Customer>()
                .Property(c => c.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Customer>()
                .Property(c => c.FamilyName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Customer>()
                .Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Customer>()
                .Property(c => c.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<Customer>()
                .Property(c => c.Address)
                .IsRequired()
                .HasMaxLength(500);
            modelBuilder.Entity<Customer>()
                .Property(c => c.PasswordHash)
                .IsRequired();
            modelBuilder.Entity<Customer>()
                .Property(c => c.CompanyName)
                .HasMaxLength(100);
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();
            modelBuilder.Entity<Customer>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("datetime('now')");
            modelBuilder.Entity<Customer>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<Employee>().ToTable("Employees");
            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            modelBuilder.Entity<Employee>()
                .Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Employee>()
                .Property(e => e.FamilyName)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Employee>()
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<Employee>()
                .Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<Employee>()
                .Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(500);
            modelBuilder.Entity<Employee>()
                .Property(e => e.PasswordHash)
                .IsRequired();
            modelBuilder.Entity<Employee>()
                .Property(e => e.EmployeeType)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();
            modelBuilder.Entity<Employee>()
                .Property(e => e.CreatedDate)
                .HasDefaultValueSql("datetime('now')");
            modelBuilder.Entity<Employee>()
                .Property(e => e.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<QuotationRequest>().ToTable("QuotationRequests");
            modelBuilder.Entity<QuotationRequest>().HasKey(q => q.Id);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.RequestId)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.CustomerId)
                .IsRequired();
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.CustomerName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.CustomerEmail)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.Source)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.Destination)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.NumberOfContainers)
                .IsRequired();
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.NatureOfPackage)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.PackageWidth)
                .IsRequired();
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.PackageHeight)
                .IsRequired();
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.ImportOrExport)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.PackingOrUnpacking)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.Status)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.QuarantineDetails)
                .HasMaxLength(500);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.FumigationDetails)
                .HasMaxLength(500);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.AdditionalRequirements)
                .HasMaxLength(1000);
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.CreatedDate)
                .HasDefaultValueSql("datetime('now')");
            modelBuilder.Entity<QuotationRequest>()
                .Property(q => q.Status)
                .HasDefaultValue("Pending");
            modelBuilder.Entity<QuotationResponse>().ToTable("QuotationResponses");
            modelBuilder.Entity<QuotationResponse>().HasKey(qr => qr.Id);
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.QuotationRequestId)
                .IsRequired();
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.OfficerId);
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.OfficerName)
                .HasMaxLength(100);
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.CustomerId);
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.CustomerName)
                .HasMaxLength(100);
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.ResponseType)
                .IsRequired()
                .HasMaxLength(20)
                .HasDefaultValue("Officer");
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.Status)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.QuotationNumber)
                .HasMaxLength(50);
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.Message)
                .HasMaxLength(1000);
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.CreatedDate)
                .HasDefaultValueSql("datetime('now')");
            modelBuilder.Entity<QuotationResponse>()
                .Property(qr => qr.IsRead)
                .HasDefaultValue(false);
            modelBuilder.Entity<RateSchedule>().ToTable("RateSchedules");
            modelBuilder.Entity<RateSchedule>().HasKey(rs => rs.Id);
            modelBuilder.Entity<RateSchedule>()
                .Property(rs => rs.ServiceType)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<RateSchedule>()
                .Property(rs => rs.Rate20Feet)
                .IsRequired();
            modelBuilder.Entity<RateSchedule>()
                .Property(rs => rs.Rate40Feet)
                .IsRequired();
            modelBuilder.Entity<RateSchedule>()
                .Property(rs => rs.Description)
                .HasMaxLength(500);
            modelBuilder.Entity<RateSchedule>()
                .Property(rs => rs.CreatedDate)
                .HasDefaultValueSql("datetime('now')");
            modelBuilder.Entity<RateSchedule>()
                .Property(rs => rs.IsActive)
                .HasDefaultValue(true);
            modelBuilder.Entity<QuotationDetails>().ToTable("QuotationDetails");
            modelBuilder.Entity<QuotationDetails>().HasKey(qd => qd.Id);
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.QuotationNumber)
                .IsRequired()
                .HasMaxLength(50);
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.QuotationRequestId)
                .IsRequired();
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.OfficerId)
                .IsRequired();
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.OfficerName)
                .IsRequired()
                .HasMaxLength(100);
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.DateIssued)
                .IsRequired();
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.ContainerType)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.Scope)
                .IsRequired()
                .HasMaxLength(1000);
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.Subtotal)
                .IsRequired();
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.AmountAfterDiscount)
                .IsRequired();
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.GST)
                .IsRequired();
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.TotalAmount)
                .IsRequired();
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.ItemizedCharges)
                .HasMaxLength(2000);
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.Status)
                .IsRequired()
                .HasMaxLength(20);
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.CreatedDate)
                .HasDefaultValueSql("datetime('now')");
            modelBuilder.Entity<QuotationDetails>()
                .Property(qd => qd.Status)
                .HasDefaultValue("Pending");
        }
    }
}
