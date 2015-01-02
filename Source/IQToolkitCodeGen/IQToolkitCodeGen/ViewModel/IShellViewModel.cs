using System.Windows;

namespace IQToolkitCodeGen.ViewModel {
    public interface IShellViewModel : IViewModel {
        double Left { get; set; }
        double Top { get; set; }
        double Width { get; set; }
        double Height { get; set; }
        WindowState WindowState { get; set; }
        bool IsBusy { get; set; }
        Microsoft.Windows.Controls.WindowState CustomSchemaSqlViewWindowState { get; set; }
    }
}