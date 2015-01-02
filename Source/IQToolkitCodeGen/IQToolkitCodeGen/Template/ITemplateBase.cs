
namespace IQToolkitCodeGen.Template {
    public interface ITemplateBase {
        void Execute();
        void Write(object value);
        void WriteLiteral(object value);
        void Clear();
    }
}
