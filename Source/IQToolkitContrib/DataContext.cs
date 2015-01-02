using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQToolkitContrib {
    /// <summary>
    /// The DataContext class is a wrapper class for a Repository Instance.  
    /// </summary>
    public class DataContext : IRepository {
        /// <summary>
        /// Repository Instance
        /// </summary>
        private IRepository repository;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="DataContext"/> class.
        /// </summary>
        /// <param name="repository">Repository Instance</param>
        public DataContext(IRepository repository) {
            this.repository = repository;
        }

        /// <summary>
        /// Gets an Entity Instance by Id
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="id">Entity Id</param>
        /// <returns>Entity Instance</returns>
        public virtual T Get<T>(object id) where T : class {
            return this.repository.Get<T>(id);
        }

        /// <summary>
        /// Creates an IQueryable Entity List
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <returns>IQueryable Entity List</returns>
        public virtual IQueryable<T> List<T>() where T : class {
            return this.repository.List<T>();
        }

        /// <summary>
        /// Insert Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        public virtual void Insert<T>(T entity) where T : class {
            this.repository.Insert<T>(entity);
        }

        /// <summary>
        /// Update Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        public virtual void Update<T>(T entity) where T : class {
            this.repository.Update<T>(entity);
        }

        /// <summary>
        /// Delete Entity
        /// </summary>
        /// <typeparam name="T">Entity Type</typeparam>
        /// <param name="entity">Entity Instance</param>
        public virtual void Delete<T>(T entity) where T : class {
            this.repository.Delete<T>(entity);
        }
    }
}
