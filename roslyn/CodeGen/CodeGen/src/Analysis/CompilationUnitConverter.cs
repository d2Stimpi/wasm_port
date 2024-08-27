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
    internal class CompilationUnitConverter
    {
        private CompilationUnitSyntax _activeAST;
        private CppSyntaxTree _cppSyntaxTree;

        public void LoadAST(CompilationUnitSyntax ast)
        {
            _activeAST = ast;

            ProcessCompilationUnit();
        }

        private void ProcessCompilationUnit()
        {
            // Convert C# Syntax classes to Cpp Syntax classes
            _cppSyntaxTree = new CppSyntaxTree();

            /*foreach (var member in _activeAST.Members)
            {
                if (member is NamespaceDeclarationSyntax)
                {
                    ProcessNamespaceDeclarationSyntax(member as NamespaceDeclarationSyntax);
                }
            }*/

            LoggingSyntaxWalker logger = new LoggingSyntaxWalker();
            logger.Visit(_activeAST);

            ConverterSyntaxWalker converterWalker = new ConverterSyntaxWalker();
            converterWalker.Visit(_activeAST);

            Console.WriteLine("\n\n==== Following is AST converted to Cpp Syntax Nodes ====");

            CppASTWalker astWalker = new CppASTWalker();
            astWalker.Visit(converterWalker.SyntaxTree);
        }

        private void ProcessNamespaceDeclarationSyntax(NamespaceDeclarationSyntax namespaceSyntax)
        {
            CppNamespaceSyntax cppNamespace = ConvertNamespaceDeclarationSyntax(namespaceSyntax);
            _cppSyntaxTree.AddSyntaxNode(cppNamespace);

            foreach (var member in namespaceSyntax.Members)
            {
                if (member is ClassDeclarationSyntax)
                {
                    ProcessClassDeclarationSyntax(member as ClassDeclarationSyntax, cppNamespace);
                }
            }
        }

        private void ProcessClassDeclarationSyntax(ClassDeclarationSyntax classSyntax, CppSyntaxNode cppParentNode)
        {
            CppClassSyntax cppClass = ConvertClassDeclarationSyntax(classSyntax);
            if (cppParentNode != null)
                cppParentNode.AddLeafNode(cppClass);
            else
                _cppSyntaxTree.AddSyntaxNode(cppClass);

            // Visit Methods
            var methods = classSyntax.Members.Where(m => m is MethodDeclarationSyntax).ToList();
            foreach (var m in methods)
            {
                ProcessMethodDeclarationSyntax(m as MethodDeclarationSyntax, cppClass);
            }

            // Visit Fields
            var fields = classSyntax.Members.Where(m => m is FieldDeclarationSyntax).ToList();
            foreach (var f in fields)
            {
                /*Console.WriteLine($"field: {f}");
                foreach (var m in (f as FieldDeclarationSyntax).Modifiers)
                    Console.WriteLine($" - modifier: {m}");*/

                ProcessFieldDeclarationSyntax(f as FieldDeclarationSyntax, cppClass);
            }

            // Visit Properties
            var properties = classSyntax.Members.Where(m => m is PropertyDeclarationSyntax).ToList();
            foreach (var p in properties)
                Console.WriteLine($"property: {p}");
        }

        private void ProcessMethodDeclarationSyntax(MethodDeclarationSyntax methodSyntax, CppClassSyntax cppParentNode)
        {
            CppMethodSyntax cppMethod = ConvertMethodDeclarationSyntax(methodSyntax);
            if (cppParentNode != null)
            {
                cppParentNode.AddLeafNode(cppMethod);
                cppMethod.OwnerClass = cppParentNode;
            }
            else
                _cppSyntaxTree.AddSyntaxNode(cppMethod);

            MethodSyntaxWalker methodWalker = new MethodSyntaxWalker(cppMethod);
            methodWalker.Visit(methodSyntax);
        }

        private void ProcessFieldDeclarationSyntax(FieldDeclarationSyntax fieldSyntax, CppClassSyntax cppParentNode)
        {
            CppVariableSyntax cppVariable = ConvertFieldDeclarationSyntax(fieldSyntax);
            if (cppParentNode != null)
                cppParentNode.AddLeafNode(cppVariable);
            else
                _cppSyntaxTree.AddSyntaxNode(cppVariable);
        }



        /**
         *  Conversion methods
         */


        public CppSyntaxTree CppSyntaxTree => _cppSyntaxTree;

        private CppNamespaceSyntax ConvertNamespaceDeclarationSyntax(NamespaceDeclarationSyntax namespaceSyntax)
        {
            return new CppNamespaceSyntax(namespaceSyntax.Name.ToString());
        }

        private CppClassSyntax ConvertClassDeclarationSyntax(ClassDeclarationSyntax classSyntax)
        {
            List<string> baseTypes = new List<string>();
            List<string> modifiers = null;

            if (classSyntax.BaseList != null)
            {
                baseTypes = classSyntax.BaseList.Types
                    .Select(x => x.ToString()).ToList();
            }

            modifiers = classSyntax.Modifiers.Select(x => x.ToString()).ToList();

            CppClassSyntax cppClass = new CppClassSyntax(classSyntax.Identifier.ToString(), baseTypes, modifiers);

            return cppClass;
        }

        private CppMethodSyntax ConvertMethodDeclarationSyntax(MethodDeclarationSyntax methodSytax)
        {
            string identifier = methodSytax.Identifier.ToString();
            List<string> modifiers = methodSytax.Modifiers.Select(x => x.ToString()).ToList();
            List<CppParameterSyntax> arguments = null;

            var args = methodSytax.ParameterList.Parameters.ToList();
            arguments = args.ConvertAll(
                new Converter<ParameterSyntax, CppParameterSyntax>(
                    arg => new CppParameterSyntax(
                        new CppTypeSyntax(arg.Type.ToString()),
                        arg.Identifier.ToString(),
                        arg.Modifiers.Select(m => m.ToString()).ToList(),
                        arg.Default != null ? arg.Default.ToString() : "")));

            CppTypeSyntax retType = new CppTypeSyntax(methodSytax.ReturnType.ToString());

            CppMethodSyntax cppMethod = new CppMethodSyntax(identifier, retType, modifiers, arguments);

            return cppMethod;
        }

        private CppVariableSyntax ConvertFieldDeclarationSyntax(FieldDeclarationSyntax fieldSyntax)
        {
            string identifier = fieldSyntax.Declaration.Variables.First().ToString();
            CppTypeSyntax type = new CppTypeSyntax(fieldSyntax.Declaration.Type.ToString());
            List<string> modifiers = fieldSyntax.Modifiers.Select(x => x.ToString()).ToList();

            CppVariableSyntax cppVariable = new CppVariableSyntax(identifier, type, modifiers);

            return cppVariable;
        }
    }
}
