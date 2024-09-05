using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CppSyntax
{
    internal sealed class CppParameterListSyntax : CppSyntaxNode
    {
        public CppParameterListSyntax() : base(CppSyntaxKind.ParameterList)
        {
        }

        public override string GetHeaderText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);

            foreach (var param in Members)
            {
                formated.Write(param.GetHeaderText(depth));
            }

            return formated.ToString();
        }

        public override string GetSourceText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);

            return "";
        }
    }
}
