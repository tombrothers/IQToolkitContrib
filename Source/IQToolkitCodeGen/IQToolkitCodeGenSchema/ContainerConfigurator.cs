using System;
using IQToolkitCodeGenSchema.Factories;
using IQToolkitCodeGenSchema.Models;
using IQToolkitCodeGenSchema.Providers;
using IQToolkitCodeGenSchema.Services;
using Microsoft.Practices.Unity;
using AssociationSchemaProviders = IQToolkitCodeGenSchema.Providers.AssociationSchemaProviders;
using ColumnSchemaProviders = IQToolkitCodeGenSchema.Providers.ColumnSchemaProviders;
using ColumnTypeSchemaProviders = IQToolkitCodeGenSchema.Providers.ColumnTypeSchemaProviders;
using DbTypeProviders = IQToolkitCodeGenSchema.Providers.DbTypeProviders;
using PrimaryKeySchemaProviders = IQToolkitCodeGenSchema.Providers.PrimaryKeySchemaProviders;
using TableSchemaProviders = IQToolkitCodeGenSchema.Providers.TableSchemaProviders;

namespace IQToolkitCodeGenSchema {
    internal static class ContainerConfigurator {
        public static void Configure(IUnityContainer container, ISchemaOptions schemaOptions) {
            container.RegisterInstance(typeof(ISchemaOptions), schemaOptions);
            container.RegisterInstance(typeof(IDatabase), schemaOptions.Database);
            container.RegisterType<IConnectionStringValidator, ConnectionStringValidator>();

            RegisterFactories(container, schemaOptions);
            RegisterProviders(container, schemaOptions);
            RegisterServices(container);
        }

        private static void RegisterServices(IUnityContainer container) {
            container.RegisterType<IAssociationSchemaService, AssociationSchemaService>();
            container.RegisterType<IColumnSchemaService, ColumnSchemaService>();
            container.RegisterType<IColumnTypeSchemaService, ColumnTypeSchemaService>();
            container.RegisterType<IDbTypeService, DbTypeService>();
            container.RegisterType<IPrimaryKeySchemaService, PrimaryKeySchemaService>();
            container.RegisterType<ITableSchemaService, TableSchemaService>();
            container.RegisterType<IPluralizationService, PluralizationService>();
            container.RegisterType<IPropertyNameDeDuplicateService, PropertyNameDeDuplicateService>();
        }

        private static void RegisterFactories(IUnityContainer container, ISchemaOptions schemaOptions) {
            var validator = container.Resolve<IConnectionStringValidator>();
            string connectionString = validator.Validate(schemaOptions.ConnectionString);

            container.RegisterType<IDbConnectionFactory, DbConnectionFactory>(new InjectionConstructor(schemaOptions.Database.ProviderName, connectionString));
            container.RegisterType<IDataTableFactory, DataTableFactory>();

            Func<IColumnSchemaService> createColumnSchemaService = () => container.Resolve<IColumnSchemaService>();
            container.RegisterType<IColumnSchemaServiceFactory, ColumnSchemaServiceFactory>(new InjectionConstructor(createColumnSchemaService));

            Func<IPrimaryKeySchemaService> createPrimaryKeySchemaService = () => container.Resolve<IPrimaryKeySchemaService>();
            container.RegisterType<IPrimaryKeySchemaServiceFactory, PrimaryKeySchemaServiceFactory>(new InjectionConstructor(createPrimaryKeySchemaService));
        }

        private static void RegisterProviders(IUnityContainer container, ISchemaOptions schemaOptions) {
            RegisterAssociationSchemaProvider(container, schemaOptions);
            RegisterColumnSchemaProvider(container, schemaOptions);
            RegisterColumnTypeSchemaProvider(container, schemaOptions.Database.Type);
            RegisterDbTypeProvider(container, schemaOptions.Database.Type);
            RegisterPrimaryKeySchemaProvider(container, schemaOptions.Database.Type);
            RegisterTableSchemaProvider(container, schemaOptions);
            container.RegisterType<ISchemaProvider, SchemaProvider>();

            if (schemaOptions.Database.Type == DatabaseType.Oracle || schemaOptions.Database.Type == DatabaseType.OracleODP) {
                container.RegisterType<IOracleUserProvider, OracleUserProvider>(new ContainerControlledLifetimeManager());
            }
        }

