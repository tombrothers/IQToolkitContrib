using System;
using System.Collections.Generic;
using System.Linq;
using IQToolkit;
using IQToolkit.Data;

namespace IQToolkitContrib {
    /// <summary>
    /// This class wraps the actions to the EntityTable to provide a hook into the Insert method.
    /// </summary>
    /// <typeparam name="T">Entity Type.</typeparam>
    internal class DbEntityTable<T> : IEntityTable<T> where T : class {
        private readonly IEntityTable<T> entityTable;
        private DbEntityInserter<T> inserter;
        private DbEntityInserterOrUpdater<T> inserterOrUpdater;

        public DbEntityTable(IEntityTable<T> entityTable) {
            if (entityTable == null) {
                throw new ArgumentNullException("entityTable");
            }

            this.entityTable = entityTable;
        }

        #region IEntityTable<T> Members

        public int Delete(T instance) {
            return this.entityTable.Delete(instance);
        }

        public T GetById(object id) {
            return this.entityTable.GetById(id);
        }

        public int Insert(T instance) {
            if (this.inserter == null) {
                this.inserter = new DbEntityInserter<T>(this.entityTable, this.Provider as DbEntityProvider);
            }

            // Using the DbEntityInserter to handle the insert.
            return this.inserter.Execute(instance);
        }

        public int InsertOrUpdate(T instance) {
            if (this.inserterOrUpdater == null) {
                this.inserterOrUpdater = new DbEntityInserterOrUpdater<T>(this.entityTable, this.Provider as DbEntityProvider);
            }

            // Using the DbEntityInserter to handle the insert.
            return this.inserterOrUpdater.Execute(instance);
        }

        public int Update(T instance) {
            return this.entityTable.Update(instance);
        }

        #endregion

        #region IEntityTable Members

        public int Delete(object instance) {
            return this.entityTable.Delete(instance);
        }

        object IEntityTable.GetById(object id) {
            return this.entityTable.GetById(id);
        }

        public int Insert(object instance) {
            return this.Insert(instance as T);
        }

        public int InsertOrUpdate(object instance) {
            return this.InsertOrUpdate(instance as T);
        }

        public IEntityProvider Provider {
            get { return this.entityTable.Provider; }
        }

        public string TableId {
            get { return this.entityTable.TableId; }
        }

        public int Update(object instance) {
            return this.entityTable.Update(instance);
        }

        #endregion

        #region IQueryable Members

        public Type ElementType {
            get { return this.entityTable.ElementType; }
        }

        public System.Linq.Expressions.Expression Expression {
            get { return this.entityTable.Expression; }
        }

        IQueryProvider IQueryable.Provider {
            get { return ((IQueryable)this.entityTable).Provider; }
        }

        #endregion

        #region IEnumerable Members

        public System.Collections.IEnumerator GetEnumerator() {
            return this.entityTable.GetEnumerator();
        }

        #endregion

        #region IEnumerable<T> Members

        IEnumerator<T> IEnumerable<T>.GetEnumerator() {
            return ((IEnumerable<T>)this.entityTable).GetEnumerator();
        }

        #endregion
    }
}
