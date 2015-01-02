using System;
using System.Collections.Generic;
using System.ComponentModel;
using IQToolkit;
using IQToolkit.Data.Common;

namespace IQToolkitContrib {
    public abstract partial class DbEntitySessionBase {
        // copied directly from IQToolkit.Data.EntitySession
        partial class EntitySession : IEntitySession {
            class TrackedTable<T> : SessionTable<T>, ITrackedTable where T : class {
                Dictionary<T, TrackedItem> tracked;
                Dictionary<object, T> identityCache;

                public TrackedTable(EntitySession session, MappingEntity entity)
                    : base(session, entity) {
                    this.tracked = new Dictionary<T, TrackedItem>();
                    this.identityCache = new Dictionary<object, T>();
                }

                IEnumerable<TrackedItem> ITrackedTable.TrackedItems {
                    get { return this.tracked.Values; }
                }

                TrackedItem ITrackedTable.GetTrackedItem(object instance) {
                    TrackedItem ti;
                    if (this.tracked.TryGetValue((T)instance, out ti))
                        return ti;
                    return null;
                }

                object ITrackedTable.GetFromCacheById(object key) {
                    T cached;
                    this.identityCache.TryGetValue(key, out cached);
                    return cached;
                }

                private bool SubmitChanges(TrackedItem item) {
                    switch (item.State) {
                        case SubmitAction.Delete:
                            this.ProviderTable.Delete(item.Instance);
                            return true;
                        case SubmitAction.Insert:
                            this.ProviderTable.Insert(item.Instance);
                            return true;
                        case SubmitAction.InsertOrUpdate:
                            this.ProviderTable.InsertOrUpdate(item.Instance);
                            return true;
                        case SubmitAction.PossibleUpdate:
                            if (item.Original != null &&
                                this.Mapping.IsModified(item.Entity, item.Instance, item.Original)) {
                                this.ProviderTable.Update(item.Instance);
                                return true;
                            }
                            break;
                        case SubmitAction.Update:
                            this.ProviderTable.Update(item.Instance);
                            return true;
                        default:
                            break; // do nothing
                    }
                    return false;
                }

                bool ITrackedTable.SubmitChanges(TrackedItem item) {
                    return this.SubmitChanges(item);
                }

                private void AcceptChanges(TrackedItem item) {
                    switch (item.State) {
                        case SubmitAction.Delete:
                            this.RemoveFromCache((T)item.Instance);
                            this.AssignAction((T)item.Instance, SubmitAction.None);
                            break;
                        case SubmitAction.Insert:
                            this.AddToCache((T)item.Instance);
                            this.AssignAction((T)item.Instance, SubmitAction.PossibleUpdate);
                            break;
                        case SubmitAction.InsertOrUpdate:
                            this.AddToCache((T)item.Instance);
                            this.AssignAction((T)item.Instance, SubmitAction.PossibleUpdate);
                            break;
                        case SubmitAction.PossibleUpdate:
                        case SubmitAction.Update:
                            this.AssignAction((T)item.Instance, SubmitAction.PossibleUpdate);
                            break;
                        default:
                            break; // do nothing
                    }
                }

                void ITrackedTable.AcceptChanges(TrackedItem item) {
                    this.AcceptChanges(item);
                }

                public override object OnEntityMaterialized(object instance) {
                    T typedInstance = (T)instance;
                    var cached = this.AddToCache(typedInstance);
                    if ((object)cached == (object)typedInstance) {
                        this.AssignAction(typedInstance, SubmitAction.PossibleUpdate);
                    }
                    return cached;
                }

                public override SubmitAction GetSubmitAction(T instance) {
                    TrackedItem ti;
                    if (this.tracked.TryGetValue(instance, out ti)) {
                        if (ti.State == SubmitAction.PossibleUpdate) {
                            if (this.Mapping.IsModified(ti.Entity, ti.Instance, ti.Original)) {
                                return SubmitAction.Update;
                            }
                            else {
                                return SubmitAction.None;
                            }
                        }
                        return ti.State;
                    }
                    return SubmitAction.None;
                }

                public override void SetSubmitAction(T instance, SubmitAction action) {
                    switch (action) {
                        case SubmitAction.None:
                        case SubmitAction.PossibleUpdate:
                        case SubmitAction.Update:
                        case SubmitAction.Delete:
                            var cached = this.AddToCache(instance);
                            if ((object)cached != (object)instance) {
                                throw new InvalidOperationException("An different instance with the same key is already in the cache.");
                            }
                            break;
                    }
                    this.AssignAction(instance, action);
                }

                private QueryMapping Mapping {
                    get { return ((EntitySession)this.Session).provider.Mapping; }
                }

                private T AddToCache(T instance) {
                    object key = this.Mapping.GetPrimaryKey(this.Entity, instance);
                    T cached;
                    if (!this.identityCache.TryGetValue(key, out cached)) {
                        cached = instance;
                        this.identityCache.Add(key, cached);
                    }
                    return cached;
                }

                private void RemoveFromCache(T instance) {
                    object key = this.Mapping.GetPrimaryKey(this.Entity, instance);
                    this.identityCache.Remove(key);
                }

                private void AssignAction(T instance, SubmitAction action) {
                    TrackedItem ti;
                    this.tracked.TryGetValue(instance, out ti);

                    switch (action) {
                        case SubmitAction.Insert:
                        case SubmitAction.InsertOrUpdate:
                        case SubmitAction.Update:
                        case SubmitAction.Delete:
                        case SubmitAction.None:
                            this.tracked[instance] = new TrackedItem(this, instance, ti != null ? ti.Original : null, action, ti != null ? ti.HookedEvent : false);
                            break;
                        case SubmitAction.PossibleUpdate:
                            INotifyPropertyChanging notify = instance as INotifyPropertyChanging;
                            if (notify != null) {
                                if (!ti.HookedEvent) {
                                    notify.PropertyChanging += new PropertyChangingEventHandler(this.OnPropertyChanging);
                                }
                                this.tracked[instance] = new TrackedItem(this, instance, null, SubmitAction.PossibleUpdate, true);
                            }
                            else {
                                var original = this.Mapping.CloneEntity(this.Entity, instance);
                                this.tracked[instance] = new TrackedItem(this, instance, original, SubmitAction.PossibleUpdate, false);
                            }
                            break;
                        default:
                            throw new InvalidOperationException(string.Format("Unknown SubmitAction: {0}", action));
                    }
                }

                protected virtual void OnPropertyChanging(object sender, PropertyChangingEventArgs args) {
                    TrackedItem ti;
                    if (this.tracked.TryGetValue((T)sender, out ti) && ti.State == SubmitAction.PossibleUpdate) {
                        object clone = this.Mapping.CloneEntity(ti.Entity, ti.Instance);
                        this.tracked[(T)sender] = new TrackedItem(this, ti.Instance, clone, SubmitAction.Update, true);
                    }
                }
            }
        }
    }
}