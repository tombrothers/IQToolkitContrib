using System;
using System.Collections.Generic;
using System.Data.Services.Common;
using System.Diagnostics;

namespace IQToolkitContrib.Tests.Entities {
    [Serializable]
    [DebuggerDisplay("OrderId = {OrderId} | CustomerId = {CustomerId}")]
    [DataServiceEntitySetName("Orders")]
    [DataServiceKey("OrderId")]
    public class Order {
        public string CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public int OrderId { get; set; }
        public Customer Customer { get; set; }
        public List<OrderDetail> Details { get; set; }			
    }
}
