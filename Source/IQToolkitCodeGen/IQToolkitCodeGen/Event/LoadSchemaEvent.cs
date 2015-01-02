using IQToolkitCodeGenSchema.Models;
using Microsoft.Practices.Prism.Events;

namespace IQToolkitCodeGen.Event {
    public class LoadSchemaEvent : CompositePresentationEvent<IDatabase>
    {
    }
}
