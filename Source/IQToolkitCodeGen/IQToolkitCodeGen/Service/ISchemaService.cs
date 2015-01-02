using System.Collections.ObjectModel;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGen.Service {
    public interface ISchemaService {
        ReadOnlyCollection<IDatabase> Databases { get; }
        void GetSchema(IDatabase database);
    }
}
