﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.225
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IQToolkitCodeGen.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "10.0.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double ShellLeft {
            get {
                return ((double)(this["ShellLeft"]));
            }
            set {
                this["ShellLeft"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double ShellTop {
            get {
                return ((double)(this["ShellTop"]));
            }
            set {
                this["ShellTop"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double ShellWidth {
            get {
                return ((double)(this["ShellWidth"]));
            }
            set {
                this["ShellWidth"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("0")]
        public double ShellHeight {
            get {
                return ((double)(this["ShellHeight"]));
            }
            set {
                this["ShellHeight"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Normal")]
        public global::System.Windows.WindowState ShellWindowState {
            get {
                return ((global::System.Windows.WindowState)(this["ShellWindowState"]));
            }
            set {
                this["ShellWindowState"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"
          SELECT UC.CONSTRAINT_NAME                     AS FOREIGNKEY,
          concat(concat(UC.owner,'.'), UC.TABLE_NAME) AS TABLENAME,
          UCC.COLUMN_NAME                             AS ColumnName,
          concat(concat(UC.owner,'.'), RT.TABLE_NAME) AS RELATEDTABLENAME,
          RTC.column_name                             AS RelatedColumnName
          FROM SYS.ALL_CONSTRAINTS UC,
          SYS.ALL_CONSTRAINTS RT,
          SYS.ALL_CONS_COLUMNS RTC,
          SYS.ALL_CONS_COLUMNS UCC
          WHERE UC.R_CONSTRAINT_NAME = RT.CONSTRAINT_NAME
          AND UC.R_OWNER             = RT.OWNER
          AND UCC.constraint_name    = UC.CONSTRAINT_NAME
          AND RTC.constraint_name    = RT.CONSTRAINT_NAME
          AND (UC.CONSTRAINT_TYPE    = 'R')
          AND (UC.OWNER              in (select user from dual))
        ")]
        public string Oracle1_FKSql {
            get {
                return ((string)(this["Oracle1_FKSql"]));
            }
            set {
                this["Oracle1_FKSql"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute(@"
          SELECT UC.CONSTRAINT_NAME                     AS FOREIGNKEY,
          concat(concat(UC.owner,'.'), UC.TABLE_NAME) AS TABLENAME,
          UCC.COLUMN_NAME                             AS ColumnName,
          concat(concat(UC.owner,'.'), RT.TABLE_NAME) AS RELATEDTABLENAME,
          RTC.column_name                             AS RelatedColumnName
          FROM SYS.ALL_CONSTRAINTS UC,
          SYS.ALL_CONSTRAINTS RT,
          SYS.ALL_CONS_COLUMNS RTC,
          SYS.ALL_CONS_COLUMNS UCC
          WHERE UC.R_CONSTRAINT_NAME = RT.CONSTRAINT_NAME
          AND UC.R_OWNER             = RT.OWNER
          AND UCC.constraint_name    = UC.CONSTRAINT_NAME
          AND RTC.constraint_name    = RT.CONSTRAINT_NAME
          AND (UC.CONSTRAINT_TYPE    = 'R')
          AND (UC.OWNER             IN
          (SELECT DISTINCT table_schema
          FROM all_tab_privs
          WHERE table_schema NOT IN ('IL_APP','EXFSYS','DBA_MAINT','TOAD','META','SYS','SYSTEM','WMSYS','GBURNS')
          ))
        ")]
        public string Oracle_FKSql {
            get {
                return ((string)(this["Oracle_FKSql"]));
            }
            set {
                this["Oracle_FKSql"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("OWNER = \'SDSR\' ")]
        public string tables_DefaultView_RowFilter {
            get {
                return ((string)(this["tables_DefaultView_RowFilter"]));
            }
            set {
                this["tables_DefaultView_RowFilter"] = value;
            }
        }
    }
}