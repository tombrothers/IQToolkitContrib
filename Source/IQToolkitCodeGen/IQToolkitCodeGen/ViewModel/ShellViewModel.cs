using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Event;
using IQToolkitCodeGen.Model;
using IQToolkitCodeGen.Service;
using IQToolkitCodeGenSchema.Models;
using Microsoft.Practices.Prism.Events;

namespace IQToolkitCodeGen.ViewModel {
    public class ShellViewModel : ViewModelBase, IShellViewModel {
        private readonly IShellSettingsService _shellSettingsService;
        private readonly ICodeGenSettingsService _codeGenSettingsService;
        private readonly ICodeGenSettings _codeGenSettings;
        private readonly IMessageService _messageService;
        private readonly ISchemaService _schemaService;
        private readonly ITemplateWriterService _templateWriterService;

        public double Left { get; set; }
        public double Top { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public WindowState WindowState { get; set; }
        public ICommand WindowClosing { get; private set; }
        public ICommand WindowDrop { get; private set; }
        public IMenuViewModel Menu { get; private set; }
        public ISettingsViewModel Settings { get; private set; }
        public IMappingViewModel Mapping { get; private set; }
        public ICustomSchemaSqlViewModel CustomSchemaSqlViewModel { get; private set; }

        #region IsBusy

        private bool _isBusy;

        public bool IsBusy {
            get {
                return this._isBusy;
            }
            set {
                if (this._isBusy != value) {
                    this._isBusy = value;
                    this.OnPropertyChanged(() => IsBusy);
                }
            }
        }

        #endregion

        #region CustomSchemaSqlViewWindowState

        private Microsoft.Windows.Controls.WindowState _customSchemaSqlViewWindowState;

        public Microsoft.Windows.Controls.WindowState CustomSchemaSqlViewWindowState {
            get {
                return this._customSchemaSqlViewWindowState;
            }
            set {
                if (this._customSchemaSqlViewWindowState != value) {
                    this._customSchemaSqlViewWindowState = value;
                    this.OnPropertyChanged(() => CustomSchemaSqlViewWindowState);
                }
            }
        }

        #endregion

        public ShellViewModel(IPresentationService presentationService,
                              IShellSettingsService shellSettingsService,
                              IEventAggregator eventAggregator,
                              IMenuViewModel menu,
                              ISettingsViewModel settings,
                              ICodeGenSettingsService codeGenSettingsService,
                              ICodeGenSettings codeGenSettings,
                              IMessageService messageService,
                              IMappingViewModel mapping,
                              ISchemaService schemaService,
                              ICustomSchemaSqlViewModel customSchemaSqlViewModel,
                              ITemplateWriterService templateWriterService) {
            this._shellSettingsService = shellSettingsService;
            this._codeGenSettingsService = codeGenSettingsService;
            this._codeGenSettings = codeGenSettings;
            this._messageService = messageService;
            this._templateWriterService = templateWriterService;
            this.Menu = menu;
            this.Settings = settings;
            this.Mapping = mapping;
            this._schemaService = schemaService;
            this.CustomSchemaSqlViewModel = customSchemaSqlViewModel;
            this.WindowClosing = new Command<CancelEventArgs>(OnWindowClosing);
            this.WindowDrop = new Command<DragEventArgs>(OnWindowDrop);

            this.DisplayText = "IQToolkit CodeGen";
            this.SetWindowPosition(presentationService);

            eventAggregator.GetEvent<LoadSchemaEvent>().Subscribe(this.LoadSchemaEventHandler);
            eventAggregator.GetEvent<GenerateFilesEvent>().Subscribe(_ => this.GenerateFilesEventHandler());
            eventAggregator.GetEvent<CustomSchemaSqlViewVisibilityChangedEvent>().Subscribe(this.CustomSchemaSqlViewVisibilityChangedEvent);
        }

        private void GenerateFilesEventHandler() {
            this.IsBusy = true;

            var task = Task.Factory.StartNew(() => this._templateWriterService.CreateFiles())
                                       .ContinueWith(_ => this.IsBusy = false, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void LoadSchemaEventHandler(IDatabase database) {
            this.IsBusy = true;

            var task = Task.Factory.StartNew(() => this._schemaService.GetSchema(database))
                                   .ContinueWith(_ => this.IsBusy = false, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void CustomSchemaSqlViewVisibilityChangedEvent(bool isVisible) {
            if (isVisible) {
                this.CustomSchemaSqlViewWindowState = Microsoft.Windows.Controls.WindowState.Open;
            }
            else {
                this.CustomSchemaSqlViewWindowState = Microsoft.Windows.Controls.WindowState.Closed;
            }
        }

        private void OnWindowDrop(DragEventArgs e) {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) {
                string[] fileNames = e.Data.GetData(DataFormats.FileDrop, true) as string[];

                if (fileNames != null && fileNames.Length > 0 && Path.GetExtension(fileNames[0]).Equals(".iqcg", StringComparison.InvariantCultureIgnoreCase)) {
                    this._codeGenSettingsService.Open(fileNames[0]);
                }
            }

            e.Handled = true;
        }

        private void OnWindowClosing(CancelEventArgs e) {
            if (this._codeGenSettings.IsDirty) {
                e.Cancel = !this._messageService.ShowContinue("Your changes have not been saved.\n\nExit without saving your changes?");
            }

            this.SaveWindowPosition();
        }

        private void SetWindowPosition(IPresentationService presentationService) {
            if (this._shellSettingsService.Left >= 0
                    && this._shellSettingsService.Top >= 0
                    && this._shellSettingsService.Width > 0
                    && this._shellSettingsService.Height > 0
                    && this._shellSettingsService.Left + this._shellSettingsService.Width <= presentationService.VirtualScreenWidth
                    && this._shellSettingsService.Top + this._shellSettingsService.Height <= presentationService.VirtualScreenHeight) {
                this.Left = this._shellSettingsService.Left;
                this.Top = this._shellSettingsService.Top;
                this.Height = this._shellSettingsService.Height;
                this.Width = this._shellSettingsService.Width;
            }
            else {
                this.Height = presentationService.PrimaryScreenHeight / 2;
                this.Width = presentationService.PrimaryScreenWidth / 2;
                this.Top = (presentationService.PrimaryScreenHeight - this.Height) / 2;
                this.Left = (presentationService.PrimaryScreenWidth - this.Width) / 2;
            }

            this.WindowState = this._shellSettingsService.WindowState;
        }

        private void SaveWindowPosition() {
            this._shellSettingsService.Left = this.Left;
            this._shellSettingsService.Top = this.Top;
            this._shellSettingsService.Height = this.Height;
            this._shellSettingsService.Width = this.Width;
            this._shellSettingsService.WindowState = this.WindowState;
            this._shellSettingsService.Save();
        }
    }
}
