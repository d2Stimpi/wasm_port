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

            var PublicMethods = Methods.Where(m => m.IsPublic).ToList();
            if (PublicMethods.Count != 0)
            {
                formated.SetTabs(depth + 1); // indent after visibility tag
                formated.WriteLine("public:");
                foreach (var member in PublicMethods)
                {
                    formated.WriteLine(member.GetHeaderText(depth));
                }
            
                formated.SetTabs(depth);
                formated.WriteLine("");
            }

            var PrivateMethods = Methods.Where(m => m.IsPrivate).ToList();
            if (PrivateMethods.Count != 0)
            {
                formated.SetTabs(depth + 1); // indent after visibility tag
                formated.WriteLine("private:");
                foreach (var member in PrivateMethods)
                {
                    formated.WriteLine(member.GetHeaderText(depth));
                }

                formated.SetTabs(depth);
                formated.WriteLine("");
            }

            var ProtectedMethods = Methods.Where(m => m.IsProtected).ToList();
            if (ProtectedMethods.Count != 0)
            {
                formated.SetTabs(depth + 1); // indent after visibility tag
                formated.WriteLine("protected:");
                foreach (var member in ProtectedMethods)
                {
                    formated.WriteLine(member.GetHeaderText(depth));
                }

                formated.SetTabs(depth);
                formated.WriteLine("");
            }



            formated.WriteLine("}");
            return formated.ToString();
        }

        public override string GetSourceText(int depth)
        {
            return "";
        }
    }
}
