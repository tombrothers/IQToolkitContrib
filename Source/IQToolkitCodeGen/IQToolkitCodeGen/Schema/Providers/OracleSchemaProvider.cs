using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using IQToolkitCodeGen.Model;
using System.Text;
using System.Diagnostics;
using System.Reflection;

namespace IQToolkitCodeGen.Schema.Provider {
    [Export(typeof(ISchemaProvider))]
    internal class OracleSchemaProvider : SchemaProviderBase
    {
        public OracleSchemaProvider()
            : base(SchemaProviderName.Oracle, SchemaProviderType.Oracle)
        {
        }

        public override List<Association> GetAssociationList()
        {
            string sql = string.Empty;

            Properties.Settings config = new Properties.Settings();

            if (config.Oracle_FKSql.Length > 0)
            {
                sql = config.Oracle_FKSql;
            }
            else
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
            return (from column in columns.AsEnumerable()
                    from dataType in dataTypes.AsEnumerable()
                    where column.Field<string>("DATATYPE").Equals(dataType.Field<string>("TypeName"), StringComparison.InvariantCultureIgnoreCase)
                    orderby this.GetTableName(column), column.Field<string>("COLUMN_NAME")
                    select new ColumnMetaInfo
                    {
                        TableName = this.GetTableName(column),
                        ColumnName = column.Field<string>("COLUMN_NAME"),
                        DbType = this.GetDbType(dataType, column),
                        DataType = Type.GetType(dataType.Field<string>("DATATYPE")),
                        IsNullable = this.IsNullable(column),
                        IsGenerated = this.IsGenerated(column),
                        MaxLength = this.GetMaxLength(column),
                        Precision = this.GetPrecision(column),
                        Scale = this.GetScale(column)
                    }).ToList();
        }

        protected override DataTable GetTablesSchema()
        {
            DataTable tables = base.GetTablesSchema();
            
            string rowFilter = GetTableRowFilter();

            if (rowFilter != string.Empty)
            {
                tables.DefaultView.RowFilter = rowFilter;
                return tables.DefaultView.ToTable();
            }
            
            return tables;
        }

        protected string GetTableRowFilter() 
        {
            string rowFilter = string.Empty;

            Properties.Settings config = new Properties.Settings();

            if (config.tables_DefaultView_RowFilter.Length > 0)
            {
                rowFilter = config.tables_DefaultView_RowFilter;
            }

            return rowFilter;
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

        protected virtual int GetProviderDbType(DataRow dr)
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
                case 1://BFile = 1,
                case 2://Blob = 2,
                    return SqlDbType.Binary;
                case 3://Char = 3,
                    return SqlDbType.Char;
                case 4://Clob = 4,
                    return SqlDbType.NText;
                case 6://DateTime = 6,
                    return SqlDbType.DateTime;
                case 9://LongRaw = 9,
                    return SqlDbType.VarBinary;
                case 10://LongVarChar = 10,
                    return SqlDbType.NVarChar;
                case 11://NChar = 11,
                    return SqlDbType.NChar;
                case 12://NClob = 12,
                    return SqlDbType.NText;
                case 13://Number = 13,
                    return SqlDbType.Decimal;
                case 14://NVarChar = 14,
                    return SqlDbType.NVarChar;
                case 15://Raw = 15,
                    return SqlDbType.VarBinary;
                case 16://RowId = 16,
                    return SqlDbType.NVarChar;
                case 22://VarChar = 22,
                    return SqlDbType.VarChar;
                case 23://Byte = 23,
                    return SqlDbType.TinyInt;
                case 24://UInt16 = 24,
                    return SqlDbType.SmallInt;
                case 25://UInt32 = 25,
                    return SqlDbType.Int;
                case 26://SByte = 26,
                    return SqlDbType.TinyInt;
                case 27://Int16 = 27,
                    return SqlDbType.SmallInt;
                case 28://Int32 = 28,
                    return SqlDbType.Int;
                case 29://Float = 29,
                    return SqlDbType.Real;
                case 30://Double = 30,
                    return SqlDbType.Float;
                //
                // Summary:
                //     An Oracle REF CURSOR. The System.Data.OracleClient.OracleDataReader object
                //     is not available.
                case 5://Cursor = 5,
                //
                // Summary:
                //     An Oracle INTERVAL DAY TO SECOND data type (Oracle 9i or later) that contains
                //     an interval of time in days, hours, minutes, and seconds, and has a fixed
                //     size of 11 bytes. Use the .NET Framework System.TimeSpan or OracleClient
                //     System.Data.OracleClient.OracleTimeSpan data type in System.Data.OracleClient.OracleParameter.Value.
                case 7://IntervalDayToSecond = 7,
                //
                // Summary:
                //     An Oracle INTERVAL YEAR TO MONTH data type (Oracle 9i or later) that contains
                //     an interval of time in years and months, and has a fixed size of 5 bytes.
                //     Use the .NET Framework System.Int32 or OracleClient System.Data.OracleClient.OracleMonthSpan
                //     data type in System.Data.OracleClient.OracleParameter.Value.
                case 8://IntervalYearToMonth = 8,
                //
                // Summary:
                //     An Oracle TIMESTAMP (Oracle 9i or later) that contains date and time (including
                //     seconds), and ranges in size from 7 to 11 bytes. Use the .NET Framework System.DateTime
                //     or OracleClient System.Data.OracleClient.OracleDateTime data type in System.Data.OracleClient.OracleParameter.Value.
                case 18://Timestamp = 18,
                //
                // Summary:
                //     An Oracle TIMESTAMP WITH LOCAL TIMEZONE (Oracle 9i or later) that contains
                //     date, time, and a reference to the original time zone, and ranges in size
                //     from 7 to 11 bytes. Use the .NET Framework System.DateTime or OracleClient
                //     System.Data.OracleClient.OracleDateTime data type in System.Data.OracleClient.OracleParameter.Value.
                case 19://TimestampLocal = 19,
                //
                // Summary:
                //     An Oracle TIMESTAMP WITH TIMEZONE (Oracle 9i or later) that contains date,
                //     time, and a specified time zone, and has a fixed size of 13 bytes. Use the
                //     .NET Framework System.DateTime or OracleClient System.Data.OracleClient.OracleDateTime
                //     data type in System.Data.OracleClient.OracleParameter.Value.
                case 20://TimestampWithTZ = 20,
                    Debugger.Log(1, "GetSqlDbType", "Missing DataType: " + providerDbType);
                    return SqlDbType.Variant;
                default:
                    throw new InvalidOperationException(string.Format("Unhandled sql type: {0}", providerDbType));
            }
        
        }
    
    
    }

}    
    