        private static void RegisterTableSchemaProvider(IUnityContainer container, ISchemaOptions schemaOptions) {
            if (schemaOptions.Database.AllowCustomSchemaSql && !string.IsNullOrWhiteSpace(schemaOptions.TableSchemaSql)) {
                container.RegisterType<TableSchemaProviders.IProvider, TableSchemaProviders.CustomSqlProvider>();
                return;
            }

            switch (schemaOptions.Database.Type) {
                case DatabaseType.Access:
                    container.RegisterType<TableSchemaProviders.IProvider, TableSchemaProviders.AccessProvider>();
                    break;
                case DatabaseType.SQLite:
                    container.RegisterType<TableSchemaProviders.IProvider, TableSchemaProviders.SQLiteProvider>();
                    break;
                case DatabaseType.Oracle:
                case DatabaseType.OracleODP:
                    container.RegisterType<TableSchemaProviders.IProvider, TableSchemaProviders.OracleProvider>();
                    break;
                case DatabaseType.Vfp:
                    container.RegisterType<TableSchemaProviders.IProvider, TableSchemaProviders.Provider>();
                    break;
                case DatabaseType.MySql:
                case DatabaseType.SqlCe35:
                case DatabaseType.SqlCe40:
                case DatabaseType.SqlServer:
                    container.RegisterType<TableSchemaProviders.IProvider, TableSchemaProviders.InformationSchemaProvider>();
                    break;
                default:
                    throw new NotImplementedException(schemaOptions.Database.Type.ToString());
            }
        }

        private static void RegisterPrimaryKeySchemaProvider(IUnityContainer container, DatabaseType databaseType) {
            Func<string, string> getQuotedName;

            switch (databaseType) {
                case DatabaseType.Vfp:
                    container.RegisterType<PrimaryKeySchemaProviders.IProvider, PrimaryKeySchemaProviders.VfpProvider>();
                    break;
                case DatabaseType.Oracle:
                case DatabaseType.OracleODP:
                    getQuotedName = x => "\"" + x + "\"";
                    container.RegisterType<PrimaryKeySchemaProviders.IProvider, PrimaryKeySchemaProviders.Provider>(new InjectionConstructor(container.Resolve<IDataTableFactory>(), getQuotedName));
                    break;
                case DatabaseType.MySql:
                    getQuotedName = x => x;
                    container.RegisterType<PrimaryKeySchemaProviders.IProvider, PrimaryKeySchemaProviders.Provider>(new InjectionConstructor(container.Resolve<IDataTableFactory>(), getQuotedName));
                    break;
                case DatabaseType.Access:
                case DatabaseType.SqlCe35:
                case DatabaseType.SqlCe40:
                case DatabaseType.SQLite:
                case DatabaseType.SqlServer:
                    getQuotedName = x => "[" + x + "]";
                    container.RegisterType<PrimaryKeySchemaProviders.IProvider, PrimaryKeySchemaProviders.Provider>(new InjectionConstructor(container.Resolve<IDataTableFactory>(), getQuotedName));
                    break;
                default:
                    throw new NotImplementedException(databaseType.ToString());
            }
        }

        private static void RegisterDbTypeProvider(IUnityContainer container, DatabaseType databaseType) {
            switch (databaseType) {
                case DatabaseType.Vfp:
                    container.RegisterType<DbTypeProviders.IProvider, DbTypeProviders.VfpProvider>();
                    break;
                case DatabaseType.Oracle:
                case DatabaseType.OracleODP:
                    container.RegisterType<DbTypeProviders.IProvider, DbTypeProviders.OracleProvider>();
                    break;
                case DatabaseType.Access:
                case DatabaseType.SQLite:
                case DatabaseType.MySql:
                case DatabaseType.SqlCe35:
                case DatabaseType.SqlCe40:
                case DatabaseType.SqlServer:
                    container.RegisterType<DbTypeProviders.IProvider, DbTypeProviders.Provider>();
                    break;
                default:
                    throw new NotImplementedException(databaseType.ToString());
            }
        }

