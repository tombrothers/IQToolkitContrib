using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Model;
using System.Threading;
using System.Text;
using System.Reflection;

namespace IQToolkitCodeGen.Schema {
    public abstract class SchemaProviderBase : ISchemaProvider {
        public string ProviderName { get; private set; }
        public string ProviderType { get; private set; }
        public DbProviderFactory Factory { get; private set; }
        public DbConnection Connection { get; private set; }

        protected SchemaProviderBase(string providerName, string providerType) {
            this.ProviderName = providerName;
            this.ProviderType = providerType;
        }

        public virtual void SetConnectionString(string connectionString) {
            this.Factory = DbProviderFactories.GetFactory(this.ProviderType);
            this.Connection = this.Factory.CreateConnection();
            this.Connection.ConnectionString = connectionString;
        }

        public abstract List<Association> GetAssociationList();

        protected void AddAssociationListItems(List<Association> list) {
            list.AddRange((from item in list
                           select new Association {
                               AssociationName = item.AssociationName + "List",
                               TableName = item.RelatedTableName,
                               ColumnName = item.RelatedColumnName,
                               RelatedTableName = item.TableName,
                               RelatedColumnName = item.ColumnName,
                               AssociationType = AssociationType.List
                           }).ToList());
        }

        public virtual List<PrimaryKey> GetPrimaryKeyList(string tableName) {
            List<PrimaryKey> primaryKeys = new List<PrimaryKey>();
            DataTable schemaDataTable = new DataTable();

            try {
                using (DbDataAdapter da = this.Factory.CreateDataAdapter()) {
                    DbCommand command = this.Connection.CreateCommand();
                    command.CommandText = string.Format("select * from {0} where 1=2", this.GetQuotedName(tableName)); ;
                    da.SelectCommand = command;
                    da.FillSchema(schemaDataTable, SchemaType.Source);
                }

                for (int index = 0, total = schemaDataTable.PrimaryKey.Length; index < total; index++) {
                    primaryKeys.Add(new PrimaryKey {
                        ColumnName = schemaDataTable.PrimaryKey[index].ColumnName,
                        AutoIncrement = schemaDataTable.PrimaryKey[index].AutoIncrement
                    });
                }
            }
            catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }

            return primaryKeys;
        }

        protected virtual string GetQuotedName(string name) {
            return "[" + name + "]";
        }

        public virtual List<ColumnMetaInfo> GetColumnMetaInfo() {
            DataTable columns = null;
            DataTable dataTypes = null;

            this.Connection.DoConnected(() => {
                columns = this.Connection.GetSchema("Columns");
                dataTypes = this.Connection.GetSchema("DataTypes");
            });

            return this.GetColumnMetaInfo(columns, dataTypes);
        }

        protected virtual List<ColumnMetaInfo> GetColumnMetaInfo(DataTable columns, DataTable dataTypes) {
            return (from column in columns.AsEnumerable()
                    from dataType in dataTypes.AsEnumerable()
                    where column.Field<string>("Data_Type").Equals(dataType.Field<string>("TypeName"), StringComparison.InvariantCultureIgnoreCase)
                    orderby this.GetTableName(column), column.Field<string>("COLUMN_NAME")
                    select new ColumnMetaInfo {
                        TableName = this.GetTableName(column),
                        ColumnName = column.Field<string>("COLUMN_NAME"),
                        DbType = this.GetDbType(dataType, column),
                        DataType = Type.GetType(dataType.Field<string>("DATATYPE")),
                        IsNullable = this.IsNullable(column),
                        IsGenerated = this.IsGenerated(column),
                        MaxLength = this.GetMaxLength(column)
                    }).ToList();
        }


        protected virtual string GetDbType(DataRow dataType, DataRow column) {
            return GetVariableDeclaration(dataType, column, false);
        }

        public virtual string GetVariableDeclaration(DataRow dataType, DataRow column, bool suppressSize) {
            string mappedType = this.GetDataTypeName(dataType);
            int dbType = GetProviderDbType(dataType);

            StringBuilder sb = new StringBuilder();
            long length = this.GetMaxLength(column);
            long precision = this.GetPrecision(column);
            long scale = this.GetScale(column);
            bool nullable = this.IsNullable(column);

            sb.Append(mappedType.ToString().ToUpper());
            if (length > 0 && IsVariableLength(dbType) && !suppressSize) {
                if (length == Int32.MaxValue) {
                    sb.Append("(max)");
                }
                else {
                    sb.AppendFormat("({0})", length);
                }
            }
            else if (precision != 0) {
                if (scale != 0) {
                    sb.AppendFormat("({0},{1})", precision, scale);
                }
                else {
                    sb.AppendFormat("({0})", precision);
                }
            }

            if (!nullable) {
                sb.AppendFormat(" NOT NULL");
            }

            return sb.ToString();
        }

