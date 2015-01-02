using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Diagnostics;

namespace IQToolkitContrib.Tests.Entities {
    [Serializable]
    [DebuggerDisplay("CustomerId = {CustomerId} | CompanyName = {CompanyName} | ContactName = {ContactName}")]
    [DataServiceEntitySetName("Customers")]
    [DataServiceKey("CustomerId")]
    public class Customer {
        public string City { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string Country { get; set; }
        public string CustomerId { get; set; }
        public string Phone { get; set; }
        public List<Order> Orders { get; set; }
    }
}
