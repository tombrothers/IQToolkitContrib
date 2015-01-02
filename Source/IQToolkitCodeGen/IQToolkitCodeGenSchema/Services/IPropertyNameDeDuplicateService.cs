using System.Collections.Generic;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Services {
    internal interface IPropertyNameDeDuplicateService {
        void Deduplicate(IEnumerable<IPropertyName> items);
    }
}