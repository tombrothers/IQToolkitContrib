using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Event;
using IQToolkitCodeGen.Model;
using IQToolkitCodeGen.Service;
using IQToolkitCodeGenSchema.Models;
using System.Windows;
using Microsoft.Practices.Prism.Events;

namespace IQToolkitCodeGen.ViewModel {
    public class SettingsViewModel : ViewModelBase, ISettingsViewModel {
        private readonly IEventAggregator _eventAggregator;

        public ICommand LoadSchemaCommand { get; private set; }
        public ICommand CustomizeSchemaSqlCommand { get; private set; }

        public ICodeGenSettings Settings { get; private set; }
        public IEnumerable<string> DataContextTemplates { get; private set; }
        public IEnumerable<string> EntityTemplates { get; private set; }
        public IEnumerable<string> MappingTemplates { get; private set; }
        public IEnumerable<string> WcfDataServiceDataContextTemplates { get; private set; }
        public IEnumerable<string> WcfDataServiceClientTemplates { get; private set; }

        public ObservableCollection<IDatabase> Databases { get; private set; }

        #region SelectedDatabase

        private IDatabase _selectedDatabase;

        public IDatabase SelectedDatabase {
            get {
                return this._selectedDatabase;
            }
            set {
                if (this._selectedDatabase != value) {
                    this._selectedDatabase = value;
                    this._eventAggregator.GetEvent<SchemaProviderChangedEvent>().Publish(value);
                    this.OnPropertyChanged(() => SelectedDatabase);
                    this.CustomizeVisiblity = value.AllowCustomSchemaSql ? Visibility.Visible : Visibility.Collapsed;
                }
            }
        }

        #endregion

        #region CustomizeVisiblity

        private Visibility _customizeVisiblity = Visibility.Collapsed;

        public Visibility CustomizeVisiblity {
            get {
                return this._customizeVisiblity;
            }
            set {
                if (this._customizeVisiblity != value) {
                    this._customizeVisiblity = value;
                    this.OnPropertyChanged(() => CustomizeVisiblity);
                }
            }
        }

        #endregion

        public SettingsViewModel(ICodeGenSettings settings, ITemplateFileService templateFileService, ISchemaService schemaService, IEventAggregator eventAggregator) {
            this.Settings = settings;
            this.Databases = new ObservableCollection<IDatabase>(schemaService.Databases);
            this.SetTemplates(templateFileService);

            this.LoadSchemaCommand = new Command(_ => this.PublishLoadSchemaEvent());
            this.CustomizeSchemaSqlCommand = new Command(x => eventAggregator.GetEvent<CustomSchemaSqlViewVisibilityChangedEvent>().Publish(true));
            this._eventAggregator = eventAggregator;
            this._eventAggregator.GetEvent<SchemaProviderNameChangedEvent>().Subscribe(this.UpdateSelectedProvider);
        }

        private void PublishLoadSchemaEvent() {
            if (this.SelectedDatabase == null) {
                throw new ApplicationException("Provider not selected.");
            }

            this._eventAggregator.GetEvent<LoadSchemaEvent>().Publish(this.SelectedDatabase);
        }

        private void UpdateSelectedProvider(string providerName) {
            this.SelectedDatabase = this.Databases.FirstOrDefault(x => x.DisplayName == providerName);
        }

        private void SetTemplates(ITemplateFileService templateFileService) {
            this.DataContextTemplates = templateFileService.DataContextTemplates;
            this.EntityTemplates = templateFileService.EntityTemplates;
            this.MappingTemplates = templateFileService.MappingTemplates;
            this.WcfDataServiceDataContextTemplates = templateFileService.WcfDataServiceDataContextTemplates;
            this.WcfDataServiceClientTemplates = templateFileService.WcfDataServiceClientTemplates;
        }
    }
}
