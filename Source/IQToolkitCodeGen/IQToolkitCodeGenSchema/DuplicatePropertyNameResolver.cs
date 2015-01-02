using System.Collections.Generic;
using System.Linq;

namespace IQToolkitCodeGenSchema {
    internal class DuplicatePropertyNameResolver {
        public void Deduplicate(IEnumerable<IPropertyName> items) {
            while (this.HasDuplicates(items)) {
                this.Rename(items);
            }
        }

        private void Rename(IEnumerable<IPropertyName> items) {
            var groups = items.GroupBy(x => x.PropertyName).Where(x => x.Count() > 1);

            foreach (var group in groups) {
                var index = -1;
                var newName = string.Empty;

                foreach (var item in group) {
                    index++;

                    // Don't rename the first item.
                    if (index == 0) {
                        continue;
                    }

                    while (true) {
                        newName = item.PropertyName + index;

                        if (!items.Any(x => x.PropertyName == newName)) {
                            item.PropertyName = newName;
                            break;
                        }

                        index++;
                    }
                }
            }
        }

        private bool HasDuplicates(IEnumerable<IPropertyName> items) {
            return items.GroupBy(x => x.PropertyName).Where(x => x.Count() > 1).Any();
        }
    }
}
