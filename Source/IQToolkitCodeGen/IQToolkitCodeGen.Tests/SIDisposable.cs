using System;
using Microsoft.Moles.Framework.Stubs;

namespace IQToolkitCodeGen.Tests {
    // Couldn't figure out how to get the Moles framework to create a stub for IDisposable.
    public class SIDisposable : StubBase, IDisposable {
        public Action DisposeAction;

        public void Dispose() {
            var action = this.DisposeAction;

            if (action != null) {
                action();

            }
            else {
                this.InstanceBehavior.VoidResult<SIDisposable>(this, null);
            }
        }
    }
}
