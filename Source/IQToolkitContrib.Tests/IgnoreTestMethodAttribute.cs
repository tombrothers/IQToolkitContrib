using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQToolkitContrib.Tests {
    /// <summary>
    /// used during codegen
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class IgnoreTestMethodsAttribute : Attribute {
        public string[] MethodNames { get; set; }

        public IgnoreTestMethodsAttribute(string methodName)
            : this(new string[] { methodName }) {
        }

        public IgnoreTestMethodsAttribute(string[] methodName) {
            this.MethodNames = methodName;
        }
    }
}
