using System.IO;
using System.Windows;
using IQToolkitCodeGen.Service;
using Microsoft.Practices.ServiceLocation;

namespace IQToolkitCodeGen {
    public partial class App : Application {
        protected override void OnStartup(StartupEventArgs e) {
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            base.OnStartup(e);

            var bootstrapper = new Bootstrapper();

            if (e.Args.Length > 0 && File.Exists(e.Args[0]))
            {
                var codeGenSettingsService = ServiceLocator.Current.GetInstance<ICodeGenSettingsService>();
                codeGenSettingsService.Open(e.Args[0]);
            }

            bootstrapper.Run();
        }

        void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e) {
            MessageBox.Show(e.Exception.Message, "Application Error", MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;
        }
    }
}
