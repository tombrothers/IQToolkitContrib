using IQToolkitCodeGen.Core;

namespace IQToolkitCodeGen.ViewModel {
    public abstract class ViewModelBase : NotifierBase, IViewModel {
        #region DisplayText

        private string _displayText;

        public string DisplayText {
            get {
                return this._displayText;
            }
            set {
                if (this._displayText != value) {
                    this._displayText = value;
                    this.OnPropertyChanged(() => this.DisplayText);
                }
            }
        }

        #endregion
    }
}
