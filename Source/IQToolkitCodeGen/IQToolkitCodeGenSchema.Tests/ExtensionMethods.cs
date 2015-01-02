using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Tests {
    internal static class ExtensionMethods {
        public static string ToXml(this IEnumerable<ITable> tables) {
            if (tables == null) {
                return string.Empty;
            }

            return (new XElement("Schema", tables.Select(x => x.ToXElement()).ToList())).ToString();
        }

        private static XElement ToXElement(this ITable table) {
            return new XElement("Table",
                    new XAttribute("TableName", table.TableName),
                    new XAttribute("IsView", table.IsView),
                    new XAttribute("EntityName", table.EntityName),
                    table.Columns.Select(x => x.ToXElement()).ToList(),
                    table.Associations.Select(x => x.ToXElement()).ToList());
        }

        private static XElement ToXElement(this IColumn column) {
            var columnElement = new XElement("Column",
                    new XAttribute("ColumnName", column.ColumnName),
                    new XAttribute("PropertyName", column.PropertyName),
                    new XAttribute("PropertyType", column.PropertyType),
                    new XAttribute("PrimaryKey", column.PrimaryKey),
                    new XAttribute("Generated", column.Generated),
                    new XAttribute("DbType", column.DbType),
                    new XAttribute("Nullable", column.Nullable));

            if (column.MaxLength.HasValue) {
                columnElement.Add(new XAttribute("MaxLength", column.MaxLength));
            }

            if (column.Precision.HasValue) {
                columnElement.Add(new XAttribute("Precision", column.Precision));
            }

            if (column.Scale.HasValue) {
                columnElement.Add(new XAttribute("Scale", column.Scale));
            }

            if (column.DefaultValue != null) {
                columnElement.Add(new XAttribute("DefaultValue", column.DefaultValue));
            }

            return columnElement;
        }

        private static XElement ToXElement(this IAssociation association) {
            return new XElement("Association",
                    new XAttribute("AssociationName", association.AssociationName),
                    new XAttribute("PropertyName", association.PropertyName),
                    new XAttribute("TableName", association.TableName),
                    new XAttribute("ColumnName", association.ColumnName),
                    new XAttribute("RelatedTableName", association.RelatedTableName),
                    new XAttribute("RelatedColumnName", association.RelatedColumnName));
        }
    }
}
