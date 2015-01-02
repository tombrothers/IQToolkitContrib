using System;
using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class PrimaryKeySchemaProvider {
        private readonly IUnityContainer _container;
        private readonly IDatabase _database;
        private readonly DbConnectionFactory _connectionFactory;

        public PrimaryKeySchemaProvider(IUnityContainer container, IDatabase database, DbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("container", container);
            ArgumentUtility.CheckNotNull("database", database);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._container = container;
            this._database = database;
            this._connectionFactory = connectionFactory;
        }

        public IList<PrimaryKeySchema> GetSchema(string tableName) {
            var provider = this.GetProvider();
            var connection = this._connectionFactory.Create();

            return provider.GetSchema(connection, tableName);
        }

        private IProvider GetProvider() {
            Func<string, string> getQuotedName;

            switch (this._database.Type) {
                case DatabaseType.Vfp:
                    return this._container.Resolve<VfpProvider>();
                case DatabaseType.Oracle:
                case DatabaseType.MySql:
                    getQuotedName = x => x;
                    return this._container.Resolve<Provider>(new ParameterOverride("getQuotedName", getQuotedName));
                case DatabaseType.Access:
                case DatabaseType.SqlCe35:
                case DatabaseType.SqlCe40:
                case DatabaseType.SQLite:
                case DatabaseType.SqlServer:
                    getQuotedName = x => "[" + x + "]";
                    return this._container.Resolve<Provider>(new ParameterOverride("getQuotedName", getQuotedName));
                default:
                    throw new NotImplementedException(this._database.Type.ToString());
            }
        }
    }
}
