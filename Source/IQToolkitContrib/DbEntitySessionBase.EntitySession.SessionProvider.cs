using System;
using System.Linq.Expressions;
using IQToolkit;
using IQToolkit.Data;
using IQToolkit.Data.Common;

namespace IQToolkitContrib {
    public abstract partial class DbEntitySessionBase {
        partial class EntitySession : IEntitySession {
            // copied directly from IQToolkit.Data.EntitySession
            class SessionProvider : QueryProvider, IEntityProvider, ICreateExecutor {
                EntitySession session;
                EntityProvider provider;

                public SessionProvider(EntitySession session, EntityProvider provider) {
                    this.session = session;
                    this.provider = provider;
                }

                public EntityProvider Provider {
                    get { return this.provider; }
                }

                public override object Execute(Expression expression) {
                    return this.provider.Execute(expression);
                }

                public override string GetQueryText(Expression expression) {
                    return this.provider.GetQueryText(expression);
                }

                public IEntityTable<T> GetTable<T>(string tableId) {
                    return this.provider.GetTable<T>(tableId);
                }

                public IEntityTable GetTable(Type type, string tableId) {
                    return this.provider.GetTable(type, tableId);
                }

                public bool CanBeEvaluatedLocally(Expression expression) {
                    return this.provider.Mapping.CanBeEvaluatedLocally(expression);
                }

                public bool CanBeParameter(Expression expression) {
                    return this.provider.CanBeParameter(expression);
                }

                QueryExecutor ICreateExecutor.CreateExecutor() {
                    return new SessionExecutor(this.session, ((ICreateExecutor)this.provider).CreateExecutor());
                }
            }
        }
    }
}