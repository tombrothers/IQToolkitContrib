using System.Collections.Generic;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Services {
    internal interface IColumnTypeSchemaService {
        IList<IColumnTypeSchema> GetSchema();
    }
}