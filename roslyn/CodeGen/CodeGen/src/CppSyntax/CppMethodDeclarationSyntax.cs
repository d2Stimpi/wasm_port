using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CppSyntax
{
    class CppMethodDeclarationSyntax : CppSyntaxNode
    {
        public CppMethodDeclarationSyntax() : base(CppSyntaxKind.MethodDeclaration)
        {

        }

        public override string GetHeaderText(int depth)
        {
            return "MethodDecl";
        }

        public override string GetSourceText(int depth)
        {
            return "";
        }
    }
}
