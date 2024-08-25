using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppArgumentSyntax : CppSyntaxNode
    {
        private CppSyntaxNode _argument;

        public CppSyntaxNode Argument { get => _argument; set => _argument = value; }

        public CppArgumentSyntax(CppSyntaxNode arg)
        {
            _argument = arg;
        }

        public override string GetHeaderText(int depth)
        {
            return "";
        }

        public override string GetSourceText(int depth)
        {
            return "";
        }
    }
}
