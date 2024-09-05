using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CppSyntax
{
    class CppMethodDeclarationSyntax : CppSyntaxNode
    {
        private string _name;
        private List<string> _modifiers;

        public string Identifier { get => _name; set => _name = value; }
        public List<string> Modifiers { get => _modifiers; set => _modifiers = value; }
        public Boolean IsStatic { get => _modifiers.Contains("static"); }
        public Boolean IsAbstract { get => _modifiers.Contains("abstract"); }
        public Boolean IsPublic { get => _modifiers.Contains("public"); }
        public Boolean IsProtected { get => _modifiers.Contains("protected"); }
        public Boolean IsPrivate { get => !IsPublic && !IsProtected; }

        public CppMethodDeclarationSyntax() : base(CppSyntaxKind.MethodDeclaration)
        {

        }

        public override string GetHeaderText(int depth)
        {
            string txt = "";

            if (IsStatic)
                txt += "static ";
            
            if (Members.OfType<CppPredefineType>().Any())
            {
                var type = Members.OfType<CppPredefineType>().First();
                txt += type.TypeName + " ";
            }

            // TODO: handle type of "generics" return val

            txt += Identifier;

            // ParameterList
            if (Members.OfType<CppParameterListSyntax>().Any())
            {
                var parameterList = Members.OfType<CppParameterListSyntax>().First();
                txt += parameterList.GetHeaderText(0);
            }

            return txt;
        }

        public override string GetSourceText(int depth)
        {
            return "";
        }
    }
}
