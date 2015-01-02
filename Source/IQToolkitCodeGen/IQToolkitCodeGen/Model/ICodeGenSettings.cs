using System;
using IQToolkitCodeGen.Model;
using System.Collections.Generic;

namespace IQToolkitCodeGen.Model {
    public interface ICodeGenSettings {
        bool IsDirty { get; set; }
        string ConnectionString { get; set; }
        string DataContextBaseClass { get; set; }
        string DataContextClassName { get; set; }
        string DataContextNamespace { get; set; }
        string DataContextOutputFile { get; set; }
        string DataContextTemplate { get; set; }
        string EntityExtension { get; set; }
        string EntityNamespace { get; set; }
        string EntityOutputPath { get; set; }
        string EntityTemplate { get; set; }
        string MappingOutputFile { get; set; }
        string MappingTemplate { get; set; }
        string ProviderName { get; set; }
        List<Table> Tables { get; set; }
        string WcfDataServiceBaseClass { get; set; }
        string WcfDataServiceClassName { get; set; }
        string WcfDataServiceClientBaseClass { get; set; }
        string WcfDataServiceClientClassName { get; set; }
        string WcfDataServiceClientNamespace { get; set; }
        string WcfDataServiceClientOutputFile { get; set; }
        string WcfDataServiceClientTemplate { get; set; }
        string WcfDataServiceNamespace { get; set; }
        string WcfDataServiceOutputFile { get; set; }
        string WcfDataServiceTemplate { get; set; }
        bool ExcludeViews { get; set; }
        bool NoPluralization { get; set; }
        string TableSchemaSql { get; set; }
        string ColumnSchemaSql { get; set; }
        string AssociationSchemaSql { get; set; }

        string ToXml();
    }
}
