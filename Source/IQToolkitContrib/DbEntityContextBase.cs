using System;
using IQToolkit;
using IQToolkit.Data;

namespace IQToolkitContrib {
    public abstract class DbEntityContextBase {
        private readonly DbEntityProvider provider;

        public DbEntityProvider Provider {
            get {
                return this.provider;
            }
        }

        public DbEntityContextBase(DbEntityProvider provider) {
            if (provider == null) {
                throw new ArgumentNullException("provider");
            }

            this.provider = provider;
        }

        protected IEntityTable<T> GetTable<T>() where T : class {
            string tableId = this.provider.Mapping.GetTableId(typeof(T));
            IEntityTable<T> table = this.provider.GetTable<T>(tableId);

            return new DbEntityTable<T>(table);
        }
    }
}
