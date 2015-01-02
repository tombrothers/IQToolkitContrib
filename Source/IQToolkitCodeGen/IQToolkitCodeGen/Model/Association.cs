using System.Diagnostics;
using System.Xml.Serialization;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGenSchema;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGen.Model {
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
    public class Association : NotifierBase, ISelected {
        #region AssociationName

        private string associationName;

        [XmlAttribute]
        public string AssociationName {
            get {
                return this.associationName;
            }
            set {
                if (this.associationName != value) {
                    this.associationName = value;
                    this.Selected = true;
                    this.OnPropertyChanged(() => AssociationName);
                }
            }

        }

        #endregion

        #region PropertyName

        private string propertyName;

        [XmlAttribute]
        public string PropertyName {
            get {
                return this.propertyName;
            }

            set {
                var safeClrValue = value.ToSafeClrName();

                if (this.propertyName != safeClrValue) {
                    this.propertyName = safeClrValue;
                    this.OnPropertyChanged(() => PropertyName);
                    this.Selected = true;
                }
            }
        }

        #endregion

        #region TableName

        private string tableName;

        [XmlAttribute]
        public string TableName {
            get {
                return this.tableName;
            }

            set {
                if (this.tableName != value) {
                    this.tableName = value;
                    this.OnPropertyChanged(() => TableName);
                }
            }
        }

        #endregion

        #region ColumnName

        private string columnName;

        [XmlAttribute]
        public string ColumnName {
            get {
                return this.columnName;
            }

            set {
                if (this.columnName != value) {
                    this.columnName = value;
                    this.OnPropertyChanged(() => ColumnName);
                }
            }
        }

        #endregion

        #region RelatedTableName

        private string relatedTableName;

        [XmlAttribute]
        public string RelatedTableName {
            get {
                return this.relatedTableName;
            }

            set {
                if (this.relatedTableName != value) {
                    this.relatedTableName = value;
                    this.OnPropertyChanged(() => RelatedTableName);
                }
            }
        }

        #endregion

        #region RelatedColumnName

        private string relatedColumnName;

        [XmlAttribute]
        public string RelatedColumnName {
            get {
                return this.relatedColumnName;
            }

            set {
                if (this.relatedColumnName != value) {
                    this.relatedColumnName = value;
                    this.OnPropertyChanged(() => RelatedColumnName);
                }
            }
        }

        #endregion

        #region AssociationType

        private AssociationType associationType;

        [XmlAttribute]
        public AssociationType AssociationType {
            get {
                return this.associationType;
            }

            set {
                if (this.associationType != value) {
                    this.associationType = value;
                    this.OnPropertyChanged(() => AssociationType);
                }
            }
        }

        #endregion

        #region Selected

        private bool selected;

        [XmlAttribute]
        public bool Selected {
            get {
                return this.selected;
            }

            set {
                if (this.selected != value) {
                    this.selected = value;
                    this.OnPropertyChanged(() => Selected);
                }
            }
        }

        #endregion

        [XmlIgnore]
        public string KeyMembers {
            get {
                if (this.Column == null) {
                    return string.Empty;
                }

                return this.Column.PropertyName;
            }
        }

        [XmlIgnore]
        public string RelatedEntityName {
            get {
                if (this.RelatedTable == null) {
                    return string.Empty;
                }

                return this.RelatedTable.EntityName;
            }
        }

        [XmlIgnore]
        public string RelatedKeyMembers {
            get {
                if (this.RelatedColumn == null) {
                    return string.Empty;
                }

                return this.RelatedColumn.PropertyName;
            }
        }

        internal Table Table { get; set; }
        internal Column Column { get; set; }
        internal Table RelatedTable { get; set; }
        internal Column RelatedColumn { get; set; }

        public Association() {
        }

        internal Association(IAssociation association) {
            this.AssociationName = association.AssociationName;
            this.PropertyName = association.PropertyName;
            this.AssociationType = association.AssociationType;
            this.TableName = association.TableName;
            this.ColumnName = association.ColumnName;
            this.RelatedTableName = association.RelatedTableName;
            this.RelatedColumnName = association.RelatedColumnName;
        }

        public void Update(Association association) {
            this.TableName = association.TableName;
            this.ColumnName = association.ColumnName;
            this.RelatedTableName = association.RelatedTableName;
            this.RelatedColumnName = association.RelatedColumnName;
            this.AssociationType = association.AssociationType;
            this.Table = association.Table;
            this.Column = association.Column;
            this.RelatedTable = association.RelatedTable;
            this.RelatedColumn = association.RelatedColumn;
            this.AssociationType = association.AssociationType;
        }

        public bool ShouldInclude() {
            return this.Selected
                        && !string.IsNullOrEmpty(this.PropertyName)
                        && !string.IsNullOrEmpty(this.KeyMembers)
                        && !string.IsNullOrEmpty(this.RelatedTableName)
                        && !string.IsNullOrEmpty(this.RelatedKeyMembers);
        }

        private string DebuggerDisplay() {
            return string.Format("AssociationName = {0} | AssociationType = {1} | PropertyName = {2} | Selected = {3}",
                    this.AssociationName, this.AssociationType, this.PropertyName, this.Selected);
        }
    }
}