using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Model;

namespace IQToolkitCodeGen.Schema.Provider {
    internal abstract class OleDbSchemaProviderBase : SchemaProviderBase {
        public OleDbSchemaProviderBase(string providerName)
            : base(providerName, SchemaProviderType.OleDb) {
        }

        public override List<Association> GetAssociationList() {
            List<Association> associations = new List<Association>();

            DataTable foreignKeys = null;

            this.Connection.DoConnected(() => {
                foreignKeys = ((OleDbConnection)this.Connection).GetOleDbSchemaTable(OleDbSchemaGuid.Foreign_Keys, null);
            });

            associations.AddRange((from row in foreignKeys.AsEnumerable()
                                   select new Association {
                                       AssociationName = string.Format("FK_{0}_{1}",
                                                                        row.Field<string>("FK_TABLE_NAME"),
                                                                        row.Field<string>("PK_TABLE_NAME")),
                                       TableName = row.Field<string>("FK_TABLE_NAME"),
                                       ColumnName = row.Field<string>("FK_COLUMN_NAME"),
                                       RelatedTableName = row.Field<string>("PK_TABLE_NAME"),
                                       RelatedColumnName = row.Field<string>("PK_COLUMN_NAME"),
                                       AssociationType = AssociationType.Item
                                   }).ToList());


            List<Association> associations2 = new List<Association>();
            associations2.AddRange(associations);
            this.AddAssociationListItems(associations2);
            associations2.RemoveRange(0, associations.Count);
            associations2.ForEach(x => x.AssociationName = string.Format("FK_{0}_{1}List",
                                                                        x.TableName,
                                                                        x.RelatedTableName));

            associations.AddRange(associations2);
            return associations;
        }

        protected override List<ColumnMetaInfo> GetColumnMetaInfo(DataTable columns, DataTable dataTypes) {
            return (from column in columns.AsEnumerable()
                    from dataType in dataTypes.AsEnumerable()
                    where column.Field<int>("Data_Type") == dataType.Field<int>("ProviderDbType")
                    orderby this.GetTableName(column), column.Field<string>("COLUMN_NAME")
                    select new ColumnMetaInfo {
                        TableName = this.GetTableName(column),
                        ColumnName = column.Field<string>("COLUMN_NAME"),
                        DataType = Type.GetType(dataType.Field<string>("DATATYPE")),
                        IsNullable = this.IsNullable(column),
                        IsGenerated = this.IsGenerated(column),
                        MaxLength = this.GetMaxLength(column)
                    }).ToList();
        }
    }
}
