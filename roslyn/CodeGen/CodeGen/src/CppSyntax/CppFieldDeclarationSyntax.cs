using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CppSyntax
{
    class CppFieldDeclarationSyntax : CppSyntaxNode
    {
        public CppFieldDeclarationSyntax() : base(CppSyntaxKind.FieldDeclaration)
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
