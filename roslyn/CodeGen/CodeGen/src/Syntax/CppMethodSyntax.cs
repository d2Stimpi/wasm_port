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
        private string _retTypeName;
        private List<string> _modifiers;
        private List<CppArgumentSyntax> _arguments;

        public string ReturnType { get => _retTypeName; }
        public bool IsStatic { get => _modifiers.Contains("static"); }
        public bool IsPublic { get => _modifiers.Contains("public"); }
        public bool IsProtected { get => _modifiers.Contains("protected"); }
        public bool IsPrivate { get => _modifiers.Contains("private"); }
        public string Identifier { get => _name; }
        public List<CppArgumentSyntax> Arguments { get => _arguments; }

        public CppMethodSyntax(string name, string retTypeName, List<string> modifiers, List<CppArgumentSyntax> args)
        {
            _name = name;
            _retTypeName = retTypeName;
            _modifiers = modifiers;
            _arguments = args;
        }

        public override string GetHeaderText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);
            string argsText = "";

            foreach (var arg in Arguments)
            {
                if (argsText.Length == 0)
                    argsText += arg.GetHeaderText(0);
                else
                    argsText += ", " + arg.GetHeaderText(0);
            }

            formated.Write($"{ReturnType} {Identifier}({argsText});");
            return formated.ToString();
        }

        public override string GetSourceText(int depth)
        {
            return "method::source";
        }
    }
}
