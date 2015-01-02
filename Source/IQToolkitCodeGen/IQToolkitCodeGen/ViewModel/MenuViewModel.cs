using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Event;
using IQToolkitCodeGen.Model;
using IQToolkitCodeGen.Service;
using Microsoft.Practices.Prism.Events;

namespace IQToolkitCodeGen.ViewModel {
    public class MenuViewModel : ViewModelBase, IMenuViewModel {
        private readonly ICodeGenSettingsService _codeGenSettingsService;
        private readonly IEventAggregator _eventAggregator;

        public ObservableCollection<string> RecentFiles { get; private set; }
        public ICommand NewCommand { get; private set; }
        public ICommand OpenCommand { get; private set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand SaveAsCommand { get; private set; }
        public ICommand ExitCommand { get; private set; }
        public ICommand OpenRecentFileCommand { get; private set; }

        #region HasRecentFiles

        private bool _hasRecentFiles;

        public bool HasRecentFiles {
            get {
                return this._hasRecentFiles;
            }
            private set {
                if (this._hasRecentFiles != value) {
                    this._hasRecentFiles = value;
                    this.OnPropertyChanged(() => HasRecentFiles);
                }
            }
        }

        #endregion

        public MenuViewModel(ICodeGenSettingsService codeGenSettingsService,
                             IMostRecentlyUsedFileService mostRecentlyUsedFileService,
                             IEventAggregator eventAggregator,
                             IApplicationService applicationService) {
            this._codeGenSettingsService = codeGenSettingsService;

            this.NewCommand = new Command(this.ExecuteNewCommand);
            this.OpenCommand = new Command(this.ExecuteOpenCommand);
            this.SaveCommand = new Command(this.ExecuteSaveCommand, this.CanExecuteSaveCommand);
            this.SaveAsCommand = new Command(this.ExecuteSaveAsCommand);
            this.OpenRecentFileCommand = new Command<RoutedEventArgs>(this.ExecuteOpenRecentFileCommand);
            this.ExitCommand = new Command(x => applicationService.Shutdown());
            this.RecentFiles = new ObservableCollection<string>();
            this.UpdateRecentFiles(mostRecentlyUsedFileService.GetList());
            this._eventAggregator = eventAggregator;
            this._eventAggregator.GetEvent<MostRecentlyUsedFileChangedEvent>().Subscribe(this.UpdateRecentFiles);
        }

        private void UpdateRecentFiles(IEnumerable<MostRecentlyUsed> recentFiles) {
            this.RecentFiles.Clear();

            foreach (MostRecentlyUsed file in recentFiles) {
                this.RecentFiles.Add(file.FileName);
            }

            this.HasRecentFiles = this.RecentFiles.Count > 0;
        }

        private void ExecuteOpenRecentFileCommand(RoutedEventArgs parameter) {
            MenuItem menuItem = parameter.OriginalSource as MenuItem;

            if (menuItem != null) {
                this._codeGenSettingsService.Open(menuItem.Header.ToString());
            }
        }

        private void ExecuteNewCommand(object parameter) {
            this._codeGenSettingsService.Reset();
        }

        private void ExecuteOpenCommand(object parameter) {
            this._codeGenSettingsService.Open();
        }

        private void ExecuteSaveCommand(object parameter) {
            this._codeGenSettingsService.Save(this._codeGenSettingsService.FileName);
        }

        private bool CanExecuteSaveCommand(object parameter) {
            return !string.IsNullOrWhiteSpace(this._codeGenSettingsService.FileName);
        }

        private void ExecuteSaveAsCommand(object parameter) {
            this._codeGenSettingsService.Save();
        }
    }
}
