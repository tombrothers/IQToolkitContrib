using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers {
    public class DatabaseProvider : IQToolkitCodeGenSchema.Providers.IDatabaseProvider {
        public ReadOnlyCollection<IDatabase> Databases { get; private set; }

        public DatabaseProvider() {
            this.Databases = this.GetInstalledDatabases(this.GetDatabases());
        }

        public IDatabase GetDatabase(DatabaseType databaseType) {
            ArgumentUtility.CheckIsDefined("databaseType", databaseType);

            return this.Databases.Where(x => x.Type == databaseType).FirstOrDefault();
        }

        private ReadOnlyCollection<IDatabase> GetInstalledDatabases(IEnumerable<IDatabase> databases) {
            DataTable dataTable = DbProviderFactories.GetFactoryClasses();

            List<string> invariantNames = dataTable.AsEnumerable()
                                                   .Select(row => row.Field<string>("InvariantName"))
                                                   .ToList();

            List<IDatabase> list = databases.Where(provider => invariantNames.Contains(provider.ProviderName))
                                            .OrderBy(provider => provider.DisplayName)
                                            .ToList();

            return new ReadOnlyCollection<IDatabase>(list);
        }

        private IEnumerable<IDatabase> GetDatabases() {
            yield return new Database("MySql", "MySql.Data.MySqlClient", DatabaseType.MySql, true);
            yield return new Database("VFP", "System.Data.OleDb", DatabaseType.Vfp, false);
            yield return new Database("Access (mdb)", "System.Data.OleDb", DatabaseType.Access, false);
            yield return new Database("SQLite 3", "System.Data.SQLite", DatabaseType.SQLite, false);
            yield return new Database("SqlServerCe.3.5", "System.Data.SqlServerCe.3.5", DatabaseType.SqlCe35, false);
            yield return new Database("SqlServerCe.4.0", "System.Data.SqlServerCe.4.0", DatabaseType.SqlCe40, false);
            yield return new Database("Sql Server", "System.Data.SqlClient", DatabaseType.SqlServer, true);
            yield return new Database("Oracle", "System.Data.OracleClient", DatabaseType.Oracle, true);
            yield return new Database("Oracle (ODP)", "Oracle.DataAccess.Client", DatabaseType.OracleODP, true);
        }
    }
}
