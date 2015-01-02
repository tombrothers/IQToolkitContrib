using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Services {
    internal interface IDbTypeService {
        string GetDbType(IColumnSchema columnSchema, IColumnTypeSchema columnTypeSchema, IPrimaryKeySchema primaryKeySchema);
    }
}