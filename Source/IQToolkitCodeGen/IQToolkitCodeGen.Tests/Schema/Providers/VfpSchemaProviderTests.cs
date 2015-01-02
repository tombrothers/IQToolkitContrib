using System;
using System.Data.OleDb;
using IQToolkitCodeGen.Schema;
using IQToolkitCodeGen.Schema.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGen.Tests.Schema.Providers {
    [TestClass]
    public class VfpSchemaProviderTests : TestBase {
        [TestMethod]
        public void VfpSchemaProviderTests_GetMaxLengthNullTest() {
            var provider = this.GetProvider();
            Assert.AreEqual(-1, provider.GetMaxLength(null));
        }

        [TestMethod]
        public void VfpSchemaProviderTests_IsNullableNullTest() {
            var provider = this.GetProvider();
            Assert.AreEqual(false, provider.IsNullable(null));
        }

        [TestMethod]
        public void VfpSchemaProviderTests_AssociationsTest() {
            var provider = this.GetProvider();
            var results = provider.GetAssociationList();
        }

        [TestMethod]
        public void VfpSchemaProviderTests_NullPrimaryKeyTest() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList(null);
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void VfpSchemaProviderTests_AutoIncrementPrimaryKeyTest() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("orders");

            Assert.AreEqual("orderid", results[0].ColumnName);
            Assert.AreEqual(true, results[0].AutoIncrement);
        }

        [TestMethod]
        public void VfpSchemaProviderTests_PrimaryKey_FreeTableTest() {
            var provider = this.GetProvider("Provider=VFPOLEDB;Data Source=" + this.TestContext.TestDeploymentDir);
            var results = provider.GetPrimaryKeyList("orders");

            Assert.AreEqual("orderid", results[0].ColumnName);
            Assert.AreEqual(false, results[0].AutoIncrement);
        }

        [TestMethod]
        public void VfpSchemaProviderTests_NonAutoIncrementPrimaryKeyTest() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("customers");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("customerid", results[0].ColumnName);
            Assert.AreEqual(false, results[0].AutoIncrement);
        }

        [TestMethod]
        public void VfpSchemaProviderTests_GetColumnsTest() {
            var provider = this.GetProvider();
            var results = provider.GetColumnMetaInfo();
        }

        [TestMethod]
        public void VfpSchemaProviderTests_GetTableNamesTest() {
            var provider = this.GetProvider();
            var tables = provider.GetTableNames();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "An OLE DB Provider was not specified in the ConnectionString.  An example would be, 'Provider=SQLOLEDB;'")]
        public void VfpSchemaProviderTests_SqlServerConnectionStringTest() {
            var provider = this.GetProvider("Data Source=.;Initial Catalog=master;Integrated Security=True");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(OleDbException), "Invalid path or file name.")]
        public void VfpSchemaProviderTests_CompleteConnectionStringWithInvalidDbcFileTest() {
            var provider = this.GetProvider("Provider=VFPOLEDB;Data Source=test.dbc");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Invalid connection string.")]
        public void VfpSchemaProviderTests_InvalidDbcFileConnectionStringTest() {
            var provider = this.GetProvider("test.dbc");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(OleDbException), "Feature is not available.")]
        public void VfpSchemaProviderTests_ProviderOnlyConnectionStringTest() {
            var provider = this.GetProvider("Provider=VFPOLEDB");
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid connection string.")]
        public void VfpSchemaProviderTests_EmptyConnectionStringTest() {
            var provider = this.GetProvider(string.Empty);
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "Invalid connection string.")]
        public void VfpSchemaProviderTests_NullConnectionStringTest() {
            var provider = this.GetProvider(null);
            provider.Connection.Open();
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException), "Invalid connection string.")]
        public void AccessSchemaProviderTests_NotDbcConnectionStringTest() {
            var provider = this.GetProvider("northwind.mdb");
            provider.Connection.Open();
        }

        [TestMethod]
        public void VfpSchemaProviderTests_DbcFileConnectionStringTest() {
            var provider = this.GetProvider("northwind.dbc");
            provider.Connection.Open();
        }

        [TestMethod]
        public void VfpSchemaProviderTests_FreeTableConnectionStringTest() {
            var provider = this.GetProvider(this.TestContext.TestDeploymentDir);
            provider.Connection.Open();
        }

        private ISchemaProvider GetProvider() {
            return this.GetProvider("Provider=VFPOLEDB;Data Source=Northwind.dbc");
        }

        private ISchemaProvider GetProvider(string connectionString) {
            var provider = new VfpSchemaProvider();
            provider.SetConnectionString(connectionString);

            return provider;
        }
    }
}
