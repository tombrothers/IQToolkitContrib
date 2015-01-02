using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGenSchema;

namespace IQToolkitCodeGen.Model {
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
    public class Table : NotifierBase, ISelected {
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

        #region EntityName

        private string entityName;

        [XmlAttribute]
        public string EntityName {
            get {
                return this.entityName;
            }

            set {
                var safeClrValue = value.ToSafeClrName();

                if (this.entityName != safeClrValue) {
                    this.entityName = safeClrValue;
                    this.Selected = true;
                    this.OnPropertyChanged(() => EntityName);
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

        [XmlArray("Columns"), XmlArrayItem("Column", typeof(Column))]
        public List<Column> Columns { get; set; }

        [XmlArray("Associations"), XmlArrayItem("Association", typeof(Association))]
        public List<Association> Associations { get; set; }

        public Table() {
        }

        public Table(string tableName) {
            this.TableName = tableName;
            this.EntityName = tableName;
            this.Columns = new List<Column>();
            this.Associations = new List<Association>();
        }

        public bool ShouldInclude() {
            return this.Selected
                        && !string.IsNullOrEmpty(this.EntityName)
                        && !string.IsNullOrEmpty(this.TableName)
                        && this.Columns.Where(column => column.ShouldInclude()).Count() > 0;
        }

        public bool HasPrimaryKey() {
            if (this.Columns != null && this.Columns.Count > 0) {
                return this.Columns.Any(c => c.PrimaryKey);
            }

            return false;
        }

        private string DebuggerDisplay() {
            return string.Format("TableName = {0} | EntityName = {1} | Selected = {2} | Columns = {3} | Associations = {4}",
                this.TableName, this.EntityName, this.Selected, this.Columns.Count, this.Associations.Count);
        }
    }
}
