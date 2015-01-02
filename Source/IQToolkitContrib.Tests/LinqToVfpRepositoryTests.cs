using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitContrib.Tests {
    [TestClass]
    [IgnoreTestMethods(new string[] { 
        "TestMathTruncate", 
        "TestDecimalTruncate"
    })]
    public partial class LinqToVfpRepositoryTests : ATest {
        private static LinqToVfpDataContext context;

        protected override DataContext GetDataContext() {
            if (context == null) {
                context = new LinqToVfpDataContext(GetConnectionString(), new TestContextWriter(this.TestContext));
            }

            return context;
        }

        public static string GetConnectionString() {
            return @"Provider=VFPOLEDB.1;Data Source=" + Path.GetFullPath("northwind.dbc")  + ";Exclusive=false";
        }
    }
}
