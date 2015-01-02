using IQToolkitCodeGenSchema.Models;
using Microsoft.Practices.Prism.Events;

namespace IQToolkitCodeGen.Event {
    public class SchemaProviderChangedEvent : CompositePresentationEvent<IDatabase>
    {
    }
}
