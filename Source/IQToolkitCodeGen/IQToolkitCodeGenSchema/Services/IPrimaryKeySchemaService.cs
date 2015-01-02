using System.Collections.Generic;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Services {
    internal interface IPrimaryKeySchemaService {
        IList<IPrimaryKeySchema> GetSchema(string tableName);
    }
}