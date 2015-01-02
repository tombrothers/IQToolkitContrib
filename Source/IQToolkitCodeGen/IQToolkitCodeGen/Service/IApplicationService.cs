
namespace IQToolkitCodeGen.Service {
    public interface IApplicationService {
        string ApplicationPath { get; }
        string TemplatePath { get; }
        void Shutdown();
    }
}
