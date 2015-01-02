using System.Collections.Generic;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Models {
    internal interface ITableSchema {
        string TableName { get; }
        bool IsView { get; }
        IList<IColumnSchema> Columns { get; set; }
        IList<IPrimaryKeySchema> PrimaryKeys { get; set; }
        IList<IAssociationSchema> Associations { get; set; }
    }
}