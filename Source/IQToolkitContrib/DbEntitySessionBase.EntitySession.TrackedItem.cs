using System.Collections.Generic;
using IQToolkit;
using IQToolkit.Data.Common;

namespace IQToolkitContrib {
    public abstract partial class DbEntitySessionBase {
        partial class EntitySession : IEntitySession {
            // copied directly from IQToolkit.Data.EntitySession
            class TrackedItem {
                ITrackedTable table;
                object instance;
                object original;
                SubmitAction state;
                bool hookedEvent;

                internal TrackedItem(ITrackedTable table, object instance, object original, SubmitAction state, bool hookedEvent) {
                    this.table = table;
                    this.instance = instance;
                    this.original = original;
                    this.state = state;
                    this.hookedEvent = hookedEvent;
                }

                public ITrackedTable Table {
                    get { return this.table; }
                }

                public MappingEntity Entity {
                    get { return this.table.Entity; }
                }

                public object Instance {
                    get { return this.instance; }
                }

                public object Original {
                    get { return this.original; }
                }

                public SubmitAction State {
                    get { return this.state; }
                }

                public bool HookedEvent {
                    get { return this.hookedEvent; }
                }

                public static readonly IEnumerable<TrackedItem> EmptyList = new TrackedItem[] { };
            }
        }
    }
}