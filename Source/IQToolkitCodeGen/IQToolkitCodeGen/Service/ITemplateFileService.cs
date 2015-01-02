using System;
using System.Collections.Generic;

namespace IQToolkitCodeGen.Service {
    public interface ITemplateFileService {
        IEnumerable<string> DataContextTemplates { get; }
        IEnumerable<string> EntityTemplates { get; }
        IEnumerable<string> MappingTemplates { get; }
        IEnumerable<string> WcfDataServiceClientTemplates { get; }
        IEnumerable<string> WcfDataServiceDataContextTemplates { get; }
    }
}
