using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppClassSyntax : CppSyntaxNode
    {
        private string _name;
        private List<string> _baseTypes;
        private List<string> _modifiers;

        public bool IsStatic { get => _modifiers.Contains("static"); }
        public string Identifier { get => _name; }

        public List<CppMethodSyntax> Methods { get => Members.OfType<CppMethodSyntax>().ToList(); }
        public List<CppVariableSyntax> Variables { get => Members.OfType<CppVariableSyntax>().ToList(); }

        public CppClassSyntax(string name, List<string> baseTypes, List<string> modifiers)
        {
            _name = name;
            _baseTypes = baseTypes;
            _modifiers = modifiers;
        }

        public override string GetHeaderText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);

            // Base class types
            string baseTypesText = "";
            foreach (var baseType in _baseTypes)
            {
                if (baseTypesText.Length == 0)
                    baseTypesText += $" : public {baseType}";
                else
                    baseTypesText += $", public {baseType}";
            }

            string classDecl = (IsStatic ? "static " : "") + $"class {_name}{baseTypesText}";

            formated.WriteLine(classDecl);
            formated.WriteLine("{");

            // Visit public class methods
            var publicMethods = Methods.Where(x => !x.IsPrivate).ToList();
            if (publicMethods.Any())
                formated.WriteLine("public:");
            foreach (var method in publicMethods)
            {
                formated.WriteLine(method.GetHeaderText(depth));
            }

            // Visit private class methods
            var privateMethods = Methods.Where(x => x.IsPrivate).ToList();
            if (privateMethods.Any())
                formated.WriteLine("private:");
            foreach (var method in privateMethods)
            {
                formated.WriteLine(method.GetHeaderText(depth));
            }

            // Visit member variables
            var privateMemberVars = Variables; // all are considered private
            if (privateMemberVars.Any())
                formated.WriteLine("private:");
            foreach (var variable in privateMemberVars)
            {
                formated.WriteLine(variable.GetHeaderText(depth));
            }

            formated.Write("}\n");
            return formated.ToString();
        }

        public override string GetSourceText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);

            // Visit public class methods
            var publicMethods = Methods.Where(x => !x.IsPrivate).ToList();
            foreach (var method in publicMethods)
            {
                formated.WriteLine(method.GetSourceText(depth));
                formated.WriteLine(""); // separator
            }

            // Visit private class methods
            var privateMethods = Methods.Where(x => x.IsPrivate).ToList();
            foreach (var method in privateMethods)
            {
                formated.WriteLine(method.GetSourceText(depth));
                formated.WriteLine(""); // separator
            }

            return formated.ToString();
        }
    }
}
