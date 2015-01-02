using System.Web.UI.WebControls;

namespace IQToolkitContrib.Web {
    /// <summary>
    /// IQToolkit Implementation of LinqDataSource
    /// </summary>
    public class DataSource : LinqDataSource {
        /// <summary>
        /// Gets or sets a value indicating whether a Data Object's Generated Id should be retrieved
        /// </summary>
        public bool RetrieveGeneratedId { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataSource"/> class.
        /// </summary>
        /// <returns>Returns a DataSourceView instance</returns>
        protected override LinqDataSourceView CreateView() {
            return new DataSourceView(this, "DefaultView", this.Context);
        }
    }
}
