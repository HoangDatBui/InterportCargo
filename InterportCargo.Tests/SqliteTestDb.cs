using InterportCargo.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace InterportCargo.Tests
{
    public sealed class SqliteTestDb : IAsyncLifetime
    {
        public string DbPath { get; } = Path.Combine(Path.GetTempPath(), $"interport_test_{Guid.NewGuid():N}.db");

        public InterportCargoDbContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<InterportCargoDbContext>()
                .UseSqlite($"Data Source={DbPath}")
                .Options;
            return new InterportCargoDbContext(options);
        }

        public async Task InitializeAsync()
        {
            using var ctx = CreateContext();
            await ctx.Database.EnsureCreatedAsync();
        }

        public Task DisposeAsync()
        {
            try { File.Delete(DbPath); } catch { }
            return Task.CompletedTask;
        }
    }
}


