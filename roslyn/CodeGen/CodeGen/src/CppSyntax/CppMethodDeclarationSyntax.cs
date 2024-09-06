﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGen.CppSyntax
{
    class CppMethodDeclarationSyntax : CppSyntaxNode
    {
        private string _name;
        private List<string> _modifiers;
        private string _retTypeName;

        public string Identifier { get => _name; set => _name = value; }
        public string ReturnType { get => _retTypeName; set => _retTypeName = value; }
        public List<string> Modifiers { get => _modifiers; set => _modifiers = value; }
        public Boolean IsStatic { get => _modifiers.Contains("static"); }
        public Boolean IsAbstract { get => _modifiers.Contains("abstract"); }
        public Boolean IsPublic { get => _modifiers.Contains("public"); }
        public Boolean IsProtected { get => _modifiers.Contains("protected"); }
        public Boolean IsPrivate { get => !IsPublic && !IsProtected; }

        public CppMethodDeclarationSyntax() : base(CppSyntaxKind.MethodDeclaration)
        {

        }

        public override string GetHeaderText(int depth)
        {
            CodeFormatString formated = new CodeFormatString(depth);
            string txt = "";

            // TODO: handle type of "generics" return val
            if (HasMember<CppTypeParameterListSyntax>())
            {
                formated.Write(GetFirstMember<CppTypeParameterListSyntax>().GetHeaderText(0));
            }

            if (IsStatic)
                txt += "static ";
            
            if (HasMember<CppPredefineType>())
            {
                var type = GetFirstMember<CppPredefineType>();
                txt += type.TypeName + " ";
            }
            else
            {
                txt += ReturnType + " ";
            }

            txt += Identifier;

            // ParameterList
            txt += "(";
            if (HasMember<CppParameterListSyntax>())
            {
                var parameterList = GetFirstMember<CppParameterListSyntax>();
                txt += parameterList.GetHeaderText(0);
            }
            txt += ");";

            formated.Write(txt);
            return formated.ToString();
        }

        public override string GetSourceText(int depth)
        {
            return "";
        }
    }
}
