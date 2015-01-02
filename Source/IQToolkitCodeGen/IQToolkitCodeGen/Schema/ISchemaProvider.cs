using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Schema {
    public interface ISchemaProvider {
        DbConnection Connection { get; }
        DbProviderFactory Factory { get; }
        string ProviderType { get; }
        string ProviderName { get; }

        List<Association> GetAssociationList();
        List<ColumnMetaInfo> GetColumnMetaInfo();
        long GetMaxLength(System.Data.DataRow dr);
        List<PrimaryKey> GetPrimaryKeyList(string tableName);
        List<string> GetTableNames();
        bool IsGenerated(DataRow dr);
        bool IsNullable(DataRow dr);
        void SetConnectionString(string connectionString);
    }
}
