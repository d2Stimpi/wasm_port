from lexer import Lexer
from lexer import TokenType
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
        super().__init__()
        self.name = name

    def __str__(self) -> str:
        str = f'namespace {self.name}'
        for exp in self.list:
            str += '\n> ' + exp.__str__()
        return str

# ====--------
# Parser
# ====--------

class Parser:
    def __init__(self, fileName):
        self.lexer = Lexer(fileName)
        self.current_token = self.get_next_token()
        # for counting visited curly brackets (levels)
        self.curly_depth = 0

    # helper method that expects a token type and eats it
    def eat_token(self, expected) -> bool:
        token = self.get_next_token()
        value = self.lexer.value
        if value is expected.value:
            # check for curly and handle depth
            if value is Token.CURLY_OPEN.value:
                self.curly_depth += 1
            if value is Token.CURLY_CLOSE.value:
                self.curly_depth -= 1
            print(f'oke (depth {self.curly_depth})')
            return True
        else:
            print(f'Error: Expected \'{expected.value}\' token, got {token}:\'{value}\'')
            return False


    def parse_namespace(self) -> Expression:
        namespace = None
        token = self.get_next_token()
        if token != TokenType.IDENTIFIER:
            print(f'Error: Expected identifier, got {token}:{self.lexer.value}')

        namespace = NamespaceExpression(self.lexer.value)
        
        # expect '{' after namespace declaration
        if not self.eat_token(Token.CURLY_OPEN):
            return None
        
        # parse namespace elements - tokens
        while self.current_token is not None:
            subexp = self.parse_top_level_expression()
            if subexp:
                namespace.list.append(subexp)
            self.get_next_token()

        # expect '}' at the end
        if not self.eat_token(Token.CURLY_CLOSE):
            return None
        # check for closing '}'
        if self.curly_depth == 0:
            print(f'exit namespace {namespace}')


        return namespace


    def parse_top_level_expression(self):
        expression = None
        token = self.current_token
        value = self.lexer.value
        if token == TokenType.KEYWORD and value == 'namespace':
            expression = self.parse_namespace()
            if not expression:
                print("Error parsing")
            else:
                print(f'Parsed namespace: {expression.name}')
        else:
            print(f'Unparsed token: {token}:  {self.lexer.value}')

        return expression


    def parse_expression(self):
        return None
    

    def get_next_token(self):
        self.current_token = self.lexer.get_token()
        return self.current_token

    def print_ast(self):
        print(
        """
        ====----------------------====
        | Contents of the parsed AST |
        ====----------------------====
        """)
        for expression in AST:
            print(expression)

    def parse(self):
        while self.current_token is not None:
            expression = self.parse_top_level_expression()
            if not expression:
                print('Stop on error.')
            AST.append(expression)
            self.get_next_token()
        
        self.print_ast()