using System.Diagnostics;
using IQToolkitCodeGenSchema.Schema;

namespace IQToolkitCodeGenSchema {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class Association : IAssociation, IPropertyName {
        public string AssociationName { get; private set; }
        public string PropertyName { get; private set; }
        public AssociationType AssociationType { get; private set; }
        public string TableName { get; private set; }
        public string ColumnName { get; private set; }
        public string RelatedTableName { get; private set; }
        public string RelatedColumnName { get; private set; }

        string IPropertyName.PropertyName {
            get { return this.PropertyName; }
            set { this.PropertyName = value; }
        }

        public Association(AssociationSchema schema) {
            ArgumentUtility.CheckNotNull("schema", schema);

            this.AssociationName = schema.AssociationName;
            this.PropertyName = schema.RelatedTableName.ToSafeClrName();
            this.TableName = schema.TableName;
            this.ColumnName = schema.ColumnName;
            this.RelatedTableName = schema.RelatedTableName;
            this.RelatedColumnName = schema.RelatedColumnName;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("AssociationName={0} | PropertyName={1} | AssociationType={2} | TableName={3} | ColumnName={4} | RelatedTableName={5} | RelatedColumnName={6}",
                this.AssociationName, this.PropertyName, this.AssociationType, this.TableName, this.ColumnName, this.RelatedTableName, this.RelatedColumnName);
        }
#endif
    }
}