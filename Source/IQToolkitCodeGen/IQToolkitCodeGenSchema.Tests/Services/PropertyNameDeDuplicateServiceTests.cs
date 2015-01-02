using System.Collections.Generic;
using System.Linq;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace IQToolkitCodeGenSchema.Tests.Services
{
    [TestClass]
    public class PropertyNameDeDuplicateServiceTests
    {
        [TestMethod]
        public void PropertyNameDeDuplicateServiceTests_DuplicateNamesWhereNewNameAlreadyExistsInListTest()
        {
            var service = new PropertyNameDeDuplicateService();

            var list = new List<IPropertyName> { 
                new MockPropertyName("Employee"), new MockPropertyName("Employee1"), new MockPropertyName("Employee")
            };

            service.Deduplicate(list);
            var actual = list.Select(x => x.PropertyName).OrderBy(x => x).ToArray();
            var expected = new[] { "Employee", "Employee1", "Employee2" };

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PropertyNameDeDuplicateServiceTests_DuplicateNamesTest()
        {
            var service = new PropertyNameDeDuplicateService();

            var list = new List<IPropertyName> { 
                new MockPropertyName("Employee"), new MockPropertyName("Employee")
            };

            service.Deduplicate(list);
            var actual = list.Select(x => x.PropertyName).ToArray();
            var expected = new[] { "Employee", "Employee1" };

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
