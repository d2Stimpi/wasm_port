using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    // Appears as a Block statement in Method's body or some other Statement list
    internal sealed class CppBlock : CppSyntaxNode  // Consider not extending
    {
        private CppSyntaxNode _owner;

        public CppSyntaxNode Owner { get => _owner; set => _owner = value; }

        public CppBlock()
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
