using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using CodeGen.Syntax;

namespace CodeGen
{
    internal sealed class MethodSyntaxWalker : CSharpSyntaxWalker
    {
        private int _tabs = 0;
        private bool _logOn = false;
        private bool _fullTreeLogOn = false;

        private CppSyntaxNode _parentMethodNode;
        
        private CppSyntaxNode _activeCppSyntax = null;
        private Stack<CppSyntaxNode> _cppSyntaxStack = new Stack<CppSyntaxNode>();


        public MethodSyntaxWalker(CppMethodSyntax parentMethodNode)
        {
            _parentMethodNode = parentMethodNode;
        }

        public override void Visit(SyntaxNode node)
        {
            _tabs++;
            if (_fullTreeLogOn)
                Console.WriteLine(new String('\t', _tabs) + node.Kind());

            base.Visit(node);
            _tabs--;
        }

        /**
         *  "Top level" statements
         */

        public override void VisitExpressionStatement(ExpressionStatementSyntax node)
        {
            base.VisitExpressionStatement(node);

            if (_activeCppSyntax != null)
                _parentMethodNode.AddLeafNode(_activeCppSyntax);
        }

        public override void VisitBlock(BlockSyntax node)
        {
            base.VisitBlock(node);
        }



        /**
         *  Expressions
         */

        public override void VisitInvocationExpression(InvocationExpressionSyntax node)
        {
            // First visit
            if (node.Parent.IsKind(SyntaxKind.ExpressionStatement))
            {
                if (_logOn) Console.WriteLine($"VisitInvocationExpression - root {node.GetText()}");
                _activeCppSyntax = new CppInvocationSyntax();

                base.VisitInvocationExpression(node);
            }
            else // Nested Invocation
            {
                _cppSyntaxStack.Push(new CppInvocationSyntax());

                if (_logOn) Console.WriteLine($"VisitInvocationExpression - nested {node.GetText()}");
                base.VisitInvocationExpression(node);

                var args = node.ArgumentList.Arguments;

                if (_activeCppSyntax == null)
                {
                    Console.WriteLine("\t*** Unexpected error: _activeCppSyntax is NULL!! ***\t");
                    Console.WriteLine($"\t*** VisitInvocationExpression - root {node.GetText()} parent kind: {node.Parent.Kind()} ***\t");
                }
                else
                {
                    var topElement = _cppSyntaxStack.Pop();

                    // If we reached last element in stack we add it to parent node - root invocation node
                    if (_cppSyntaxStack.Count == 0)
                    {
                        _activeCppSyntax.AddLeafNode(topElement);
                    }
                    else
                    {
                        var prevElement = _cppSyntaxStack.Peek();
                        prevElement.AddLeafNode(topElement);
                    }
                }

            }
        }

        public override void VisitMemberAccessExpression(MemberAccessExpressionSyntax node)
        {
            if (node.Parent.IsKind(SyntaxKind.InvocationExpression))
            {
                // Check for root node
                if (_cppSyntaxStack.Count == 0)
                {
                    // Convert created cppSyntaxNode to MemberInvocation
                    _activeCppSyntax = new CppMemberInvocationSyntax();
                }
                else
                {
                    // Convert top object to MemberInvocation
                    _cppSyntaxStack.Pop();
                    _cppSyntaxStack.Push(new CppMemberInvocationSyntax());
                }
                
                base.VisitMemberAccessExpression(node);
            }
            else
            {
                Console.WriteLine($"Unexpected parent for SimpleMemberAccessExpression - [{node.Parent.Kind()}]");
            }

        }

        public override void VisitIdentifierName(IdentifierNameSyntax node)
        {
            // Move to a function - get active object
            CppSyntaxNode active = null;
            if (_cppSyntaxStack.Count == 0)
            {
                active = _activeCppSyntax;
            }
            else
            {
                active = _cppSyntaxStack.Peek();
            }


            if (node.Parent.IsKind(SyntaxKind.SimpleMemberAccessExpression))
            {
                var memberInvocationSyntax = active as CppMemberInvocationSyntax;
                if (memberInvocationSyntax.ClassName == "")
                    memberInvocationSyntax.ClassName = node.Identifier.ToString();
                else
                    memberInvocationSyntax.MethodName = node.Identifier.ToString();

                base.VisitIdentifierName(node);
            }
            else if (node.Parent.IsKind(SyntaxKind.InvocationExpression))
            {
                var invocationSyntax = active as CppInvocationSyntax;
                invocationSyntax.MethodName = node.Identifier.ToString();

                base.VisitIdentifierName(node);
            }
            else
            {
                Console.WriteLine($"Unsupported identifier's parent kind [{node.Parent.Kind()}]");
            }
        }

        public override void VisitArgumentList(ArgumentListSyntax node)
        {
            base.VisitArgumentList(node);

            Console.WriteLine("====================================================");
            Console.WriteLine($"Node: {node.Parent.Kind()}");
            foreach (var arg in node.Arguments)
                Console.WriteLine($"Arg: {arg}");

            if (_cppSyntaxStack.Count == 0)
                Console.WriteLine($"On stack cpp node: {_activeCppSyntax.GetSourceText(0)}");
            else
                Console.WriteLine($"On stack cpp node: {_cppSyntaxStack.Peek().GetSourceText(0)}");
            Console.WriteLine("====================================================");
        }
    }
}
