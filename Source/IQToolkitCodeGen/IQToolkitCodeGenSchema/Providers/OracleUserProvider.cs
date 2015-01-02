using System;
using System.Data;
using System.Linq;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGenSchema.Providers {
    internal class OracleUserProvider : IOracleUserProvider {
        private readonly ISchemaOptions _schemaOptions;
        private readonly IDbConnectionFactory _connectionFactory;
        public string User { get; private set; }

        public OracleUserProvider(ISchemaOptions schemaOptions, IDbConnectionFactory connectionFactory) {
            ArgumentUtility.CheckNotNull("schemaOptions", schemaOptions);
            ArgumentUtility.CheckNotNull("connectionFactory", connectionFactory);

            this._schemaOptions = schemaOptions;
            this._connectionFactory = connectionFactory;

            this.User = this.GetUser();
        }

        private string GetUser() {
            var user = this.ParseUserFromConnectionString();

            if (string.IsNullOrWhiteSpace(user)) {
                return string.Empty;
            }

            DataTable users = null;

            using (var connection = this._connectionFactory.Create()) {
                connection.DoConnected(() => {
                    users = connection.GetSchema("Users");
                });
            }

            return users.AsEnumerable()
                        .Select(x => x.Field<string>("Name"))
                        .FirstOrDefault(x => x.Equals(user, StringComparison.InvariantCultureIgnoreCase));
        }

        private string ParseUserFromConnectionString() {
            const string USERID = "USER ID=";
            var index = this._schemaOptions.ConnectionString.ToUpper().IndexOf(USERID);

            if (index >= 0) {
                var temp = this._schemaOptions.ConnectionString.Substring(index + USERID.Length);

                index = temp.IndexOf(";");

                if (index >= 0) {
                    return temp.Substring(0, index);
                }
            }

            return string.Empty;
        }
    }
}
