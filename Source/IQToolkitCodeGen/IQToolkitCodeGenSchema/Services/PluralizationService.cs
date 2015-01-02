using System.Globalization;
using IQToolkitCodeGenSchema.Models;
using EF = System.Data.Entity.Design.PluralizationServices;

namespace IQToolkitCodeGenSchema.Services {
    public class PluralizationService : IPluralizationService {
        private readonly EF.PluralizationService _pluralizationService = EF.PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));
        private readonly ISchemaOptions _schemaOptions;

        public PluralizationService(ISchemaOptions schemaOptions) {
            ArgumentUtility.CheckNotNull("schemaOptions", schemaOptions);

            this._schemaOptions = schemaOptions;
        }

        public string Singularize(string word) {
            if (this._schemaOptions.NoPluralization) {
                return word;
            }

            return this._pluralizationService.Singularize(word);
        }

        public string Pluralize(string word) {
            if (this._schemaOptions.NoPluralization) {
                return word;
            }

            return this._pluralizationService.Pluralize(word);
        }
    }
}
