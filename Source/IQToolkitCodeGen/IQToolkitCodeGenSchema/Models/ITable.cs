using System.Collections.Generic;

namespace IQToolkitCodeGenSchema.Models {
    public interface ITable {
        string TableName { get; }
        bool IsView { get; }
        string EntityName { get; }
        IEnumerable<IColumn> Columns { get; }
        IEnumerable<IAssociation> Associations { get; }
    }
}