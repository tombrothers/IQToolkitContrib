using System.Collections.Generic;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers.PrimaryKeySchemaProviders;

namespace IQToolkitCodeGenSchema.Services {
    internal class PrimaryKeySchemaService : IPrimaryKeySchemaService {
        private readonly IProvider _provider;
        private readonly IDbConnectionFactory _connectionFactory;

        public PrimaryKeySchemaService(IProvider provider, IDbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("provider", provider);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._provider = provider;
            this._connectionFactory = connectionFactory;
        }

        public IList<IPrimaryKeySchema> GetSchema(string tableName) {
            using (var connection = this._connectionFactory.Create()) {
                return this._provider.GetSchema(connection, tableName);
            }
        }
    }
}
