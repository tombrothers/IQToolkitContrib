using System.Diagnostics;

namespace IQToolkitCodeGenSchema.Schema {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    public class AssociationSchema {
        public string AssociationName { get; internal set; }
        public string TableName { get; private set; }
        public string ColumnName { get; private set; }
        public string RelatedTableName { get; private set; }
        public string RelatedColumnName { get; private set; }
        public AssociationType AssociationType { get; private set; }

        public AssociationSchema(string associationName, string tableName, string columnName, string relatedTableName, string relatedColumnName, AssociationType associationType) {
            ArgumentUtility.CheckNotNullOrEmpty("associationName", associationName);
            ArgumentUtility.CheckNotNullOrEmpty("tableName", tableName);
            ArgumentUtility.CheckNotNullOrEmpty("columnName", columnName);
            ArgumentUtility.CheckNotNullOrEmpty("relatedTableName", relatedTableName);
            ArgumentUtility.CheckNotNullOrEmpty("relatedColumnName", relatedColumnName);
            ArgumentUtility.CheckIsDefined("associationType", associationType);

            this.AssociationName = associationName;
            this.TableName = tableName;
            this.ColumnName = columnName;
            this.RelatedTableName = relatedTableName;
            this.RelatedColumnName = relatedColumnName;
            this.AssociationType = associationType;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("AssociationName={0} | TableName={1} | ColumnName={2} | RelatedTableName={3} | RelatedColumnName={4} | AssociationType={5}",
                this.AssociationName, this.TableName, this.ColumnName, this.RelatedTableName, this.RelatedColumnName, this.AssociationType);
        }
#endif
    }
}
