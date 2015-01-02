using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.ColumnTypeSchemaProviders {
    internal class VfpProvider : Provider {
        public override IList<IColumnTypeSchema> GetSchema(DbConnection connection) {
            var list = base.GetSchema(connection);
            var item = list.Single(x => x.CreateFormat == "W");

            list.Add(new ColumnTypeSchema("128", item.ProviderDbType, item.CreateFormat, item.Type));

            return list;
        }

        protected override string GetColumnType(DataRow row) {
            return row["ProviderDbType"].ToString();
        }

        protected override string GetCreateFormatString(DataRow row) {
            var typeName = row.Field<string>("TypeName");

            switch (typeName) {
                case "B":
                case "C":
                case "V":
                    return typeName + "({0})";
                case "N":
                case "F":
                    return typeName + "({0},{1})";
                default:
                    return typeName;
            }
        }

        protected override DataTable GetDataTable(DbConnection connection) {
            DataTable dataTypes = base.GetDataTable(connection);

            return dataTypes.AsEnumerable()
                            .Where(x => x.Field<string>("TypeName").Length == 1)
                            .CopyToDataTable();
        }
    }
}