﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CppSyntax
{
    class CppVariableDeclaratorSyntax : CppSyntaxNode
    {
        private string _name;

        public string Identifier { get => _name; set => _name = value; }

        public CppVariableDeclaratorSyntax() : base(CppSyntaxKind.VariableDeclarator)
        {

        }

        public override string GetHeaderText(int depth)
        {
            return Identifier;
        }

        public override string GetSourceText(int depth)
        {
            return "";
        }
    }
}
