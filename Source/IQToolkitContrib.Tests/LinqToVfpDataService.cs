using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Services;

namespace IQToolkitContrib.Tests {
#if DEBUG
    [System.ServiceModel.ServiceBehavior(IncludeExceptionDetailInFaults = true)]
#endif
    public class LinqToVfpDataService : DataService<LinqToVfpDataServiceContext> {
        protected override LinqToVfpDataServiceContext CreateDataSource() {
            return new LinqToVfpDataServiceContext(new LinqToVfpDataContext(LinqToVfpRepositoryTests.GetConnectionString(), null));
        }
        
        public static void InitializeService(IDataServiceConfiguration config) {
#if DEBUG
            config.UseVerboseErrors = true;
#endif
            config.SetEntitySetAccessRule("*", EntitySetRights.All);
            config.SetServiceOperationAccessRule("*", ServiceOperationRights.AllRead);
        }
    }
}
