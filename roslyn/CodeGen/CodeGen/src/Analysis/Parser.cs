using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CSharp;

using CodeGen.Syntax;

namespace CodeGen
{
    internal class Parser
    {
        public CppSyntaxTree ParseFile(String filePath)
        {
            SyntaxTree tree = null;
            CppSyntaxTree cppTree = null;

            try
            {
                StreamReader sr = new StreamReader(filePath);
                tree = CSharpSyntaxTree.ParseText(sr.ReadToEnd());
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }

            if (tree != null)
            {
                var root = (CompilationUnitSyntax)tree.GetRoot();
                Console.WriteLine("AST:");

                CompilationUnitConverter converter = new CompilationUnitConverter();
                // Combine to one method
                converter.LoadAST(root);
                cppTree = converter.CppSyntaxTree;

               /* foreach (var node in root.Members)
                {
                    if (node is NamespaceDeclarationSyntax)
                    {
                        Console.WriteLine($"Namespace: {(node as NamespaceDeclarationSyntax).Name}");
                        foreach (var nsElem in (node as NamespaceDeclarationSyntax).Members)
                        {
                            if (nsElem is ClassDeclarationSyntax)
                            {
                                Console.WriteLine($"Class identifier: {(nsElem as ClassDeclarationSyntax).Identifier}");
                                Console.WriteLine($"Class modifiers: {(nsElem as ClassDeclarationSyntax).Modifiers}");
                                Console.WriteLine($"Class base list: {(nsElem as ClassDeclarationSyntax).BaseList}");

                                Console.WriteLine($"\nMethods of class {(nsElem as ClassDeclarationSyntax).Identifier}:\n");
                                foreach (var clsElem in (nsElem as ClassDeclarationSyntax).Members)
                                {
                                    if (clsElem is MethodDeclarationSyntax)
                                    {
                                        var method = clsElem as MethodDeclarationSyntax;
                                        Console.WriteLine($"Method identifier: {method.Identifier}");
                                        Console.WriteLine($"Method return type: {method.ReturnType}");
                                        Console.WriteLine($"Method modifiers: {method.Modifiers}");
                                        Console.WriteLine($"Method params: {method.ParameterList}");
                                        Console.WriteLine($"Method constraints: {method.ConstraintClauses}");
                                        Console.WriteLine("-----------------------------------------");

                                        var constraints = method.ConstraintClauses;
                                        foreach (var constraint in constraints)
                                        {
                                            Console.WriteLine($"  constr: {constraint}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }*/
            }

            return cppTree;
        }
    }
}
