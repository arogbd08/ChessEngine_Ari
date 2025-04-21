using ChessChallenge.API;
using System;
using System.Collections.Generic;

public class MyBot : IChessBot
{// Pawn Table: Encourage central pawns and penalize early advancement of edge pawns.
static int[] PawnTable = new int[]
{
    0,   0,   0,   0,   0,   0,   0,   0,
    5,  10,  10, -20, -20,  10,  10,   5,
    5,  -5, -10,   0,   0, -10,  -5,   5,
    0,   0,   0,  20,  20,   0,   0,   0,
    5,   5,  10,  25,  25,  10,   5,   5,
   10,  10,  20,  30,  30,  20,  10,  10,
   50,  50,  50,  50,  50,  50,  50,  50,
    0,   0,   0,   0,   0,   0,   0,   0
};

// Knight Table: Encourage centralization and penalize edge positions.
static int[] KnightTable = new int[]
{
    -50, -40, -30, -30, -30, -30, -40, -50,
    -40, -20,   0,   0,   0,   0, -20, -40,
    -30,   0,  10,  15,  15,  10,   0, -30,
    -30,   5,  15,  20,  20,  15,   5, -30,
    -30,   0,  15,  20,  20,  15,   0, -30,
    -30,   5,  10,  15,  15,  10,   5, -30,
    -40, -20,   0,   5,   5,   0, -20, -40,
    -50, -40, -30, -30, -30, -30, -40, -50
};

// Bishop Table: Encourage activity and central positioning.
static int[] BishopTable = new int[]
{
    -20, -10, -10, -10, -10, -10, -10, -20,
    -10,   5,   0,   0,   0,   0,   5, -10,
    -10,  10,  10,  10,  10,  10,  10, -10,
    -10,   0,  10,  10,  10,  10,   0, -10,
    -10,   5,   5,  10,  10,   5,   5, -10,
    -10,   0,   5,  10,  10,   5,   0, -10,
    -10,   0,   0,   0,   0,   0,   0, -10,
    -20, -10, -10, -10, -10, -10, -10, -20
};

// Rook Table: Favor open files and 7th rank activity.
static int[] RookTable = new int[]
{
     0,   0,   5,  10,  10,   5,   0,   0,
    -5,   0,   0,   0,   0,   0,   0,  -5,
    -5,   0,   0,   0,   0,   0,   0,  -5,
    -5,   0,   0,   0,   0,   0,   0,  -5,
    -5,   0,   0,   0,   0,   0,   0,  -5,
    -5,   0,   0,   0,   0,   0,   0,  -5,
     5,  10,  10,  10,  10,  10,  10,   5,
     0,   0,   5,  10,  10,   5,   0,   0
};

// Queen Table: Maximize mobility and central control.
static int[] QueenTable = new int[]
{
    -20, -10, -10,  -5,  -5, -10, -10, -20,
    -10,   0,   5,   0,   0,   0,   0, -10,
    -10,   5,   5,   5,   5,   5,   0, -10,
     -5,   0,   5,   5,   5,   5,   0,  -5,
      0,   0,   5,   5,   5,   5,   0,  -5,
    -10,   0,   5,   5,   5,   0,   0, -10,
    -10,   0,   0,   0,   0,   0,   0, -10,
    -20, -10, -10,  -5,  -5, -10, -10, -20
};

// King Table (Middle Game): Encourage safety, reward castling.
static int[] KingTableMiddleGame = new int[]
{
    -30, -40, -40, -50, -50, -40, -40, -30,
    -30, -40, -40, -50, -50, -40, -40, -30,
    -30, -40, -40, -50, -50, -40, -40, -30,
    -30, -40, -40, -50, -50, -40, -40, -30,
    -20, -30, -30, -40, -40, -30, -30, -20,
    -10, -20, -20, -20, -20, -20, -20, -10,
     20,  20,   0,   0,   0,   0,  20,  20,
     20,  30,  10,   0,   0,  10,  30,  20
};

// King Table (Endgame): Favor centralization and activity.
static int[] KingTableEndgame = new int[]
{
    -50, -30, -30, -30, -30, -30, -30, -50,
    -30, -20, -20, -20, -20, -20, -20, -30,
    -30, -20,  20,  30,  30,  20, -20, -30,
    -30, -20,  30,  40,  40,  30, -20, -30,
    -30, -20,  30,  40,  40,  30, -20, -30,
    -30, -20,  20,  30,  30,  20, -20, -30,
    -30, -30, -20, -20, -20, -20, -30, -30,
    -50, -30, -30, -30, -30, -30, -30, -50
};



