using IQToolkit;
using LinqToVfp;

namespace IQToolkitContrib.Tests.DbEntityTests {
    class NorthwindContext : DbEntityContextBase {
        public NorthwindContext(string connectionString)
            : this(connectionString, typeof(NorthwindAttributes).FullName) {
        }

        public NorthwindContext(string connectionString, string mappingId)
            : base(VfpQueryProvider.Create(connectionString, mappingId)) {
        }

        public virtual IEntityTable<Category> Categories {
            get { return this.GetTable<Category>(); }
        }
    }
}
