using System.Data.Common;

namespace IQToolkitCodeGenSchema {
    internal class DbConnectionFactory {
        private readonly DbProviderFactory _factory;
        private readonly string _connectionString;

        public DbConnectionFactory(string providerName, string connectionString) {
            ArgumentUtility.CheckNotNullOrEmpty("providerName", providerName);
            ArgumentUtility.CheckNotNullOrEmpty("connectionString", connectionString);

            this._factory = DbProviderFactories.GetFactory(providerName);
            this._connectionString = connectionString;
        }

        public DbConnection Create() {
            var connection = this._factory.CreateConnection();

            connection.ConnectionString = this._connectionString;

            return connection;
        }
    }
}
