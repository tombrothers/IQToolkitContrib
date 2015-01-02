using System;
using System.Collections.Generic;

namespace IQToolkitCodeGenSchema {
    public interface ISchemaProvider {
        IEnumerable<ITable> GetSchema();
    }
}
