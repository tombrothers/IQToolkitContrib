using System;
using IQToolkitCodeGenSchema.Services;

namespace IQToolkitCodeGenSchema.Factories {
    internal interface IColumnSchemaServiceFactory {
        IColumnSchemaService Create();
    }
}