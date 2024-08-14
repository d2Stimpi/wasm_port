import argparse
from pathlib import Path
from typing import List, Dict

from lexer import Lexer

def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument('input_file', type=Path)
    return parser.parse_args()

def main():
    args = parse_args()
    input_file = args.input_file
    
    lexer = Lexer(input_file)
    token = lexer.get_token()
    while token != None: 
        print(token, '[', lexer.value, ']')
        token = lexer.get_token()

if __name__ == '__main__':
    main()