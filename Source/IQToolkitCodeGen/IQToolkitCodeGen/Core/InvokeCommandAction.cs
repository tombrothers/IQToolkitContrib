using System.Windows;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace IQToolkitCodeGen.Core {
    public class InvokeCommandAction : TriggerAction<DependencyObject> {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached("Command",
                                                typeof(ICommand),
                                                typeof(InvokeCommandAction));

        public ICommand Command {
            get {
                return (ICommand)this.GetValue(CommandProperty);
            }

            set {
                this.SetValue(CommandProperty, value);
            }
        }

        protected override void Invoke(object parameter) {
            // Get a strong type reference to the associated item.
            FrameworkElement associatedElement = this.AssociatedObject as FrameworkElement;

            // Don't do anything is the associated item is disabled.
            if (associatedElement == null || !associatedElement.IsEnabled) {
                return;
            }

            if (this.Command != null && this.Command.CanExecute(parameter)) {
                this.Command.Execute(parameter);
            }
        }
    }
}
