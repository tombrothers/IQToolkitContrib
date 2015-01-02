using System.Data;
using System.Data.Common;
using Microsoft.Practices.Unity;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class TableSchemaProvider {
        private class InformationSchemaProvider : Provider {
            private readonly DataTableFactory _dataTableFactory;

            public InformationSchemaProvider(IUnityContainer container, DataTableFactory dataTableFactory)
                : base(container) {
                ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

                this._dataTableFactory = dataTableFactory;
            }

            protected override DataTable GetDataTable(DbConnection connection) {
                var command = connection.CreateCommand();
                var tableTypes = "'Base Table', 'Table'";

                if (!this._container.ExcludeViews()) {
                    tableTypes += ", 'View'";
                }
                
                command.CommandText = string.Format(@"
select Table_Name, Table_Type
    from Information_Schema.Tables
    where Table_Type in ({0})", tableTypes);

                return this._dataTableFactory.CreateDataTable(command);
            }
        }
    }
}
