using System;
using System.Collections.Generic;

namespace IQToolkitCodeGen.Core {
    public partial class EventAggregator : IEventAggregator, IDisposable {
        private bool _disposed;
        private Dictionary<Type, List<WeakReference>> _subscriptions = new Dictionary<Type, List<WeakReference>>();

        public IDisposable Subscribe<T>(Action<T> action) {
            Type type = typeof(T);
            List<WeakReference> list = null;

            if (!_subscriptions.TryGetValue(type, out list)) {
                list = new List<WeakReference>();
                _subscriptions.Add(type, list);
            }

            var subscription = new Subscription<T>(action);
            var weakSubscription = new WeakReference(subscription);

            list.Add(weakSubscription);

            subscription.UnSubscribe = () => {
                if (list.Contains(weakSubscription)) {
                    list.Remove(weakSubscription);
                }
            };

            return subscription;
        }

        public void Publish<T>(T eventData) {
            Type type = typeof(T);
            List<WeakReference> list = null;

            if (_subscriptions.TryGetValue(type, out list)) {
                for (int index = list.Count - 1; index >= 0; index--) {
                    WeakReference weakSubscription = list[index];
                    Subscription<T> item = weakSubscription.Target as Subscription<T>;

                    if (item == null) {
                        list.Remove(weakSubscription);
                    }
                    else {
                        item.Action(eventData);
                    }
                }
            }
        }

        ~EventAggregator() {
            this.Dispose();
        }

        void IDisposable.Dispose() {
            this.Dispose();
            GC.SuppressFinalize(this);
        }

        protected void Dispose() {
            if (!this._disposed) {
                this._disposed = true;

                foreach (var subscription in this._subscriptions) {
                    for (int index = subscription.Value.Count - 1; index >= 0; index--) {
                        WeakReference weakSubscription = subscription.Value[index];
                        IDisposable item = weakSubscription.Target as IDisposable;

                        if (item != null) {
                            item.Dispose();
                        }

                        subscription.Value.Remove(weakSubscription);
                    }
                }
            }
        }
    }
}