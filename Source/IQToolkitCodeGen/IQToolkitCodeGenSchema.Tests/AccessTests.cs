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
    public class AccessTests : TestBase {
        #region Type

        public override DatabaseType Type {
            get {
                return DatabaseType.Access;
            }
        }

        #endregion

        private const string CONNECTIONSTRING = "Northwind.mdb";

        [TestMethod]
        public void AccessTests_AutoIncrementPrimaryKeyTest() {
            var primaryKeys = this.GetSchemaProvider()
                                  .GetSchema()
                                  .Where(x => x.TableName == "Orders")
                                  .Single()
                                  .Columns
                                  .Where(x => x.PrimaryKey)
                                  .ToList();

            Assert.AreEqual(1, primaryKeys.Count);
            Assert.AreEqual("OrderID", primaryKeys[0].ColumnName);
            Assert.AreEqual(true, primaryKeys[0].Generated);
        }

        [TestMethod]
        public void AccessTests_NonAutoIncrementPrimaryKeyTest() {
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
        public void AccessTests_GetSchemaNoPluralizationTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.NoPluralization).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.AccessSchema, actual);
            Assert.AreNotEqual(Resources.AccessSchemaExcludeViews, actual);
            Assert.AreEqual(Resources.AccessSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void AccessTests_GetSchemaExcludeViewsTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.ExcludeViews).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.AccessSchema, actual);
            Assert.AreEqual(Resources.AccessSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.AccessSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void AccessTests_GetSchemaTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreEqual(Resources.AccessSchema, actual);
            Assert.AreNotEqual(Resources.AccessSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.AccessSchemaNoPluralization, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void AccessTests_SqlServerConnectionStringTest() {
            this.GetSchemaProvider("Data Source=.;Initial Catalog=master;Integrated Security=True");
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void AccessTests_CompleteConnectionStringWithInvalidMdbFileTest() {
            this.GetSchemaProvider("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=test.mdb");
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void AccessTests_InvalidMdbFileConnectionStringTest() {
            this.GetSchemaProvider("test.mdb");
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void AccessTests_ProviderOnlyConnectionStringTest() {
            this.GetSchemaProvider("Provider=Microsoft.Jet.OLEDB.4.0");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void AccessTests_EmptyConnectionStringTest() {
            this.GetSchemaProvider(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void AccessTests_NotMdbConnectionStringTest() {
            this.GetSchemaProvider("northwind.dbc");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AccessTests_NullConnectionStringTest() {
            this.GetSchemaProvider(null);
        }

        [TestMethod]
        public void AccessTests_MdbConnectionStringTest() {
            var provider = this.GetSchemaProvider("Northwind.mdb");
            Assert.IsNotNull(provider.GetSchema());
        }

        private ISchemaProvider GetSchemaProvider() {
            return this.GetSchemaProvider(CONNECTIONSTRING);
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) {
            File.WriteAllBytes("Northwind.mdb", Properties.Resources.NorthwindMdb);
        }
    }
}
