using System;
using System.Collections.ObjectModel;

namespace IQToolkitCodeGenSchema {
    public interface IDatabaseProvider {
        ReadOnlyCollection<IDatabase> Databases { get; }
        IDatabase GetDatabase(DatabaseType type);
    }
}
