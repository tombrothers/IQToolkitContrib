using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.DbTypeProviders {
    internal interface IProvider {
        string GetDbType(IColumnSchema columnSchema, IColumnTypeSchema columnTypeSchema, IPrimaryKeySchema primaryKeySchema);
    }
}