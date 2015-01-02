using System;
using IQToolkitCodeGenSchema.Services;

namespace IQToolkitCodeGenSchema.Factories {
    internal class PrimaryKeySchemaServiceFactory : IPrimaryKeySchemaServiceFactory {
        private readonly Func<IPrimaryKeySchemaService> _createService;

        public PrimaryKeySchemaServiceFactory(Func<IPrimaryKeySchemaService> createService) {
            ArgumentUtility.CheckNotNull("createService", createService);

            this._createService = createService;
        }

        public IPrimaryKeySchemaService Create() {
            return this._createService();
        }
    }
}