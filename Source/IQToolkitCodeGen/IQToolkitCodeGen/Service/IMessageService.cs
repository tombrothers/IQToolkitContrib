using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQToolkitCodeGen.Service {
    public interface IMessageService {
        bool ShowContinue(string message);
        bool ShowSelectAll(string message);
        void ShowError(string message);
    }
}
