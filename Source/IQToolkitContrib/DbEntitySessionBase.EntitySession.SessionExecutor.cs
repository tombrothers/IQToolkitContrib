using System;
using System.Collections.Generic;
using IQToolkit;
using IQToolkit.Data.Common;

namespace IQToolkitContrib {
    public abstract partial class DbEntitySessionBase {
        // derived from IQToolkit.Data.EntitySession
        partial class EntitySession : IEntitySession {
            class SessionExecutor : QueryExecutor {
                EntitySession session;
                QueryExecutor executor;

                public SessionExecutor(EntitySession session, QueryExecutor executor) {
                    this.session = session;
                    this.executor = executor;
                }

                public override int RowsAffected {
                    get { return this.executor.RowsAffected; }
                }

                public override object Convert(object value, Type type) {
                    return this.executor.Convert(value, type);
                }

                // Added entity null check...
                public override IEnumerable<T> Execute<T>(QueryCommand command, Func<FieldReader, T> fnProjector, MappingEntity entity, object[] paramValues) {
                    if (entity != null) {
                        fnProjector = Wrap(fnProjector, entity);
                    }
                    
                    return this.executor.Execute<T>(command, fnProjector, entity, paramValues);
                }

                public override IEnumerable<int> ExecuteBatch(QueryCommand query, IEnumerable<object[]> paramSets, int batchSize, bool stream) {
                    return this.executor.ExecuteBatch(query, paramSets, batchSize, stream);
                }

                public override IEnumerable<T> ExecuteBatch<T>(QueryCommand query, IEnumerable<object[]> paramSets, Func<FieldReader, T> fnProjector, MappingEntity entity, int batchSize, bool stream) {
                    return this.executor.ExecuteBatch<T>(query, paramSets, Wrap(fnProjector, entity), entity, batchSize, stream);
                }

                public override IEnumerable<T> ExecuteDeferred<T>(QueryCommand query, Func<FieldReader, T> fnProjector, MappingEntity entity, object[] paramValues) {
                    return this.executor.ExecuteDeferred<T>(query, Wrap(fnProjector, entity), entity, paramValues);
                }

                public override int ExecuteCommand(QueryCommand query, object[] paramValues) {
                    return this.executor.ExecuteCommand(query, paramValues);
                }

                private Func<FieldReader, T> Wrap<T>(Func<FieldReader, T> fnProjector, MappingEntity entity) {
                    Func<FieldReader, T> fnWrapped = (fr) => (T)this.session.OnEntityMaterialized(entity, fnProjector(fr));
                    return fnWrapped;
                }
            }
        }
    }
}