using ChessChallenge.API;
using System;

namespace ChessChallenge.Example
{
    // A simple bot that can spot mate in one, and always captures the most valuable piece it can.
    // Plays randomly otherwise.
    public class EvilBot : IChessBot
    {
        
    static int[] PawnTable = new int[]
       {
        50, 50, 50, 50, 50, 50, 50, 50,
        55, 60, 60, 30, 30, 60, 60, 55,
        55, 45, 40, 50, 50, 40, 45, 55,
        50, 50, 50, 70, 70, 50, 50, 50,
        55, 55, 60, 105, 105, 60, 55, 55,
        60, 60, 70, 80, 80, 70, 60, 60,
        100, 100, 100, 100, 100, 100, 100, 100,
        50, 50, 50, 50, 50, 50, 50, 50
       };

    static int[] KnightTable = new int[]
    {
        0, 10, 20, 20, 20, 20, 10, 0,
        10, 30, 50, 50, 50, 50, 30, 10,
        20, 50, 60, 65, 65, 60, 50, 20,
        20, 55, 65, 70, 70, 65, 55, 20,
        20, 50, 65, 70, 70, 65, 50, 20,
        20, 55, 60, 65, 65, 60, 55, 20,
        10, 30, 50, 55, 55, 50, 30, 10,
        0, 10, 20, 20, 20, 20, 10, 0
    };

    static int[] BishopTable = new int[]
    {
        30, 40, 40, 40, 40, 40, 40, 30,
        40, 50, 50, 50, 50, 50, 50, 40,
        40, 50, 55, 60, 60, 55, 50, 40,
        40, 55, 55, 60, 60, 55, 55, 40,
        40, 50, 60, 60, 60, 60, 50, 40,
        40, 60, 60, 60, 60, 60, 60, 40,
        40, 55, 50, 50, 50, 50, 55, 40,
        30, 40, 40, 40, 40, 40, 40, 30
    };

    static int[] RookTable = new int[]
    {
        50, 50, 50, 55, 55, 50, 50, 50,
        45, 50, 50, 50, 50, 50, 50, 45,
        45, 50, 50, 50, 50, 50, 50, 45,
        45, 50, 50, 50, 50, 50, 50, 45,
        45, 50, 50, 50, 50, 50, 50, 45,
        45, 50, 50, 50, 50, 50, 50, 45,
        55, 60, 60, 60, 60, 60, 60, 55,
        50, 50, 50, 55, 55, 50, 50, 50
    };

    static int[] QueenTable = new int[]
    {
        30, 40, 40, 45, 45, 40, 40, 30,
        40, 50, 50, 50, 50, 50, 50, 40,
        40, 50, 55, 55, 55, 55, 50, 40,
        45, 50, 55, 55, 55, 55, 50, 45,
        50, 50, 55, 55, 55, 55, 50, 45,
        40, 55, 55, 55, 55, 55, 50, 40,
        40, 50, 50, 50, 50, 50, 50, 40,
        30, 40, 40, 45, 45, 40, 40, 30
    };

    static int[] KingTableMiddleGame = new int[]
    {
        20, 10, 10, 0, 0, 10, 10, 20,
        20, 10, 10, 0, 0, 10, 10, 20,
        20, 10, 10, 0, 0, 10, 10, 20,
        20, 10, 10, 0, 0, 10, 10, 20,
        30, 20, 20, 10, 10, 20, 20, 30,
        40, 30, 30, 30, 30, 30, 30, 40,
        70, 70, 50, 50, 50, 50, 70, 70,
        70, 80, 60, 50, 50, 60, 80, 70
    };


    public Move Think(Board board, Timer timer)
    {
        int depth = 2;
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
                int positionValue = GetPositionalValue(piece, isBotWhite);
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

    public int GetPositionalValue(Piece piece, bool isBotWhite)
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
                table = KingTableMiddleGame;
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
}