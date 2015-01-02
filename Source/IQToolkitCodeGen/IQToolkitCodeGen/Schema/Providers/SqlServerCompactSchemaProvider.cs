using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;
using System.IO;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Schema.Provider {
    [Export(typeof(ISchemaProvider))]
    internal class SqlServerCompactSchemaProvider : SchemaProviderBase {
        public SqlServerCompactSchemaProvider()
            : base(SchemaProviderName.SqlServerCE, SchemaProviderType.SqlServerCE) {
        }

        public override List<Association> GetAssociationList() {
            return new List<Association>();
        }

        public override List<ColumnMetaInfo> GetColumnMetaInfo() {
            DataTable columns = this.GetDataTable("select * from information_schema.columns");
            DataTable dataTypes = this.GetDataTypes();

            return this.GetColumnMetaInfo(columns, dataTypes);
        }

        // generated this from sql server
        private DataTable GetDataTypes() {
            DataTable dataTable = new DataTable();

            #region columns

            dataTable.Columns.Add("TypeName", typeof(System.String));
            dataTable.Columns.Add("ProviderDbType", typeof(System.Int32));
            dataTable.Columns.Add("ColumnSize", typeof(System.Int64));
            dataTable.Columns.Add("CreateFormat", typeof(System.String));
            dataTable.Columns.Add("CreateParameters", typeof(System.String));
            dataTable.Columns.Add("DataType", typeof(System.String));
            dataTable.Columns.Add("IsAutoIncrementable", typeof(System.Boolean));
            dataTable.Columns.Add("IsBestMatch", typeof(System.Boolean));
            dataTable.Columns.Add("IsCaseSensitive", typeof(System.Boolean));
            dataTable.Columns.Add("IsFixedLength", typeof(System.Boolean));
            dataTable.Columns.Add("IsFixedPrecisionScale", typeof(System.Boolean));
            dataTable.Columns.Add("IsLong", typeof(System.Boolean));
            dataTable.Columns.Add("IsNullable", typeof(System.Boolean));
            dataTable.Columns.Add("IsSearchable", typeof(System.Boolean));
            dataTable.Columns.Add("IsSearchableWithLike", typeof(System.Boolean));
            dataTable.Columns.Add("IsUnsigned", typeof(System.Boolean));
            dataTable.Columns.Add("MaximumScale", typeof(System.Int16));
            dataTable.Columns.Add("MinimumScale", typeof(System.Int16));
            dataTable.Columns.Add("IsConcurrencyType", typeof(System.Boolean));
            dataTable.Columns.Add("IsLiteralSupported", typeof(System.Boolean));
            dataTable.Columns.Add("LiteralPrefix", typeof(System.String));
            dataTable.Columns.Add("LiteralSuffix", typeof(System.String));

            #endregion columns

            #region rows

            DataRow row;

            #region row 0

            row = dataTable.NewRow();
            row["TypeName"] = "smallint";
            row["ProviderDbType"] = 16;
            row["ColumnSize"] = 5;
            row["CreateFormat"] = "smallint";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Int16";
            row["IsAutoIncrementable"] = true;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = true;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = false;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 1

            row = dataTable.NewRow();
            row["TypeName"] = "int";
            row["ProviderDbType"] = 8;
            row["ColumnSize"] = 10;
            row["CreateFormat"] = "int";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Int32";
            row["IsAutoIncrementable"] = true;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = true;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = false;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 2

            row = dataTable.NewRow();
            row["TypeName"] = "real";
            row["ProviderDbType"] = 13;
            row["ColumnSize"] = 7;
            row["CreateFormat"] = "real";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Single";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = false;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 3

            row = dataTable.NewRow();
            row["TypeName"] = "float";
            row["ProviderDbType"] = 6;
            row["ColumnSize"] = 53;
            row["CreateFormat"] = "float({0})";
            row["CreateParameters"] = "number of bits used to store the mantissa";
            row["DataType"] = "System.Double";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = false;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 4

            row = dataTable.NewRow();
            row["TypeName"] = "money";
            row["ProviderDbType"] = 9;
            row["ColumnSize"] = 19;
            row["CreateFormat"] = "money";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Decimal";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = false;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = true;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = false;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 5

            row = dataTable.NewRow();
            row["TypeName"] = "smallmoney";
            row["ProviderDbType"] = 17;
            row["ColumnSize"] = 10;
            row["CreateFormat"] = "smallmoney";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Decimal";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = false;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = true;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = false;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 6

            row = dataTable.NewRow();
            row["TypeName"] = "bit";
            row["ProviderDbType"] = 2;
            row["ColumnSize"] = 1;
            row["CreateFormat"] = "bit";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Boolean";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = false;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 7

            row = dataTable.NewRow();
            row["TypeName"] = "tinyint";
            row["ProviderDbType"] = 20;
            row["ColumnSize"] = 3;
            row["CreateFormat"] = "tinyint";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.SByte";
            row["IsAutoIncrementable"] = true;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = true;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = true;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 8

            row = dataTable.NewRow();
            row["TypeName"] = "bigint";
            row["ProviderDbType"] = 0;
            row["ColumnSize"] = 19;
            row["CreateFormat"] = "bigint";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Int64";
            row["IsAutoIncrementable"] = true;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = true;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = false;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 9

            row = dataTable.NewRow();
            row["TypeName"] = "timestamp";
            row["ProviderDbType"] = 19;
            row["ColumnSize"] = 8;
            row["CreateFormat"] = "timestamp";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Byte[]";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = false;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = false;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = true;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "0x";
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 10

            row = dataTable.NewRow();
            row["TypeName"] = "binary";
            row["ProviderDbType"] = 1;
            row["ColumnSize"] = 8000;
            row["CreateFormat"] = "binary({0})";
            row["CreateParameters"] = "length";
            row["DataType"] = "System.Byte[]";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "0x";
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 11

            row = dataTable.NewRow();
            row["TypeName"] = "image";
            row["ProviderDbType"] = 7;
            row["ColumnSize"] = 2147483647;
            row["CreateFormat"] = "image";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Byte[]";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = true;
            row["IsNullable"] = true;
            row["IsSearchable"] = false;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "0x";
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 12

            row = dataTable.NewRow();
            row["TypeName"] = "text";
            row["ProviderDbType"] = 18;
            row["ColumnSize"] = 2147483647;
            row["CreateFormat"] = "text";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.String";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = true;
            row["IsNullable"] = true;
            row["IsSearchable"] = false;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "'";
            row["LiteralSuffix"] = "'";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 13

            row = dataTable.NewRow();
            row["TypeName"] = "ntext";
            row["ProviderDbType"] = 11;
            row["ColumnSize"] = 1073741823;
            row["CreateFormat"] = "ntext";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.String";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = true;
            row["IsNullable"] = true;
            row["IsSearchable"] = false;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "N'";
            row["LiteralSuffix"] = "'";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 14

            row = dataTable.NewRow();
            row["TypeName"] = "decimal";
            row["ProviderDbType"] = 5;
            row["ColumnSize"] = 38;
            row["CreateFormat"] = "decimal({0}, {1})";
            row["CreateParameters"] = "precision,scale";
            row["DataType"] = "System.Decimal";
            row["IsAutoIncrementable"] = true;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = false;
            row["MaximumScale"] = 38;
            row["MinimumScale"] = 0;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 15

            row = dataTable.NewRow();
            row["TypeName"] = "numeric";
            row["ProviderDbType"] = 5;
            row["ColumnSize"] = 38;
            row["CreateFormat"] = "numeric({0}, {1})";
            row["CreateParameters"] = "precision,scale";
            row["DataType"] = "System.Decimal";
            row["IsAutoIncrementable"] = true;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = false;
            row["MaximumScale"] = 38;
            row["MinimumScale"] = 0;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 16

            row = dataTable.NewRow();
            row["TypeName"] = "datetime";
            row["ProviderDbType"] = 4;
            row["ColumnSize"] = 23;
            row["CreateFormat"] = "datetime";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.DateTime";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "{ts '";
            row["LiteralSuffix"] = "'}";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 17

            row = dataTable.NewRow();
            row["TypeName"] = "smalldatetime";
            row["ProviderDbType"] = 15;
            row["ColumnSize"] = 16;
            row["CreateFormat"] = "smalldatetime";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.DateTime";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "{ts '";
            row["LiteralSuffix"] = "'}";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 18

            row = dataTable.NewRow();
            row["TypeName"] = "sql_variant";
            row["ProviderDbType"] = 23;
            row["ColumnSize"] = DBNull.Value;
            row["CreateFormat"] = "sql_variant";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Object";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = false;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 19

            row = dataTable.NewRow();
            row["TypeName"] = "xml";
            row["ProviderDbType"] = 25;
            row["ColumnSize"] = 2147483647;
            row["CreateFormat"] = "xml";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.String";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = false;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = true;
            row["IsNullable"] = true;
            row["IsSearchable"] = false;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = false;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 20

            row = dataTable.NewRow();
            row["TypeName"] = "varchar";
            row["ProviderDbType"] = 22;
            row["ColumnSize"] = 2147483647;
            row["CreateFormat"] = "varchar({0})";
            row["CreateParameters"] = "max length";
            row["DataType"] = "System.String";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "'";
            row["LiteralSuffix"] = "'";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 21

            row = dataTable.NewRow();
            row["TypeName"] = "char";
            row["ProviderDbType"] = 3;
            row["ColumnSize"] = 2147483647;
            row["CreateFormat"] = "char({0})";
            row["CreateParameters"] = "length";
            row["DataType"] = "System.String";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "'";
            row["LiteralSuffix"] = "'";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 22

            row = dataTable.NewRow();
            row["TypeName"] = "nchar";
            row["ProviderDbType"] = 10;
            row["ColumnSize"] = 1073741823;
            row["CreateFormat"] = "nchar({0})";
            row["CreateParameters"] = "length";
            row["DataType"] = "System.String";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "N'";
            row["LiteralSuffix"] = "'";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 23

            row = dataTable.NewRow();
            row["TypeName"] = "nvarchar";
            row["ProviderDbType"] = 12;
            row["ColumnSize"] = 1073741823;
            row["CreateFormat"] = "nvarchar({0})";
            row["CreateParameters"] = "max length";
            row["DataType"] = "System.String";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "N'";
            row["LiteralSuffix"] = "'";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 24

            row = dataTable.NewRow();
            row["TypeName"] = "varbinary";
            row["ProviderDbType"] = 21;
            row["ColumnSize"] = 1073741823;
            row["CreateFormat"] = "varbinary({0})";
            row["CreateParameters"] = "max length";
            row["DataType"] = "System.Byte[]";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "0x";
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 25

            row = dataTable.NewRow();
            row["TypeName"] = "uniqueidentifier";
            row["ProviderDbType"] = 14;
            row["ColumnSize"] = 16;
            row["CreateFormat"] = "uniqueidentifier";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.Guid";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = false;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "'";
            row["LiteralSuffix"] = "'";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 26

            row = dataTable.NewRow();
            row["TypeName"] = "date";
            row["ProviderDbType"] = 31;
            row["ColumnSize"] = 3;
            row["CreateFormat"] = "date";
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = "System.DateTime";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = false;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = true;
            row["IsFixedPrecisionScale"] = true;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "{ts '";
            row["LiteralSuffix"] = "'}";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 27

            row = dataTable.NewRow();
            row["TypeName"] = "time";
            row["ProviderDbType"] = 32;
            row["ColumnSize"] = 5;
            row["CreateFormat"] = "time({0})";
            row["CreateParameters"] = "scale";
            row["DataType"] = "System.TimeSpan";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = false;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = 7;
            row["MinimumScale"] = 0;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "{ts '";
            row["LiteralSuffix"] = "'}";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 28

            row = dataTable.NewRow();
            row["TypeName"] = "datetime2";
            row["ProviderDbType"] = 33;
            row["ColumnSize"] = 8;
            row["CreateFormat"] = "datetime2({0})";
            row["CreateParameters"] = "scale";
            row["DataType"] = "System.DateTime";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = 7;
            row["MinimumScale"] = 0;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "{ts '";
            row["LiteralSuffix"] = "'}";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 29

            row = dataTable.NewRow();
            row["TypeName"] = "datetimeoffset";
            row["ProviderDbType"] = 34;
            row["ColumnSize"] = 10;
            row["CreateFormat"] = "datetimeoffset({0})";
            row["CreateParameters"] = "scale";
            row["DataType"] = "System.DateTimeOffset";
            row["IsAutoIncrementable"] = false;
            row["IsBestMatch"] = true;
            row["IsCaseSensitive"] = false;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = false;
            row["IsLong"] = false;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = true;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = 7;
            row["MinimumScale"] = 0;
            row["IsConcurrencyType"] = false;
            row["IsLiteralSupported"] = DBNull.Value;
            row["LiteralPrefix"] = "{ts '";
            row["LiteralSuffix"] = "'}";
            dataTable.Rows.Add(row);

            #endregion row

            #region row 30

            row = dataTable.NewRow();
            row["TypeName"] = "Microsoft.SqlServer.Types.SqlHierarchyId, Microsoft.SqlServer.Types, Version=10.0.0.0, PublicKeyToken=89845dcd8080cc91";
            row["ProviderDbType"] = 29;
            row["ColumnSize"] = 892;
            row["CreateFormat"] = DBNull.Value;
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = DBNull.Value;
            row["IsAutoIncrementable"] = DBNull.Value;
            row["IsBestMatch"] = DBNull.Value;
            row["IsCaseSensitive"] = DBNull.Value;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = DBNull.Value;
            row["IsLong"] = DBNull.Value;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = DBNull.Value;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = DBNull.Value;
            row["IsLiteralSupported"] = false;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 31

            row = dataTable.NewRow();
            row["TypeName"] = "Microsoft.SqlServer.Types.SqlGeometry, Microsoft.SqlServer.Types, Version=10.0.0.0, PublicKeyToken=89845dcd8080cc91";
            row["ProviderDbType"] = 29;
            row["ColumnSize"] = -1;
            row["CreateFormat"] = DBNull.Value;
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = DBNull.Value;
            row["IsAutoIncrementable"] = DBNull.Value;
            row["IsBestMatch"] = DBNull.Value;
            row["IsCaseSensitive"] = DBNull.Value;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = DBNull.Value;
            row["IsLong"] = DBNull.Value;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = DBNull.Value;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = DBNull.Value;
            row["IsLiteralSupported"] = false;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #region row 32

            row = dataTable.NewRow();
            row["TypeName"] = "Microsoft.SqlServer.Types.SqlGeography, Microsoft.SqlServer.Types, Version=10.0.0.0, PublicKeyToken=89845dcd8080cc91";
            row["ProviderDbType"] = 29;
            row["ColumnSize"] = -1;
            row["CreateFormat"] = DBNull.Value;
            row["CreateParameters"] = DBNull.Value;
            row["DataType"] = DBNull.Value;
            row["IsAutoIncrementable"] = DBNull.Value;
            row["IsBestMatch"] = DBNull.Value;
            row["IsCaseSensitive"] = DBNull.Value;
            row["IsFixedLength"] = false;
            row["IsFixedPrecisionScale"] = DBNull.Value;
            row["IsLong"] = DBNull.Value;
            row["IsNullable"] = true;
            row["IsSearchable"] = true;
            row["IsSearchableWithLike"] = DBNull.Value;
            row["IsUnsigned"] = DBNull.Value;
            row["MaximumScale"] = DBNull.Value;
            row["MinimumScale"] = DBNull.Value;
            row["IsConcurrencyType"] = DBNull.Value;
            row["IsLiteralSupported"] = false;
            row["LiteralPrefix"] = DBNull.Value;
            row["LiteralSuffix"] = DBNull.Value;
            dataTable.Rows.Add(row);

            #endregion row

            #endregion rows

            return dataTable;
        }

        protected override DataTable GetTablesSchema() {
            return this.GetDataTable("select * from information_schema.tables");
        }

        public override void SetConnectionString(string connectionString) {
            if (string.IsNullOrEmpty(connectionString)) {
                throw new ArgumentException("Invalid connection string.");
            }

            // Assumes that if the connectionString has an equals sign it is considered to be a complete connection string.  Otherwise, it 
            // is either a path to a sdf file.
            if (!connectionString.Contains("=")) {
                if (this.IsValidDataPath(connectionString)) {
                    connectionString = string.Format("Data Source={0}", connectionString);
                }
                else {
                    throw new ApplicationException("Invalid connection string.");
                }
            }

            base.SetConnectionString(connectionString);
        }

        private bool IsValidDataPath(string path) {
            return (Path.GetExtension(path).ToLower() == ".sdf" && File.Exists(path));
        }
    }
}
