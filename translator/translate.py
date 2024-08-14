import argparse
from pathlib import Path
from typing import List, Dict

from csharp.parser import Parser

def parse_args():
    parser = argparse.ArgumentParser()
    parser.add_argument('input_file', type=Path)
    return parser.parse_args()

def main():
    args = parse_args()
    input_file = args.input_file
    
    parser = Parser(input_file)
    parser.parse()

if __name__ == '__main__':
    main()