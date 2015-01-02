using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Data;
using System.Data.Common;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Schema {
    [Export]
    public class SchemaManager {
        private readonly PluralizationService _pluralizationService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
        public ReadOnlyCollection<ISchemaProvider> Providers { get; private set; }

        [ImportingConstructor]
        private SchemaManager([ImportMany(typeof(ISchemaProvider))] 
                                IEnumerable<ISchemaProvider> providers) {
            this.Providers = this.GetInstalledProviders(providers);
        }

        private ReadOnlyCollection<ISchemaProvider> GetInstalledProviders(IEnumerable<ISchemaProvider> providers) {
            DataTable dataTable = DbProviderFactories.GetFactoryClasses();

            List<string> invariantNames = dataTable.AsEnumerable()
                                                   .Select(row => row.Field<string>("InvariantName"))
                                                   .ToList();

            List<ISchemaProvider> list = providers.Where(provider => invariantNames.Contains(provider.ProviderType))
                                                  .OrderBy(provider => provider.ProviderName)
                                                  .ToList();

            return new ReadOnlyCollection<ISchemaProvider>(list);
        }

        public List<Table> GetTables(ISchemaProvider schemaProvider) {
            List<Table> tables = (from tableName in schemaProvider.GetTableNames()
                                  select new Table(tableName)).ToList();

            List<ColumnMetaInfo> columnMetaInfoLIst = schemaProvider.GetColumnMetaInfo();
            List<Association> associations = schemaProvider.GetAssociationList();

            for (int tableIndex = 0, tableTotal = tables.Count; tableIndex < tableTotal; tableIndex++) {
                Table table = tables[tableIndex];
                List<PrimaryKey> primaryKeys = schemaProvider.GetPrimaryKeyList(table.TableName);

                List<ColumnMetaInfo> tableColumnMetaInfoLIst = (from column in columnMetaInfoLIst
                                                                where column.TableName.Equals(table.TableName, StringComparison.InvariantCultureIgnoreCase)
                                                                select column).ToList();

                List<Association> tableAssociations = (from a in associations
                                                       where a.TableName.Equals(table.TableName, StringComparison.InvariantCultureIgnoreCase)
                                                       select a).ToList();

                List<Association> relatedTableAssociations = (from a in associations
                                                              where a.RelatedTableName.Equals(table.TableName, StringComparison.InvariantCultureIgnoreCase)
                                                              select a).ToList();
                
                Dictionary<string, int> propertyNames = new Dictionary<string, int>();

                for (int columnIndex = 0, columnTotal = tableColumnMetaInfoLIst.Count; columnIndex < columnTotal; columnIndex++) {
                    ColumnMetaInfo columnMetaInfo = tableColumnMetaInfoLIst[columnIndex];

                    Column column = new Column(columnMetaInfo.ColumnName);
                    column.PropertyType = this.GetPropertyType(columnMetaInfo);
                    column.MaxLength = columnMetaInfo.MaxLength;
                    column.DbType = columnMetaInfo.DbType;
                    column.Nullable = columnMetaInfo.IsNullable;
                    column.Precision = columnMetaInfo.Precision;
                    column.Scale = columnMetaInfo.Scale;

                    PrimaryKey primaryKey = primaryKeys.Find(pk => pk.ColumnName.Equals(column.ColumnName, StringComparison.CurrentCultureIgnoreCase));

                    if (primaryKey != null) {
                        column.PrimaryKey = true;
                        column.Generated = primaryKey.AutoIncrement || columnMetaInfo.IsGenerated;
                    }

                    table.Columns.Add(column);

                    List<Association> columnAssociations = (from a in tableAssociations
                                                            where a.ColumnName.Equals(column.ColumnName, StringComparison.InvariantCultureIgnoreCase)
                                                            select a).ToList();

                    for (int index = 0, total = columnAssociations.Count; index < total; index++) {
                        Association association = columnAssociations[index];
                        association.Table = table;
                        association.Column = column;

                        string propertyName = association.RelatedEntityName.Replace(" ", "_").Replace(".", "_");

                        if (string.IsNullOrEmpty(propertyName)) {
                            propertyName = association.RelatedTableName.Replace(" ", "_").Replace(".", "_");
                        }

                        if (propertyName == table.TableName.Replace(" ", "_").Replace(".", "_"))
                        { 
                            propertyName = propertyName + "sr"; 
                        }


                        if (propertyNames.ContainsKey(propertyName))
                        {
                            int cnt = propertyNames[propertyName];
                            propertyNames[propertyName] = cnt + 1;
                            propertyName = propertyName + cnt.ToString();
                            propertyNames.Add(propertyName, 1);
                        } 
                        else 
                        {
                            propertyNames.Add(propertyName, 1);
                        }
                        if (association.AssociationType == AssociationType.Item) {
                            association.PropertyName = this._pluralizationService.Singularize(propertyName);
                        }
                        else {
                            association.PropertyName = this._pluralizationService.Pluralize(propertyName);
                        }

                        table.Associations.Add(association);
                    }
                }

                table.Columns = table.Columns.OrderBy(column => column.ColumnName).ToList();
                table.Associations = table.Associations.Distinct().OrderBy(association => association.AssociationName).ToList();

            }

            return tables;
        }

        private string GetPropertyType(ColumnMetaInfo columnMetaInfo) {
            return columnMetaInfo.DataType.Name;
        }
    }
}
