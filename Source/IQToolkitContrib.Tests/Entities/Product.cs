using System;
using System.Data.Services.Common;
using System.Diagnostics;

namespace IQToolkitContrib.Tests.Entities {
    [Serializable]
    [DebuggerDisplay("ProductId = {ProductId} | ProductName = {ProductName}")]
    [DataServiceEntitySetName("Products")]
    [DataServiceKey("ProductId")]
    public class Product : IValidate {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int UnitsOnOrder { get; set; }
        public bool Discontinued { get; set; }

        public void Validate() {
            if (this.Discontinued && this.UnitsOnOrder > 0) {
                throw new ValidationException("Reorder level can't be greater than 0 if Discontinued");
            }
        }
    }
}
