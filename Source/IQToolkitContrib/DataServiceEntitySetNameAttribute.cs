using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQToolkitContrib {
    [AttributeUsage(AttributeTargets.Class)]
    public class DataServiceEntitySetNameAttribute : System.Attribute {
        public string EntitySetName { get; set; }

        public DataServiceEntitySetNameAttribute(string entitySetName) {
            this.EntitySetName = entitySetName;
        }
    }
}
