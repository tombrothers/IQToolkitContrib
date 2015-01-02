using System;
using System.Data.SQLite;
using IQToolkitCodeGen.Schema;
using IQToolkitCodeGen.Schema.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGen.Tests.Schema.Providers {
    [TestClass]
    public class SQLiteSchemaProviderTests : TestBase {
        [TestMethod]
        public void SQLiteSchemaProviderTests_AssociationsTest() {
            var provider = this.GetProvider();
            var results = provider.GetAssociationList();
        }

        [TestMethod]
        public void SQLiteSchemaProviderTests_AutoIncrementPrimaryKeyTest() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("orders");

            Assert.AreEqual("OrderID", results[0].ColumnName);
            Assert.AreEqual(true, results[0].AutoIncrement);
        }

        [TestMethod]
        public void SQLiteSchemaProviderTests_NotAutoIncrementPrimaryKeyTest() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("customers");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CustomerID", results[0].ColumnName);
            Assert.AreEqual(false, results[0].AutoIncrement);
        }

        [TestMethod]
        public void SQLiteSchemaProviderTests_GetColumnsTest() {
            var provider = this.GetProvider();
            var results = provider.GetColumnMetaInfo();
        }

        [TestMethod]
        public void SQLiteSchemaProviderTests_GetTableNamesTest() {
            var provider = this.GetProvider();
            var tables = provider.GetTableNames();
        }

        [TestMethod]
        [ExpectedException(typeof(SQLiteException), "Unable to open the database file")]
        public void SQLiteSchemaProviderTests_SqlServerConnectionStringTest() {
            var provider = this.GetProvider("Data Source=.;Initial Catalog=master;Integrated Security=True");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Invalid connection string.")]
        public void SQLiteSchemaProviderTests_InvalidSl3FileConnectionStringTest() {
            var provider = this.GetProvider("test.sl3");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid connection string.")]
        public void SQLiteSchemaProviderTests_EmptyConnectionStringTest() {
            var provider = this.GetProvider(string.Empty);
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Invalid connection string.")]
        public void SQLiteSchemaProviderTests_NotSl3ConnectionStringTest() {
            var provider = this.GetProvider("northwind.dbc");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid connection string.")]
        public void SQLiteSchemaProviderTests_NullConnectionStringTest() {
            var provider = this.GetProvider(null);
            provider.Connection.Open();
        }

        [TestMethod]
        public void SQLiteSchemaProviderTests_Sl3ConnectionStringTest() {
            var provider = this.GetProvider("Northwind.sl3");
            provider.Connection.Open();
        }

        private ISchemaProvider GetProvider() {
            return this.GetProvider("Data Source=Northwind.sl3;Version=3;");
        }

        private ISchemaProvider GetProvider(string connectionString) {
            var provider = new SQLiteSchemaProvider();
            provider.SetConnectionString(connectionString);

            return provider;
        }
    }
}
