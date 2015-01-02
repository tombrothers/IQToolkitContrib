using System.Linq;

namespace IQToolkitContrib {
    /// <summary>
    /// Repository Interface
    /// </summary>
    public interface IRepository {
        /// <summary>
        /// Gets an Entity Instance by Id
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Entity Id</param>
        /// <returns>Entity Instance</returns>
        T Get<T>(object id) where T : class;

        /// <summary>
        /// Creates an IQueryable Entity List
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>IQueryable Entity List</returns>
        IQueryable<T> List<T>() where T : class;

        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        void Insert<T>(T entity) where T : class;

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        void Update<T>(T entity) where T : class;

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        void Delete<T>(T entity) where T : class;
    }
}
