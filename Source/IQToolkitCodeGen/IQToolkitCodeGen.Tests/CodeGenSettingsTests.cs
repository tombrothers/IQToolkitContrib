using System.Collections.Generic;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Model;
using IQToolkitCodeGen.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using IQToolkitCodeGen.Service;
using IQToolkitCodeGenSchema.Providers;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGen.Tests {
    [TestClass]
    public class CodeGenSettingsTests : TestBase {
        [TestMethod]
        public void SerializeTest() {
            CodeGenSettings settings = this.GetSettings();
            XmlSerializerService service = new XmlSerializerService();

            string actualXml = service.ToXml(settings);
            
            Assert.AreEqual(Resources.CodeGenSettingsSerializeTestXml, actualXml);
        }

        private CodeGenSettings GetSettings() {
            var databaseProvider = new DatabaseProvider();
            var database = databaseProvider.GetDatabase(DatabaseType.Vfp);

            return new CodeGenSettings {
                ProviderName = database.DisplayName,
                ConnectionString = @"NorthwindData\Vfp\northwind.dbc",
                DataContextBaseClass = "DataContext",
                DataContextClassName = "MyDataContextClassName",
                DataContextNamespace = "MyDataContextNamespace",
                DataContextOutputFile = "EntityProviderDataContextOutput.cs",
                DataContextTemplate = "EntityProvider.cshtml",

                EntityExtension = "Entity.cshtml",
                EntityOutputPath = "Entities",
                EntityTemplate = "Entity.cshtml",
                MappingOutputFile = "XmlMappingOutput.xml",
                MappingTemplate = "XmlMapping.cshtml",
                Tables = this.GetTables()
            };
        }

        private List<Table> GetTables() {
            return new List<Table> {
                 new Table { 
                    TableName = "Categories", 
                    EntityName = "Category", 
                    Selected = true,
                    Columns = new List<Column>() {
                        new Column { 
                            ColumnName="CategoryId", 
                            PropertyName="Id", 
                            PropertyType = "int", 
                            Selected= true, 
                            PrimaryKey = true, 
                            Generated= true, 
                            MaxLength = -1
                        }, 
                        new Column{ 
                            ColumnName = "CategoryName",
                            PropertyName = "CategoryName",
                            PropertyType = "string",
                            Selected = true,
                            Generated = false,
                            PrimaryKey = false,
                            MaxLength = 15
                        }, 
                        new Column{ 
                            ColumnName = "ColumnName3",
                            PropertyName = "PropertyName3",
                            PropertyType = "string",
                            Selected = false,
                            Generated = true,
                            PrimaryKey = false,
                            MaxLength = -1
                        }, 
                        new Column{ 
                            ColumnName = "ColumnName4",
                            PropertyName = "PropertyName4",
                            PropertyType = "string",
                            Selected = false,
                            Generated = false,
                            PrimaryKey = true,
                            MaxLength = -1
                        }
                    }
                }, 
                new Table{
                 TableName = "CustomerCustomerDemo",
                  EntityName = "CustomerCustomerDemo",
                   Selected = true,
                   Columns = new List<Column> {
                            new Column {
                            ColumnName = "CustomerId",
                            PropertyName = "CustomerId",
                            PropertyType = "int",
                            Selected = false,
                            PrimaryKey = false,
                            Generated = false, MaxLength=5
                        }
                   }
                }
            };
        }
    }
}