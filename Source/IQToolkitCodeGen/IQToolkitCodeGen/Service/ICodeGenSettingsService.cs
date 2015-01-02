using System;

namespace IQToolkitCodeGen.Service {
    public interface ICodeGenSettingsService {
        void Open();
        void Open(string fileName);
        void Reset();
        void Save();
        void Save(string fileName);
        string FileName { get; }
    }
}
