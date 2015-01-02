using System;
using IQToolkitCodeGenSchema.Services;

namespace IQToolkitCodeGenSchema.Factories {
    internal class ColumnSchemaServiceFactory : IColumnSchemaServiceFactory {
        private readonly Func<IColumnSchemaService> _createService;

        public ColumnSchemaServiceFactory(Func<IColumnSchemaService> createService) {
            ArgumentUtility.CheckNotNull("createService", createService);

            this._createService = createService;
        }

        public IColumnSchemaService Create() {
            return this._createService();
        }
    }
}