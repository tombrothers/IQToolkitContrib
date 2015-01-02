using System.Collections.Generic;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers.ColumnTypeSchemaProviders;

namespace IQToolkitCodeGenSchema.Services {
    internal class ColumnTypeSchemaService : IColumnTypeSchemaService {
        private readonly IProvider _provider;
        private readonly IDbConnectionFactory _connectionFactory;

        public ColumnTypeSchemaService(IProvider provider, IDbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("provider", provider);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._provider = provider;
            this._connectionFactory = connectionFactory;
        }

        public IList<IColumnTypeSchema> GetSchema() {
            using (var connection = this._connectionFactory.Create()) {
                return this._provider.GetSchema(connection);
            }
        }
    }
}
