using System.Collections.Generic;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Services {
    internal interface IColumnSchemaService {
        IList<IColumnSchema> GetSchema(string tableName);
    }
}
