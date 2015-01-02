using IQToolkitCodeGenSchema.Services;

namespace IQToolkitCodeGenSchema.Factories {
    internal interface IPrimaryKeySchemaServiceFactory {
        IPrimaryKeySchemaService Create();
    }
}