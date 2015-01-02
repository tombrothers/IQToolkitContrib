using System;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema {
    internal partial class ConnectionStringValidator : IConnectionStringValidator {
        private readonly IDatabase _database;

        public ConnectionStringValidator(IDatabase database) {
            ArgumentUtility.CheckNotNull("database", database);

            this._database = database;
        }

        public string Validate(string connectionString) {
            return this.GetValidator().Validate(connectionString);
        }

        private Validator GetValidator() {
            switch (this._database.Type) {
                case DatabaseType.MySql:
                case DatabaseType.SqlServer:
                case DatabaseType.Oracle:
                case DatabaseType.OracleODP:
                    return new Validator();
                case DatabaseType.Vfp:
                    return new VfpValidator();
                case DatabaseType.Access:
                    return new FileValidator("mdb", "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0}");
                case DatabaseType.SQLite:
                    return new FileValidator("sl3", "Version=3;Data Source={0}");
                case DatabaseType.SqlCe35:
                case DatabaseType.SqlCe40:
                    return new FileValidator("sdf", "Data Source={0}");
                default:
                    throw new NotImplementedException(this._database.Type.ToString());
            }
        }
    }
}
