using IQToolkitCodeGen.Schema;
using IQToolkitCodeGen.Schema.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGen.Tests.Schema.Providers {
    [TestClass]
    public class MySqlSchemaProviderTests : TestBase {
        [TestMethod]
        public void MySqlSchemaProviderTests_Associations() {
            var provider = this.GetProvider();
            var results = provider.GetAssociationList();
        }

        [TestMethod]
        public void MySqlSchemaProviderTests_AutoIncrementPrimaryKey() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("products");

            Assert.AreEqual("ProductID", results[0].ColumnName);
            Assert.AreEqual(true, results[0].AutoIncrement);
        }

        [TestMethod]
        public void MySqlSchemaProviderTests_NonAutoIncrementPrimaryKey() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("customers");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CustomerID", results[0].ColumnName);
            Assert.AreEqual(false, results[0].AutoIncrement);
        }

        [TestMethod]
        public void MySqlSchemaProviderTests_GetColumns() {
            var provider = this.GetProvider();
            var results = provider.GetColumnMetaInfo();
        }

        [TestMethod]
        public void MySqlSchemaProviderTests_GetTableNames() {
            var provider = this.GetProvider();
            var tables = provider.GetTableNames();
        }

        private ISchemaProvider GetProvider() {
            var provider = new MySqlSchemaProvider();
            provider.SetConnectionString("Host=127.0.0.1;UserName=test;Password=test;Database=Northwind");

            return provider;
        }
    }
}
