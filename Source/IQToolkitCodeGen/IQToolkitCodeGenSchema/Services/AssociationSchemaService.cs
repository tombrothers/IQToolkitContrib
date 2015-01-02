using System.Collections.Generic;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers.AssociationSchemaProviders;

namespace IQToolkitCodeGenSchema.Services {
    internal class AssociationSchemaService : IAssociationSchemaService {
        private readonly IProvider _provider;
        private readonly IDbConnectionFactory _connectionFactory;

        public AssociationSchemaService(IProvider provider, IDbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("provider", provider);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._provider = provider;
            this._connectionFactory = connectionFactory;
        }

        public IList<IAssociationSchema> GetSchema() {
            using (var connection = this._connectionFactory.Create()) {
                return this._provider.GetSchema(connection);
            }
        }
    }
}