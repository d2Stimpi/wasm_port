using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppInvocationSyntax : CppSyntaxNode
    {
        private string _methodName;
        // TODO: change List<string> args -> List<CppArgumentSytanx> args
        private List<CppArgumentSyntax> _args;

        public string MethodName { get => _methodName; set => _methodName = value; }
        public List<CppArgumentSyntax> Arguments { get => _args; }

        public CppInvocationSyntax()
        {
            _methodName = "";
            _args = new List<CppArgumentSyntax>();
        }

        public CppInvocationSyntax(string methodName, string className, List<CppArgumentSyntax> args)
        {
            _methodName = methodName;
            _args = args;
        }

        public override string GetHeaderText(int depth)
        {
            return "";
        }

        public override string GetSourceText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);

            formated.Write($"{MethodName}(");
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
