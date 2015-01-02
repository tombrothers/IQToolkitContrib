using System;
using System.Data;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Factories {
    internal interface IDataTableFactory {
        DataTable CreateDataTable(DbCommand command);
        DataTable CreateSchemaDataTable(DbCommand command);
    }
}