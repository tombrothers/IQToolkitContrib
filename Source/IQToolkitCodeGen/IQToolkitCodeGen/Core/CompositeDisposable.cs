using System;
using System.Collections.Generic;

namespace IQToolkitCodeGen.Core {
    public class CompositeDisposable : IDisposable {
        private List<IDisposable> _disposables = new List<IDisposable>();
        private bool _disposed;

        public IDisposable this[int index] {
            get {
                //Contract.Requires<ArgumentNullException>(index >= 0);
                return this._disposables[index];
            }
            set {
                //Contract.Requires<ArgumentNullException>(index >= 0);
                this._disposables[index] = value;
            }
        }

        public int Count {
            get {
                return this._disposables.Count;
            }
        }

        public void Add(IDisposable item) {
            this._disposables.Add(item);
        }

        public void Clear() {
            this._disposables.Clear();
        }

        public bool Contains(IDisposable item) {
            return this._disposables.Contains(item);
        }

        public bool Remove(IDisposable item) {
            return this._disposables.Remove(item);
        }

        ~CompositeDisposable() {
            this.Dispose();
        }

        void IDisposable.Dispose() {
            this.Dispose();
            GC.SuppressFinalize(this);
        }

        protected void Dispose() {
            if (!this._disposed) {
                this._disposed = true;

                for (int index = this._disposables.Count - 1; index >= 0; index--) {
                    IDisposable item = this._disposables[index];
                    item.Dispose();
                    this._disposables.Remove(item);
                }
            }
        }
    }
}
