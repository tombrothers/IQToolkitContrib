using IQToolkit;
using IQToolkit.Data;

namespace IQToolkitContrib.Tests.DbEntityTests {
    class NorthwindSession : IQToolkit.Data.EntitySession {
        private readonly DbEntityContextBase entityContext;

        public NorthwindSession(DbEntityContextBase entityContext)
            : base(entityContext.Provider) {
            this.entityContext = entityContext;
        }

        public ISessionTable<Category> Categories {
            get { return this.GetTable<Category>("Categories"); }
        }
    }
}
