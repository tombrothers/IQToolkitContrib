using System;
using System.Data.Services.Common;
using System.Diagnostics;

namespace IQToolkitContrib.Tests.Entities {
    [Serializable]
    [DebuggerDisplay("OrderDetailId = {OrderDetailId} | OrderId = {OrderId} | ProductId = {ProductId}")]
    [DataServiceEntitySetName("OrderDetails")]
    [DataServiceKey("OrderDetailId")]
    public partial class OrderDetail {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}