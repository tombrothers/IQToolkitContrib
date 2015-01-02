using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers.DbTypeProviders;

namespace IQToolkitCodeGenSchema.Services {
    internal class DbTypeService : IDbTypeService {
        private readonly IProvider _provider;

        public DbTypeService(IProvider provider) {
            ArgumentUtility.CheckNotNull("provider", provider);

            this._provider = provider;
        }

        public string GetDbType(IColumnSchema columnSchema, IColumnTypeSchema columnTypeSchema, IPrimaryKeySchema primaryKeySchema) {
            ArgumentUtility.CheckNotNull("columnSchema", columnSchema);
            ArgumentUtility.CheckNotNull("columnTypeSchema", columnTypeSchema);

            return this._provider.GetDbType(columnSchema, columnTypeSchema, primaryKeySchema);
        }
    }
}
