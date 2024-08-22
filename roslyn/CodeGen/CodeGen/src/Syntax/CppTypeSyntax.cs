using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.Syntax
{
    internal sealed class CppTypeSyntax
    {
        private string _name;

        public string Identifier { get => _name; }

        public CppTypeSyntax(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return Identifier;
        }
    }
}
