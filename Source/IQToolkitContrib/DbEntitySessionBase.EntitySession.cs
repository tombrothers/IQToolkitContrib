using System;
using System.Collections.Generic;
using System.Linq;
using IQToolkit;
using IQToolkit.Data;
using IQToolkit.Data.Common;

namespace IQToolkitContrib {
    public abstract partial class DbEntitySessionBase {
        partial class EntitySession : IEntitySession {
            // copied directly from IQToolkit.Data.EntitySession
            EntityProvider provider;
            SessionProvider sessionProvider;
            Dictionary<MappingEntity, ISessionTable> tables;

            public EntitySession(EntityProvider provider) {
                this.provider = provider;
                this.sessionProvider = new SessionProvider(this, provider);
                this.tables = new Dictionary<MappingEntity, ISessionTable>();
            }

            public IEntityProvider Provider {
                get { return this.sessionProvider; }
            }

            IEntityProvider IEntitySession.Provider {
                get { return this.Provider; }
            }

            protected IEnumerable<ISessionTable> GetTables() {
                return this.tables.Values;
            }

            public ISessionTable GetTable(Type elementType, string tableId) {
                return this.GetTable(this.sessionProvider.Provider.Mapping.GetEntity(elementType, tableId));
            }

            public ISessionTable<T> GetTable<T>(string tableId) {
                return (ISessionTable<T>)this.GetTable(typeof(T), tableId);
            }

            protected ISessionTable GetTable(MappingEntity entity) {
                ISessionTable table;
                if (!this.tables.TryGetValue(entity, out table)) {
                    table = this.CreateTable(entity);
                    this.tables.Add(entity, table);
                }
                return table;
            }

            private object OnEntityMaterialized(MappingEntity entity, object instance) {
                IEntitySessionTable table = (IEntitySessionTable)this.GetTable(entity);
                return table.OnEntityMaterialized(instance);
            }

            public virtual void SubmitChanges() {
                this.provider.DoTransacted(
                    delegate {
                        var submitted = new List<TrackedItem>();

                        // do all submit actions
                        foreach (var item in this.GetOrderedItems()) {
                            if (item.Table.SubmitChanges(item)) {
                                submitted.Add(item);
                            }
                        }

                        // on completion, accept changes
                        foreach (var item in submitted) {
                            item.Table.AcceptChanges(item);
                        }
                    }
                );
            }

            protected virtual ISessionTable CreateTable(MappingEntity entity) {
                return (ISessionTable)Activator.CreateInstance(typeof(TrackedTable<>).MakeGenericType(entity.ElementType), new object[] { this, entity });
            }

            private IEnumerable<TrackedItem> GetOrderedItems() {
                var items = (from tab in this.GetTables()
                             from ti in ((ITrackedTable)tab).TrackedItems
                             where ti.State != SubmitAction.None
                             select ti).ToList();

                // build edge maps to represent all references between entities
                var edges = this.GetEdges(items).Distinct().ToList();
                var stLookup = edges.ToLookup(e => e.Source, e => e.Target);
                var tsLookup = edges.ToLookup(e => e.Target, e => e.Source);

                return TopologicalSorter.Sort(items, item => {
                    switch (item.State) {
                        case SubmitAction.Insert:
                        case SubmitAction.InsertOrUpdate:
                            // all things this instance depends on must come first
                            var beforeMe = stLookup[item];

                            // if another object exists with same key that is being deleted, then the delete must come before the insert
                            object cached = item.Table.GetFromCacheById(this.provider.Mapping.GetPrimaryKey(item.Entity, item.Instance));
                            if (cached != null && cached != item.Instance) {
                                var ti = item.Table.GetTrackedItem(cached);
                                if (ti != null && ti.State == SubmitAction.Delete) {
                                    beforeMe = beforeMe.Concat(new[] { ti });
                                }
                            }
                            return beforeMe;

                        case SubmitAction.Delete:
                            // all things that depend on this instance must come first
                            return tsLookup[item];
                        default:
                            return TrackedItem.EmptyList;
                    }
                });
            }

            private TrackedItem GetTrackedItem(EntityInfo entity) {
                ITrackedTable table = (ITrackedTable)this.GetTable(entity.Mapping);
                return table.GetTrackedItem(entity.Instance);
            }

            private IEnumerable<Edge> GetEdges(IEnumerable<TrackedItem> items) {
                foreach (var c in items) {
                    foreach (var d in this.provider.Mapping.GetDependingEntities(c.Entity, c.Instance)) {
                        var dc = this.GetTrackedItem(d);
                        if (dc != null) {
                            yield return new Edge(dc, c);
                        }
                    }
                    foreach (var d in this.provider.Mapping.GetDependentEntities(c.Entity, c.Instance)) {
                        var dc = this.GetTrackedItem(d);
                        if (dc != null) {
                            yield return new Edge(c, dc);
                        }
                    }
                }
            }
        }

    }
}
