using System;

namespace IQToolkitCodeGenSchema {
    public interface IPluralizationService {
        string Pluralize(string word);
        string Singularize(string word);
    }
}
