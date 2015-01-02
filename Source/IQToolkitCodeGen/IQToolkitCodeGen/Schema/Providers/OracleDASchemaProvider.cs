using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using IQToolkitCodeGen.Model;
using System.Text;
using System.Diagnostics;
using System.Reflection;


namespace IQToolkitCodeGen.Schema.Provider {
    [Export(typeof(ISchemaProvider))]
    internal class OracleDASchemaProvider : SchemaProviderBase
    {
        public OracleDASchemaProvider()
            : base(SchemaProviderName.OracleDA, SchemaProviderType.OracleDA)
        {
        }

        public override List<Association> GetAssociationList()
        {
            string sql = base.GetForeignKeySql();

            if (sql == string.Empty)
            {
                
                    sql = @"SELECT UC.CONSTRAINT_NAME                     AS FOREIGNKEY,
                  concat(concat(UC.owner,'.'), UC.TABLE_NAME) AS TABLENAME,
                  UCC.COLUMN_NAME                             AS ColumnName,
                  concat(concat(UC.owner,'.'), RT.TABLE_NAME) AS RELATEDTABLENAME,
                  RTC.column_name                             AS RelatedColumnName
                FROM SYS.ALL_CONSTRAINTS UC,
                  SYS.ALL_CONSTRAINTS RT,
                  SYS.ALL_CONS_COLUMNS RTC,
                  SYS.ALL_CONS_COLUMNS UCC
                WHERE UC.R_CONSTRAINT_NAME = RT.CONSTRAINT_NAME
                AND UC.R_OWNER             = RT.OWNER
                AND UCC.constraint_name    = UC.CONSTRAINT_NAME
                AND RTC.constraint_name    = RT.CONSTRAINT_NAME
                AND (UC.CONSTRAINT_TYPE    = 'R')
                AND (UC.OWNER              in (select user from dual))";
                
            }
            DataTable dataTable = this.GetDataTable(sql);
            List<Association> associations = new List<Association>();

            associations.AddRange((from row in dataTable.AsEnumerable()
                                   select new Association
                                   {
                                       AssociationName = row.Field<string>("ForeignKey"),
                                       TableName = row.Field<string>("TableName"),
                                       ColumnName = row.Field<string>("ColumnName"),
                                       RelatedTableName = row.Field<string>("RelatedTableName"),
                                       RelatedColumnName = row.Field<string>("RelatedColumnName"),
                                       AssociationType = AssociationType.Item
                                   }).ToList());

            this.AddAssociationListItems(associations);

            return associations;
        }

        public DataTable GetDefaultData()
        {

            string sql = base.GetColumnMetaInfoExSql();

            if (sql == string.Empty)
            {
                
                sql = @"SELECT OWNER,
                    TABLE_NAME,
                    COLUMN_NAME,
                    DATA_DEFAULT
                FROM all_tab_columns
                WHERE
                (OWNER              in (select user from dual))";
            }

            DataTable dataTable = this.GetDataTableEx(sql);
            return dataTable;

        }

        protected override string GetQuotedName(string name)
        {
            string quotedname = string.Empty;
            if (name.Contains("."))
            {
                string[] nameParts = name.Split('.');

                foreach (string part in nameParts)
                {
                    quotedname += string.Format("\"{0}\".", part);
                }

                if (quotedname.EndsWith(".")) { quotedname = quotedname.Substring(0, quotedname.Length - 1); }

            }
            else { quotedname = name; }


            return quotedname;
            //return name;
        }

        protected override List<ColumnMetaInfo> GetColumnMetaInfo(DataTable columns, DataTable dataTypes)
        {
            List<ColumnMetaInfo> columnmetainfo = null;

            columnmetainfo = (from column in columns.AsEnumerable()
                    from dataType in dataTypes.AsEnumerable()
                    where column.Field<string>("DATATYPE").Equals(dataType.Field<string>("TypeName"), StringComparison.InvariantCultureIgnoreCase)
                    orderby this.GetTableName(column), column.Field<string>("COLUMN_NAME")
                    select new ColumnMetaInfo
                    {
                        TableName = this.GetTableName(column),
                        ColumnName = column.Field<string>("COLUMN_NAME"),
                        DbType = this.GetDbType(dataType, column),
                        NativeDbType = this.GetDataTypeName(dataType),
                        DataType = Type.GetType(dataType.Field<string>("DATATYPE")),
                        IsNullable = this.IsNullable(column),
                        IsGenerated = this.IsGenerated(column),
                        MaxLength = this.GetMaxLength(column),
                        Precision = this.GetPrecision(column),
                        Scale = this.GetScale(column)
                    }).ToList();


            return GetColumnMetaInfoExtended( columns, dataTypes, columnmetainfo);
        }

