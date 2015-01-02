using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGen.Tests {
    [TestClass]
    public abstract class TestBase {
        public TestContext TestContext { get; set; }

        [AssemblyInitialize()]
        public static void AssemblyInit(TestContext context) {
            File.WriteAllBytes("Northwind.sdf", Properties.Resources.NorthwindSdf);
            File.WriteAllBytes("Northwind.sl3", Properties.Resources.NorthwindSl3);
            File.WriteAllBytes("Northwind.mdb", Properties.Resources.NorthwindMdb);
            File.WriteAllBytes("NorthwindVfp.zip", Properties.Resources.NorthwindVfp);

            FastZip zip = new FastZip();
            zip.ExtractZip("NorthwindVfp.zip", context.TestDeploymentDir, string.Empty);
        }
    }
}
