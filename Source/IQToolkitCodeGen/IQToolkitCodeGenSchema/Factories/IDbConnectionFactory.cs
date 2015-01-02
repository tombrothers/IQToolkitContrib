using System;
using System.Data.Common;

namespace IQToolkitCodeGenSchema.Factories {
    internal interface IDbConnectionFactory {
        DbConnection Create();
    }
}