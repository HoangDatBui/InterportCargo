using Microsoft.EntityFrameworkCore;
using InterportCargo.BusinessLogic.Entities;

namespace InterportCargo.DataAccess.Data
{
    /// <summary>
    /// Database context for InterportCargo application using Entity Framework Core
    /// </summary>
    public class InterportCargoDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the InterportCargoDbContext
        /// </summary>
        /// <param name="options">Database context options</param>
        public InterportCargoDbContext(DbContextOptions<InterportCargoDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// DbSet for Customer entities
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// DbSet for Employee entities
        /// </summary>
        public DbSet<Employee> Employees { get; set; }

        /// <summary>
        /// Configures the model for Entity Framework
        /// </summary>
        /// <param name="modelBuilder">Model builder instance</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure Customer entity
            modelBuilder.Entity<Customer>().ToTable("Customers");
            
            // Configure primary key
            modelBuilder.Entity<Customer>().HasKey(c => c.Id);
            
            // Configure required fields
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
            
            // Configure unique constraint on email
            modelBuilder.Entity<Customer>()
                .HasIndex(c => c.Email)
                .IsUnique();
            
            // Configure default values
            modelBuilder.Entity<Customer>()
                .Property(c => c.CreatedDate)
                .HasDefaultValueSql("datetime('now')");
                
            modelBuilder.Entity<Customer>()
                .Property(c => c.IsActive)
                .HasDefaultValue(true);

            // Configure Employee entity
            modelBuilder.Entity<Employee>().ToTable("Employees");
            
            // Configure primary key
            modelBuilder.Entity<Employee>().HasKey(e => e.Id);
            
            // Configure required fields
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
            
            // Configure unique constraint on email
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();
            
            // Configure default values
            modelBuilder.Entity<Employee>()
                .Property(e => e.CreatedDate)
                .HasDefaultValueSql("datetime('now')");
                
            modelBuilder.Entity<Employee>()
                .Property(e => e.IsActive)
                .HasDefaultValue(true);
        }
    }
}