        protected virtual int GetProviderDbType(DataRow dr) {
            object value = dr["ProviderDbType"];

            if (Enum.IsDefined(typeof(DbType), value)) {
                return (int)value;
            }
            else { return (int)DbType.Object; }
        }

        private PropertyInfo ProviderDbType(DbParameter providerParameter) {
            PropertyInfo providerDbTypeProperty = null;

            Type parameterType = providerParameter.GetType();
            PropertyInfo[] pis =
                parameterType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            foreach (PropertyInfo pi in pis) {
                if (pi.Name.Contains("DbType")) {
                    providerDbTypeProperty = pi;
                    break;
                }
            }
            if (providerDbTypeProperty == null) {
                throw new Exception("couldn't find providers native DbType");
            }

            return providerDbTypeProperty;

        }

        public virtual bool IsVariableLength(SqlDbType dbType) {
            switch (dbType) {
                //case SqlDbType.Image:
                case SqlDbType.NChar:
                //case SqlDbType.NText:
                case SqlDbType.NVarChar:
                case SqlDbType.Text:
                case SqlDbType.VarBinary:
                case SqlDbType.VarChar:
                case SqlDbType.Xml:
                    return true;
                default:
                    return false;
            }
        }

        public virtual bool IsVariableLength(DbType dbType) {

            switch (dbType) {
                case DbType.StringFixedLength:
                case DbType.String:
                case DbType.AnsiString:
                case DbType.AnsiStringFixedLength:
                case DbType.Binary:
                    return true;
                default:
                    return false;
            }
        }

        public virtual bool IsVariableLength(int ProviderDbType) {
            return false;
        }

        protected virtual string GetDataTypeName(DataRow dr) {
            const string COLUMN_NAME = "TypeName";

            if (dr == null || dr.Table == null || !dr.Table.Columns.Contains(COLUMN_NAME) || dr.IsNull(COLUMN_NAME)) {
                return string.Empty;
            }

            return dr[COLUMN_NAME].ToString();
        }

        protected virtual short GetPrecision(DataRow dr) {
            if (dr.IsNull("NUMERIC_PRECISION")) { return 0; }

            string value = dr["NUMERIC_PRECISION"].ToString();
            short precision = 0;
            short.TryParse(value, out precision);
            return precision;
        }

        protected virtual short GetScale(DataRow dr) {
            if (dr.IsNull("NUMERIC_SCALE")) { return 0; }

            string value = dr["NUMERIC_SCALE"].ToString();
            short scale = 0;
            short.TryParse(value, out scale);
            return scale;
        }

        public virtual long GetMaxLength(DataRow dr) {
            const string COLUMN_NAME = "CHARACTER_MAXIMUM_LENGTH";

            if (dr == null || dr.Table == null || !dr.Table.Columns.Contains(COLUMN_NAME) || dr.IsNull(COLUMN_NAME)) {
                return -1;
            }

            string value = dr[COLUMN_NAME].ToString();
            long maxLength = -1;
            long.TryParse(value, out maxLength);

            return maxLength;
        }

        public virtual bool IsNullable(DataRow dr) {
            const string COLUMN_NAME = "IS_NULLABLE";

            if (dr == null || dr.Table == null || !dr.Table.Columns.Contains(COLUMN_NAME) || dr.IsNull(COLUMN_NAME)) {
                return false;
            }

            if (dr.Table.Columns[COLUMN_NAME].DataType == typeof(bool)) {
                return dr.Field<bool>(COLUMN_NAME);
            }

            return dr.Field<string>(COLUMN_NAME).Equals("YES", StringComparison.InvariantCultureIgnoreCase);
        }

        public virtual bool IsGenerated(DataRow dr) {
            return false;
        }

        public virtual List<string> GetTableNames() {
            DataTable tables = this.GetTablesSchema();

            return (from row in tables.AsEnumerable()
                    orderby this.GetTableName(row)
                    select this.GetTableName(row)).ToList();
        }

        protected virtual DataTable GetTablesSchema() {
            DataTable tables = null;

            this.Connection.DoConnected(() => {
                tables = this.Connection.GetSchema("Tables");
            });

            return tables;
        }

        protected virtual string GetTableName(DataRow dr) {
            return dr.Field<string>("TABLE_NAME").ToProper();
        }

        protected DataTable GetDataTable(string sql) {
            DataTable dataTable = new DataTable();

            using (DbDataAdapter adapter = this.Factory.CreateDataAdapter()) {
                using (DbCommand command = this.Connection.CreateCommand()) {
                    command.CommandText = sql;
                    adapter.SelectCommand = command;
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

    }
}
