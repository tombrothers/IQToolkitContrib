using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Tests {
    internal class MockPropertyName : IPropertyName {
        public string PropertyName { get; set; }

        public MockPropertyName(string propertyName) {
            this.PropertyName = propertyName;
        }
    }
}
