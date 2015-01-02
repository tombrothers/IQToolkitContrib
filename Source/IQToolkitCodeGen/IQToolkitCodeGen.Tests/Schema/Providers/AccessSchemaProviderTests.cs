using System;
using System.Data.OleDb;
using IQToolkitCodeGen.Schema;
using IQToolkitCodeGen.Schema.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGen.Tests.Schema.Providers {
    [TestClass]
    public class AccessSchemaProviderTests : TestBase {
        [TestMethod]
        public void AccessSchemaProviderTests_AssociationsTest() {
            var provider = this.GetProvider();
            var results = provider.GetAssociationList();
        }

        [TestMethod]
        public void AccessSchemaProviderTests_AutoIncrementPrimaryKeyTest() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("orders");

            Assert.AreEqual("OrderID", results[0].ColumnName);
            Assert.AreEqual(true, results[0].AutoIncrement);
        }

        [TestMethod]
        public void AccessSchemaProviderTests_NonAutoIncrementPrimaryKeyTest() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("customers");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CustomerID", results[0].ColumnName);
            Assert.AreEqual(false, results[0].AutoIncrement);
        }

        [TestMethod]
        public void AccessSchemaProviderTests_GetColumnsTest() {
            var provider = this.GetProvider();
            var results = provider.GetColumnMetaInfo();
        }

        [TestMethod]
        public void AccessSchemaProviderTests_GetTableNamesTest() {
            var provider = this.GetProvider();
            var tables = provider.GetTableNames();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "An OLE DB Provider was not specified in the ConnectionString.  An example would be, 'Provider=SQLOLEDB;'")]
        public void AccessSchemaProviderTests_SqlServerConnectionStringTest() {
            var provider = this.GetProvider("Data Source=.;Initial Catalog=master;Integrated Security=True");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(OleDbException), "Invalid path or file name.")]
        public void AccessSchemaProviderTests_CompleteConnectionStringWithInvalidMdbFileTest() {
            var provider = this.GetProvider("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=test.mdb");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Invalid connection string.")]
        public void AccessSchemaProviderTests_InvalidMdbFileConnectionStringTest() {
            var provider = this.GetProvider("test.mdb");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(OleDbException), "Feature is not available.")]
        public void AccessSchemaProviderTests_ProviderOnlyConnectionStringTest() {
            var provider = this.GetProvider("Provider=Microsoft.Jet.OLEDB.4.0");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid connection string.")]
        public void AccessSchemaProviderTests_EmptyConnectionStringTest() {
            var provider = this.GetProvider(string.Empty);
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Invalid connection string.")]
        public void AccessSchemaProviderTests_NotMdbConnectionStringTest() {
            var provider = this.GetProvider("northwind.dbc");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid connection string.")]
        public void AccessSchemaProviderTests_NullConnectionStringTest() {
            var provider = this.GetProvider(null);
            provider.Connection.Open();
        }

        [TestMethod]
        public void AccessSchemaProviderTests_MdbConnectionStringTest() {
            var provider = this.GetProvider("Northwind.mdb");
            provider.Connection.Open();
        }

        private ISchemaProvider GetProvider() {
            return this.GetProvider("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=Northwind.mdb");
        }

        private ISchemaProvider GetProvider(string connectionString) {
            var provider = new AccessSchemaProvider();
            provider.SetConnectionString(connectionString);

            return provider;
        }
    }
}
