using System;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class DbTypeProvider {
        private readonly IUnityContainer _container;
        private readonly IDatabase _database;

        public DbTypeProvider(IUnityContainer container, IDatabase database) {
            ArgumentUtility.CheckNotNull("container", container);
            ArgumentUtility.CheckNotNull("database", database);

            this._container = container;
            this._database = database;
        }

        public string GetDbType(ColumnSchema columnSchema, ColumnTypeSchema columnTypeSchema, PrimaryKeySchema primaryKeySchema) {
            ArgumentUtility.CheckNotNull("columnSchema", columnSchema);
            ArgumentUtility.CheckNotNull("columnTypeSchema", columnTypeSchema);

            return this.GetProvider().GetDbType(columnSchema, columnTypeSchema, primaryKeySchema);
        }

        private Provider GetProvider() {
            switch (this._database.Type) {
                case DatabaseType.Vfp:
                    return this._container.Resolve<VfpProvider>();
                case DatabaseType.Access:
                case DatabaseType.Oracle:
                case DatabaseType.SQLite:
                case DatabaseType.MySql:
                case DatabaseType.SqlCe35:
                case DatabaseType.SqlCe40:
                case DatabaseType.SqlServer:
                    return this._container.Resolve<Provider>();
                default:
                    throw new NotImplementedException(this._database.Type.ToString());
            }
        }
    }
}
