using System;
using System.IO;
using Microsoft.Win32;

namespace IQToolkitCodeGenSchema {
    internal partial class ConnectionStringValidator {
        private class VfpValidator : FileValidator {
            public VfpValidator()
                : base("dbc", "Provider=VFPOLEDB;Data Source={0}") {
                if (!IsInstalled()) {
                    throw new ApplicationException("VfpOleDb provider is not installed.");
                }
            }

            protected override bool IsValidDataPath(string path) {
                return base.IsValidDataPath(path) || Directory.Exists(path);
            }

            private static bool IsInstalled() {
                return Registry.ClassesRoot.OpenSubKey("TypeLib\\{50BAEECA-ED25-11D2-B97B-000000000000}") != null;
            }
        }
    }
}
