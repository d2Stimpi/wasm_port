using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CppSyntax
{
    class CppVariableDeclarationSyntax : CppSyntaxNode
    {
        public CppVariableDeclarationSyntax() : base(CppSyntaxKind.VariableDeclaration)
        {

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
