using System;
using System.Diagnostics;

namespace IQToolkitContrib.Tests.Entities {
    [Serializable]
    [DebuggerDisplay("SessionInfoId = {SessionInfoId}")]
    public class SessionInfo {
        public Guid SessionInfoId { get; set; }
    }
}
