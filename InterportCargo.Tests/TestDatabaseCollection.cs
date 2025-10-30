using Xunit;

namespace InterportCargo.Tests
{
    // Share one temp SQLite DB across all tests in this collection
    [CollectionDefinition("E2E-Sqlite")] 
    public sealed class TestDatabaseCollection : ICollectionFixture<SqliteTestDb> { }
}


