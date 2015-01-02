using System;
using System.IO;
using System.Linq;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers;
using IQToolkitCodeGenSchema.Tests.Properties;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace IQToolkitCodeGenSchema.Tests {
    [TestClass]
    public class SQLiteTests : TestBase {
        #region Type

        public override DatabaseType Type
        {
            get
            {
                return DatabaseType.SQLite;
            }
        }

        #endregion

        private const string CONNECTIONSTRING = "Northwind.sl3";

        [TestMethod]
        public void SQLiteTests_AutoIncrementPrimaryKeyTest() {
            var primaryKeys = this.GetSchemaProvider()
                       .GetSchema()
                       .Where(x => x.TableName == "Orders")
                       .Single()
                       .Columns
                       .Where(x => x.PrimaryKey)
                       .ToList();

            Assert.AreEqual("OrderID", primaryKeys[0].ColumnName);
            Assert.AreEqual(true, primaryKeys[0].Generated);
        }

        [TestMethod]
        public void SQLiteTests_NotAutoIncrementPrimaryKeyTest() {
            var primaryKeys = this.GetSchemaProvider()
                                  .GetSchema()
                                  .Where(x => x.TableName == "Customers")
                                  .Single()
                                  .Columns
                                  .Where(x => x.PrimaryKey)
                                  .ToList();

            Assert.AreEqual(1, primaryKeys.Count);
            Assert.AreEqual("CustomerID", primaryKeys[0].ColumnName);
            Assert.AreEqual(false, primaryKeys[0].Generated);
        }

        [TestMethod]
        public void SQLiteTests_GetSchemaNoPluralizationTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.NoPluralization).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.SQLiteSchema, actual);
            Assert.AreNotEqual(Resources.SQLiteSchemaExcludeViews, actual);
            Assert.AreEqual(Resources.SQLiteSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void SQLiteTests_GetSchemaExcludeViewsTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.ExcludeViews).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.SQLiteSchema, actual);
            Assert.AreEqual(Resources.SQLiteSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.SQLiteSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void SQLiteTests_GetSchemaTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreEqual(Resources.SQLiteSchema, actual);
            Assert.AreNotEqual(Resources.SQLiteSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.SQLiteSchemaNoPluralization, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void SQLiteTests_SqlServerConnectionStringTest() {
            this.GetSchemaProvider("Data Source=.;Initial Catalog=master;Integrated Security=True");
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void SQLiteTests_InvalidSl3FileConnectionStringTest() {
            this.GetSchemaProvider("test.sl3");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SQLiteTests_EmptyConnectionStringTest() {
            this.GetSchemaProvider(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void SQLiteTests_NotSl3ConnectionStringTest() {
            this.GetSchemaProvider("northwind.dbc");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SQLiteTests_NullConnectionStringTest() {
            this.GetSchemaProvider(null);
        }

        [TestMethod]
        public void SQLiteTests_Sl3ConnectionStringTest() {
            var provider = this.GetSchemaProvider(CONNECTIONSTRING);
            Assert.IsNotNull(provider.GetSchema());
        }

        private ISchemaProvider GetSchemaProvider() {
            return this.GetSchemaProvider(CONNECTIONSTRING);
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) {
            File.WriteAllBytes(CONNECTIONSTRING, Properties.Resources.NorthwindSl3);
        }
    }
}
