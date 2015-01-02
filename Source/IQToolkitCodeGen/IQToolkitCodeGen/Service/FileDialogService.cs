using Microsoft.Win32;

namespace IQToolkitCodeGen.Service {
    public class FileDialogService : IFileDialogService {
        public string GetOpenFileName() {
            return this.GetFileName(new OpenFileDialog());
        }

        public string GetSaveFileName() {
            return this.GetFileName(new SaveFileDialog());
        }

        private string GetFileName(FileDialog dialog) {
            dialog.DefaultExt = ".iqcg";
            dialog.Filter = "IQToolkit CodeGen File (.iqcg)|*.iqcg";

            if (dialog.ShowDialog() == true) {
                return dialog.FileName;
            }

            return null;
        }
    }
}
