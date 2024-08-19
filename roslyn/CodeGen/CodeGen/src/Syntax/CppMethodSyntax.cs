using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppMethodSyntax : CppSyntaxNode
    {
        private string _name;
        private List<string> _modifiers;
        private List<CppArgumentSyntax> _arguments;

        public bool IsStatic { get => _modifiers.Contains("static"); }
        public bool IsPublic { get => _modifiers.Contains("public"); }
        public bool IsProtected { get => _modifiers.Contains("protected"); }
        public bool IsPrivate { get => _modifiers.Contains("private"); }
        public string Identifier { get => _name; }
        public List<CppArgumentSyntax> Arguments { get => _arguments; }

        public CppMethodSyntax(string name, List<string> modifiers, List<CppArgumentSyntax> args)
        {
            _name = name;
            _modifiers = modifiers;
            _arguments = args;
        }

        public override string GetHeaderText(int depth)
        {
            string argsText = "";

            foreach (var arg in Arguments)
            {
                if (argsText.Length == 0)
                    argsText += arg.GetHeaderText(0);
                else
                    argsText += ", " + arg.GetHeaderText(0);
            }

            return $"retType {Identifier}({argsText});";
        }

        public override string GetSourceText(int depth)
        {
            return "method::source";
        }
    }
}
