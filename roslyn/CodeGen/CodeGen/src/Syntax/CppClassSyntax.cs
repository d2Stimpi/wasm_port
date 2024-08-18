using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppClassSyntax : CppSyntaxNode
    {
        private string _name;
        private List<string> _baseTypes;
        private List<string> _modifiers;

        public bool IsStatic { get => _modifiers.Contains("static"); }
        public string Identifier { get => _name; }

        public List<CppMethodSyntax> Methods { get => Members.OfType<CppMethodSyntax>().ToList(); }

        public CppClassSyntax(string name, List<string> baseTypes, List<string> modifiers)
        {
            _name = name;
            _baseTypes = baseTypes;
            _modifiers = modifiers;
        }

        public override string GetHeaderText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);

            // Base class types
            string baseTypesText = "";
            foreach (var baseType in _baseTypes)
            {
                if (baseTypesText.Length == 0)
                    baseTypesText += $" : public {baseType}";
                else
                    baseTypesText += $", public {baseType}";
            }

            string classDecl = (IsStatic ? "static " : "") + $"class {_name}{baseTypesText}";

            formated.WriteLine(classDecl);
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
            return "";
        }
    }
}
