using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQToolkitContrib.Tests.Entities;

namespace IQToolkitContrib.Tests {
    public class LinqToVfpDataServiceContext : IQToolkitContrib.ADataServiceContext {
        private LinqToVfpDataContext context;

        public IQueryable<Customer> Customers {
            get {
                return this.context.List<Customer>();
            }
        }

        public IQueryable<Order> Orders {
            get {
                return this.context.List<Order>();
            }
        }

        public IQueryable<OrderDetail> OrderDetails {
            get {
                return this.context.List<OrderDetail>();
            }
        }

        public IQueryable<Product> Products {
            get {
                return this.context.List<Product>();
            }
        }

        public LinqToVfpDataServiceContext(LinqToVfpDataContext context) : base(context.Provider) {
            this.context = context;
        }
    }
}
