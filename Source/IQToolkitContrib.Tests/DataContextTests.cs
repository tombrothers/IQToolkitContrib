using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IQToolkitContrib.Tests.Entities;
using System.Reflection;

namespace IQToolkitContrib.Tests {
    [TestClass]
    public class DataContextTests {
        [TestMethod]
        public void GetItem_ModifyWithUpdate_ShouldChangeItemInTheContext() {
            DataContext context = this.DataContextWithData();
            Customer customer = context.Get<Customer>("1");
            customer.CompanyName = "test";
            context.Update<Customer>(customer);

            Assert.AreEqual(customer.CompanyName, context.Get<Customer>("1").CompanyName);
        }

        [TestMethod]
        public void GetItem_ModifyWithoutUpdate_ShouldNotChangeItemInTheContext() {
            DataContext context = this.DataContextWithData();
            Customer customer = context.Get<Customer>("1");
            customer.CompanyName = "test";


            Assert.AreNotEqual(customer.CompanyName, context.Get<Customer>("1").CompanyName);
        }

        [TestMethod]
        public void UpdateTest() {
            DataContext context = new DataContext(new MemoryRepository());
            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer { CompanyName = "Customer3" });
            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer());

            context.Insert<Order>(new Order());

            context.Insert<Product>(new Product());
            context.Insert<Product>(new Product());

            context.Insert<SessionInfo>(new SessionInfo());
            context.Insert<SessionInfo>(new SessionInfo());
            context.Insert<SessionInfo>(new SessionInfo());

            Customer customer3 = context.Get<Customer>("3");
            customer3.CompanyName = "TestCustomer";
            context.Update<Customer>(customer3);


            Assert.AreEqual(5, context.List<Customer>().Count());
            Assert.AreEqual("3", context.List<Customer>().Where<Customer>(customer => customer.CompanyName == "TestCustomer").Single().CustomerId);

            Assert.AreEqual(1, context.List<Order>().Single().OrderId);
            Assert.AreEqual(2, context.List<Product>().Count());
            Assert.AreEqual(3, context.List<SessionInfo>().Count());
        }

        [TestMethod]
        public void DeleteTest() {
            DataContext context = this.DataContextWithData();

            context.Delete<Customer>(context.Get<Customer>("3"));

            Assert.AreEqual(4, context.List<Customer>().Count());
            Assert.IsNull(context.List<Customer>().Where<Customer>(customer => customer.CustomerId == "3").FirstOrDefault());

            Assert.AreEqual(1, context.List<Order>().Single().OrderId);
            Assert.AreEqual(2, context.List<Product>().Count());
            Assert.AreEqual(3, context.List<SessionInfo>().Count());
        }

        [TestMethod]
        public void InsertMultipleItems_MultipleTypes() {
            DataContext context = this.DataContextWithData();

            Assert.AreEqual(5, context.List<Customer>().Count());
            Assert.AreEqual("5", context.List<Customer>().Last().CustomerId);

            Assert.AreEqual(1, context.List<Order>().Single().OrderId);

            Assert.AreEqual(2, context.List<Product>().Count());
            Assert.AreEqual(1, context.List<Product>().First().ProductId);

            Assert.AreEqual(3, context.List<SessionInfo>().Count());
        }

        [TestMethod]
        public void InsertMultipleItems_SingleType() {
            DataContext context = new DataContext(new MemoryRepository());
            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer());

            Assert.AreEqual(5, context.List<Customer>().Count());
            Assert.AreEqual("5", context.List<Customer>().Last().CustomerId);
        }

        [TestMethod]
        [ExpectedException(typeof(ValidationException))]
        public void InsertSingleItem_IValidationError() {
            DataContext context = new DataContext(new MemoryRepository());
            context.Insert<Product>(new Product {
                Discontinued = true,
                UnitsOnOrder = 5
            });
        }

        [TestMethod]
        public void InsertSingleItem_GuidPrimaryKey_IdSpecified() {
            DataContext context = new DataContext(new MemoryRepository());
            Guid guidId = new Guid("11111111-1111-1111-1111-111111111111");
            context.Insert<SessionInfo>(new SessionInfo { SessionInfoId = guidId });
            Assert.AreEqual(guidId, context.List<SessionInfo>().Single().SessionInfoId);
        }

        [TestMethod]
        public void InsertSingleItem_StringPrimaryKey_IdSpecified() {
            DataContext context = new DataContext(new MemoryRepository());
            context.Insert<Customer>(new Customer { CustomerId = "99" });
            Assert.AreEqual("99", context.List<Customer>().Single().CustomerId);
        }

        [TestMethod]
        public void InsertSingleItem_Int32PrimaryKey_IdSpecified() {
            DataContext context = new DataContext(new MemoryRepository());
            context.Insert<Product>(new Product { ProductId = 99 });
            Assert.AreEqual(99, context.List<Product>().Single().ProductId);
        }

        [TestMethod]
        public void InsertSingleItem_Int64PrimaryKey_IdSpecified() {
            DataContext context = new DataContext(new MemoryRepository());
            context.Insert<Order>(new Order { OrderId = 99 });
            Assert.AreEqual(99, context.List<Order>().Single().OrderId);
        }

        [TestMethod]
        public void InsertSingleItem_GuidPrimaryKey_AutoGeneratedId() {
            DataContext context = new DataContext(new MemoryRepository());
            context.Insert<SessionInfo>(new SessionInfo());
            Assert.AreEqual(1, context.List<SessionInfo>().Count());
        }

        [TestMethod]
        public void InsertSingleItem_StringPrimaryKey_AutoGeneratedId() {
            DataContext context = new DataContext(new MemoryRepository());
            context.Insert<Customer>(new Customer());
            Assert.AreEqual("1", context.List<Customer>().Single().CustomerId);
        }

        [TestMethod]
        public void InsertSingleItem_Int32PrimaryKey_AutoGenerateId() {
            DataContext context = new DataContext(new MemoryRepository());
            context.Insert<Product>(new Product());
            Assert.AreEqual(1, context.List<Product>().Single().ProductId);
        }

        [TestMethod]
        public void InsertSingleItem_Int64PrimaryKey_AutoGenerateId() {
            DataContext context = new DataContext(new MemoryRepository());
            context.Insert<Order>(new Order());
            Assert.AreEqual(1, context.List<Order>().Single().OrderId);
        }

        private DataContext DataContextWithData() {
            DataContext context = new DataContext(new MemoryRepository());

            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer());
            context.Insert<Customer>(new Customer());

            context.Insert<Order>(new Order());

            context.Insert<Product>(new Product());
            context.Insert<Product>(new Product());

            context.Insert<SessionInfo>(new SessionInfo());
            context.Insert<SessionInfo>(new SessionInfo());
            context.Insert<SessionInfo>(new SessionInfo());

            return context;
        }
    }
}
