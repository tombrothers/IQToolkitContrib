using System;
using IQToolkitCodeGen.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGen.Tests.Core {
    [TestClass]
    public class CompositeDisposableTests {
        [TestMethod]
        public void CompositeDisposableTests_FinalizerTest() {
            var compositeDisposable = new CompositeDisposable();
            var item = new SIDisposable();
            compositeDisposable.Add(item);
            bool disposed = false;
            item.DisposeAction = () => disposed = true;

            compositeDisposable = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();

            Assert.IsTrue(disposed);
        }

        [TestMethod]
        public void CompositeDisposableTests_DisposeTest() {
            bool disposed = false;

            using (var compositeDisposable = new CompositeDisposable()) {
                var item = new SIDisposable();

                compositeDisposable.Add(item);
                Assert.AreEqual(1, compositeDisposable.Count);
                compositeDisposable.Remove(item);
                Assert.AreEqual(0, compositeDisposable.Count);

                compositeDisposable.Add(item);
                Assert.AreEqual(1, compositeDisposable.Count);
                Assert.AreEqual(item, compositeDisposable[0]);
                compositeDisposable[0] = new SIDisposable();
                Assert.AreNotEqual(item, compositeDisposable[0]);
                compositeDisposable.Clear();
                Assert.AreEqual(0, compositeDisposable.Count);

                compositeDisposable.Add(item);
                Assert.IsTrue(compositeDisposable.Contains(item));

                item.DisposeAction = () => disposed = true;
            }

            Assert.IsTrue(disposed);
        }
    }
}
