using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace IQToolkitCodeGenSchema.Tests
{
    [TestClass]
    public abstract class TestBase
    {
        public TestContext TestContext { get; set; }
        public abstract DatabaseType Type { get; }

        protected ISchemaProvider GetSchemaProvider(string connectionString)
        {
            var schemaOptions = this.GetSchemaOptions(connectionString);

            return SchemaProvider.Create(schemaOptions);
        }

        protected ISchemaOptions GetSchemaOptions(string connectionString) {
            var schemaOptions = MockRepository.GenerateMock<ISchemaOptions>();

            schemaOptions.Stub(x => x.Database).Return(this.GetDatabase());
            schemaOptions.Stub(x => x.ConnectionString).Return(connectionString);

            return schemaOptions;
        }

        protected virtual IDatabase GetDatabase() {
            var databaseProvider = new DatabaseProvider();
            var database = databaseProvider.GetDatabase(this.Type);

            if (database == null) {
                Assert.Inconclusive("Cannot create database instance for databaseType:  " + this.Type);
            }

            return database;
        }
    }
}
