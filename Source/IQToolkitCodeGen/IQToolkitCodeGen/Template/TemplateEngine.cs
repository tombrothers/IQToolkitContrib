using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Razor;
using Microsoft.CSharp;

namespace IQToolkitCodeGen.Template {
    public class TemplateEngine<T>
        where T : ITemplateBase, new() {
        private readonly RazorTemplateEngine engine;
        private readonly string templateText;
        private readonly CSharpCodeProvider codeProvider = new CSharpCodeProvider();
        public T Template { get; private set; }

        internal TemplateEngine(string templateText) {
            this.templateText = templateText ?? string.Empty;
            this.engine = this.GetRazorTemplateEngine();
            this.Template = this.CreateTemplate();
        }

        private RazorTemplateEngine GetRazorTemplateEngine() {
            RazorEngineHost host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultBaseClass = typeof(T).ToString();

            foreach (string @namespace in this.GetNamespaces()) {
                host.NamespaceImports.Add(@namespace);
            }

            return new RazorTemplateEngine(host);
        }

        private IEnumerable<string> GetNamespaces() {
            yield return "System";
            yield return "System.Linq";
            yield return "IQToolkitCodeGenSchema.Models";
        }

        private T CreateTemplate() {
            CodeCompileUnit templateWriter = this.GetTemplateWriterCodeCompileUnit();
            CompilerParameters compilerParameters = this.GetCompilerParameters();
            CompilerResults compilerResults = this.codeProvider.CompileAssemblyFromDom(compilerParameters, templateWriter);

            if (compilerResults.Errors.Count > 0) {
                StringBuilder compileErrors = new StringBuilder();

                foreach (CompilerError compileError in compilerResults.Errors) {
                    compileErrors.Append(string.Format("Line:  {0}\t Column:  {1}\r\nError: {2}", compileError.Line, compileError.Column, compileError.ErrorText));
                }

                throw new ApplicationException(compileErrors.ToString() + Environment.NewLine + this.GetTemplateWriterSourceCode(templateWriter));
            }

            Debug.WriteLine(this.GetTemplateWriterSourceCode(templateWriter));

            Type type = compilerResults.CompiledAssembly.GetTypes().First();
            return (T)Activator.CreateInstance(type);
        }

        private CompilerParameters GetCompilerParameters() {
            CompilerParameters compilerParameters = new CompilerParameters();

            foreach (string assembly in this.GetAssemblies()) {
                compilerParameters.ReferencedAssemblies.Add(assembly);
            }

            compilerParameters.ReferencedAssemblies.Add(typeof(T).Assembly.Location);
            compilerParameters.GenerateInMemory = true;

            return compilerParameters;
        }

        private IEnumerable<string> GetAssemblies() {
            yield return "System.dll";
            yield return "System.Core.dll";
            yield return "Microsoft.CSharp.dll";
            yield return @"bin\IQToolkitCodeGenSchema.dll";
        }

        private string GetTemplateWriterSourceCode(CodeCompileUnit codeCompileUnit) {
            using (StringWriter writer = new StringWriter()) {
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                this.codeProvider.GenerateCodeFromCompileUnit(codeCompileUnit, writer, options);
                return writer.ToString();
            }
        }

        private CodeCompileUnit GetTemplateWriterCodeCompileUnit() {
            using (StringReader reader = new StringReader(this.templateText)) {
                return this.engine.GenerateCode(reader).GeneratedCode;
            }
        }

        public override string ToString() {
            this.Template.Execute();
            return this.Template.ToString();
        }

        public void ToFile(string file) {
            File.WriteAllText(file, this.ToString());
        }
    }
}