        private List<ColumnMetaInfo> GetColumnMetaInfoExtended(DataTable columns, DataTable dataTypes, List<ColumnMetaInfo> columnmetainfo)
        {

            DataTable columnsEx = this.GetDefaultData();

            List<ColumnMetaInfo> columnmetainfoEx = null;

            columnmetainfoEx = (from c in columnmetainfo.AsEnumerable()
                                join cEx in columnsEx.AsEnumerable()
                                on c.TableName equals this.GetTableName(cEx) into cG
                                from cEx in cG.Where(cEx => cEx.Field<string>("COLUMN_NAME") == c.ColumnName).DefaultIfEmpty()
                                orderby c.TableName, c.ColumnName
                                select new ColumnMetaInfo
                                {
                                    TableName = c.TableName,
                                    ColumnName = c.ColumnName,
                                    DbType = c.DbType,
                                    NativeDbType = c.NativeDbType,
                                    DataType = c.DataType,
                                    IsNullable = c.IsNullable,
                                    IsGenerated = c.IsGenerated,
                                    DefaultValue = this.GetDefaultValue(cEx),
                                    MaxLength = c.MaxLength,
                                    Precision = c.Precision,
                                    Scale = c.Scale
                                }).ToList();

            return columnmetainfoEx;
        }

        protected override DataTable GetTablesSchema()
        {
            DataTable tables = base.GetTablesSchema();
            
            string rowFilter = base.GetTableRowFilter();

            if (rowFilter != string.Empty)
            {
                tables.DefaultView.RowFilter = rowFilter;
                return tables.DefaultView.ToTable();
            }
            
            return tables;
        }

        protected override DataTable GetColumnsSchema()
        {
            DataTable tables = base.GetColumnsSchema();

            string rowFilter = base.GetColumnMetaInfoRowFilter();

            if (rowFilter != string.Empty)
            {
                tables.DefaultView.RowFilter = rowFilter;
                return tables.DefaultView.ToTable();
            }

            return tables;
        }

        public override List<string> GetTableNames()
        {
            DataTable tables = this.GetTablesSchema();
               
            var tableNames = (from row in tables.AsEnumerable()
                              orderby this.GetTableName(row)
                              select this.GetTableName(row)).ToList();
            
            return tableNames;
        }

        protected override string GetTableName(DataRow dr)
        {
            return string.Format("{0}.{1}",
                                            dr.Field<string>("OWNER"),
                                            dr.Field<string>("TABLE_NAME"));
        }

        protected string GetDefaulted(DataTable columns, DataRow row)
        {

            var dataRows = (from column in columns.AsEnumerable()
                            where column.Field<string>("OWNER").Equals(row["OWNER"].ToString())
                                    && column.Field<string>("TABLE_NAME").Equals(row["TABLE_NAME"].ToString())
                                    && column.Field<string>("COLUMN_NAME").Equals(row["COLUMN_NAME"].ToString())
                            select column).ToList();

            if (dataRows.Count > 0)
            {
                return this.GetDefaultValue(dataRows[0]);
            }

            return string.Empty;
        }

        protected override string GetDefaultValue(DataRow dr)
        {
            const string COLUMN_NAME = "DATA_DEFAULT";

            if (dr == null || dr.Table == null || !dr.Table.Columns.Contains(COLUMN_NAME) || dr.IsNull(COLUMN_NAME))
            {
                return string.Empty;
            }

            return dr[COLUMN_NAME].ToString();
        }

        protected override short GetPrecision(DataRow dr)
        {
            if (dr.IsNull("PRECISION")) { return 0; }

            string value = dr["PRECISION"].ToString();
            short precision = 0;
            short.TryParse(value, out precision);
            return precision;
        }

        protected override short GetScale(DataRow dr)
        {
            if (dr.IsNull("SCALE")) { return 0; }

            string value = dr["SCALE"].ToString();
            short scale = 0;
            short.TryParse(value, out scale);
            return scale;
        }

        public override long GetMaxLength(DataRow dr)
        {
            if (dr.IsNull("LENGTH"))
            {
                return -1;
            }

            string value = dr["LENGTH"].ToString();
            long maxLength = -1;
            long.TryParse(value, out maxLength);
            return maxLength;
        }

        public override bool IsNullable(DataRow dr)
        {
            const string COLUMN_NAME = "NULLABLE";

            if (dr == null || dr.Table == null || !dr.Table.Columns.Contains(COLUMN_NAME) || dr.IsNull(COLUMN_NAME))
            {
                return false;
            }

            return dr[COLUMN_NAME].ToString().Equals("Y", StringComparison.InvariantCultureIgnoreCase);
        }

        public override bool IsVariableLength(int ProviderDbType)
        {
            switch (ProviderDbType)
            {
                case 9: //LongRaw = 9
                case 10: //LongVarChar = 10
                case 11: //NChar = 11
                //case 13: //Number = 13
                case 14: //NVarChar = 14
                case 15: //Raw = 15
                case 22: //VarChar = 22
                    return true;
                default:
                    return false;
            }
        }

