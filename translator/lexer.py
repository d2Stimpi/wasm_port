from enum import Enum

class Token(Enum):
    IDENTIFIER = "identifier"
    SEPARATOR = "separator"
    KEYWORD = "keyword"
    NUMBER = "number"


# matcher lists:
keywords_list = {
    "public", "class", "void", "int"
}

separators_list = {
    "(", ")", "{", "}", ";", ",", ":", "."
}

skip_list = {
    " ", "\n", "\t"
}

class Lexer:
    def __init__(self, fileName):
        self.value = ''
        self.fileName = fileName
        self.file = open(fileName, 'r')
        # fast forward to the namespace declaration
        while True:
            line = self.file.readline()
            if line.startswith('namespace'):
                break

    def __del__(self):
        self.file.close()

    def get_token(self) -> Token:
        tok = ''
        char = ''
        while True:
            char = self.file.read(1)
            if not char:
                if tok != '':
                    break 
                return None

            #special case for const strings that start and end with ""
            if char == '"':
                tok += char
                char = self.file.read(1)
                while char != '"':
                    tok += char
                    char = self.file.read(1)
                tok += char
                break

            # tokens to skip (consume) " ", "\n", "\t"
            if char in skip_list:
                if tok == '':
                    continue
                break
            else:
                if char in separators_list:
                    if tok != '':
                        # go back one character, since we would consume it otherwise
                        self.file.seek(self.file.tell() - 1)
                        break
                    else:
                        tok = char
                        break
                else:
                    tok += char

        self.value = tok
        if tok in keywords_list:
            return Token.KEYWORD
        elif tok in separators_list:
            return Token.SEPARATOR
        elif tok.isnumeric():
            return Token.NUMBER
        
        return Token.IDENTIFIER