using System;

namespace IQToolkitCodeGen.Core
{
    public partial class EventAggregator {
        private class Subscription<T> : IDisposable {
            private bool _disposed;
            internal Action<T> Action { get; private set; }
            internal Action UnSubscribe { get; set; }

            public Subscription(Action<T> action) {
                this.Action = action;
            }

            ~Subscription() {
                this.Dispose(false);
            }

            public void Dispose() {
                this.Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected void Dispose(bool dispose) {
                if (!this._disposed && dispose) {
                    this._disposed = true;
                    this.Action = null;

                    if (this.UnSubscribe != null) {
                        this.UnSubscribe();
                    }
                }
            }
        }
    }
}
