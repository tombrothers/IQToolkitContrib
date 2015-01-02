using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers.AssociationSchemaProviders {
    internal class CustomSqlProvider : Provider {
        private readonly ISchemaOptions _schemaOptions;
        private readonly IDataTableFactory _dataTableFactory;

        public CustomSqlProvider(ISchemaOptions schemaOptions, IDataTableFactory dataTableFactory) {
            ArgumentUtility.CheckNotNull("schemaOptions", schemaOptions);
            ArgumentUtility.CheckNotNull("dataTableFactory", dataTableFactory);

            this._schemaOptions = schemaOptions;
            this._dataTableFactory = dataTableFactory;
        }

        public override IList<IAssociationSchema> GetSchema(DbConnection connection) {
            var dataTable = this.GetDataTable(connection);
            var associations = new List<IAssociationSchema>();

            associations.AddRange(dataTable.AsEnumerable()
                                           .Select(row => new AssociationSchema(row.Field<string>("ForeignKey"),
                                                                                row.Field<string>("TableName"),
                                                                                row.Field<string>("ColumnName"),
                                                                                row.Field<string>("RelatedTableName"),
                                                                                row.Field<string>("RelatedColumnName"),
                                                                                AssociationType.Item)));

            this.AddAssociationListItems(associations);

            return associations;
        }

        private DataTable GetDataTable(DbConnection connection) {
            using (var command = connection.CreateCommand()) {
                command.CommandText = this._schemaOptions.AssociationSchemaSql;

                return this._dataTableFactory.CreateDataTable(command);
            }
        }
    }
}
