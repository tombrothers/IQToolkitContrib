using System;
using IQToolkitCodeGen.Schema;
using IQToolkitCodeGen.Schema.Provider;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGen.Tests.Schema.Providers {
    [TestClass]
    public class OracleSchemaProviderTests {
        [TestMethod]
        public void OracleSchemaProviderTests_Associations() {
            var provider = this.GetProvider();
            var results = provider.GetAssociationList();
        }

        [TestMethod, Ignore]
        public void OracleSchemaProviderTests_AutoIncrementPrimaryKey() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("orders");

            Assert.AreEqual("ORDERID", results[0].ColumnName);
            Assert.AreEqual(true, results[0].AutoIncrement);
        }

        [TestMethod]
        public void OracleSchemaProviderTests_NonAutoIncrementPrimaryKey() {
            var provider = this.GetProvider();
            var results = provider.GetPrimaryKeyList("customers");

            Assert.AreEqual(1, results.Count);
            Assert.AreEqual("CUSTOMERID", results[0].ColumnName);
            Assert.AreEqual(false, results[0].AutoIncrement);
        }

        [TestMethod]
        public void OracleSchemaProviderTests_GetColumns() {
            var provider = this.GetProvider();
            var results = provider.GetColumnMetaInfo();
        }

        [TestMethod]
        public void OracleSchemaProviderTests_GetTableNames() {
            var provider = this.GetProvider();
            var tables = provider.GetTableNames();
        }

        private ISchemaProvider GetProvider() {
            if (Environment.Is64BitOperatingSystem) {
                Assert.Inconclusive("Cannot run tests on a 64 bit OS.");
            }

            var provider = new OracleSchemaProvider();
            provider.SetConnectionString("Data Source=XE;User Id=northwind;Password=northwind;");
            provider.Connection.Open();
            return provider;
        }
    }
}
