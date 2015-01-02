using IQToolkit;
using IQToolkit.Data.Mapping;

namespace IQToolkitContrib.Tests.DbEntityTests {
    class NorthwindAttributes : NorthwindContext {
        public NorthwindAttributes(string connectionString)
            : base(connectionString) {
        }

        [Table(Name = "Categories")]
        [Column(Member = "CategoryId", IsPrimaryKey = true, IsGenerated = true)]
        [Column(Member = "CategoryName")]
        public override IEntityTable<Category> Categories {
            get { return base.Categories; }
        }
    }
}
