using IQToolkitCodeGen.Model;
using IQToolkitCodeGen.Service;
using IQToolkitCodeGen.View;
using IQToolkitCodeGen.ViewModel;
using IQToolkitCodeGenSchema.Providers;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGen
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override System.Windows.DependencyObject CreateShell()
        {
            var shell = this.Container.Resolve<ShellView>();
            shell.DataContext = this.Container.Resolve<IShellViewModel>();

            shell.Show();
            return shell;
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            this.Container.RegisterInstance(typeof(ShellView), this.Container.Resolve<ShellView>());
            this.Container.RegisterType<ICodeGenSettings, CodeGenSettings>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IDatabaseProvider, DatabaseProvider>(new ContainerControlledLifetimeManager());

            ConfigureServices();
            ConfigureViewModels();
        }


        private void ConfigureViewModels()
        {
            this.Container.RegisterType<IMappingViewModel, MappingViewModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IMenuViewModel, MenuViewModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ISettingsViewModel, SettingsViewModel>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IShellViewModel, ShellViewModel>(new ContainerControlledLifetimeManager());
            
            this.Container.RegisterType<ICustomSchemaSqlViewModel, CustomSchemaSqlViewModel>();
        }

        private void ConfigureServices()
        {
            this.Container.RegisterType<IApplicationService, ApplicationService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IFileDialogService, FileDialogService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ICodeGenSettingsService, CodeGenSettingsService>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<IMostRecentlyUsedFileService, MostRecentlyUsedFileService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IPresentationService, PresentationService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ISchemaService, SchemaService>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<IMessageService, MessageService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IShellSettingsService, ShellSettingsService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<ITemplateFileService, TemplateFileService>(new ContainerControlledLifetimeManager());
            this.Container.RegisterType<IXmlSerializerService, XmlSerializerService>(new ContainerControlledLifetimeManager());

            this.Container.RegisterType<ITemplateWriterService, TemplateWriterService>(new ContainerControlledLifetimeManager());
        }
    }
}
