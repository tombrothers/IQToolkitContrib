using System;
using System.Windows.Input;

namespace IQToolkitCodeGen.ViewModel {
    public interface IMenuViewModel {
        ICommand ExitCommand { get; }
        ICommand NewCommand { get; }
        ICommand OpenCommand { get; }
        ICommand SaveAsCommand { get; }
        ICommand SaveCommand { get; }
    }
}
