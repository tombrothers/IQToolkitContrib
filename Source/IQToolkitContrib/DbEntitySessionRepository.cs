using IQToolkit;
using IQToolkit.Data;
using IQToolkit.Data.Common;

namespace IQToolkitContrib {
    public class DbEntitySessionRepository : DbEntityRepository {
        public IEntitySession Session { get; private set; }

        public DbEntitySessionRepository(DbEntityProvider provider)
            : base(provider) {
            this.Session = new EntitySession(provider);
        }

        public override void Insert<T>(T entity) {
            base.Insert<T>(entity);
        }

        public override void Update<T>(T entity) {
            base.Update<T>(entity);
        }

        public override void Delete<T>(T entity) {
            base.Delete<T>(entity);
        }

        private ISessionTable<T> GetEntityTable<T>() {
            return this.Session.GetTable<T>(this.Provider.Mapping.GetTableId(typeof(T)));
        }
    }
}
