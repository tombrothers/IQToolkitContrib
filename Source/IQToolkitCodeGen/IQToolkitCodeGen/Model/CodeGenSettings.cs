using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Xml.Serialization;
using IQToolkitCodeGen.Core;
using IQToolkitCodeGen.Event;
using IQToolkitCodeGen.Service;
using Microsoft.Practices.Prism.Events;
using IQToolkitCodeGenSchema.Models;

namespace IQToolkitCodeGen.Model
{
    public class CodeGenSettings : NotifierBase, ICodeGenSettings
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IXmlSerializerService _xmlSerializerService;

        [XmlIgnore]
        public bool IsDirty { get; set; }

        #region Tables

        private List<Table> tables;

        [XmlArray("Tables"), XmlArrayItem("Table", typeof(Table))]
        public List<Table> Tables
        {
            get
            {
                return this.tables;
            }

            set
            {
                if (this.tables != value)
                {
                    this.tables = value;
                    this.OnPropertyChanged(() => Tables);

                    if (this._eventAggregator != null)
                    {
                        this._eventAggregator.GetEvent<TablesChangedEvent>().Publish(value);
                    }
                }
            }
        }

        #endregion

        #region ConnectionString

        private string connectionString;

        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }

            set
            {
                if (this.connectionString != value)
                {
                    this.connectionString = value;
                    this.OnPropertyChanged(() => ConnectionString);
                }
            }
        }

        #endregion

        #region ProviderName

        private string providerName;

        public string ProviderName
        {
            get
            {
                return this.providerName;
            }

            set
            {
                if (this.providerName != value)
                {
                    this.providerName = value;

                    this.OnPropertyChanged(() => ProviderName);

                    if (this._eventAggregator != null)
                    {
                        this._eventAggregator.GetEvent<SchemaProviderNameChangedEvent>().Publish(value);
                    }
                }
            }
        }

        #endregion

        #region DataContextBaseClass

        private string dataContextBaseClass;

        public string DataContextBaseClass
        {
            get
            {
                return this.dataContextBaseClass;
            }

            set
            {
                if (this.dataContextBaseClass != value)
                {
                    this.dataContextBaseClass = value;
                    this.OnPropertyChanged(() => DataContextBaseClass);
                }
            }
        }

        #endregion

        #region DataContextClassName

        private string dataContextClassName;

        public string DataContextClassName
        {
            get
            {
                return this.dataContextClassName;
            }

            set
            {
                if (this.dataContextClassName != value)
                {
                    this.dataContextClassName = value;
                    this.OnPropertyChanged(() => DataContextClassName);
                }
            }
        }

        #endregion

        #region DataContextNamespace

        private string dataContextNamespace;

        public string DataContextNamespace
        {
            get
            {
                return this.dataContextNamespace;
            }

            set
            {
                if (this.dataContextNamespace != value)
                {
                    this.dataContextNamespace = value;
                    this.OnPropertyChanged(() => DataContextNamespace);
                }
            }
        }

        #endregion

        #region DataContextOutputFile

        private string dataContextOutputFile;

        public string DataContextOutputFile
        {
            get
            {
                return this.dataContextOutputFile;
            }

            set
            {
                if (this.dataContextOutputFile != value)
                {
                    this.dataContextOutputFile = value;
                    this.OnPropertyChanged(() => DataContextOutputFile);
                }
            }
        }

        #endregion

        #region DataContextTemplate

        private string dataContextTemplate;

        public string DataContextTemplate
        {
            get
            {
                return this.dataContextTemplate;
            }

            set
            {
                if (this.dataContextTemplate != value)
                {
                    this.dataContextTemplate = value;
                    this.OnPropertyChanged(() => DataContextTemplate);
                }
            }
        }

        #endregion

        #region WcfDataServiceBaseClass

        private string wcfDataServiceBaseClass;

        public string WcfDataServiceBaseClass
        {
            get
            {
                return this.wcfDataServiceBaseClass;
            }

            set
            {
                if (this.wcfDataServiceBaseClass != value)
                {
                    this.wcfDataServiceBaseClass = value;
                    this.OnPropertyChanged(() => WcfDataServiceBaseClass);
                }
            }
        }

        #endregion

        #region WcfDataServiceClassName

        private string wcfDataServiceClassName;

        public string WcfDataServiceClassName
        {
            get
            {
                return this.wcfDataServiceClassName;
            }

            set
            {
                if (this.wcfDataServiceClassName != value)
                {
                    this.wcfDataServiceClassName = value;
                    this.OnPropertyChanged(() => WcfDataServiceClassName);
                }
            }
        }

        #endregion

        #region WcfDataServiceNamespace

        private string wcfDataServiceNamespace;

        public string WcfDataServiceNamespace
        {
            get
            {
                return this.wcfDataServiceNamespace;
            }

            set
            {
                if (this.wcfDataServiceNamespace != value)
                {
                    this.wcfDataServiceNamespace = value;
                    this.OnPropertyChanged(() => WcfDataServiceNamespace);
                }
            }
        }

        #endregion

        #region WcfDataServiceOutputFile

        private string wcfDataServiceOutputFile;

        public string WcfDataServiceOutputFile
        {
            get
            {
                return this.wcfDataServiceOutputFile;
            }

            set
            {
                if (this.wcfDataServiceOutputFile != value)
                {
                    this.wcfDataServiceOutputFile = value;
                    this.OnPropertyChanged(() => WcfDataServiceOutputFile);
                }
            }
        }

        #endregion

        #region WcfDataServiceTemplate

        private string wcfDataServiceTemplate;

        public string WcfDataServiceTemplate
        {
            get
            {
                return this.wcfDataServiceTemplate;
            }

            set
            {
                if (this.wcfDataServiceTemplate != value)
                {
                    this.wcfDataServiceTemplate = value;
                    this.OnPropertyChanged(() => WcfDataServiceTemplate);
                }
            }
        }

        #endregion

        #region WcfDataServiceClientBaseClass

        private string wcfDataServiceClientBaseClass;

        public string WcfDataServiceClientBaseClass
        {
            get
            {
                return this.wcfDataServiceClientBaseClass;
            }

            set
            {
                if (this.wcfDataServiceClientBaseClass != value)
                {
                    this.wcfDataServiceClientBaseClass = value;
                    this.OnPropertyChanged(() => WcfDataServiceClientBaseClass);
                }
            }
        }

        #endregion

        #region WcfDataServiceClientClassName

        private string wcfDataServiceClientClassName;

        public string WcfDataServiceClientClassName
        {
            get
            {
                return this.wcfDataServiceClientClassName;
            }

            set
            {
                if (this.wcfDataServiceClientClassName != value)
                {
                    this.wcfDataServiceClientClassName = value;
                    this.OnPropertyChanged(() => WcfDataServiceClientClassName);
                }
            }
        }

        #endregion

        #region WcfDataServiceClientNamespace

        private string wcfDataServiceClientNamespace;

        public string WcfDataServiceClientNamespace
        {
            get
            {
                return this.wcfDataServiceClientNamespace;
            }

            set
            {
                if (this.wcfDataServiceClientNamespace != value)
                {
                    this.wcfDataServiceClientNamespace = value;
                    this.OnPropertyChanged(() => WcfDataServiceClientNamespace);
                }
            }
        }

        #endregion

        #region WcfDataServiceClientOutputFile

        private string wcfDataServiceClientOutputFile;

        public string WcfDataServiceClientOutputFile
        {
            get
            {
                return this.wcfDataServiceClientOutputFile;
            }

            set
            {
                if (this.wcfDataServiceClientOutputFile != value)
                {
                    this.wcfDataServiceClientOutputFile = value;
                    this.OnPropertyChanged(() => WcfDataServiceClientOutputFile);
                }
            }
        }

        #endregion

        #region WcfDataServiceClientTemplate

        private string wcfDataServiceClientTemplate;

        public string WcfDataServiceClientTemplate
        {
            get
            {
                return this.wcfDataServiceClientTemplate;
            }

            set
            {
                if (this.wcfDataServiceClientTemplate != value)
                {
                    this.wcfDataServiceClientTemplate = value;
                    this.OnPropertyChanged(() => WcfDataServiceClientTemplate);
                }
            }
        }

        #endregion

        #region EntityExtension

        private string entityExtension;

        public string EntityExtension
        {
            get
            {
                return this.entityExtension;
            }

            set
            {
                if (this.entityExtension != value)
                {
                    this.entityExtension = value;
                    this.OnPropertyChanged(() => EntityExtension);
                }
            }
        }

        #endregion

        #region EntityNamespace

        private string entityNamespace;

        public string EntityNamespace
        {
            get
            {
                return this.entityNamespace;
            }

            set
            {
                if (this.entityNamespace != value)
                {
                    this.entityNamespace = value;
                    this.OnPropertyChanged(() => EntityNamespace);
                }
            }
        }

        #endregion

        #region EntityOutputPath

        private string entityOutputPath;

        public string EntityOutputPath
        {
            get
            {
                return this.entityOutputPath;
            }

            set
            {
                if (this.entityOutputPath != value)
                {
                    this.entityOutputPath = value;
                    this.OnPropertyChanged(() => EntityOutputPath);
                }
            }
        }

        #endregion

        #region EntityTemplate

        private string entityTemplate;

        public string EntityTemplate
        {
            get
            {
                return this.entityTemplate;
            }

            set
            {
                if (this.entityTemplate != value)
                {
                    this.entityTemplate = value;
                    this.OnPropertyChanged(() => EntityTemplate);
                }
            }
        }

        #endregion

        #region MappingTemplate

        private string mappingTemplate;

        public string MappingTemplate
        {
            get
            {
                return this.mappingTemplate;
            }

            set
            {
                if (this.mappingTemplate != value)
                {
                    this.mappingTemplate = value;
                    this.OnPropertyChanged(() => MappingTemplate);
                }
            }
        }

        #endregion

        #region MappingOutputFile

        private string mappingOutputFile;

        public string MappingOutputFile
        {
            get
            {
                return this.mappingOutputFile;
            }

            set
            {
                if (this.mappingOutputFile != value)
                {
                    this.mappingOutputFile = value;
                    this.OnPropertyChanged(() => MappingOutputFile);
                }
            }
        }

        #endregion

        #region ExcludeViews

        private bool excludeViews;

        public bool ExcludeViews
        {
            get
            {
                return this.excludeViews;
            }

            set
            {
                if (this.excludeViews != value)
                {
                    this.excludeViews = value;
                    this.OnPropertyChanged(() => ExcludeViews);
                }
            }
        }

        #endregion

        #region NoPluralization

        private bool noPluralization;

        public bool NoPluralization
        {
            get
            {
                return this.noPluralization;
            }

            set
            {
                if (this.noPluralization != value)
                {
                    this.noPluralization = value;
                    this.OnPropertyChanged(() => NoPluralization);
                }
            }
        }

        #endregion

        #region TableSchemaSql

        private string tableSchemaSql;

        public string TableSchemaSql
        {
            get
            {
                return this.tableSchemaSql;
            }

            set
            {
                if (this.tableSchemaSql != value)
                {
                    this.tableSchemaSql = value;
                    this.OnPropertyChanged(() => TableSchemaSql);
                }
            }
        }

        #endregion

        #region ColumnSchemaSql

        private string columnSchemaSql;

        public string ColumnSchemaSql
        {
            get
            {
                return this.columnSchemaSql;
            }

            set
            {
                if (this.columnSchemaSql != value)
                {
                    this.columnSchemaSql = value;
                    this.OnPropertyChanged(() => ColumnSchemaSql);
                }
            }
        }

        #endregion

        #region AssociationSchemaSql

        private string associationSchemaSql;

        public string AssociationSchemaSql
        {
            get
            {
                return this.associationSchemaSql;
            }

            set
            {
                if (this.associationSchemaSql != value)
                {
                    this.associationSchemaSql = value;
                    this.OnPropertyChanged(() => AssociationSchemaSql);
                }
            }
        }

        #endregion

        public CodeGenSettings()
        {
        }

        public CodeGenSettings(IEventAggregator eventAggregator, IXmlSerializerService xmlSerializerService) {
            this._eventAggregator = eventAggregator;
            this._xmlSerializerService = xmlSerializerService;
            this._eventAggregator.GetEvent<SchemaProviderChangedEvent>().Subscribe(this.UpdateProviderName);
        }

        private void UpdateProviderName(IDatabase database)
        {
            if (database == null)
            {
                this.ProviderName = string.Empty;
            }
            else
            {
                this.ProviderName = database.DisplayName;
            }
        }

        public override void OnPropertyChanged<T>(Expression<Func<T>> propertyExpression)
        {
            base.OnPropertyChanged(propertyExpression);
            this.IsDirty = true;
        }

        public string ToXml()
        {
            return this._xmlSerializerService.ToXml(this);
        }
    }
}
