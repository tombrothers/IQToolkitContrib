using System;
using System.Collections.Generic;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers {
    public interface ISchemaProvider {
        IEnumerable<ITable> GetSchema();
    }
}