using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using IQToolkit;
using IQToolkit.Data;
using IQToolkit.Data.Common;

namespace IQToolkitContrib {
    /// <summary>
    /// Repository to work with IQToolkit.Data.Common.DbEntityProvider
    /// </summary>
    public class DbEntityRepository : ARepository {
        /// <summary>
        /// Gets a Provider Instance
        /// </summary>
        public DbEntityProvider Provider { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbEntityRepository"/> class.
        /// </summary>
        /// <param name="provider">Provider Instance</param>
        public DbEntityRepository(DbEntityProvider provider) {
            this.Provider = provider;
        }

        /// <summary>
        /// Gets the Primary Key Property Name
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>Primary Key Property Name</returns>
        protected override string GetPrimaryKeyPropertyName<T>() {
            MappingEntity mappingEntity = this.Provider.Mapping.GetEntity(typeof(T));
            List<MemberInfo> memberInfoList = this.Provider.Mapping.GetPrimaryKeyMembers(mappingEntity).ToList();

            if (memberInfoList.Count != 1) {
                throw new ApplicationException(string.Format("Cannot determine the primary key for {0}", typeof(T)));
            }

            MemberInfo primaryKeyMemberInfo = memberInfoList[0];
            return primaryKeyMemberInfo.Name;
        }

        /// <summary>
        /// Gets an Entity Instance by Id
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Entity Id</param>
        /// <returns>Entity Instance</returns>
        public override T Get<T>(object id) {
            //// The "FirstOrDefault" will not work with LINQtoVFP.  The following statement would cause an error because it does not include an OrderBy.
            //// return this.List<T>().FirstOrDefault(this.CreateGetExpression<T>(id, primaryKeyMemberInfo.Name));   

            List<T> list = this.List<T>().Where<T>(this.CreateGetExpression<T>(id)).ToList();

            if (list.Count == 0) {
                return default(T);
            }

            return list[0];
        }
        
        /// <summary>
        /// Creates an IQueryable Entity List
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>IQueryable Entity List</returns>
        public override IQueryable<T> List<T>() {
            return this.GetEntityTable<T>();
        }

        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        public override void Insert<T>(T entity) {
            if (entity is IValidate) {
                ((IValidate)entity).Validate();
            }

            this.GetEntityTable<T>().Insert<T>(entity);
        }
        
        /// <summary>
        /// Update Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        public override void Update<T>(T entity) {
            if (entity is IValidate) {
                ((IValidate)entity).Validate();
            }

            this.GetEntityTable<T>().Update<T>(entity);
        }

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        public override void Delete<T>(T entity) {
            this.GetEntityTable<T>().Delete<T>(entity);
        }

        /// <summary>
        /// Get an IQToolkit.IEntityTable Instance for the Entity Type
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>IEntityTable Instance</returns>
        private IEntityTable<T> GetEntityTable<T>() {
            return this.Provider.GetTable<T>(this.Provider.Mapping.GetTableId(typeof(T)));
        }
    }
}
