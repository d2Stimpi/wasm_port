from lexer import Lexer
from lexer import TokenType
from lexer import Token

dbg = True

# ====--------
# AST classes
# ====--------

AST = []

class Expression:
    def __init__(self) -> None:
        self.list = []

# namespace expression
# namespace <name> { [expressions] }
class NamespaceExpression(Expression):
    def __init__(self, name):
        super().__init__()
        self.name = name
        # internal use only
        self.curly_depth = 0

    def internal_str(self, lvl) -> str:
        str = f'namespace {self.name}'
        for exp in self.list:
            str += '\n' + ('  ' * lvl) + '> ' + exp.internal_str(lvl + 1)
        return str

    def __str__(self) -> str:
        return self.internal_str(1)

# class expression
# [visiblity] class <name> : [parent] { [expressions] }
class ClassExpression(Expression):
    def __init__(self, name, visibility = 'private'):
        super().__init__()
        self.name = name
        self.visibliity = visibility
        self.parent_name = ''

    def internal_str(self, lvl) -> str:
        str = f'class {self.name}, visibility: {self.visibliity}'
        for exp in self.list:
            str += '\n' + ('  ' * lvl) + '> ' + exp.internal_str(lvl + 1)
        return str

    def __str__(self) -> str:
        return self.internal_str(1)

# ====--------
# Parser
# ====--------

class Parser:
    def __init__(self, fileName):
        # for counting visited curly brackets (levels)
        self.curly_depth = 0
        self.currnet_visibility = 'private'

        self.lexer = Lexer(fileName)
        self.current_token = self.get_next_token()

    # ====--------
    # Helper methods
    # ====--------

    # helper method that expects a token type and eats it
    def eat_token(self, expected) -> bool:
        token = self.get_next_token()
        value = self.lexer.value
        if value is expected.value:
            if dbg: print(f'consumed {expected.value}')
            # consume the token and move to the next
            self.get_next_token()
            return True
        else:
            print(f'Error: Expected \'{expected.value}\' token, got {token}:\'{value}\'')
            return False

    # helper method for stroing the last read visibility token
    def set_current_visibility(self, value):
        self.current_visibility = value
        if dbg: print(f'visibility set to {self.current_visibility}')

    # helper for reading and consuming last stored visibility token
    def get_current_visibility(self):
        ret = self.current_visibility
        self.current_visibility = 'private'
        return ret

    # ====--------
    # Parser methods
    # ====--------

    # parse namespace, expected '{' after namespace identifier
    # process subexpressions until reaching closing '}'
    def parse_namespace(self) -> Expression:
        namespace = None
        token = self.get_next_token()
        if token != TokenType.IDENTIFIER:
            print(f'Error: Expected identifier, got {token}:{self.lexer.value}')

        namespace = NamespaceExpression(self.lexer.value)
        # set namespace's curly depth
        namespace.curly_depth = self.curly_depth
        #if dbg: print(f'---- enter namespace {namespace.name} (depth {namespace.curly_depth})')
        
        # expect '{' after namespace declaration
        if not self.eat_token(Token.CURLY_OPEN):
            return None

        # parse namespace elements - tokens
        while self.curly_depth > namespace.curly_depth:
            subexp = self.parse_top_level_expression()
            if subexp:
                if dbg: print(f'Add {subexp.name} to namespace {namespace.name}')
                namespace.list.append(subexp)
            self.get_next_token()

        # expect '}' at the end of namespace
        
        #if dbg: print(f'---- exit namespace {namespace.name}')
        return namespace



    def parse_class(self) -> Expression:
        classexp = None
        token = self.get_next_token()
        if token != TokenType.IDENTIFIER:
            print(f'Error: Expected identifier, got {token}:{self.lexer.value}')

        classexp = ClassExpression(self.lexer.value, self.get_current_visibility())

        # expect '{' after namespace declaration
        if not self.eat_token(Token.CURLY_OPEN):
            return None
        
        return classexp

    # preceding token check
    #   visibility token (public, private)
    #   storage token (stacic)
    def check_for_preceding_token(self):
        token = self.current_token
        value = self.lexer.value
        if token == TokenType.KEYWORD and value in ['public', 'private']:
            self.set_current_visibility(value)
            self.get_next_token()

    def parse_top_level_expression(self):
        # check for preceding (visibility, storage) token
        self.check_for_preceding_token()

        expression = None
        token = self.current_token
        value = self.lexer.value

        # check for namespace
        if token == TokenType.KEYWORD and value == 'namespace':
            expression = self.parse_namespace()
            if not expression:
                print("Error parsing namespace")
            else:
                print(f'Parsed namespace: {expression.name}')

        # check for class
        if token == TokenType.KEYWORD and value == 'class':
            expression = self.parse_class()
            if not expression:
                print("Error parsing class")
            else:
                print(f'Parsed class: {expression.name}')
        else:
            print(f'Unparsed token: {token}:  {self.lexer.value}')

        return expression


    def parse_expression(self):
        return None
    

    def get_next_token(self):
        self.current_token = self.lexer.get_token()
        value = self.lexer.value
        # check for curly and handle depth
        if value is Token.CURLY_OPEN.value:
            self.curly_depth += 1
        if value is Token.CURLY_CLOSE.value:
            self.curly_depth -= 1

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