    public Move Think(Board board, Timer timer)
    {
        int depth = 4;
        Move move = Negamax(board, depth, int.MinValue, int.MaxValue, 1);
        return move;
    }

    public int Evaluate(Board board)
    {
        int score = 0;
        bool isBotWhite = board.IsWhiteToMove;

       

       foreach (var pieceList in board.GetAllPieceLists())
        {
            foreach (var piece in pieceList)
            {
                int pieceValue = GetPieceValues(piece);
                int positionValue = GetPositionalValue(piece);
                score += (piece.IsWhite == isBotWhite ? 1 : -1) * (pieceValue + positionValue);
                
            }
        }


        if (board.IsInCheck())
        {
            score += board.IsWhiteToMove == isBotWhite ? 50 : -50;
        }

        return score;
    }
    public int GetPieceValues(Piece piece)
    {
        switch (piece.PieceType)
        {
            case PieceType.Pawn:
                return 100;
            case PieceType.Knight:
                return 320;
            case PieceType.Bishop:
                return 330;
            case PieceType.Rook:
                return 500;
            case PieceType.Queen:
                return 900;
            case PieceType.King:
                return 20000;
            default:
                return 0;
        }
    }

    public int GetPositionalValue(Piece piece)
    {
        int[] table;
        switch (piece.PieceType)
        {
            case PieceType.Pawn:
                table = PawnTable;
                break;
            case PieceType.Knight:
                table = KnightTable;
                break;
            case PieceType.Bishop:
                table = BishopTable;
                break;
            case PieceType.Rook:
                table = RookTable;
                break;
            case PieceType.Queen:
                table = QueenTable;
                break;
            case PieceType.King:
            table =  KingTableMiddleGame;
                break;
            default:
                return 0;
        }
        int index = piece.IsWhite ? piece.Square.Index : 63 - piece.Square.Index;
        return table[index];
    }



    public Move Negamax(Board board, int depth, int alpha, int beta, int color)
    {
        Move[] allMoves = board.GetLegalMoves();
        if (allMoves.Length == 0)
        {
            return Move.NullMove;
        }

        Move bestMove = allMoves[0];
        int bestValue = int.MinValue;

        foreach (Move move in allMoves)
        {
            board.MakeMove(move);
            int boardValue = -NegamaxSearch(board, depth - 1, alpha, beta, -color);
            board.UndoMove(move);

            if (boardValue > bestValue)
            {
                bestValue = boardValue;
                bestMove = move;
            }

            if (bestValue > alpha)
            {
                alpha = bestValue;
            }
            if (beta <= alpha)
            {
                break;
            }
        }

        return bestMove;
    }

    public int NegamaxSearch(Board board, int depth, int alpha, int beta, int color)
    {
        if (depth == 0 || board.IsInCheckmate() || board.IsDraw())
        {
            if (board.IsInCheckmate())
            {
                return color * 10000;
            }
            if (board.IsDraw())
            {
                return 0;
            }
            return color * Evaluate(board);
        }

        Move[] allMoves = board.GetLegalMoves();
        int bestValue = int.MinValue;

        foreach (Move move in allMoves)
        {
            board.MakeMove(move);
            int boardValue = -NegamaxSearch(board, depth - 1, alpha, beta, -color);
            board.UndoMove(move);

            bestValue = Math.Max(bestValue, boardValue);
            alpha = Math.Max(alpha, bestValue);

            if (beta <= alpha)
            {
                break;
            }
        }

        return bestValue;
    }




}