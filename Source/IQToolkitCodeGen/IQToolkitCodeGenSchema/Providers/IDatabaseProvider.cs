using System.Collections.ObjectModel;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers {
    public interface IDatabaseProvider {
        ReadOnlyCollection<IDatabase> Databases { get; }
        IDatabase GetDatabase(DatabaseType databaseType);
    }
}