        public override string GetVariableDeclaration(DataRow dataType, DataRow column, bool suppressSize)
        {
            string mappedType = this.GetDataTypeName(dataType);
            int dbType = GetProviderDbType(dataType);
            SqlDbType sqldbtype = GetSqlDbType(dbType);

            string sqldbtypeName = Enum.GetName(typeof(SqlDbType), sqldbtype);

            StringBuilder sb = new StringBuilder();
            long length = this.GetMaxLength(column);
            long precision = this.GetPrecision(column);
            long scale = this.GetScale(column);
            bool nullable = this.IsNullable(column);

            //sb.Append(mappedType.ToString().ToUpper());
            sb.Append(sqldbtypeName);
            if (length > 0 && IsVariableLength(dbType) && !suppressSize)
            {
                if (length == Int32.MaxValue)
                {
                    sb.Append("(max)");
                }
                else
                {
                    sb.AppendFormat("({0})", length);
                }
            }
            else if (precision != 0)
            {
                if (scale != 0)
                {
                    sb.AppendFormat("({0},{1})", precision, scale);
                }
                else
                {
                    sb.AppendFormat("({0})", precision);
                }
            }

            if (!nullable)
            {
                sb.AppendFormat(" NOT NULL");
            }

            return sb.ToString();
        }

        protected override int GetProviderDbType(DataRow dr)
        {
            if (dr.IsNull("ProviderDbType"))
            {
                return -1;
            }

            string value = dr["ProviderDbType"].ToString();
            int providerdbtype = -1;
            int.TryParse(value, out providerdbtype);
            return providerdbtype;

        }

        protected SqlDbType GetSqlDbType(int providerDbType) 
        {
            
            switch (providerDbType)
            {
                case 101://BFile = 101,
                case 102://Blob = 102,
                    return SqlDbType.Binary;
                case 103://Byte = 103,
                    return SqlDbType.TinyInt;
                case 104://Char = 104,
                    return SqlDbType.Char;
                case 105://Clob = 105,
                    return SqlDbType.Text;
                case 106://DateTime = 106,
                    return SqlDbType.DateTime;
                case 107://Decimal = 107,
                    return SqlDbType.Decimal;
                case 108://Double = 108,
                    return SqlDbType.Float;
                case 109://Long = 109,
                    return SqlDbType.Text;
                case 110://LongRaw = 110,
                    return SqlDbType.VarBinary;
                case 111://Int16 = 111,
                    return SqlDbType.SmallInt;
                case 112://Int32 = 112,
                    return SqlDbType.Int;
                case 113://Int64 = 113,
                    return SqlDbType.BigInt;
                case 114://IntervalDS = 114,
                case 115://IntervalYM = 115,
                    return SqlDbType.Timestamp;
                case 116://NClob = 116,
                    return SqlDbType.NText;
                case 117://NChar = 117,
                    return SqlDbType.NChar;
                case 119://NVarchar2 = 119,
                    return SqlDbType.NVarChar;
                case 120://Raw = 120,
                    return SqlDbType.VarBinary;
                case 121://RefCursor = 121,
                    return SqlDbType.Variant;
                case 122://Single = 122,
                    return SqlDbType.Real;
                case 123://TimeStamp = 123,
                case 124://TimeStampLTZ = 124,
                case 125://TimeStampTZ = 125,
                    return SqlDbType.DateTime;
                case 126://VarChar2 = 126,
                    return SqlDbType.VarChar;
                case 127://XmlType = 127,
                    return SqlDbType.Xml;
                case 128://Array = 128,
                case 129://Object = 129,
                case 130://Ref = 130,
                    return SqlDbType.Variant;
                case 132://BinaryDouble = 132,
                    return SqlDbType.Float;
                case 133://BinaryFloat = 133,
                    return SqlDbType.Real;
                
                default:
                    throw new InvalidOperationException(string.Format("Unhandled sql type: {0}", providerDbType));
            }
        
        }

        protected DataTable GetDataTableEx(string sql)
        {
            DataTable dataTable = new DataTable();

            using (DbDataAdapter adapter = this.Factory.CreateDataAdapter())
            {
                using (DbCommand command = this.Connection.CreateCommand())
                {
                    SetInitialLONGFetchSize(command, -1);
                    command.CommandText = sql;
                    
                    adapter.SelectCommand = command;
                    adapter.Fill(dataTable);
                }
            }

            return dataTable;
        }

        public static void SetInitialLONGFetchSize(DbCommand oracleCommand, object value)
        {
            if (oracleCommand == null) return;
            var initialLONGFetchSizeProperty = oracleCommand.GetType().GetProperty("InitialLONGFetchSize");
            if (initialLONGFetchSizeProperty != null)
                initialLONGFetchSizeProperty.SetValue(oracleCommand, value, null);
        }
    
    
    }

}    
    