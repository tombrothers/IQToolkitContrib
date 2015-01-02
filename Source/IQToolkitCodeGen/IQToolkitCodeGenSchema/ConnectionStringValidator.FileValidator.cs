using System;
using System.IO;

namespace IQToolkitCodeGenSchema {
    internal partial class ConnectionStringValidator {
        private class FileValidator : Validator {
            private readonly string _fileExtension;
            private readonly string _connectionStringTemplate;

            public FileValidator(string fileExtension, string connectionStringTemplate) {
                ArgumentUtility.CheckNotNullOrEmpty("fileExtension", fileExtension);
                ArgumentUtility.CheckNotNullOrEmpty("connectionStringTemplate", connectionStringTemplate);

                this._fileExtension = fileExtension.ToLower();
                this._connectionStringTemplate = connectionStringTemplate;

                if (!this._fileExtension.StartsWith(".")) {
                    this._fileExtension = "." + this._fileExtension;
                }
            }

            public override string Validate(string connectionString) {
                connectionString = base.Validate(connectionString);

                // Assumes that if the connectionString has an equals sign it is considered to be a complete connection string.  Otherwise, it 
                // is a path to a file.
                if (!connectionString.Contains("=")) {
                    if (this.IsValidDataPath(connectionString)) {
                        connectionString = string.Format(this._connectionStringTemplate, connectionString);
                    }
                    else {
                        throw new ApplicationException("Invalid connection string.");
                    }
                }

                return connectionString;
            }

            protected virtual bool IsValidDataPath(string path) {
                return Path.GetExtension(path).ToLower() == this._fileExtension && File.Exists(path);
            }
        }
    }
}
