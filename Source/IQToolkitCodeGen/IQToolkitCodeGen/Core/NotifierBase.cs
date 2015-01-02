using System;
using System.ComponentModel;
using System.Linq.Expressions;

namespace IQToolkitCodeGen.Core {
    [Serializable]
    public abstract class NotifierBase : INotifyPropertyChanged {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public virtual void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            var lambda = (LambdaExpression)propertyExpression;
            MemberExpression memberExpression;

            if (lambda.Body is UnaryExpression) {
                var unaryExpression = (UnaryExpression)lambda.Body;
                memberExpression = (MemberExpression)unaryExpression.Operand;
            }
            else {
                memberExpression = (MemberExpression)lambda.Body;
            }

            this.PropertyChanged(this, new PropertyChangedEventArgs(memberExpression.Member.Name));
        }
    }
}
