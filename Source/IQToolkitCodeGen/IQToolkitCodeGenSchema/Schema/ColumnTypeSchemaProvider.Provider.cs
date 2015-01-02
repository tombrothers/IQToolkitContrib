using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class ColumnTypeSchemaProvider {
        private class Provider {
            public virtual IList<ColumnTypeSchema> GetSchema(DbConnection connection) {
                DataTable dataTypes = this.GetDataTable(connection);

                return dataTypes.AsEnumerable()
                                .Select(row => new { Row = row, Type = this.GetType(row) })
                                .Where(x => x.Type != null)
                                .Select(x => new ColumnTypeSchema(this.GetColumnType(x.Row),
                                                                  this.GetProviderDbType(x.Row),
                                                                  this.GetCreateFormatString(x.Row),
                                                                  x.Type))
                                .ToList();
            }

            protected virtual string GetCreateFormatString(DataRow row) {
                if (row.IsNull("CreateFormat")) {
                    return string.Empty;
                }

                return row.Field<string>("CreateFormat");
            }

            protected int GetProviderDbType(DataRow row) {
                return row.Field<int>("ProviderDbType");
            }

            protected virtual string GetColumnType(DataRow row) {
                return row.Field<string>("TypeName");
            }

            protected virtual Type GetType(DataRow row) {
                if (row.IsNull("DataType")) {
                    return null;
                }

                return Type.GetType(row.Field<string>("DataType"));
            }

            protected virtual DataTable GetDataTable(DbConnection connection) {
                DataTable dataTypes = null;

                connection.DoConnected(() => {
                    dataTypes = connection.GetSchema("DataTypes");
                });

                return dataTypes;
            }
        }
    }
}
