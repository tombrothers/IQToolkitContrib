using System.IO;
using System.Reflection;
using IQToolkitCodeGen.View;

namespace IQToolkitCodeGen.Service {
    public class ApplicationService : IApplicationService {
        private readonly ShellView _shellView;
        public string ApplicationPath { get; private set; }
        public string TemplatePath { get; private set; }

        public ApplicationService(ShellView shellView) {
            this._shellView = shellView;
            this.ApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            this.TemplatePath = Path.Combine(this.ApplicationPath, "Template");
        }

        public void Shutdown() {
            this._shellView.Close();
        }
    }
}
