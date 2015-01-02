using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.TableSchemaProviders {
    internal class OracleProvider : Provider {
        private readonly IOracleUserProvider _oracleUserProvider;

        public OracleProvider(ISchemaOptions schemaOptions, IOracleUserProvider oracleUserProvider)
            : base(schemaOptions) {
            ArgumentUtility.CheckNotNull("oracleUserProvider", oracleUserProvider);

            this._oracleUserProvider = oracleUserProvider;
        }

        protected override DataTable GetDataTable(DbConnection connection) {
            var tables = this.GetTables(connection);

            if (!this.SchemaOptions.ExcludeViews) {
                var views = this.GetViews(connection);
                tables.Merge(views);
            }

            return tables;
        }

        private DataTable GetViews(DbConnection connection) {
            DataTable views = null;

            connection.DoConnected(() => {
                views = connection.GetSchema("Views", new[] { this._oracleUserProvider.User });
            });

            views.Columns["View_Name"].ColumnName = "TABLE_NAME";
            views = views.DefaultView.ToTable(true, "TABLE_NAME");
            views.Columns.Add("TABLE_TYPE");
            views.AsEnumerable().ForEach(row => row["TABLE_TYPE"] = "VIEW");

            return views;
        }

        private DataTable GetTables(DbConnection connection) {
            DataTable tables = null;

            connection.DoConnected(() => {
                tables = connection.GetSchema("Tables", new[] { this._oracleUserProvider.User });
            });

            tables.Columns["Type"].ColumnName = "TABLE_TYPE";

            return tables.DefaultView.ToTable(true, "TABLE_NAME", "TABLE_TYPE");
        }
    }
}
