using System.Linq;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers;
using IQToolkitCodeGenSchema.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace IQToolkitCodeGenSchema.Tests {
    //[TestClass]
    public class MySqlTests : TestBase {
        #region Type

        public override DatabaseType Type
        {
            get
            {
                return DatabaseType.MySql;
            }
        }

        #endregion

        private const string CONNECTIONSTRING = "Host=127.0.0.1;UserName=test;Password=test;Database=Northwind";

        [TestMethod]
        public void MySqlTests_AutoIncrementPrimaryKeyTest() {
            var primaryKeys = this.GetSchemaProvider()
                                  .GetSchema()
                                  .Where(x => x.TableName == "products")
                                  .Single()
                                  .Columns
                                  .Where(x => x.PrimaryKey)
                                  .ToList();

            Assert.AreEqual("ProductID", primaryKeys[0].ColumnName);
            Assert.AreEqual(true, primaryKeys[0].Generated);
        }

        [TestMethod]
        public void MySqlTests_NonAutoIncrementPrimaryKeyTest() {
            var primaryKeys = this.GetSchemaProvider()
                                  .GetSchema()
                                  .Where(x => x.TableName == "customers")
                                  .Single()
                                  .Columns
                                  .Where(x => x.PrimaryKey)
                                  .ToList();

            Assert.AreEqual(1, primaryKeys.Count);
            Assert.AreEqual("CustomerID", primaryKeys[0].ColumnName);
            Assert.AreEqual(false, primaryKeys[0].Generated);
        }

        [TestMethod]
        public void MySqlTests_GetSchemaNoPluralizationTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.NoPluralization).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.MySqlSchema, actual);
            Assert.AreNotEqual(Resources.MySqlSchemaExcludeViews, actual);
            Assert.AreEqual(Resources.MySqlSchemaNoPluralization, actual);
        }

        // The database doesn't have views.
        [TestMethod, Ignore]
        public void MySqlTests_GetSchemaExcludeViewsTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.ExcludeViews).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.MySqlSchema, actual);
            Assert.AreEqual(Resources.MySqlSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.MySqlSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void MySqlTests_GetSchemaTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreEqual(Resources.MySqlSchema, actual);
            Assert.AreNotEqual(Resources.MySqlSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.MySqlSchemaNoPluralization, actual);
        }

        private ISchemaProvider GetSchemaProvider()
        {
            return this.GetSchemaProvider(CONNECTIONSTRING);
        }
    }
}
