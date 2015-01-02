using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.ColumnSchemaProviders {
    internal class Provider : IProvider {
        public IList<IColumnSchema> GetSchema(DbConnection connection, string tableName) {
            DataTable columns = this.GetDataTable(connection, tableName);

            return columns.AsEnumerable()
                          .Select(row => new { ColumnType = this.GetColumnType(row), Row = row })
                          .Where(x => !string.IsNullOrWhiteSpace(x.ColumnType))
                          .Select(x => new ColumnSchema(this.GetColumnName(x.Row),
                                                        x.ColumnType,
                                                        this.GetDefaultValue(x.Row),
                                                        this.GetIsNullable(x.Row),
                                                        this.GetMaxLength(x.Row),
                                                        this.GetPrecision(x.Row),
                                                        this.GetScale(x.Row)))
                           .Cast<IColumnSchema>()
                           .ToList();
        }

        private short? GetScale(DataRow row) {
            return this.GetShortValue(row, "NUMERIC_SCALE");
        }

        private short? GetPrecision(DataRow row) {
            return this.GetShortValue(row, "NUMERIC_PRECISION");
        }

        protected virtual long? GetMaxLength(DataRow row) {
            return this.GetLongValue(row, "CHARACTER_MAXIMUM_LENGTH");
        }

        private short? GetShortValue(DataRow row, string columnName) {
            if (!row.Table.Columns.Contains(columnName) || row.IsNull(columnName)) {
                return null;
            }

            string value = row[columnName].ToString();
            short precision = -1;

            if (short.TryParse(value, out precision)) {
                return precision;
            }

            return null;
        }

        private long? GetLongValue(DataRow row, string columnName) {
            if (!row.Table.Columns.Contains(columnName) || row.IsNull(columnName)) {
                return null;
            }

            string value = row[columnName].ToString();
            long maxLength = -1;

            if (long.TryParse(value, out maxLength)) {
                return maxLength;
            }

            return null;
        }

        private bool GetIsNullable(DataRow row) {
            const string COLUMN_NAME = "Is_Nullable";

            if (!row.Table.Columns.Contains(COLUMN_NAME) || row.IsNull(COLUMN_NAME)) {
                return false;
            }

            if (row.Table.Columns[COLUMN_NAME].DataType == typeof(bool)) {
                return row.Field<bool>(COLUMN_NAME);
            }

            return row.Field<string>(COLUMN_NAME).Substring(0, 1).ToUpper() == "Y";
        }

        protected virtual string GetDefaultValue(DataRow row) {
            const string COLUMN_NAME = "Column_Default";

            if (!row.Table.Columns.Contains(COLUMN_NAME) || row.IsNull(COLUMN_NAME)) {
                return null;
            }

            return row.Field<string>(COLUMN_NAME);
        }

        protected virtual string GetColumnType(DataRow row) {
            return row.Field<string>("Data_Type");
        }

        private string GetColumnName(DataRow row) {
            return row.Field<string>("Column_Name");
        }

        protected virtual DataTable GetDataTable(DbConnection connection, string tableName) {
            DataTable columns = null;

            connection.DoConnected(() => {
                columns = connection.GetSchema("Columns", new string[] { null, null, tableName });
            });

            return columns;
        }
    }
}