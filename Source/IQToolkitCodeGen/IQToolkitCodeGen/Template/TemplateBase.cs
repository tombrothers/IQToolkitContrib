using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Text;

namespace IQToolkitCodeGen.Template {
    public class TemplateBase : ITemplateBase {
        private readonly StringBuilder _output = new StringBuilder();
        private PluralizationService pluralizationService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));

        // Razor engine will override this method.
        public virtual void Execute() { }

        public void WriteLiteral(object value) {
            this.Write(value);
        }

        public void Write(object value) {
            this._output.Append(value);
        }

        public string Singularize(string word) {
            return this.pluralizationService.Singularize(word);
        }

        public string Pluralize(string word) {
            return this.pluralizationService.Pluralize(word);
        }

        public override string ToString() {
            return this._output.ToString();
        }

        public void Clear() {
            this._output.Clear();
        }
    }
}