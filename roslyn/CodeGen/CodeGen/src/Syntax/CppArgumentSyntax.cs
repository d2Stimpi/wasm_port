using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppArgumentSyntax : CppSyntaxNode
    {
        private CppTypeSyntax _type;
        private string _name;
        private List<string> _modifiers;
        private string _default;

        public bool IsConst { get => _modifiers.Contains("readonly"); }
        public bool IsRef { get => _modifiers.Contains("ref") || _modifiers.Contains("out") || _modifiers.Contains("in"); }
        public string Identifier { get => _name; }
        public string DefaultValue { get => _default; }
        public CppTypeSyntax Type { get => _type; }

        public CppArgumentSyntax(CppTypeSyntax type, string name, List<string> modifiers, string defaultValue)
        {
            _type = type;
            _name = name;
            _modifiers = modifiers;
            _default = defaultValue;
        }

        public override string GetHeaderText(int depth)
        {
            // TODO: add defaults for header file
            return $"{Type} {Identifier}";
        }

        public override string GetSourceText(int depth)
        {
            return $"{Type} {Identifier}";
        }
    }

    internal class CppArgumentSyntaxList : List<CppArgumentSyntax>
    {

        public override string ToString()
        {
            string argsText = "";

            foreach (var item in this)
            {
                if (argsText.Length == 0)
                    argsText += item.GetHeaderText(0);
                else
                    argsText += ", " + item.GetHeaderText(0);
            }

            return argsText;
        }
    }
}
