using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CppSyntax
{
    internal sealed class CppParameterSyntax : CppSyntaxNode
    {
        private string _name;

        public string Identifier { get => _name; set => _name = value; }

        public CppParameterSyntax() : base(CppSyntaxKind.Parameter)
        {

        }

        public override string GetHeaderText(int depth)
        {
            return "";
        }

        public override string GetSourceText(int depth)
        {
            return "";
        }
    }
}
