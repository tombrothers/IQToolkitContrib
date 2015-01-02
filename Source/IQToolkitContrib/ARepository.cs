using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace IQToolkitContrib {
    /// <summary>
    /// Repository Abstract Class
    /// </summary>
    public abstract class ARepository : IRepository {
        /// <summary>
        /// Creates a Expression used to Get an Entity Instance by Id
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Entity Id</param>
        /// <returns>Expression used to Get an Entity Instance by Id</returns>
        protected Expression<Func<T, bool>> CreateGetExpression<T>(object id) {
            ParameterExpression e = Expression.Parameter(typeof(T), "e");
            PropertyInfo pi = typeof(T).GetProperty(this.GetPrimaryKeyPropertyName<T>());
            MemberExpression m = Expression.MakeMemberAccess(e, pi);
            ConstantExpression c = Expression.Constant(id, id.GetType());
            BinaryExpression b = Expression.Equal(m, c);
            Expression<Func<T, bool>> lambda = Expression.Lambda<Func<T, bool>>(b, e);
            return lambda;
        }

        /// <summary>
        /// Gets the Primary Key Property Name
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>Primary Key Property Name</returns>
        protected abstract string GetPrimaryKeyPropertyName<T>();

        /// <summary>
        /// Gets an Entity Instance by Id
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Entity Id</param>
        /// <returns>Entity Instance</returns>
        public abstract T Get<T>(object id) where T : class;

        /// <summary>
        /// Creates an IQueryable Entity List
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>IQueryable Entity List</returns>
        public abstract IQueryable<T> List<T>() where T : class;

        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        public abstract void Insert<T>(T entity) where T : class;

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        public abstract void Update<T>(T entity) where T : class;

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        public abstract void Delete<T>(T entity) where T : class;
    }
}
