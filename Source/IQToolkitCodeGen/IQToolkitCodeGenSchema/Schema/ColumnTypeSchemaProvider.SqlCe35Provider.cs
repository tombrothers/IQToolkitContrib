using System.Data;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class ColumnTypeSchemaProvider {
        private class SqlCe35Provider : Provider {
            protected override DataTable GetDataTable(DbConnection connection) {
                // The DataTable was created using SqlCe40's DataTypes.
                var dataTable = new DataTable();

                #region columns

                dataTable.Columns.Add("TypeName", typeof(System.String));
                dataTable.Columns.Add("DataType", typeof(System.String));
                dataTable.Columns.Add("ProviderDbType", typeof(System.Int32));
                dataTable.Columns.Add("CreateFormat", typeof(System.String));
                dataTable.Columns.Add("IsFixedLength", typeof(System.Boolean));

                #endregion columns

                #region rows

                DataRow row;

                #region row 0

                row = dataTable.NewRow();
                row["TypeName"] = "smallint";
                row["DataType"] = "System.Int16";
                row["ProviderDbType"] = 2;
                row["CreateFormat"] = "smallint";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 1

                row = dataTable.NewRow();
                row["TypeName"] = "int";
                row["DataType"] = "System.Int32";
                row["ProviderDbType"] = 3;
                row["CreateFormat"] = "int";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 2

                row = dataTable.NewRow();
                row["TypeName"] = "real";
                row["DataType"] = "System.Single";
                row["ProviderDbType"] = 4;
                row["CreateFormat"] = "real";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 3

                row = dataTable.NewRow();
                row["TypeName"] = "float";
                row["DataType"] = "System.Double";
                row["ProviderDbType"] = 5;
                row["CreateFormat"] = "float";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 4

                row = dataTable.NewRow();
                row["TypeName"] = "money";
                row["DataType"] = "System.Decimal";
                row["ProviderDbType"] = 6;
                row["CreateFormat"] = "money";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 5

                row = dataTable.NewRow();
                row["TypeName"] = "bit";
                row["DataType"] = "System.Boolean";
                row["ProviderDbType"] = 11;
                row["CreateFormat"] = "bit";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 6

                row = dataTable.NewRow();
                row["TypeName"] = "tinyint";
                row["DataType"] = "System.SByte";
                row["ProviderDbType"] = 17;
                row["CreateFormat"] = "tinyint";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 7

                row = dataTable.NewRow();
                row["TypeName"] = "bigint";
                row["DataType"] = "System.Int64";
                row["ProviderDbType"] = 20;
                row["CreateFormat"] = "bigint";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 8

                row = dataTable.NewRow();
                row["TypeName"] = "uniqueidentifier";
                row["DataType"] = "System.Guid";
                row["ProviderDbType"] = 72;
                row["CreateFormat"] = "uniqueidentifier";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 9

                row = dataTable.NewRow();
                row["TypeName"] = "varbinary";
                row["DataType"] = "System.Byte[]";
                row["ProviderDbType"] = 128;
                row["CreateFormat"] = "varbinary({0})";
                row["IsFixedLength"] = false;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 10

                row = dataTable.NewRow();
                row["TypeName"] = "binary";
                row["DataType"] = "System.Byte[]";
                row["ProviderDbType"] = 128;
                row["CreateFormat"] = "binary({0})";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 11

                row = dataTable.NewRow();
                row["TypeName"] = "image";
                row["DataType"] = "System.Byte[]";
                row["ProviderDbType"] = 128;
                row["CreateFormat"] = "image";
                row["IsFixedLength"] = false;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 12

                row = dataTable.NewRow();
                row["TypeName"] = "nvarchar";
                row["DataType"] = "System.String";
                row["ProviderDbType"] = 130;
                row["CreateFormat"] = "nvarchar({0})";
                row["IsFixedLength"] = false;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 13

                row = dataTable.NewRow();
                row["TypeName"] = "nchar";
                row["DataType"] = "System.String";
                row["ProviderDbType"] = 130;
                row["CreateFormat"] = "nchar({0})";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 14

                row = dataTable.NewRow();
                row["TypeName"] = "ntext";
                row["DataType"] = "System.String";
                row["ProviderDbType"] = 130;
                row["CreateFormat"] = "ntext";
                row["IsFixedLength"] = false;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 15

                row = dataTable.NewRow();
                row["TypeName"] = "numeric";
                row["DataType"] = "System.Decimal";
                row["ProviderDbType"] = 131;
                row["CreateFormat"] = "numeric({0}, {1})";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 16

                row = dataTable.NewRow();
                row["TypeName"] = "datetime";
                row["DataType"] = "System.DateTime";
                row["ProviderDbType"] = 135;
                row["CreateFormat"] = "datetime";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #region row 17

                row = dataTable.NewRow();
                row["TypeName"] = "rowversion";
                row["DataType"] = "System.Byte[]";
                row["ProviderDbType"] = 128;
                row["CreateFormat"] = "timestamp";
                row["IsFixedLength"] = true;
                dataTable.Rows.Add(row);

                #endregion row

                #endregion rows

                return dataTable;
            }
        }
    }
}
