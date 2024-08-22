using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppStatementSyntax : CppSyntaxNode
    {
        private string _text;

        public CppStatementSyntax(string text)
        {
            _text = text;
        }


        public override string GetHeaderText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);

            formated.Write(_text);

            return formated.ToString();
        }

        public override string GetSourceText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);

            formated.Write(_text);

            return formated.ToString();
        }
    }
}
