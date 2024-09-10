# board.py
from pieces import Pawn
from pieces import Rook
from pieces import Knight
from pieces import Bishop
from pieces import Queen
from pieces import King


class Position:
    def __init__(self, x, y, piece=None):
        self.x = x
        self.y = y
        self.piece = piece  # Piece at this position, None if empty

    def __repr__(self):
        return f"({self.x}, {self.y}, {self.piece})"

class Board:
    def __init__(self):
        # Create an 8x8 grid filled with Position objects
        self.grid = [[Position(x, y) for y in range(8)] for x in range(8)]
        self.setup_board()

    def setup_board(self):
        # Set up the initial positions of the pieces on the board
        for i in range(8):
            self.grid[i][1].piece = Pawn('white')
            self.grid[i][6].piece = Pawn('black')

        self.grid[0][0].piece = Rook('white')
        self.grid[7][0].piece = Rook('white')
        self.grid[0][7].piece = Rook('black')
        self.grid[7][7].piece = Rook('black')

        self.grid[1][0].piece = Knight('white')
        self.grid[6][0].piece = Knight('white')
        self.grid[1][7].piece = Knight('black')
        self.grid[6][7].piece = Knight('black')

        self.grid[2][0].piece = Bishop('white')
        self.grid[5][0].piece = Bishop('white')
        self.grid[2][7].piece = Bishop('black')
        self.grid[5][7].piece = Bishop('black')

        self.grid[3][0].piece = Queen('white')
        self.grid[3][7].piece = Queen('black')

        self.grid[4][0].piece = King('white')
        self.grid[4][7].piece = King('black')

    def display(self):
        # Define symbols for each piece type
        piece_symbols = {
            'Pawn': 'P',
            'Rook': 'R',
            'Knight': 'N',
            'Bishop': 'B',
            'Queen': 'Q',
            'King': 'K'
        }
        
        print("   a  b  c  d  e  f  g  h")  # Column labels for readability
        print("  +--+--+--+--+--+--+--+--+")
        for y in range(7, -1, -1):
            row = f"{y + 1} |"  # Row labels for readability
            for x in range(8):
                piece = self.grid[x][y].piece
                if piece is None:
                    row += "  |"
                else:
                    symbol = piece_symbols[type(piece).__name__]
                    row += f"{symbol.lower() if piece.color == 'black' else symbol} |"
            print(row)
            print("  +--+--+--+--+--+--+--+--+")
        print("\n")
