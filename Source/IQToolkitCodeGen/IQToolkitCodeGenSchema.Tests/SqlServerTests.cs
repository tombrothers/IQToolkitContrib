using System.Linq;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers;
using IQToolkitCodeGenSchema.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace IQToolkitCodeGenSchema.Tests {
    [TestClass]
    public class SqlServerTests : TestBase {
        #region Type

        public override Models.DatabaseType Type {
            get { return DatabaseType.SqlServer; }
        }

        #endregion

        private const string CONNECTIONSTRING = @"Data Source=(LocalDb)\v11.0;Initial Catalog=Northwind;Integrated Security=true";

        [TestMethod]
        public void SqlServerTests_CustomAssociationSchemaSql() {
            string sql = @"
select object_name(f.ConstId) ForeignKey,
		t.Table_Name TableName,
		sc.Name ColumnName,
		rt.Table_Name RelatedTableName,
		r.Name RelatedColumnName
	from sysforeignkeys f
	inner join syscolumns sc on f.fkeyid = sc.id 
		and f.fkey = sc.colid
	inner join syscolumns r on f.rkeyid = r.id 
		and f.rkey = r.colid
	inner join information_schema.tables t on object_name(f.fkeyid) = t.Table_Name
	inner join information_schema.tables rt on object_name(f.rkeyid) = rt.Table_Name
	where object_name(f.ConstId) = 'FK_Orders_Employees'
";

            var schemaOptions = MockRepository.GenerateMock<ISchemaOptions>();

            schemaOptions.Stub(x => x.Database).Return(this.GetDatabase());
            schemaOptions.Stub(x => x.ConnectionString).Return(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.ExcludeViews).Return(false);
            schemaOptions.Stub(x => x.NoPluralization).Return(false);
            schemaOptions.Stub(x => x.TableSchemaSql).Return(string.Empty);
            schemaOptions.Stub(x => x.ColumnSchemaSql).Return(string.Empty);
            schemaOptions.Stub(x => x.AssociationSchemaSql).Return(sql);

            var expected = new[] { "FK_Orders_Employees", "FK_Orders_EmployeesList" };
            var actual = Schema.GetSchema(schemaOptions).SelectMany(x => x.Associations).Select(x => x.AssociationName).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SqlServerTests_CustomColumnSchemaSql() {
            string sql = @"
select Table_Name, Column_Name, Data_Type, Is_Nullable, Character_Maximum_Length, Numeric_Precision, Numeric_Scale
	from information_schema.columns
	where table_name = '{{tableName}}'
		and column_name like '%date'
";

            var schemaOptions = MockRepository.GenerateMock<ISchemaOptions>();

            schemaOptions.Stub(x => x.Database).Return(this.GetDatabase());
            schemaOptions.Stub(x => x.ConnectionString).Return(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.ExcludeViews).Return(false);
            schemaOptions.Stub(x => x.NoPluralization).Return(false);
            schemaOptions.Stub(x => x.TableSchemaSql).Return(string.Empty);
            schemaOptions.Stub(x => x.ColumnSchemaSql).Return(sql);
            schemaOptions.Stub(x => x.AssociationSchemaSql).Return(string.Empty);

            var expected = new[] { "OrderDate", "RequiredDate", "ShippedDate" };
            var actual = Schema.GetSchema(schemaOptions).Single(x => x.TableName == "Orders").Columns.Select(x => x.ColumnName).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SqlServerTests_CustomTableSchemaSql() {
            string sql = @"
select table_name, table_type
	from information_schema.tables 
	where table_name like 'o%'
		and table_type = 'base table'
";

            var schemaOptions = MockRepository.GenerateMock<ISchemaOptions>();

            schemaOptions.Stub(x => x.Database).Return(this.GetDatabase());
            schemaOptions.Stub(x => x.ConnectionString).Return(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.ExcludeViews).Return(false);
            schemaOptions.Stub(x => x.NoPluralization).Return(false);
            schemaOptions.Stub(x => x.TableSchemaSql).Return(sql);
            schemaOptions.Stub(x => x.ColumnSchemaSql).Return(string.Empty);
            schemaOptions.Stub(x => x.AssociationSchemaSql).Return(string.Empty);

            var expected = new[] { "Orders", "Order Details" };
            var actual = Schema.GetSchema(schemaOptions).Select(x => x.TableName).ToArray();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SqlServerTests_AutoIncrementPrimaryKey() {
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
        public void SqlServerTests_NonAutoIncrementPrimaryKey() {
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
        public void SqlServerTests_GetSchemaNoPluralizationTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.NoPluralization).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.SqlServerSchema, actual);
            Assert.AreNotEqual(Resources.SqlServerSchemaExcludeViews, actual);
            Assert.AreEqual(Resources.SqlServerSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void SqlServerTests_GetSchemaExcludeViewsTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);
            schemaOptions.Stub(x => x.ExcludeViews).Return(true);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreNotEqual(Resources.SqlServerSchema, actual);
            Assert.AreEqual(Resources.SqlServerSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.SqlServerSchemaNoPluralization, actual);
        }

        [TestMethod]
        public void SqlServerTests_GetSchemaTest() {
            var schemaOptions = this.GetSchemaOptions(CONNECTIONSTRING);

            var list = Schema.GetSchema(schemaOptions);
            string actual = list.ToXml();

            Assert.AreEqual(Resources.SqlServerSchema, actual);
            Assert.AreNotEqual(Resources.SqlServerSchemaExcludeViews, actual);
            Assert.AreNotEqual(Resources.SqlServerSchemaNoPluralization, actual);
        }

        private ISchemaProvider GetSchemaProvider() {
            return this.GetSchemaProvider(CONNECTIONSTRING);
        }
    }
}
