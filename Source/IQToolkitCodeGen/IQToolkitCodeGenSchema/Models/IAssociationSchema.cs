using System;

namespace IQToolkitCodeGenSchema.Models {
    internal interface IAssociationSchema {
        string AssociationName { get; set; }
        AssociationType AssociationType { get; }
        string ColumnName { get; }
        string RelatedColumnName { get; }
        string RelatedTableName { get; }
        string TableName { get; }
    }
}