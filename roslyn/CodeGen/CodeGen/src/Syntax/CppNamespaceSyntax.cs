using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppNamespaceSyntax : CppSyntaxNode
    {
        private string _name;

        public CppNamespaceSyntax(string name)
        {
            _name = name;
        }

        public List<CppClassSyntax> Classes { get => Members.OfType<CppClassSyntax>().ToList(); }

        public override string GetHeaderText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);
            string namespaceDecl = $"namespace {_name}";

            formated.WriteLine(namespaceDecl);
            formated.WriteLine("{");

            depth++;
            foreach (var leaf in Members)
            {
                formated.WriteLine(leaf.GetHeaderText(depth));
            }

            formated.Write("}\n");
            return formated.ToString();
        }

        public override string GetSourceText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);
            string namespaceDecl = $"namespace {_name}";

            formated.WriteLine(namespaceDecl);
            formated.WriteLine("{");

            depth++;
            // Go trough all members of classes in namespace
            foreach (CppClassSyntax classSyntax in Classes)
            {
                formated.WriteLine(classSyntax.GetSourceText(depth));
            }

            formated.Write("}\n");
            return formated.ToString();
        }
    }
}
