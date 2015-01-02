using System;
using IQToolkitCodeGen.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGen.Tests.Core {
    [TestClass]
    public class CommandTests {
        [TestMethod]
        public void CommandTests_CanExecuteChangedTest() {
            var command = new Command(x => { }, x => false);
            EventHandler changed = (sender, e) => { };

            command.CanExecuteChanged += changed;
            command.CanExecuteChanged -= changed;
        }

        [TestMethod]
        public void CommandTests_CanExecuteTest() {
            bool executed = false;
            var command = new Command(x => executed = true, x => false);

            command.Execute(null);

            Assert.IsTrue(executed);
            Assert.IsFalse(command.CanExecute(null));
        }

        [TestMethod]
        public void CommandTests_ExecuteTest() {
            bool executed = false;
            var command = new Command(x => executed = true);

            command.Execute(null);

            Assert.IsTrue(executed);
            Assert.IsTrue(command.CanExecute(null));
        }

        [TestMethod]
        public void CommandTests_ConstructorActionTest() {
            new Command(x => { });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException), "Precondition failed: execute != null  execute")]
        public void CommandTests_ConstructorNullActionTest() {
            new Command(null);
        }
    }
}
