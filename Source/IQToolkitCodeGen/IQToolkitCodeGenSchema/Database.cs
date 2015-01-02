using System.Diagnostics;

namespace IQToolkitCodeGenSchema {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class Database : IDatabase {
        public string DisplayName { get; private set; }
        public string ProviderName { get; private set; }
        public DatabaseType Type { get; private set; }
        public bool AllowCustomSchemaSql { get; private set; }

        public Database(string displayName, string providerName, DatabaseType type, bool allowCustomSchemaSql) {
            ArgumentUtility.CheckNotNullOrEmpty("displayName", displayName);
            ArgumentUtility.CheckNotNullOrEmpty("providerName", providerName);
            ArgumentUtility.CheckIsDefined("type", type);

            this.DisplayName = displayName;
            this.ProviderName = providerName;
            this.Type = type;
            this.AllowCustomSchemaSql = allowCustomSchemaSql;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("DisplayName={0} | ProviderName={1} | Type={2} | AllowCustomSchemaSql={3}",
                                 this.DisplayName, this.ProviderName, this.Type, this.AllowCustomSchemaSql);
        }
#endif
    }
}
