using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LinqToVfp;
using System.IO;
using System.Net;

namespace IQToolkitContrib.Tests {
    public class LinqToVfpDataContext : DataContext {
        public VfpQueryProvider Provider { get; private set; }

        public LinqToVfpDataContext(string connectionString, TestContextWriter testContextWriter)
            : this(CreateRepository(connectionString), testContextWriter) {
        }

        public LinqToVfpDataContext(Uri uri, TestContextWriter testContextWriter)
            : base(CreateRepository(uri, testContextWriter)) {
        }

        private static IRepository CreateRepository(Uri uri, TestContextWriter testContextWriter) {
            return new DataServiceClientRepository(uri, (context) => {
                context.SendingRequest += (sender, e) => {
                    HttpWebRequest r = e.Request as HttpWebRequest;
                    testContextWriter.WriteLine(r.RequestUri.ToString());
                };
            });
        }

        public LinqToVfpDataContext(IRepository repository, TestContextWriter testContextWriter)
            : base(repository) {

            this.Provider = ((DbEntityRepository)repository).Provider as VfpQueryProvider;
            this.Provider.AutoRightTrimStrings = true;

            if (testContextWriter == null) {
                this.Provider.Log = new IQToolkitContrib.DebuggerWriter();
            }
            else {
                this.Provider.Log = testContextWriter;
            }
            
        }

        private static DbEntityRepository CreateRepository(string connectionString) {
            VfpQueryProvider provider = VfpQueryProvider.Create(connectionString, null);

            Type type = typeof(LinqToVfpDataContext);

            // path of the xml file in the dll
            string xmlPath = Path.GetFileNameWithoutExtension(type.FullName) + ".Mapping.xml";

            using (StreamReader streamReader = new StreamReader(type.Assembly.GetManifestResourceStream(xmlPath))) {
                provider = provider.New(VfpXmlMapping.FromXml(streamReader.ReadToEnd()));
            }

            return new DbEntityRepository(provider);
        }
    }
}
