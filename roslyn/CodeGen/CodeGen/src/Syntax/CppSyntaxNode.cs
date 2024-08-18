using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal abstract class CppSyntaxNode
    {
        private List<CppSyntaxNode> _leafNodes = new List<CppSyntaxNode>();

        public List<CppSyntaxNode> Members => _leafNodes;

        public void AddLeafNode(CppSyntaxNode node)
        {
            _leafNodes.Add(node);
        }

        public abstract string GetHeaderText(int depth);
        public abstract string GetSourceText(int depth);
    }
}
