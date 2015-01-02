using System;
using IQToolkitCodeGen.Schema;
using IQToolkitCodeGen.Schema.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGen.Tests.Schema.Providers {
    [TestClass]
    public class SqlServerCompactSchemaProviderTests : TestBase {
        [TestMethod]
        public void SqlServerCompactSchemaProviderTests_AssociationsTest() {
            var provider = this.GetProvider();
            var results = provider.GetAssociationList();
        }

        [TestMethod]
        public void SqlServerCompactSchemaProviderTests_AutoIncrementPrimaryKeyTest() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("products");

            Assert.AreEqual("Product ID", results[0].ColumnName);
            Assert.AreEqual(true, results[0].AutoIncrement);
        }

        [TestMethod]
        public void SqlServerCompactSchemaProviderTests_NonAutoIncrementPrimaryKeyTest() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("customers");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("Customer ID", results[0].ColumnName);
            Assert.AreEqual(false, results[0].AutoIncrement);
        }

        [TestMethod]
        public void SqlServerCompactSchemaProviderTests_GetColumnsTest() {
            var provider = this.GetProvider();
            var results = provider.GetColumnMetaInfo();
        }

        [TestMethod]
        public void SqlServerCompactSchemaProviderTests_GetTableNamesTest() {
            var provider = this.GetProvider();
            var tables = provider.GetTableNames();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Unknown connection option in connection string: initial catalog")]
        public void SqlServerCompactSchemaProviderTests_SqlServerConnectionStringTest() {
            var provider = this.GetProvider("Data Source=.;Initial Catalog=master;Integrated Security=True");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Invalid connection string.")]
        public void SqlServerCompactSchemaProviderTests_InvalidSdfFileConnectionStringTest() {
            var provider = this.GetProvider("test.sdf");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid connection string.")]
        public void SqlServerCompactSchemaProviderTests_EmptyConnectionStringTest() {
            var provider = this.GetProvider(string.Empty);
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Invalid connection string.")]
        public void SqlServerCompactSchemaProviderTests_NotSdfConnectionStringTest() {
            var provider = this.GetProvider("northwind.dbc");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid connection string.")]
        public void SqlServerCompactSchemaProviderTests_NullConnectionStringTest() {
            var provider = this.GetProvider(null);
            provider.Connection.Open();
        }

        [TestMethod]
        public void SqlServerCompactSchemaProviderTests_SdfConnectionStringTest() {
            var provider = this.GetProvider("Northwind.sdf");
            provider.Connection.Open();
        }

        private ISchemaProvider GetProvider() {
            return this.GetProvider("Data Source=Northwind.sdf;Persist Security Info=False;");
        }

        private ISchemaProvider GetProvider(string connectionString) {
            var provider = new SqlServerCompactSchemaProvider();
            provider.SetConnectionString(connectionString);

            return provider;
        }
    }
}
