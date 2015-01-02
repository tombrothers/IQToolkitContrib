using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace IQToolkitContrib {
    public partial class MemoryRepository : ARepository {
        private Dictionary<Type, object> entities = new Dictionary<Type, object>();

        // allow disabling cloning ... some test scenarios don't require the object uniqueness and could do without the overhead
        public bool DisableCloning { get; set; } 

        protected override string GetPrimaryKeyPropertyName<T>() {
            return Path.GetExtension(typeof(T).FullName).Substring(1) + "Id";
        }

        protected virtual void SetIdentityValue<T>(T entity) where T : class {
            PropertyInfo pi = typeof(T).GetProperty(this.GetPrimaryKeyPropertyName<T>());

            switch (pi.PropertyType.FullName) {
                case "System.Decimal":
                    decimal decimalId = Convert.ToInt64(pi.GetValue(entity, null));

                    if (decimalId <= 0) {
                        pi.SetValue(entity, Convert.ToDecimal(this.List<T>().Count() + 1), null);
                    }

                    break;
                case "System.Int32":
                case "System.Int64":
                    long longId = Convert.ToInt64(pi.GetValue(entity, null));

                    if (longId <= 0) {
                        pi.SetValue(entity, this.List<T>().Count() + 1, null);
                    }

                    break;
                case "System.String":
                    if (string.IsNullOrEmpty(pi.GetValue(entity, null) as string)) {
                        pi.SetValue(entity, (this.List<T>().Count() + 1).ToString(), null);
                    }

                    break;
                case "System.Guid":
                    Guid guidId = (Guid)pi.GetValue(entity, null);

                    if (guidId == default(Guid)) {
                        pi.SetValue(entity, Guid.NewGuid(), null);
                    }

                    break;
                default:
                    throw new NotImplementedException(string.Format("MemoryRepository.SetIdentityValue PropertyType {0} not handled.", pi.PropertyType.FullName));
            }
        }

        public override T Get<T>(object id) {
            return this.Get<T>(id, true);
        }

        private T Get<T>(object id, bool clone) {
            List<T> list = this.GetEntityList<T>();

            T item = list.AsQueryable().FirstOrDefault(this.CreateGetExpression<T>(id));

            if (item != null && clone && !this.DisableCloning) {
                item = this.Clone<T>(item);
            }

            return item;
        }

        public override IQueryable<T> List<T>() {
            return this.List<T>(true);
        }

        private IQueryable<T> List<T>(bool clone) {
            List<T> list = this.GetEntityList<T>();

            if (clone && !this.DisableCloning) {
                list = this.Clone<List<T>>(list);
            }

            return list.AsQueryable();
        }

        public override void Insert<T>(T entity) {
            if (entity is IValidate) {
                ((IValidate)entity).Validate();
            }

            this.SetIdentityValue<T>(entity);
            this.GetEntityList<T>().Add(this.Clone<T>(entity));
        }

        public override void Update<T>(T entity) {
            if (entity is IValidate) {
                ((IValidate)entity).Validate();
            }

            this.Delete<T>(entity);
            this.Insert<T>(entity);
        }

        public override void Delete<T>(T entity) {
            string primaryKeyPropertyName = this.GetPrimaryKeyPropertyName<T>();
            PropertyInfo pi = typeof(T).GetProperty(primaryKeyPropertyName);
            object id = pi.GetValue(entity, null);
            T originalEntity = this.Get<T>(id, false);
            this.GetEntityList<T>().Remove(originalEntity);
        }

        private List<T> GetEntityList<T>() {
            Type t = typeof(T);

            if (!this.entities.ContainsKey(t)) {
                this.entities.Add(t, new List<T>());
            }

            return this.entities[t] as List<T>;
        }

        private T Clone<T>(T o) {
            if (this.DisableCloning) {
                return o;
            }
            else {
                using (MemoryStream ms = new MemoryStream()) {
                    BinaryFormatter bf = new BinaryFormatter();
                    bf.Serialize(ms, o);
                    ms.Position = 0;
                    return (T)bf.Deserialize(ms);
                }
            }
        }
    }
}
