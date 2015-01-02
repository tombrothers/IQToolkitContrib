using System;
using IQToolkit;
using IQToolkit.Data.Common;

namespace IQToolkitContrib {
    public abstract partial class DbEntitySessionBase {
        partial class EntitySession : IEntitySession {
            // derived from IQToolkit.Data.EntitySession
            abstract class SessionTable<T> : Query<T>, ISessionTable<T>, ISessionTable, IEntitySessionTable where T : class {
                EntitySession session;
                MappingEntity entity;
                IEntityTable<T> underlyingTable;

                public SessionTable(EntitySession session, MappingEntity entity)
                    : base(session.sessionProvider, typeof(ISessionTable<T>)) {
                    this.session = session;
                    this.entity = entity;

                    // Changed the class so that the underlyingTable is of type DbEntityTable.
                    var table = this.session.Provider.GetTable<T>(entity.TableId);
                    this.underlyingTable = new DbEntityTable<T>(table);
                }

                public IEntitySession Session {
                    get { return this.session; }
                }

                public MappingEntity Entity {
                    get { return this.entity; }
                }

                public IEntityTable<T> ProviderTable {
                    get { return this.underlyingTable; }
                }

                IEntityTable ISessionTable.ProviderTable {
                    get { return this.underlyingTable; }
                }

                public T GetById(object id) {
                    return this.underlyingTable.GetById(id);
                }

                object ISessionTable.GetById(object id) {
                    return this.GetById(id);
                }

                public virtual object OnEntityMaterialized(object instance) {
                    return instance;
                }

                public virtual void SetSubmitAction(T instance, SubmitAction action) {
                    throw new NotImplementedException();
                }

                void ISessionTable.SetSubmitAction(object instance, SubmitAction action) {
                    this.SetSubmitAction((T)instance, action);
                }

                public virtual SubmitAction GetSubmitAction(T instance) {
                    throw new NotImplementedException();
                }

                SubmitAction ISessionTable.GetSubmitAction(object instance) {
                    return this.GetSubmitAction((T)instance);
                }
            }
        }
    }
}