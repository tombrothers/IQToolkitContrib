using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGenSchema.Tests {
    [TestClass]
    public class DuplicatePropertyNameResolverTests {
        [TestMethod]
        public void DuplicatePropertyNameResolverTests_DuplicateNamesWhereNewNameAlreadyExistsInListTest() {
            var resolver = new DuplicatePropertyNameResolver();

            var list = new List<IPropertyName> { 
                new MockPropertyName("Employee"), new MockPropertyName("Employee1"), new MockPropertyName("Employee")
            };

            resolver.Deduplicate(list);
            var actual = list.Select(x => x.PropertyName).OrderBy(x => x).ToArray();
            var expected = new[] { "Employee", "Employee1", "Employee2" };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DuplicatePropertyNameResolverTests_DuplicateNamesTest() {
            var resolver = new DuplicatePropertyNameResolver();

            var list = new List<IPropertyName> { 
                new MockPropertyName("Employee"), new MockPropertyName("Employee")
            };

            resolver.Deduplicate(list);
            var actual = list.Select(x => x.PropertyName).ToArray();
            var expected = new[] { "Employee", "Employee1" };
            
            CollectionAssert.AreEqual(expected, actual);
        }
    }

    class MockPropertyName : IPropertyName {
        public string PropertyName { get; set; }

        public MockPropertyName(string propertyName) {
            this.PropertyName = propertyName;
        }
    }
}
