using System.Collections.Generic;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Services {
    internal interface ITableSchemaService {
        IList<ITableSchema> GetSchema();
    }
}