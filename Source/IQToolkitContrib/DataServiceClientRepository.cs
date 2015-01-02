using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Data.Services.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace IQToolkitContrib {
    public class DataServiceClientRepository : ARepository {
        private DataServiceContext context;
        private Dictionary<Type, DataServiceInfo> dataServiceInfoDictionary = new Dictionary<Type, DataServiceInfo>();

        public DataServiceClientRepository(Uri serviceRoot)
            : this(serviceRoot, null) {
        }

        public DataServiceClientRepository(Uri serviceRoot, Action<DataServiceContext> action) {
            this.context = new DataServiceContext(serviceRoot);

            if (action != null) {
                action(this.context);
            }
        }

        public override void Delete<T>(T entity) {
            this.context.DeleteObject(entity);
            this.context.SaveChanges();
        }

        public override T Get<T>(object id) {
            return this.Execute<T>(() => {
                //// The "FirstOrDefault" will not work with LINQtoVFP.  The following statement would cause an error because it does not include an OrderBy.
                //// return this.List<T>().FirstOrDefault(this.CreateGetExpression<T>(id, primaryKeyMemberInfo.Name));               
                List<T> list = this.List<T>().Where<T>(this.CreateGetExpression<T>(id)).ToList();

                if (list.Count == 0) {
                    return default(T);
                }

                return list[0];
            });
        }

        public IEnumerable<T> ExecuteNonEntity<T>(string query) {
            Type type = typeof(T);
            bool isDto = !type.IsPrimitive && type != typeof(string);

            WebRequest req = WebRequest.Create(this.GetQueryUri(query));

            using (WebResponse res = req.GetResponse()) {
                using (StreamReader reader = new StreamReader(res.GetResponseStream())) {
                    XDocument doc = XDocument.Load(reader);

                    if (isDto) {
                        string className = System.IO.Path.GetExtension(type.FullName).Substring(1);

                        // get a list of elements without attributes
                        List<XElement> elements = (from d in doc.Root.Elements()
                                                   select new XElement(className, d.Elements().Select(e => new XElement(e.Name.LocalName, e.Value)).ToArray())).ToList();

                        List<T> list = new List<T>();

                        for (int index = 0, total = elements.Count; index < total; index++) {
                            XmlSerializer serializer = new XmlSerializer(type);

                            using (StringReader sr = new StringReader(elements[index].ToString())) {
                                list.Add((T)serializer.Deserialize(sr));
                            }
                        }

                        return list;
                    }
                    else {
                        return doc.Root.Descendants().Select(d => (T)Convert.ChangeType(d.Value, type, null)).Cast<T>();
                    }
                }
            }
        }

        public IEnumerable<T> Execute<T>(string query) {
            return this.Execute<IEnumerable<T>>(() => {
                return this.context.Execute<T>(this.GetQueryUri(query));
            });
        }

        public T Execute<T>(Func<T> func) {
            return func();

            try {
                return func();
            }
            catch (DataServiceQueryException ex) {
                if (ex.InnerException == null || !ex.InnerException.Message.Contains("Resource not found for the segment")) {
                    throw;
                }
            }

            return default(T);
        }

        private Uri GetQueryUri(string query) {
            return new Uri(this.context.BaseUri + "/" + Uri.EscapeUriString(query));
        }

        protected override string GetPrimaryKeyPropertyName<T>() {
            return this.GetDataServiceInfo<T>().Key;
        }

        public override void Insert<T>(T entity) {
            this.context.AddObject(this.GetDataServiceInfo<T>().EntitySetName, entity);
            this.context.SaveChanges();
        }

        public override IQueryable<T> List<T>() {
            return this.GetDataServiceQuery<T>();
        }

        public override void Update<T>(T entity) {
            this.context.UpdateObject(entity);
            this.context.SaveChanges();
        }

        private DataServiceQuery<T> GetDataServiceQuery<T>() {
            return this.context.CreateQuery<T>(this.GetDataServiceInfo<T>().EntitySetName);
        }

        private DataServiceInfo GetDataServiceInfo<T>() {
            Type type = typeof(T);

            if (this.dataServiceInfoDictionary.ContainsKey(type)) {
                return this.dataServiceInfoDictionary[type];
            }

            DataServiceInfo info = new DataServiceInfo();

            object[] attribs = type.GetCustomAttributes(typeof(DataServiceEntitySetNameAttribute), true);

            if (attribs.Length > 0) {
                info.EntitySetName = ((DataServiceEntitySetNameAttribute)attribs[0]).EntitySetName;
            }

            attribs = type.GetCustomAttributes(typeof(DataServiceKeyAttribute), true);

            if (attribs.Length > 0) {
                DataServiceKeyAttribute attribute = (DataServiceKeyAttribute)attribs[0];

                if (attribute.KeyNames.Count > 1) {
                    throw new NotImplementedException("Multiple KeyNames");
                }

                info.Key = attribute.KeyNames[0];
            }

            this.dataServiceInfoDictionary.Add(type, info);
            return info;
        }
    }
}
