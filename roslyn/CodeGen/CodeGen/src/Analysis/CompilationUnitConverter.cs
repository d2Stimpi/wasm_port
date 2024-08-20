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

            foreach (var member in _activeAST.Members)
            {
                if (member is NamespaceDeclarationSyntax)
                {
                    ProcessNamespaceDeclarationSyntax(member as NamespaceDeclarationSyntax);
                }
            }
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
                Console.WriteLine($"field: {f}");

            // Visit Properties
            var properties = classSyntax.Members.Where(m => m is PropertyDeclarationSyntax).ToList();
            foreach (var p in properties)
                Console.WriteLine($"property: {p}");
        }

        private void ProcessMethodDeclarationSyntax(MethodDeclarationSyntax methodSyntax, CppClassSyntax cppParentNode)
        {
            CppMethodSyntax cppMethod = ConvertMethodDeclarationSyntax(methodSyntax);
            if (cppParentNode != null)
                cppParentNode.AddLeafNode(cppMethod);
            else
                _cppSyntaxTree.AddSyntaxNode(cppMethod);

            Console.WriteLine($"{methodSyntax.Identifier} method's body statements:");
            foreach (var member in methodSyntax.Body.Statements)
            {
                Console.WriteLine($" - {member}");
                Console.WriteLine($" - Statement's kind: {member.Kind()}");
            }
        }

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
            List<CppArgumentSyntax> arguments = null;

            var args = methodSytax.ParameterList.Parameters.ToList();
            arguments = args.ConvertAll(
                new Converter<ParameterSyntax, CppArgumentSyntax>(
                    arg => new CppArgumentSyntax(
                        arg.Type.ToString(),
                        arg.Identifier.ToString(),
                        arg.Modifiers.Select(m => m.ToString()).ToList(),
                        arg.Default != null ? arg.Default.ToString() : "")));

            var retTypeName = methodSytax.ReturnType.ToString();
            Console.WriteLine($" ** retType = {retTypeName}");

            CppMethodSyntax cppMethod = new CppMethodSyntax(identifier, retTypeName, modifiers, arguments);

            return cppMethod;
        }
    }
}