        private static void RegisterColumnTypeSchemaProvider(IUnityContainer container, DatabaseType databaseType) {
            switch (databaseType) {
                case DatabaseType.Access:
                    container.RegisterType<ColumnTypeSchemaProviders.IProvider, ColumnTypeSchemaProviders.AccessProvider>();
                    break;
                case DatabaseType.Vfp:
                    container.RegisterType<ColumnTypeSchemaProviders.IProvider, ColumnTypeSchemaProviders.VfpProvider>();
                    break;
                case DatabaseType.SqlCe35:
                    container.RegisterType<ColumnTypeSchemaProviders.IProvider, ColumnTypeSchemaProviders.SqlCe35Provider>();
                    break;
                case DatabaseType.SQLite:
                case DatabaseType.Oracle:
                case DatabaseType.OracleODP:
                case DatabaseType.MySql:
                case DatabaseType.SqlCe40:
                case DatabaseType.SqlServer:
                    container.RegisterType<ColumnTypeSchemaProviders.IProvider, ColumnTypeSchemaProviders.Provider>();
                    break;
                default:
                    throw new NotImplementedException(databaseType.ToString());
            }
        }

        private static void RegisterColumnSchemaProvider(IUnityContainer container, ISchemaOptions schemaOptions) {
            if (schemaOptions.Database.AllowCustomSchemaSql && !string.IsNullOrWhiteSpace(schemaOptions.ColumnSchemaSql)) {
                container.RegisterType<ColumnSchemaProviders.IProvider, ColumnSchemaProviders.CustomSqlProvider>();
                return;
            }

            switch (schemaOptions.Database.Type) {
                case DatabaseType.Access:
                    container.RegisterType<ColumnSchemaProviders.IProvider, ColumnSchemaProviders.OleDbProvider>();
                    break;
                case DatabaseType.Oracle:
                case DatabaseType.OracleODP:
                    container.RegisterType<ColumnSchemaProviders.IProvider, ColumnSchemaProviders.OracleProvider>();
                    break;
                case DatabaseType.SQLite:
                    container.RegisterType<ColumnSchemaProviders.IProvider, ColumnSchemaProviders.Provider>();
                    break;
                case DatabaseType.Vfp:
                    container.RegisterType<ColumnSchemaProviders.IProvider, ColumnSchemaProviders.VfpProvider>();
                    break;
                case DatabaseType.MySql:
                case DatabaseType.SqlCe35:
                case DatabaseType.SqlCe40:
                case DatabaseType.SqlServer:
                    container.RegisterType<ColumnSchemaProviders.IProvider, ColumnSchemaProviders.InformationSchemaProvider>();
                    break;
                default:
                    throw new NotImplementedException(schemaOptions.Database.Type.ToString());
            }
        }

        private static void RegisterAssociationSchemaProvider(IUnityContainer container, ISchemaOptions schemaOptions) {
            if (schemaOptions.Database.AllowCustomSchemaSql && !string.IsNullOrWhiteSpace(schemaOptions.AssociationSchemaSql)) {
                container.RegisterType<AssociationSchemaProviders.IProvider, AssociationSchemaProviders.CustomSqlProvider>();
                return;
            }

            switch (schemaOptions.Database.Type) {
                case DatabaseType.Access:
                case DatabaseType.Vfp:
                    container.RegisterType<AssociationSchemaProviders.IProvider, AssociationSchemaProviders.OleDbProvider>();
                    break;
                case DatabaseType.Oracle:
                case DatabaseType.OracleODP:
                    container.RegisterType<AssociationSchemaProviders.IProvider, AssociationSchemaProviders.OracleProvider>();
                    break;
                case DatabaseType.MySql:
                    container.RegisterType<AssociationSchemaProviders.IProvider, AssociationSchemaProviders.MySqlProvider>();
                    break;
                case DatabaseType.SqlServer:
                    container.RegisterType<AssociationSchemaProviders.IProvider, AssociationSchemaProviders.SqlServerProvider>();
                    break;
                case DatabaseType.SqlCe35:
                case DatabaseType.SqlCe40:
                case DatabaseType.SQLite:
                    container.RegisterType<AssociationSchemaProviders.IProvider, AssociationSchemaProviders.EmptyProvider>();
                    break;
                default:
                    throw new NotImplementedException(schemaOptions.Database.Type.ToString());
            }
        }
    }
}