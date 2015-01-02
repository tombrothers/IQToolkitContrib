using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;
using IQToolkit;
using IQToolkit.Data;
using IQToolkit.Data.Common;

namespace IQToolkitContrib.Web {
    /// <summary>
    /// IQToolkit Implementation of LinqDataSourceView
    /// </summary>
    public class DataSourceView : LinqDataSourceView {
        /// <summary>
        /// DataSource Instance
        /// </summary>
        private DataSource owner;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSourceView"/> class.
        /// </summary>
        /// <param name="owner">DataSource Instance</param>
        /// <param name="name">Data View Name</param>
        /// <param name="context">Http Context Instance</param>
        public DataSourceView(LinqDataSource owner, string name, HttpContext context)
            : base(owner, name, context) {
            this.owner = (DataSource)owner;
        }

        /// <summary>
        /// Make sure that the data context has a property that implements IEntityTable
        /// </summary>
        /// <param name="contextType">Context Type</param>
        /// <param name="selecting">Determines if called by a Select command</param>
        protected override void ValidateContextType(Type contextType, bool selecting) {
            if (!selecting && contextType.GetProperties().Where(p => p.PropertyType.GetInterface("IEntityTable") != null).Count() == 0) {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The data context used by IQToolkit-DataSourceView '{0}' must have an IEntityTable Property when the Delete, Insert or Update operations are enabled.", this.owner.ID));
            }
        }

        /// <summary>
        /// Make sure that the table implements IEntityTable
        /// </summary>
        /// <param name="tableType">Table Type</param>
        /// <param name="selecting">Determines if called by a Select command</param>
        protected override void ValidateTableType(Type tableType, bool selecting) {
            if (!selecting && (!tableType.IsGenericType || tableType.GetInterface("IEntityTable") == null)) {
                throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The table property used by IQToolkit-DataSourceView '{0}' must extend IEntityTable when the Delete, Insert or Update operations are enabled.", this.owner.ID));
            }
        }

        /// <summary>
        /// Delete Data Object
        /// </summary>
        /// <param name="dataContext">Data Context</param>
        /// <param name="table">Data Table</param>
        /// <param name="oldDataObject">Old Data Object</param>
        protected override void DeleteDataObject(object dataContext, object table, object oldDataObject) {
            ((IEntityTable)table).Delete(oldDataObject);
        }

        /// <summary>
        /// Update Data Object
        /// </summary>
        /// <param name="dataContext">Data Context</param>
        /// <param name="table">Data Table</param>
        /// <param name="oldDataObject">Old Data Object</param>
        /// <param name="newDataObject">New Data Object</param>
        protected override void UpdateDataObject(object dataContext, object table, object oldDataObject, object newDataObject) {
            if (newDataObject is IValidate) {
                ((IValidate)newDataObject).Validate();
            }

            ((IEntityTable)table).Update(newDataObject);
        }

        /// <summary>
        /// Insert Data Object
        /// </summary>
        /// <param name="dataContext">Data Context</param>
        /// <param name="table">Data Table</param>
        /// <param name="newDataObject">New Data Object</param>
        protected override void InsertDataObject(object dataContext, object table, object newDataObject) {
            if (newDataObject is IValidate) {
                ((IValidate)newDataObject).Validate();
            }

            if (this.owner.RetrieveGeneratedId) {
                // look for the Provider property
                PropertyInfo pi = dataContext.GetType().GetProperties().Where(p => p.PropertyType.IsSubclassOf(typeof(DbEntityProvider))).FirstOrDefault();

                if (pi != null) {
                    // cast the Provider
                    DbEntityProvider provider = (DbEntityProvider)pi.GetValue(dataContext);
                    
                    // get the Mapping for the Provider
                    BasicMapping mapping = (BasicMapping)provider.Mapping;
                    
                    // get the Mapping Entity for the data object
                    MappingEntity mappingEntity = mapping.GetEntity(newDataObject.GetType());

                    // get the primary key MemberInfo
                    MemberInfo primaryKeyMemberInfo = mapping.GetMappedMembers(mappingEntity).Where(m => mapping.IsPrimaryKey(mappingEntity, m)).Single();

                    // verify that the primary key is generated by calling the protected method IsGenerated
                    MethodBase isGeneratedMethod = mapping.GetType().GetMethod("IsGenerated", BindingFlags.NonPublic | BindingFlags.Instance);
                    bool isGeneratedId = (bool)isGeneratedMethod.Invoke(mapping, new object[] { mappingEntity, primaryKeyMemberInfo });

                    if (isGeneratedId) {
                        // create a type array for the generic types for the Insert<T, S> method
                        Type[] genericTypes = new Type[] { newDataObject.GetType(), typeof(int) };
                        
                        // create type for Expression<Func<T, S>>
                        Type functionExpressionType = Expression.GetFuncType(genericTypes);
                        
                        // create a Expression Parameter for the data object type
                        var p1 = Expression.Parameter(newDataObject.GetType(), "instance");
                        
                        // create a Lambda Expression for the Func<T, S> call ... expression example:  d => d.ID
                        LambdaExpression expression = Expression.Lambda(functionExpressionType, Expression.Property(p1, primaryKeyMemberInfo.Name), p1);

                        // MethodInfo for: public static S Insert<T, S>(this IUpdatable<T> collection, T instance, Expression<Func<T, S>> resultSelector)
                        MethodInfo mi = typeof(IQToolkit.Updatable).GetMethods().Where(d => d.Name == "Insert" && d.IsGenericMethod && d.ReturnType.FullName == null).First();
                        mi = mi.MakeGenericMethod(genericTypes);
                        object id = mi.Invoke(null, new object[] { table, newDataObject, expression });

                        // set the newDataObject's primary key property
                        newDataObject.GetType().GetProperty(primaryKeyMemberInfo.Name).SetValue(newDataObject, (int)id);
                        return;
                    }
                }
            }

            ((IEntityTable)table).Insert(newDataObject);
        }
    }
}