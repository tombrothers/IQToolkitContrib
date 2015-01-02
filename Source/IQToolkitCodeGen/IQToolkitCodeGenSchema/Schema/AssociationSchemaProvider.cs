using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class AssociationSchemaProvider {
        private readonly IUnityContainer _container;
        private readonly IDatabase _database;
        private readonly DbConnectionFactory _connectionFactory;

        public AssociationSchemaProvider(IUnityContainer container, IDatabase database, DbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("container", container);
            ArgumentUtility.CheckNotNull("database", database);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._container = container;
            this._database = database;
            this._connectionFactory = connectionFactory;
        }

        public IList<AssociationSchema> GetSchema() {
            var provider = this.GetProvider();

            using (var connection = this._connectionFactory.Create()) {
                return provider.GetSchema(connection);
            }
        }

        private Provider GetProvider() {
            switch (this._database.Type) {
                case DatabaseType.Access:
                case DatabaseType.Vfp:
                    return this._container.Resolve<OleDbProvider>();
                case DatabaseType.Oracle:
                    return this._container.Resolve<OracleProvider>();
                case DatabaseType.MySql:
                    return this._container.Resolve<MySqlProvider>();
                case DatabaseType.SqlServer:
                    return this._container.Resolve<SqlServerProvider>();
                case DatabaseType.SqlCe35:
                case DatabaseType.SqlCe40:
                case DatabaseType.SQLite:
                    return this._container.Resolve<EmptyProvider>();
                default:
                    throw new NotImplementedException(this._database.Type.ToString());
            }
        }
    }
}
