using System;
using System.IO;
using System.Linq;
using ICSharpCode.SharpZipLib.Zip;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers;
using IQToolkitCodeGenSchema.Tests.Properties;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace IQToolkitCodeGenSchema.Tests {
    [TestClass]
    public class VfpTests : TestBase {
        #region Type

        public override DatabaseType Type {
            get {
                return DatabaseType.Vfp;
            }
        }

        #endregion

        private const string CONNECTIONSTRING = "Northwind.dbc";

        [TestMethod]
        public void VfpTests_PrimaryKey_FreeTableTest() {
            var primaryKeys = this.GetSchemaProvider()
                                  .GetSchema()
                                  .Where(x => x.TableName == "Orders")
                                  .Single()
                                  .Columns
                                  .Where(x => x.PrimaryKey)
                                  .ToList();

            Assert.AreEqual(1, primaryKeys.Count);
            Assert.AreEqual("orderid", primaryKeys[0].ColumnName);
            Assert.AreEqual(true, primaryKeys[0].Generated);
        }

        [TestMethod]
        public void VfpTests_NonAutoIncrementPrimaryKeyTest() {
            var primaryKeys = this.GetSchemaProvider()
                                  .GetSchema()
                                  .Where(x => x.TableName == "Customers")
                                  .Single()
                                  .Columns
                                  .Where(x => x.PrimaryKey)
                                  .ToList();

            Assert.AreEqual(1, primaryKeys.Count);
            Assert.AreEqual("customerid", primaryKeys[0].ColumnName);
            Assert.AreEqual(false, primaryKeys[0].Generated);
        }

        [TestMethod]
        public void VfpTests_GetSchemaNoPluralizationTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.NoPluralization).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.VfpSchema, actual);
            Assert.AreNotEqual(Resources.VfpSchemaExcludeViews, actual);
            Assert.AreEqual(Resources.VfpSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void VfpTests_GetSchemaExcludeViewsTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.ExcludeViews).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.VfpSchema, actual);
            Assert.AreEqual(Resources.VfpSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.VfpSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void VfpTests_GetSchemaTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreEqual(Resources.VfpSchema, actual);
            Assert.AreNotEqual(Resources.VfpSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.VfpSchemaNoPluralization, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void VfpTests_SqlServerConnectionStringTest() {
            this.GetSchemaProvider("Data Source=.;Initial Catalog=master;Integrated Security=True");
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void VfpTests_CompleteConnectionStringWithInvalidDbcFileTest() {
            this.GetSchemaProvider("Provider=VFPOLEDB;Data Source=test.dbc");
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void VfpTests_InvalidDbcFileConnectionStringTest() {
            this.GetSchemaProvider("test.dbc");
        }

        [TestMethod]
        [ExpectedException(typeof(ResolutionFailedException))]
        public void VfpTests_ProviderOnlyConnectionStringTest() {
            this.GetSchemaProvider("Provider=VFPOLEDB");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void VfpTests_EmptyConnectionStringTest() {
            this.GetSchemaProvider(string.Empty);
        }

        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public void AccessSchemaProviderTests_NotDbcConnectionStringTest() {
            this.GetSchemaProvider("northwind.mdb");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void VfpTests_NullConnectionStringTest() {
            this.GetSchemaProvider(null);
        }

        [TestMethod]
        public void VfpTests_DbcFileConnectionStringTest() {
            var provider = this.GetSchemaProvider("Northwind.dbc");
            Assert.IsNotNull(provider.GetSchema());
        }

        [TestMethod]
        public void VfpTests_FreeTableConnectionStringTest() {
            var provider = this.GetSchemaProvider(this.TestContext.TestDeploymentDir);
            Assert.IsNotNull(provider.GetSchema());
        }

        private ISchemaProvider GetSchemaProvider() {
            return this.GetSchemaProvider(CONNECTIONSTRING);
        }

        [ClassInitialize]
        public static void ClassInitialize(TestContext context) {
            File.WriteAllBytes("NorthwindVfp.zip", Properties.Resources.NorthwindVfp);

            FastZip zip = new FastZip();
            zip.ExtractZip("NorthwindVfp.zip", Path.GetDirectoryName(Path.GetFullPath("NorthwindVfp.zip")), string.Empty);
        }
    }
}
