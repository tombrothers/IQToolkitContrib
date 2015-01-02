using System.Collections.Generic;
using IQToolkit;

namespace IQToolkitContrib {
    public abstract partial class DbEntitySessionBase {
        partial class EntitySession : IEntitySession {
            // copied directly from IQToolkit.Data.EntitySession
            interface ITrackedTable : IEntitySessionTable {
                object GetFromCacheById(object key);
                IEnumerable<TrackedItem> TrackedItems { get; }
                TrackedItem GetTrackedItem(object instance);
                bool SubmitChanges(TrackedItem item);
                void AcceptChanges(TrackedItem item);
            }
        }
    }
}
