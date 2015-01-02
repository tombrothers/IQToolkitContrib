using System.Diagnostics;
using System.Xml.Serialization;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGenSchema;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGen.Model {
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
    public class Column : NotifierBase, ISelected {
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

        #region PropertyType

        private string propertyType;

        [XmlAttribute]
        public string PropertyType {
            get {
                return this.propertyType;
            }

            set {
                if (this.propertyType != value) {
                    this.propertyType = value;
                    this.OnPropertyChanged(() => PropertyType);
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

        #region DbType

        private string dbType;

        [XmlAttribute]
        public string DbType {
            get {
                return this.dbType;
            }

            set {
                if (this.dbType != value) {
                    this.dbType = value;
                    this.OnPropertyChanged(() => DbType);
                }
            }
        }

        #endregion

        #region DefaultValue

        private string defaultValue;

        [XmlAttribute]
        public string DefaultValue {
            get {
                return this.defaultValue;
            }

            set {
                if (this.defaultValue != value) {
                    this.defaultValue = value;
                    this.OnPropertyChanged(() => DefaultValue);
                }
            }
        }

        #endregion

        #region PrimaryKey

        private bool primaryKey;

        [XmlAttribute]
        public bool PrimaryKey {
            get {
                return this.primaryKey;
            }

            set {
                if (this.primaryKey != value) {
                    this.primaryKey = value;
                    this.Selected = true;
                    this.OnPropertyChanged(() => PrimaryKey);
                }
            }
        }

        #endregion

        #region Generated

        private bool generated;

        [XmlAttribute]
        public bool Generated {
            get {
                return this.generated;
            }

            set {
                if (this.generated != value) {
                    this.generated = value;
                    this.OnPropertyChanged(() => Generated);
                }
            }
        }

        #endregion

        #region Nullable

        private bool nullable;

        [XmlAttribute]
        public bool Nullable {
            get {
                return this.nullable;
            }

            set {
                if (this.nullable != value) {
                    this.nullable = value;
                    this.OnPropertyChanged(() => Nullable);
                }
            }
        }

        #endregion

        #region MaxLength

        private long maxLength;

        [XmlAttribute]
        public long MaxLength {
            get {
                return this.maxLength;
            }

            set {
                if (this.maxLength != value) {
                    this.maxLength = value;
                    this.OnPropertyChanged(() => MaxLength);
                }
            }
        }

        #endregion

        #region Precision

        private short precision;

        [XmlAttribute]
        public short Precision {
            get {
                return this.precision;
            }

            set {
                if (this.precision != value) {
                    this.precision = value;
                    this.OnPropertyChanged(() => Precision);
                }
            }
        }

        #endregion

        #region Scale

        private short scale;

        [XmlAttribute]
        public short Scale {
            get {
                return this.scale;
            }

            set {
                if (this.scale != value) {
                    this.scale = value;
                    this.OnPropertyChanged(() => Scale);
                }
            }
        }

        #endregion

        public Column() {
        }

        public Column(string columnName) {
            this.ColumnName = columnName;
            this.PropertyName = columnName;
        }

        internal Column(IColumn column) {
            this.ColumnName = column.ColumnName;
            this.PropertyName = column.PropertyName;
            this.PropertyType = column.PropertyType;
            this.PrimaryKey = column.PrimaryKey;
            this.Generated = column.Generated;
            this.MaxLength = column.MaxLength ?? default(long);
            this.Precision = column.Precision ?? default(short);
            this.Scale = column.Scale ?? default(short);
            this.DbType = column.DbType;
            this.DefaultValue = column.DefaultValue;
            this.Nullable = column.Nullable;
        }

        public bool ShouldInclude() {
            return this.Selected
                        && !string.IsNullOrEmpty(this.ColumnName)
                        && !string.IsNullOrEmpty(this.PropertyName)
                        && !string.IsNullOrEmpty(this.PropertyType);
        }

        private string DebuggerDisplay() {
            return string.Format("ColumnName = {0} | PropertyName = {1} | PropertyType = {2} | DbType = {3} | DefaultValue = {4} | Selected = {5} | PrimaryKey = {6} | Generated = {7} | Nullable = {8} | Precision = {9} | Scale = {10}",
                this.ColumnName, this.PropertyName, this.PropertyType, this.DbType, this.DefaultValue, this.Selected, this.PrimaryKey, this.Generated, this.Nullable, this.Precision, this.Scale);
        }
    }
}
