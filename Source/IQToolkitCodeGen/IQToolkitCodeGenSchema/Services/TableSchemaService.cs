using System.Collections.Generic;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers.TableSchemaProviders;

namespace IQToolkitCodeGenSchema.Services {
    internal class TableSchemaService : ITableSchemaService {
        private readonly IProvider _provider;
        private readonly IDbConnectionFactory _connectionFactory;

        public TableSchemaService(IProvider provider, IDbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("provider", provider);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._provider = provider;
            this._connectionFactory = connectionFactory;
        }

        public IList<ITableSchema> GetSchema() {
            using (var connection = this._connectionFactory.Create()) {
                return this._provider.GetSchema(connection);
            }
        }
    }
}
