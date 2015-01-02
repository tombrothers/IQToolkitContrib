using System;

namespace IQToolkitCodeGenSchema {
    internal class ArgumentUtility {
        public static T CheckIsDefined<T>(string argumentName, T value) where T : struct {
            if (!Enum.IsDefined(typeof(T), value)) {
                throw new ArgumentOutOfRangeException(argumentName);
            }

            return value;
        }

        public static string CheckNotNullOrEmpty(string argumentName, string value) {
            CheckNotNull(argumentName, value);

            if (string.IsNullOrWhiteSpace(value)) {
                throw new ArgumentException(argumentName);
            }

            return value;
        }

        public static T CheckNotNull<T>(string argumentName, T value) where T : class {
            if (value == null) {
                throw new ArgumentNullException(argumentName);
            }

            return value;
        }
    }
}
