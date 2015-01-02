using System;
using IQToolkit;
using IQToolkit.Data;

namespace IQToolkitContrib {
    public abstract partial class DbEntitySessionBase {
        private readonly DbEntityProvider provider;
        private readonly IEntitySession session;

        public DbEntitySessionBase(DbEntityProvider provider) {
            if (provider == null) {
                throw new ArgumentNullException("provider");
            }

            this.provider = provider;
            this.session = new EntitySession(provider);
        }

        public void SubmitChanges() {
            this.session.SubmitChanges();
        }

        protected ISessionTable<T> GetTable<T>() {
            string tableId = this.provider.Mapping.GetTableId(typeof(T));
            return this.session.GetTable<T>(tableId);
        }
    }
}
