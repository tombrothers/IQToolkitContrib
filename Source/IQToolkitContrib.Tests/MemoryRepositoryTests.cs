using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Linq;
using IQToolkitContrib.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitContrib.Tests {
    [TestClass]
    [IgnoreTestMethods(new string[] { 
        "TestCompareEntityEqual", 
        "TestCompareEntityNotEqual",
        "TestCompareEntityNotEqual",
        "TestCompareConstructedEqual",
        "TestCompareConstructedMultiValueEqual",
        "TestCompareConstructedMultiValueNotEqual",
        "TestSelectManyJoinedDefaultIfEmpty"
    })]
    public partial class MemoryRepositoryTests : ATest {
        private static DataContext context;
        //private static object contextLock = new object();

        protected override DataContext GetDataContext() {
            if (context == null) {
                context = new DataContext(new MemoryRepository());
                this.LoadCustomers(context);
                this.LoadOrders(context);
                this.LoadOrderDetails(context);
                this.LoadProducts(context);

                List<Customer> customers = context.List<Customer>().ToList();

                for (int index = 0, total = customers.Count; index < total; index++) {
                    Customer c = customers[index];
                    c.Orders = context.List<Order>().Where(o => o.CustomerId == c.CustomerId).ToList();
                    context.Update<Customer>(c);
                }

                context.List<Order>().ToList().ForEach(order => {
                    order.Customer = context.Get<Customer>(order.CustomerId) ?? new Customer();
                    context.Update<Order>(order);
                });
            }

            return context;
        }

        private void LoadProducts(DataContext context) {
            XDocument doc = this.CreateXDocument("Products.xml");

            List<Product> list = (from d in doc.Descendants("product")
                                  select new Product {
                                      ProductId = Convert.ToInt32(d.Attribute("productid").Value),
                                      ProductName = d.Attribute("productid").Value,
                                      Discontinued = bool.Parse(d.Attribute("discontinued").Value),
                                      UnitsOnOrder = Convert.ToInt32(d.Attribute("unitsonorder").Value)
                                  }).ToList();

            list.ForEach(p => context.Insert<Product>(p));
        }

        private void LoadOrderDetails(DataContext context) {
            XDocument doc = this.CreateXDocument("OrderDetails.xml");

            List<OrderDetail> list = (from d in doc.Descendants("orderdetail")
                                      select new OrderDetail {
                                          OrderId = Convert.ToInt32(d.Attribute("orderid").Value),
                                          ProductId = Convert.ToInt32(d.Attribute("productid").Value)
                                      }).ToList();

            list.ForEach(o => context.Insert<OrderDetail>(o));
        }

        private void LoadOrders(DataContext context) {
            XDocument doc = this.CreateXDocument("Orders.xml");

            List<Order> list = (from d in doc.Descendants("order")
                                select new Order {
                                    OrderId = Convert.ToInt32(d.Attribute("orderid").Value),
                                    CustomerId = d.Attribute("customerid").Value,
                                    OrderDate = DateTime.Parse(d.Attribute("orderdate").Value)
                                }).ToList();

            list.ForEach(o => context.Insert<Order>(o));
        }

        private void LoadCustomers(DataContext context) {
            XDocument doc = this.CreateXDocument("Customers.xml");

            List<Customer> list = (from d in doc.Descendants("customer")
                                   select new Customer {
                                       CustomerId = d.Attribute("customerid").Value,
                                       CompanyName = d.Attribute("companyname").Value,
                                       ContactName = d.Attribute("contactname").Value,
                                       City = d.Attribute("city").Value,
                                       Country = d.Attribute("country").Value,
                                       Phone = d.Attribute("phone").Value
                                   }).ToList();

            list.ForEach(c => context.Insert<Customer>(c));
        }

        private XDocument CreateXDocument(string xmlPath) {
            xmlPath = "IQToolkitContrib.Tests.Data." + xmlPath;
            string xml;

            using (StreamReader streamReader = new StreamReader(typeof(ATest).Assembly.GetManifestResourceStream(xmlPath))) {
                xml = streamReader.ReadToEnd();
            }

            XDocument xdoc = XDocument.Parse(xml);
            return xdoc;
        }
    }
}
