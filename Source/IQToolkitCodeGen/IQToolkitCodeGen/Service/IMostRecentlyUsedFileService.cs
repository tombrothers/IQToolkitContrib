using System.Collections.ObjectModel;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Service {
    public interface IMostRecentlyUsedFileService {
        void AddFile(string fileName);
        ReadOnlyCollection<MostRecentlyUsed> GetList();
    }
}
