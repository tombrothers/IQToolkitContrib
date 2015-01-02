using System;

namespace IQToolkitCodeGenSchema {
    internal partial class ConnectionStringValidator {
        private class Validator {
            public virtual string Validate(string connectionString) {
                if (string.IsNullOrWhiteSpace(connectionString)) {
                    throw new ApplicationException("Connection string was not provided.");
                }

                return connectionString;
            }
        }
    }
}
