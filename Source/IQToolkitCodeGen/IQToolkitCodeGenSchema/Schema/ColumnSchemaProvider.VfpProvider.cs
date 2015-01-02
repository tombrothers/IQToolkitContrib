using System.Data;

namespace IQToolkitCodeGenSchema.Schema {
    internal partial class ColumnSchemaProvider {
        private class VfpProvider : OleDbProvider {
            protected override long? GetMaxLength(DataRow row) {
                var maxLength = base.GetMaxLength(row);

                const int MAX_VFP_CHAR_SIZE = 254;

                if (maxLength.HasValue && maxLength.Value > MAX_VFP_CHAR_SIZE) {
                    return null;
                }

                return maxLength;
            }
        }
    }
}
