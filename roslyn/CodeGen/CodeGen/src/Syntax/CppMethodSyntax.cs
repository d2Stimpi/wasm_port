using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppMethodSyntax : CppSyntaxNode
    {



        public override string GetHeaderText(int depth)
        {
            return "method::header";
        }

        public override string GetSourceText(int depth)
        {
            return "method::source";
        }
    }
}
