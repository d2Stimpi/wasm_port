from lexer import Lexer
from lexer import Token

# ====--------
# AST classes
# ====--------

AST = []

class Expression:
    def __init__(self) -> None:
        self.list = []

class NamespaceExpression(Expression):
    def __init__(self, name):
        self.name = name

    def __str__(self) -> str:
        return f'namespace {self.name}'

# ====--------
# Parser
# ====--------

class Parser:
    def __init__(self, fileName):
        self.lexer = Lexer(fileName)
        self.current_token = self.get_next_token()

    def parse_namespace(self):
        token = self.get_next_token()
        if token == Token.IDENTIFIER:
            return NamespaceExpression(self.lexer.value)
        
        print(f'Error: Expected identifier, got {token}:{self.lexer.value}')
        return None


    def parse_top_level_expression(self):
        token = self.current_token
        if token == 'namespace':
            exp = self.parse_namespace()
            AST.append(exp)
            print(f'Parsed namespace: {exp.name}')
        else:
            print(f'Unparsed token: {token}:  {self.lexer.value}')


    def parse_expression(self):
        return None
    

    def get_next_token(self):
        self.current_token = self.lexer.get_token()
        return self.current_token

    def print_ast(self):
        for expression in AST:
            print(expression)

    def parse(self):
        while self.current_token is not None:
            self.parse_top_level_expression()
            self.get_next_token()
        
        self.print_ast()