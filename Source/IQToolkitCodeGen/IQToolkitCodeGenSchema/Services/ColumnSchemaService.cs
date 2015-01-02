using System.Collections.Generic;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers.ColumnSchemaProviders;

namespace IQToolkitCodeGenSchema.Services {
    internal class ColumnSchemaService : IColumnSchemaService {
        private readonly IProvider _provider;
        private readonly IDbConnectionFactory _connectionFactory;

        public ColumnSchemaService(IProvider provider, IDbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("provider", provider);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._provider = provider;
            this._connectionFactory = connectionFactory;
        }

        public IList<IColumnSchema> GetSchema(string tableName) {
            using (var connection = this._connectionFactory.Create()) {
                return this._provider.GetSchema(connection, tableName);
            }
        }
    }
}