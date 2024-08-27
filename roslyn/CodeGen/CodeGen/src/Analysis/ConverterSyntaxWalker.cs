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
    internal sealed class ConverterSyntaxWalker : CSharpSyntaxWalker
    {
        private CppSyntaxNode _rootNode;
        private Stack<CppSyntaxNode> _nodeStack = new Stack<CppSyntaxNode>();

        private List<SyntaxKind> _skipSyntaxKinds = new List<SyntaxKind>()
        {
            SyntaxKind.UsingDirective,
        };


        public CppSyntaxNode SyntaxTree { get => _rootNode; }

        public ConverterSyntaxWalker()
        {
            _rootNode = new CppRootSyntaxNode();
            _nodeStack.Push(_rootNode);
        }

        private void StackReplace(CppSyntaxNode node)
        {
            _nodeStack.Pop();
            _nodeStack.Push(node);
        }

        public override void Visit(SyntaxNode node)
        {
            // Check if we want to skip a Kind
            var kind = _skipSyntaxKinds.Where(x => node.IsKind(x)).FirstOrDefault();
            if (kind == SyntaxKind.None)
            {

                _nodeStack.Push(new CppUnhandledSyntax(node.Kind()));

                base.Visit(node);

                var leafNode = _nodeStack.Pop();
                _nodeStack.Peek().AddNode(leafNode);
            }
        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            StackReplace(new CppIdentifierSyntax());

            base.VisitIdentifierName(node);
        }

        public override void VisitNamespaceDeclaration(NamespaceDeclarationSyntax node)
        {
            StackReplace(new CppNamespaceSyntax());

            base.VisitNamespaceDeclaration(node);
        }

        public override void VisitClassDeclaration(ClassDeclarationSyntax node)
        {
            StackReplace(new CppClassSyntax());

            base.VisitClassDeclaration(node);
        }

        public override void VisitBaseList(BaseListSyntax node)
        {
            StackReplace(new CppBaseListSyntax());

            base.VisitBaseList(node);
        }

        public override void VisitSimpleBaseType(SimpleBaseTypeSyntax node)
        {
            StackReplace(new CppSimpleBaseTypeSyntax());

            base.VisitSimpleBaseType(node);
        }

        public override void VisitFieldDeclaration(FieldDeclarationSyntax node)
        {
            StackReplace(new CppFieldDeclarationSyntax());

            base.VisitFieldDeclaration(node);
        }

        public override void VisitPredefinedType(PredefinedTypeSyntax node)
        {
            StackReplace(new CppPredefineType());

            base.VisitPredefinedType(node);
        }

        public override void VisitVariableDeclarator(VariableDeclaratorSyntax node)
        {
            StackReplace(new CppVariableDeclaratorSyntax());

            base.VisitVariableDeclarator(node);
        }
    }
}
