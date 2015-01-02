using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using IQToolkitCodeGenSchema.Services;

namespace IQToolkitCodeGenSchema.Models {
#if DEBUGGER_DISPLAY
    [DebuggerDisplay("{DebuggerDisplay(),nq}")]
#endif
    internal class Table : ITable {
        public string TableName { get; private set; }
        public string EntityName { get; private set; }
        public bool IsView { get; private set; }
        public IEnumerable<IColumn> Columns { get; internal set; }
        public IEnumerable<IAssociation> Associations { get; internal set; }

        public Table(ITableSchema schema, IPluralizationService pluralizationService) {
            ArgumentUtility.CheckNotNull("schema", schema);
            ArgumentUtility.CheckNotNullOrEmpty("schema.TableName", schema.TableName);
            ArgumentUtility.CheckNotNull("pluralizationService", pluralizationService);

            this.TableName = schema.TableName;
            this.EntityName = pluralizationService.Singularize(schema.TableName)
                                                  .ToSafeClrName(schema.TableName.ShouldForceProperCase());
            this.IsView = schema.IsView;
        }

#if DEBUGGER_DISPLAY
        private string DebuggerDisplay() {
            return string.Format("TableName={0} | EntityName={1} | IsView={2}", this.TableName, this.EntityName, this.IsView);
        }
#endif
    }
}
