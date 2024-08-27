using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CppSyntax
{
    public enum CppSyntaxKind
    {
        None = 1,
        Root = 2,

        NamespaceDeclaration = 3,
        ClassDeclaration = 4,
        IdentifierName = 5,
        BaseList = 6,
        SimpleBaseType = 7,
        FieldDeclaration = 8,       // TODO => MemberVariableDeclaration
        VariableDeclaration = 9,    // Example: static class member
        PredefinedType  = 10,
        VariableDeclarator = 11,    // Variable name + default value

        Unhandled = 1000
    }
}
