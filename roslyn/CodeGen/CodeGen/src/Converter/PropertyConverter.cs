using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using CodeGen.CppSyntax;

namespace CodeGen
{ 
    internal sealed class PropertyConverter : CSharpSyntaxWalker
    {
        private CppFieldDeclarationSyntax _fieldSyntax;
        private CppMethodDeclarationSyntax _getMethodSyntax;
        private CppMethodDeclarationSyntax _setMethodSyntax;

        public PropertyConverter()
        {
            _fieldSyntax = new CppFieldDeclarationSyntax(); 
        }

        public override void Visit(SyntaxNode node)
        {
            base.Visit(node);
        }

        public override void VisitPropertyDeclaration(PropertyDeclarationSyntax node)
        {
            base.VisitPropertyDeclaration(node);
        }

        // Get / Set accessor
        public override void VisitAccessorDeclaration(AccessorDeclarationSyntax node)
        {
            base.VisitAccessorDeclaration(node);
        }

        public override void VisitArrowExpressionClause(ArrowExpressionClauseSyntax node)
        {
            base.VisitArrowExpressionClause(node);
        }
    }
}
