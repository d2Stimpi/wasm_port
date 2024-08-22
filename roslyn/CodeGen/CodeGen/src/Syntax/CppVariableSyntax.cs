using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppVariableSyntax : CppSyntaxNode
    {
        private string _name;
        private CppTypeSyntax _type;
        private List<string> _modifiers;

        public string Identifier { get => _name; }
        public CppTypeSyntax Type { get => _type; }

        public CppVariableSyntax(string name, CppTypeSyntax type, List<string> modifiers)
        {
            // Name string contains the assigned initializer value
            _name = name;
            _type = type;
            _modifiers = modifiers;
        }

        public override string GetHeaderText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);

            formated.Write($"{Type} {Identifier};");

            return formated.ToString();
        }

        public override string GetSourceText(int depth)
        {
            return "";
        }
    }
}
