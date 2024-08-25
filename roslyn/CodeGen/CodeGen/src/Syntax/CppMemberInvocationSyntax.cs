using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppMemberInvocationSyntax : CppSyntaxNode
    {
        private string _methodName;
        private string _className;
        // TODO: change List<string> args -> List<CppArgumentSytanx> args
        private List<CppArgumentSyntax> _args;

        public string MethodName { get => _methodName; set => _methodName = value; }
        public string ClassName { get => _className; set => _className = value; }
        public List<CppArgumentSyntax> Arguments { get => _args; }

        public CppMemberInvocationSyntax()
        {
            _methodName = "";
            _className = "";
            _args = new List<CppArgumentSyntax>();
        }

        public CppMemberInvocationSyntax(string methodName, string className, List<CppArgumentSyntax> args)
        {
            _methodName = methodName;
            _className = className;
            _args = args;
        }

        public override string GetHeaderText(int depth)
        {
            return "";
        }

        public override string GetSourceText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);

            formated.Write($"{ClassName}.{MethodName}(");
            if (Members.Any())
            {
                foreach (var statement in Members)
                {
                    formated.Write(statement.GetSourceText(0));
                }
                formated.Write($", arg?)");
            }
            else
            {

                formated.Write($"args)");
            }

            return formated.ToString();
        }
    }
}
