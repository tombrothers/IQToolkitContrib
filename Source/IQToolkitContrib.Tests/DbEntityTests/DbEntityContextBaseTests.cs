using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitContrib.Tests.DbEntityTests {
    [TestClass]
    public class DbEntityContextBaseTests : ATest {
        [TestMethod]
        public void DbEntityContextBaseTests_InsertOrUpdateTest() {
            var context = new NorthwindContext(@"Northwind.dbc");
            var category = new Category { CategoryName = "TestCategory" };

            // This section proves that the auto generated id isn't getting updated when using the provider directly.
            var categoriesTable = context.Provider.GetTable<Category>("Categories");
            categoriesTable.InsertOrUpdate(category);

            Assert.AreEqual(0, category.CategoryId);

            // This section proves that the DbEntityContextBase alters the behavior of the insert statement so that it does retrieve the auto generated id.
            context.Categories.InsertOrUpdate(category);
            Assert.AreNotEqual(0, category.CategoryId);
        }

        [TestMethod]
        public void DbEntityContextBaseTests_InsertTest() {
            var context = new NorthwindContext(@"Northwind.dbc");
            var category = new Category { CategoryName = "TestCategory" };

            // This section proves that the auto generated id isn't getting updated when using the provider directly.
            var categoriesTable = context.Provider.GetTable<Category>("Categories");
            categoriesTable.Insert(category);
            
            Assert.AreEqual(0, category.CategoryId);

            // This section proves that the DbEntityContextBase alters the behavior of the insert statement so that it does retrieve the auto generated id.
            context.Categories.Insert(category);
            Assert.AreNotEqual(0, category.CategoryId);
        }

        [TestInitialize]
        public void TestInitialize() {
            CopyData(BackupPath, DbcPath);
        }

        [TestCleanup]
        public void TestCleanup() {
            CopyData(BackupPath, DbcPath);
        }

        protected override DataContext GetDataContext() {
            throw new NotImplementedException();
        }
    }
}
