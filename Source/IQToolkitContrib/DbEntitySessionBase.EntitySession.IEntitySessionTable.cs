using IQToolkit;
using IQToolkit.Data.Common;

namespace IQToolkitContrib {
    public abstract partial class DbEntitySessionBase {
        partial class EntitySession : IEntitySession {
            // copied directly from IQToolkit.Data.EntitySession
            interface IEntitySessionTable : ISessionTable {
                object OnEntityMaterialized(object instance);
                MappingEntity Entity { get; }
            }
        }
    }
}
