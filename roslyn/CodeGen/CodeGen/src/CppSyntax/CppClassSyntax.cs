using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CppSyntax
{
    internal sealed class CppClassSyntax : CppSyntaxNode
    {
        private string _name;

        public string Identifier { get => _name; set => _name = value; }
        public List<CppBaseListSyntax> BaseList { get => Members.OfType<CppBaseListSyntax>().ToList(); }
        public List<CppMethodDeclarationSyntax> Methods { get => Members.OfType<CppMethodDeclarationSyntax>().ToList(); }
        public List<CppFieldDeclarationSyntax> Fields { get => Members.OfType<CppFieldDeclarationSyntax>().ToList(); }

        public CppClassSyntax() : base(CppSyntaxKind.ClassDeclaration)
        {
            _name = "Nameless";
        }

        public override string GetHeaderText(int depth)
        {
            // format: Identifier : BaseList [ Type Identifier ]
            CodeFormatString formated = new CodeFormatString(depth);

            formated.Write($"class {Identifier}");
            if (BaseList.Count() != 0)
            {
                formated.Write(" : ");
                foreach (var member in BaseList)
                {
                    formated.Write(member.GetHeaderText(depth));
                }
            }
            // complete the class declaration line
            formated.WriteLine("");
            formated.WriteLine("{");


            formated.SetTabs(depth + 1); // indent after visibility tag
            formated.WriteLine("public:");
            foreach (var member in Methods)
            {
                formated.WriteLine(member.GetHeaderText(depth));
            }

            formated.SetTabs(depth);
            formated.WriteLine("");
            formated.WriteLine("}");
            return formated.ToString();
        }

        public override string GetSourceText(int depth)
        {
            return "";
        }
    }
}
