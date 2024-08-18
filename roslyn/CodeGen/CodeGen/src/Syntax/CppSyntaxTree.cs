using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal class CppSyntaxTree
    {
        private List<CppSyntaxNode> _nodes = new List<CppSyntaxNode>();

        public List<CppSyntaxNode> Members => _nodes;

        public CppSyntaxTree()
        {

        }

        public void AddSyntaxNode(CppSyntaxNode node)
        {
            _nodes.Add(node);
        }
    }
}
