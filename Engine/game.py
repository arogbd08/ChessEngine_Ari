# game.py
from board import Board

class Game:
    def __init__(self):
        self.board = Board()
        self.current_turn = 'white'  # Game starts with White's turn
        self.game_over = False

    def switch_turn(self):
        self.current_turn = 'black' if self.current_turn == 'white' else 'white'

    def is_valid_turn(self, piece):
        return piece and piece.color == self.current_turn

    def move_piece(self, start_x, start_y, end_x, end_y):
        start_pos = self.board.get_position(start_x, start_y)
        end_pos = self.board.get_position(end_x, end_y)

        if not start_pos or not end_pos:
            print("Invalid move: Out of bounds.")
            return False

        if not self.is_valid_turn(start_pos.piece):
            print("Invalid move: Not your turn.")
            return False

        if self.board.move_piece(start_pos, end_pos):
            print(f"{self.current_turn.capitalize()} moved {type(start_pos.piece).__name__} from ({start_x}, {start_y}) to ({end_x}, {end_y})")
            self.check_for_checkmate()
            self.switch_turn()
            return True
        else:
            print("Invalid move.")
            return False

    def check_for_check(self, color):
        king_pos = self.find_king(color)
        opponent_color = 'black' if color == 'white' else 'white'
        
        for row in self.board.grid:
            for position in row:
                if position.piece and position.piece.color == opponent_color:
                    if position.piece.is_valid_move(position, king_pos, self.board.grid):
                        return True
        return False

    def check_for_checkmate(self):
        if self.check_for_check(self.current_turn):
            print(f"Check on {self.current_turn} king!")

    def find_king(self, color):
        for row in self.board.grid:
            for position in row:
                if isinstance(position.piece, King) and position.piece.color == color:
                    return position
        return None

    def display_board(self):
        self.board.display()

    def is_game_over(self):
        return self.game_over

    def start(self):
        while not self.is_game_over():
            self.display_board()
            print(f"{self.current_turn.capitalize()}'s turn.")
            
            try:
                start_x = int(input("Enter start X (0-7): "))
                start_y = int(input("Enter start Y (0-7): "))
                end_x = int(input("Enter end X (0-7): "))
                end_y = int(input("Enter end Y (0-7): "))
                
                self.move_piece(start_x, start_y, end_x, end_y)
            
            except ValueError:
                print("Invalid input. Please enter valid coordinates.")

        print("Game Over!")

if __name__ == "__main__":
    game = Game()
    game.start()
