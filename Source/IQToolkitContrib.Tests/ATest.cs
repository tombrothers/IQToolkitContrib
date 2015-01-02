using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using IQToolkit.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitContrib.Tests {
    [TestClass]
    public abstract partial class ATest {
        public TestContext TestContext { get; set; }

        protected static string DbcPath {
            get {
                return Path.GetDirectoryName(Path.GetFullPath("northwind.dbc"));
            }
        }

        protected static string BackupPath {
            get {
                return Path.Combine(DbcPath, "Backup");
            }
        }

        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context) {
            File.WriteAllBytes("NorthwindVfp.zip", Properties.Resources.NorthwindVfpZip);

            FastZip zip = new FastZip();
            zip.ExtractZip("NorthwindVfp.zip", context.TestDeploymentDir, string.Empty);

            Directory.CreateDirectory(BackupPath);
            CopyData(DbcPath, BackupPath);
        }

        protected static void CopyData(string sourcePath, string destinationPath) {
            string[] tables = new string[] { "customers", "orders", "categories" };
            string[] extensions = new string[] { ".dbf", ".cdx" };

            for (int tableIndex = 0, tableTotal = tables.Length; tableIndex < tableTotal; tableIndex++) {
                for (int extensionIndex = 0, extensionTotal = extensions.Length; extensionIndex < extensionTotal; extensionIndex++) {
                    string file = tables[tableIndex] + extensions[extensionIndex];
                    File.Copy(Path.Combine(sourcePath, file), Path.Combine(destinationPath, file), true);
                }
            }
        }

        protected abstract DataContext GetDataContext();

        protected virtual DbEntityProvider GetProvider() {
            return null;
        }
    }
}