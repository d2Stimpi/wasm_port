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
        private CppTypeSyntax _retType;
        private List<string> _modifiers;
        private List<CppArgumentSyntax> _arguments;
        private CppClassSyntax _ownerClass;

        public CppTypeSyntax ReturnType { get => _retType; }
        public bool IsStatic { get => _modifiers.Contains("static"); }
        public bool IsPublic { get => _modifiers.Contains("public"); }
        public bool IsProtected { get => _modifiers.Contains("protected"); }
        public bool IsPrivate { get => _modifiers.Contains("private"); }
        public string Identifier { get => _name; }
        public List<CppArgumentSyntax> Arguments { get => _arguments; }
        public CppClassSyntax OwnerClass { get => _ownerClass; set => _ownerClass = value; }
        public List<CppStatementSyntax> Statements { get => Members.OfType<CppStatementSyntax>().ToList(); }

        public CppMethodSyntax(string name, CppTypeSyntax retType, List<string> modifiers, List<CppArgumentSyntax> args)
        {
            _name = name;
            _retType = retType;
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
            CodeFormatString formated = new CodeFormatString(depth);
            string argsText = "";

            foreach (var arg in Arguments)
            {
                if (argsText.Length == 0)
                    argsText += arg.GetSourceText(0);
                else
                    argsText += ", " + arg.GetSourceText(0);
            }

            // Method definition
            formated.WriteLine($"{ReturnType} {OwnerClass.Identifier}::{Identifier}({argsText})");

            // Add method body
            formated.AddTabs(1);
            formated.WriteLine("{");
            foreach (var statement in Statements)
            {
                formated.WriteLine(statement.GetSourceText(depth));
            }
            formated.WriteLine("\t // To be implemented.");
            formated.WriteLine("}");

            return formated.ToString();
        }
    }
}
