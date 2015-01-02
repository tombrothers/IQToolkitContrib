using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Model;
using IQToolkitCodeGen.Service;
using IQToolkitCodeGen.View;
using IQToolkitCodeGen.ViewModel;
using IQToolkitCodeGenSchema.Providers;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGen
{
    internal static class ContainerConfigurator
    {
        public static IUnityContainer Configure(IUnityContainer container)
        {
            container.RegisterInstance(typeof (ShellView), container.Resolve<ShellView>());
            container.RegisterType<ICodeGenSettings, CodeGenSettings>(new ContainerControlledLifetimeManager());
            container.RegisterType<IEventAggregator, EventAggregator>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDatabaseProvider, DatabaseProvider>(new ContainerControlledLifetimeManager());

            ConfigureServices(container);
            ConfigureViewModels(container);

            return container;
        }

        private static void ConfigureViewModels(IUnityContainer container)
        {
            container.RegisterType<IMappingViewModel, MappingViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<IMenuViewModel, MenuViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISettingsViewModel, SettingsViewModel>(new ContainerControlledLifetimeManager());
            container.RegisterType<IShellViewModel, ShellViewModel>(new ContainerControlledLifetimeManager());
        }
        
        private static void ConfigureServices(IUnityContainer container)
        {
            container.RegisterType<IApplicationService, ApplicationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IFileDialogService, FileDialogService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ICodeGenSettingsService, CodeGenSettingsService>(new ContainerControlledLifetimeManager());

            container.RegisterType<IMostRecentlyUsedFileService, MostRecentlyUsedFileService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IPresentationService, PresentationService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ISchemaService, SchemaService>(new ContainerControlledLifetimeManager());

            container.RegisterType<IMessageService, MessageService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IShellSettingsService, ShellSettingsService>(new ContainerControlledLifetimeManager());
            container.RegisterType<ITemplateFileService, TemplateFileService>(new ContainerControlledLifetimeManager());
            container.RegisterType<IXmlSerializerService, XmlSerializerService>(new ContainerControlledLifetimeManager());

            container.RegisterType<ITemplateWriterService, TemplateWriterService>(new ContainerControlledLifetimeManager());
        }
    }
}
