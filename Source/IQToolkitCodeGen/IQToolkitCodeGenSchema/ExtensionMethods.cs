using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading;

namespace IQToolkitCodeGenSchema {
    public static class ExtensionMethods {
        public static void DoConnected(this DbConnection connection, Action action) {
            if (action == null) {
                return;
            }

            bool closeConnection = false;

            if (connection.State == ConnectionState.Closed) {
                connection.Open();
                closeConnection = true;
            }

            try {
                action();
            }
            finally {
                if (closeConnection) {
                    connection.Close();
                }
            }
        }

        public static bool ShouldForceProperCase(this string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return false;
            }

            return value.ToCharArray().All(x => char.IsUpper(x) || char.IsWhiteSpace(x) || char.IsNumber(x));
        }

        public static string ToSafeClrName(this string value, bool? forceToProper = null) {
            if (string.IsNullOrWhiteSpace(value)) {
                return value;
            }

            if (forceToProper.HasValue && forceToProper.Value) {
                value = value.ToLower();
            }

            if (!char.IsUpper(value[0])) {
                value = value.ToProper();
            }

            return value.Replace(" ", "_")
                        .Replace(".", "_");
        }

        public static string ToProper(this string value) {
            if (string.IsNullOrWhiteSpace(value)) {
                return value;
            }

            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(value);
        }

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) {
            if (enumerable == null || action == null) {
                return;
            }

            foreach (var item in enumerable) {
                action(item);
            }
        }
    }
}
