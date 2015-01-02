using System.Collections.Generic;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Services {
    internal interface IAssociationSchemaService {
        IList<IAssociationSchema> GetSchema();
    }
}