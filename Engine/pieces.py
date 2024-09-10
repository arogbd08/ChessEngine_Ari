# move.py

class Piece:
    def __init__(self, color, value):
        self.color = color
        self.value = value
    
    def is_valid_move(self, start_pos, end_pos, board):
        # This method should be overridden in derived classes
        raise NotImplementedError("This method should be overridden in derived classes")

class Pawn(Piece):
    def __init__(self, color):
        super().__init__(color, value=1)  # Pawns are typically valued at 1

    def is_valid_move(self, start_pos, end_pos, board):
        # Get the direction based on the color
        direction = 1 if self.color == 'white' else -1
        
        # Single move forward
        if start_pos.x == end_pos.x and end_pos.y == start_pos.y + direction and end_pos.piece is None:
            # Pawn promotion
            if (self.color == 'white' and end_pos.y == 7) or (self.color == 'black' and end_pos.y == 0):
                # Promote to queen or other piece (additional implementation needed)
                return True
            return True
        
        # Double move forward from starting position
        if start_pos.x == end_pos.x and end_pos.y == start_pos.y + 2 * direction:
            if (self.color == 'white' and start_pos.y == 1) or (self.color == 'black' and start_pos.y == 6):
                if board[end_pos.x][end_pos.y] is None:
                    return True
        
        # Capture move
        if abs(start_pos.x - end_pos.x) == 1 and end_pos.y == start_pos.y + direction:
            if end_pos.piece and end_pos.piece.color != self.color:
                return True
        
        return False

class Rook(Piece):
    def __init__(self, color):
        super().__init__(color, value=5)  # Rooks are typically valued at 5

    def is_valid_move(self, start_pos, end_pos, board):
        # Check if the move is in a straight line
        if start_pos.x == end_pos.x or start_pos.y == end_pos.y:
            # Check for obstacles in the way
            if self.is_path_clear(start_pos, end_pos, board):
                return True
        return False
    
    def is_path_clear(self, start_pos, end_pos, board):
        # Check if the path between start_pos and end_pos is clear (no pieces in between)
        if start_pos.x == end_pos.x:  # Moving vertically
            step = 1 if end_pos.y > start_pos.y else -1
            for y in range(start_pos.y + step, end_pos.y, step):
                if board[start_pos.x][y] is not None:
                    return False
        else:  # Moving horizontally
            step = 1 if end_pos.x > start_pos.x else -1
            for x in range(start_pos.x + step, end_pos.x, step):
                if board[x][start_pos.y] is not None:
                    return False
        return True

class Knight(Piece):
    def __init__(self, color):
        super().__init__(color, value=3)  # Knights are typically valued at 3

    def is_valid_move(self, start_pos, end_pos, board):
        # Knight moves in an L-shape: two squares in one direction and one in the perpendicular direction
        dx = abs(start_pos.x - end_pos.x)
        dy = abs(start_pos.y - end_pos.y)
        return (dx == 2 and dy == 1) or (dx == 1 and dy == 2)

class Bishop(Piece):
    def __init__(self, color):
        super().__init__(color, value=3)  # Bishops are typically valued at 3

    def is_valid_move(self, start_pos, end_pos, board):
        # Bishop moves diagonally
        if abs(start_pos.x - end_pos.x) == abs(start_pos.y - end_pos.y):
            # Check for obstacles in the way
            if self.is_path_clear(start_pos, end_pos, board):
                return True
        return False
    
    def is_path_clear(self, start_pos, end_pos, board):
        # Check if the diagonal path is clear
        step_x = 1 if end_pos.x > start_pos.x else -1
        step_y = 1 if end_pos.y > start_pos.y else -1
        x, y = start_pos.x + step_x, start_pos.y + step_y
        while x != end_pos.x and y != end_pos.y:
            if board[x][y] is not None:
                return False
            x += step_x
            y += step_y
        return True

class Queen(Piece):
    def __init__(self, color):
        super().__init__(color, value=9)  # Queens are typically valued at 9

    def is_valid_move(self, start_pos, end_pos, board):
        # Queen moves like both a Rook and a Bishop
        if (start_pos.x == end_pos.x or start_pos.y == end_pos.y) or (abs(start_pos.x - end_pos.x) == abs(start_pos.y - end_pos.y)):
            if self.is_path_clear(start_pos, end_pos, board):
                return True
        return False
    
    def is_path_clear(self, start_pos, end_pos, board):
        # Combine rook and bishop path checking logic (similar to Rook and Bishop methods)
        if start_pos.x == end_pos.x or start_pos.y == end_pos.y:
            return Rook.is_path_clear(self, start_pos, end_pos, board)
        return Bishop.is_path_clear(self, start_pos, end_pos, board)

class King(Piece):
    def __init__(self, color):
        super().__init__(color, value=float('inf'))  # Kings are invaluable

    def is_valid_move(self, start_pos, end_pos, board):
        # King moves one square in any direction
        if max(abs(start_pos.x - end_pos.x), abs(start_pos.y - end_pos.y)) == 1:
            if not end_pos.piece or end_pos.piece.color != self.color:
                return True
        return False
