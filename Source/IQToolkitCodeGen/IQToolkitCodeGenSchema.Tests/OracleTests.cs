using System;
using System.Linq;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers;
using IQToolkitCodeGenSchema.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace IQToolkitCodeGenSchema.Tests {
    [TestClass]
    public class OracleTests : TestBase {
        #region Type

        public override DatabaseType Type
        {
            get
            {
                return DatabaseType.Oracle;
            }
        }

        #endregion

        private const string CONNECTIONSTRING = "Password=NORTHWIND;USER ID=NORTHWIND;DATA SOURCE=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=Localhost)(PORT=1521))(CONNECT_DATA=(SERVER = DEDICATED)(SERVICE_NAME = XE)))";

        [TestMethod, Ignore]
        public void OracleSchemaProviderTests_AutoIncrementPrimaryKeyTest() {
            var primaryKeys = this.GetSchemaProvider()
                                  .GetSchema()
                                  .Where(x => x.TableName == "orders")
                                  .Single()
                                  .Columns
                                  .Where(x => x.PrimaryKey)
                                  .ToList();

            Assert.AreEqual("ORDERID", primaryKeys[0].ColumnName);
            Assert.AreEqual(true, primaryKeys[0].Generated);
        }

        [TestMethod]
        public void OracleSchemaProviderTests_NonAutoIncrementPrimaryKeyTest() {
            var primaryKeys = this.GetSchemaProvider()
                                  .GetSchema()
                                  .Where(x => x.TableName == "CUSTOMERS")
                                  .Single()
                                  .Columns
                                  .Where(x => x.PrimaryKey)
                                  .ToList();

            Assert.AreEqual(1, primaryKeys.Count);
            Assert.AreEqual("CUSTOMERID", primaryKeys[0].ColumnName);
            Assert.AreEqual(false, primaryKeys[0].Generated);
        }

        [TestMethod]
        public void OracleSchemaProviderTests_GetSchemaNoPluralizationTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.NoPluralization).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.OracleSchema, actual);
            Assert.AreNotEqual(Resources.OracleSchemaExcludeViews, actual);
            Assert.AreEqual(Resources.OracleSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void OracleSchemaProviderTests_GetSchemaExcludeViewsTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.ExcludeViews).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.OracleSchema, actual);
            Assert.AreEqual(Resources.OracleSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.OracleSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void OracleSchemaProviderTests_GetSchemaTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreEqual(Resources.OracleSchema, actual);
            Assert.AreNotEqual(Resources.OracleSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.OracleSchemaNoPluralization, actual);
        }

        private ISchemaProvider GetSchemaProvider() {
            return this.GetSchemaProvider(CONNECTIONSTRING);
        }

        protected override IDatabase GetDatabase() {
            if (Environment.Is64BitOperatingSystem) {
                Assert.Inconclusive("Cannot run tests on a 64 bit OS.");
            }            
            
            return base.GetDatabase();
        }
    }
}
