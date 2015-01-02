using IQToolkitCodeGen.Schema;
using IQToolkitCodeGen.Schema.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGen.Tests.Schema.Providers {
    [TestClass]
    public class SqlServerSchemaProviderTests {
        [TestMethod]
        public void SqlServerSchemaProviderTests_Associations() {
            var provider = this.GetProvider();
            var results = provider.GetAssociationList();
        }

        [TestMethod]
        public void SqlServerSchemaProviderTests_AutoIncrementPrimaryKey() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("orders");

            Assert.AreEqual("OrderID", results[0].ColumnName);
            Assert.AreEqual(true, results[0].AutoIncrement);
        }

        [TestMethod]
        public void SqlServerSchemaProviderTests_NonAutoIncrementPrimaryKey() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("customers");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CustomerID", results[0].ColumnName);
            Assert.AreEqual(false, results[0].AutoIncrement);
        }

        [TestMethod]
        public void SqlServerSchemaProviderTests_GetColumns() {
            var provider = this.GetProvider();
            var results = provider.GetColumnMetaInfo();
        }

        [TestMethod]
        public void SqlServerSchemaProviderTests_GetTableNames() {
            var provider = this.GetProvider();
            var tables = provider.GetTableNames();
        }

        private ISchemaProvider GetProvider() {
            var provider = new SqlServerSchemaProvider();
            provider.SetConnectionString("Data Source=.;Initial Catalog=Northwind;Integrated Security=true;");

            return provider;
        }
    }
}
