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
    public class SqlCe35Tests : TestBase {
        #region Type

        public override DatabaseType Type
        {
            get
            {
                return DatabaseType.SqlCe35;
            }
        }

        #endregion

        private const string CONNECTIONSTRING = "Northwind35.sdf";

        [TestMethod]
        public void SqlCe35Tests_AutoIncrementPrimaryKeyTest() {
            var primaryKeys = this.GetSchemaProvider()
                                  .GetSchema()
                                  .Where(x => x.TableName == "Products")
                                  .Single()
                                  .Columns
                                  .Where(x => x.PrimaryKey)
                                  .ToList();

            Assert.AreEqual("Product ID", primaryKeys[0].ColumnName);
            Assert.AreEqual(true, primaryKeys[0].Generated);
        }

        [TestMethod]
        public void SqlCe35Tests_NonAutoIncrementPrimaryKeyTest() {
            var primaryKeys = this.GetSchemaProvider()
                                  .GetSchema()
                                  .Where(x => x.TableName == "Customers")
                                  .Single()
                                  .Columns
                                  .Where(x => x.PrimaryKey)
                                  .ToList();

            Assert.AreEqual(1, primaryKeys.Count);
            Assert.AreEqual("Customer ID", primaryKeys[0].ColumnName);
            Assert.AreEqual(false, primaryKeys[0].Generated);
        }
        
        [TestMethod]
        public void SqlCe35Tests_GetSchemaNoPluralizationTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.NoPluralization).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.SqlCe35Schema, actual);
            Assert.AreNotEqual(Resources.SqlCe35SchemaExcludeViews, actual);
            Assert.AreEqual(Resources.SqlCe35SchemaNoPluralization, actual);
        }

        // The database doesn't have views.
        [TestMethod, Ignore]
        public void SqlCe35Tests_GetSchemaExcludeViewsTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.ExcludeViews).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.SqlCe35Schema, actual);
            Assert.AreEqual(Resources.SqlCe35SchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.SqlCe35SchemaNoPluralization, actual);
        }

        [TestMethod]
        public void SqlCe35Tests_GetSchemaTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreEqual(Resources.SqlCe35Schema, actual);
            Assert.AreNotEqual(Resources.SqlCe35SchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.SqlCe35SchemaNoPluralization, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void SqlCe35Tests_SqlServerConnectionStringTest() {
            this.GetSchemaProvider("Data Source=.;Initial Catalog=master;Integrated Security=True");
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void SqlCe35Tests_InvalidSdfFileConnectionStringTest() {
            this.GetSchemaProvider("test.sdf");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SqlCe35Tests_EmptyConnectionStringTest() {
            this.GetSchemaProvider(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void SqlCe35Tests_NotSdfConnectionStringTest() {
            this.GetSchemaProvider("northwind.dbc");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SqlCe35Tests_NullConnectionStringTest() {
            this.GetSchemaProvider(null);
        }

        [TestMethod]
        public void SqlCe35Tests_SdfConnectionStringTest() {
            var provider = this.GetSchemaProvider(CONNECTIONSTRING);
            Assert.IsNotNull(provider.GetSchema());
        }

        private ISchemaProvider GetSchemaProvider() {
            return this.GetSchemaProvider(CONNECTIONSTRING);
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) {
            File.WriteAllBytes(CONNECTIONSTRING, Properties.Resources.Northwind35Sdf);
        }
    }
}
