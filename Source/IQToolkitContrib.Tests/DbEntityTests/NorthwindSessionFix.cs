using IQToolkit;
using IQToolkit.Data;

namespace IQToolkitContrib.Tests.DbEntityTests {
    class NorthwindSessionFix : DbEntitySessionBase {
        public NorthwindSessionFix(DbEntityProvider provider)
            : base(provider) {
        }

        public ISessionTable<Category> Categories {
            get { return this.GetTable<Category>(); }
        }
    }
}
