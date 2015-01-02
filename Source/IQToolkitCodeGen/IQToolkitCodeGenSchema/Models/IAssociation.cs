namespace IQToolkitCodeGenSchema.Models {
    public interface IAssociation {
        string AssociationName { get; }
        string PropertyName { get; }
        AssociationType AssociationType { get; }
        string TableName { get; }
        string ColumnName { get; }
        string RelatedTableName { get; }
        string RelatedColumnName { get; }
    }
}