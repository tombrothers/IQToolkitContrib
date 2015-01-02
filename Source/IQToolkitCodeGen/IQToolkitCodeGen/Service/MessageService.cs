using System.Windows;

namespace IQToolkitCodeGen.Service {
    public class MessageService : IMessageService {
        public bool ShowContinue(string message) {
            return MessageBox.Show(message, "Continue", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public bool ShowSelectAll(string message){
            return MessageBox.Show(message, "Select All", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;
        }

        public void ShowError(string message) {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}