using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Services;
using System.Linq;
using System.Reflection;
using IQToolkit;
using IQToolkit.Data;

namespace IQToolkitContrib {
    // This class was derived from the following links
    // http://code.msdn.microsoft.com/IUpdateableLinqToSql/
    // http://mtaulty.com/CommunityServer/blogs/mike_taultys_blog/archive/2008/06/14/10513.aspx
    public abstract class ADataServiceContext : System.Data.Services.IUpdatable {
        private DbEntityProvider provider;
        private IEntitySession session;
        private Dictionary<string, Type> typeDictionary = new Dictionary<string, Type>();

        public ADataServiceContext(DbEntityProvider provider) {
            this.provider = provider;
            this.session = new EntitySession(provider);
        }

        void System.Data.Services.IUpdatable.ClearChanges() {
            this.session = new EntitySession(this.provider);
        }

        void System.Data.Services.IUpdatable.DeleteResource(object targetResource) {
            this.session
                    .GetTable(targetResource.GetType(), this.GetTableId(targetResource.GetType()))
                    .SetSubmitAction(targetResource, SubmitAction.Delete);
        }

        object System.Data.Services.IUpdatable.ResolveResource(object resource) {
            return resource;
        }

        void System.Data.Services.IUpdatable.SaveChanges() {
            this.session.SubmitChanges();
        }

        object System.Data.Services.IUpdatable.GetResource(IQueryable query, string fullTypeName) {
            object result = null;

            foreach (object item in query) {
                if (result != null) {
                    throw new DataServiceException("A single resource is expected");
                }

                result = item;
            }

            if (result == null) {
                throw new DataServiceException(404, "Resource not found");
            }

            Type resultType = result.GetType();

            if (fullTypeName != null) {
                if (resultType.FullName != fullTypeName) {
                    throw new DataServiceException("Resource type mismatch");
                }
            }

            this.session.GetTable(resultType, this.GetTableId(resultType)).SetSubmitAction(result, SubmitAction.Update);

            return result;
        }

        private string GetTableId(Type type) {
            return this.provider.Mapping.GetTableId(type);
        }

        void System.Data.Services.IUpdatable.RemoveReferenceFromCollection(object targetResource, string propertyName, object resourceToBeRemoved) {
            PropertyInfo pi = this.GetPropertyInfoForType(targetResource.GetType(), propertyName, false);
            IList collection = (IList)pi.GetValue(targetResource, null);
            collection.Remove(resourceToBeRemoved);
        }

        void System.Data.Services.IUpdatable.AddReferenceToCollection(object targetResource, string propertyName, object resourceToBeAdded) {
            PropertyInfo pi = this.GetPropertyInfoForType(targetResource.GetType(), propertyName, false);
            IList collection = (IList)pi.GetValue(targetResource, null);
            collection.Add(resourceToBeAdded);
        }

        void System.Data.Services.IUpdatable.SetReference(object targetResource, string propertyName, object propertyValue) {
            ((System.Data.Services.IUpdatable)this).SetValue(targetResource, propertyName, propertyValue);
        }

        object System.Data.Services.IUpdatable.GetValue(object targetResource, string propertyName) {
            Type t = targetResource.GetType();

            PropertyInfo pi = this.GetPropertyInfoForType(t, propertyName, false);

            object value = null;

            try {
                value = pi.GetValue(targetResource, null);
            }
            catch (Exception ex) {
                throw new DataServiceException(string.Format("Failed getting property {0} value", propertyName), ex);
            }

            return value;
        }

        void System.Data.Services.IUpdatable.SetValue(object targetResource, string propertyName, object propertyValue) {
            PropertyInfo pi = this.GetPropertyInfoForType(targetResource.GetType(), propertyName, true);

            try {
                pi.SetValue(targetResource, propertyValue, null);
            }
            catch (Exception ex) {
                throw new DataServiceException(string.Format("Error setting property {0} to {1}", propertyName, propertyValue), ex);
            }
        }

        object System.Data.Services.IUpdatable.CreateResource(string containerName, string fullTypeName) {
            Type type = Type.GetType(fullTypeName);

            if (type == null) {
                if (this.typeDictionary.ContainsKey(fullTypeName)) {
                    type = this.typeDictionary[fullTypeName];
                }
                else {
                    type = AppDomain.CurrentDomain.GetAssemblies()
                            .Where(a => a.GetType(fullTypeName) != null)
                            .Select(a => a.GetType(fullTypeName))
                            .Single();

                    this.typeDictionary.Add(fullTypeName, type);
                }
            }

            object value = this.Construct(type);

            this.session.GetTable(type, this.GetTableId(type)).SetSubmitAction(value, SubmitAction.Insert);
            return value;
        }

        object System.Data.Services.IUpdatable.ResetResource(object resource) {
            throw new NotImplementedException();
        }

        private PropertyInfo GetPropertyInfoForType(Type t, string propertyName, bool setter) {
            PropertyInfo pi = null;

            try {
                BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
                flags |= setter ? BindingFlags.SetProperty : BindingFlags.GetProperty;

                pi = t.GetProperty(propertyName, flags);

                if (pi == null) {
                    throw new DataServiceException(string.Format("Failed to find property {0} on type {1}", propertyName, t.Name));
                }
            }
            catch (Exception exception) {
                throw new DataServiceException(string.Format("Error finding property {0}", propertyName), exception);
            }

            return pi;
        }

        private object Construct(Type t) {
            ConstructorInfo ci = t.GetConstructor(Type.EmptyTypes);

            if (ci == null) {
                throw new DataServiceException(string.Format("No default ctor found for type {0}", t.Name));
            }

            return ci.Invoke(null);
        }
    }
}
