using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Solution
{

    public static PrintLevel printLevel = PrintLevel.None;
    static string pathEnvironmenVariable = "HACKERRANK_OUTPUT_PATH";
    static int initialNumberOfMoves;

    /*
     * Complete the simplifiedChessEngine function below.
     */
    static string simplifiedChessEngine(char[][] whites, char[][] blacks, int moves)
    {
        Piece[,] board = new Piece[4, 4];

        for (int i = 0; i < whites.GetLength(0); i++)
        {
            Rank rank;
            int row;
            int column;
            GetPieceOnBoard(whites[i], out rank, out row, out column);
            board[row, column].color = Color.White;
            board[row, column].rank = rank;
        }

        for (int i = 0; i < blacks.GetLength(0); i++)
        {
            Rank rank;
            int row;
            int column;
            GetPieceOnBoard(blacks[i], out rank, out row, out column);
            board[row, column].color = Color.Black;
            board[row, column].rank = rank;
        }

        initialNumberOfMoves = moves;

        if (printLevel == PrintLevel.All || printLevel == PrintLevel.Selected)
        {
            Console.BufferWidth = 200;
            Console.BufferHeight = 2000;
        }

        if (printLevel == PrintLevel.All || printLevel == PrintLevel.Selected)
        {
            PrintBoard(board, "Initial board, white moves: " + moves);
        }

        string result = CheckWhiteForWinner(board, moves, Color.White) ? "YES" : "NO";

        return result;
    }

    static void Main(string[] args)
    {

        TextWriter textWriter = new StreamWriter(@System.Environment.GetEnvironmentVariable(pathEnvironmenVariable), false);

        int g = Convert.ToInt32(Console.ReadLine());

        // Once the first line has been read, the rest is automatic, so start measuring
        var watch = System.Diagnostics.Stopwatch.StartNew();

        for (int gItr = 0; gItr < g; gItr++)
        {
            string[] wbm = Console.ReadLine().Split(' ');

            int w = Convert.ToInt32(wbm[0]);

            int b = Convert.ToInt32(wbm[1]);

            int m = Convert.ToInt32(wbm[2]);

            char[][] whites = new char[w][];

            for (int whitesRowItr = 0; whitesRowItr < w; whitesRowItr++)
            {
                whites[whitesRowItr] = Array.ConvertAll(Console.ReadLine().Split(' '), whitesTemp => whitesTemp[0]);
            }

            char[][] blacks = new char[b][];

            for (int blacksRowItr = 0; blacksRowItr < b; blacksRowItr++)
            {
                blacks[blacksRowItr] = Array.ConvertAll(Console.ReadLine().Split(' '), blacksTemp => blacksTemp[0]);
            }

            string result = simplifiedChessEngine(whites, blacks, m);

            if (printLevel == PrintLevel.All || printLevel == PrintLevel.Selected)
            {
                Console.WriteLine("Result of run: " + result);
            }

            textWriter.WriteLine(result);
            textWriter.Flush();
        }

        textWriter.Flush();
        textWriter.Close();

        watch.Stop();
        var elapsedMs = watch.ElapsedMilliseconds;

        if (printLevel == PrintLevel.All || printLevel == PrintLevel.Selected)
        {
            Console.WriteLine("Time spent (ms): " + elapsedMs);
            Console.ReadLine();
        }
    }

    private static bool CheckWhiteForWinner(Piece[,] board, int numberOfWhiteMovesLeft, Color playerInTurn)
    {
        MoveCalculator mc = new MoveCalculator(board, playerInTurn);

        int movesMade = 0;
        if (playerInTurn == Color.White)
        {
            bool noMovesAvailable = true;
            foreach (var move in mc.OrderBy(m => (board[m[2], m[3]].rank == Rank.Queen ? 0 : 1)))
            {
                noMovesAvailable = false;

                movesMade++;
                if(movesMade > 5 * 12)
                {
                    if (printLevel == PrintLevel.All || printLevel == PrintLevel.Selected)
                    {
                        Console.WriteLine("Large number of moves made for white");
                        Console.ReadLine();
                    }
                }

                Piece[,] boardWithMove = (Piece[,])board.Clone();
                Piece movingPiece = boardWithMove[move[0], move[1]];

                //Tjek om dronning bliver taget
                if (board[move[2], move[3]].rank == Rank.Queen)
                {
                    if (printLevel == PrintLevel.All)
                    {
                        PrintBoard(boardWithMove, $"Black queen captured - White moving {movingPiece.rank} from ({move[0]},{move[1]}) to ({move[2]},{move[3]})");
                    }
                    return true;
                }

                boardWithMove[move[2], move[3]] = movingPiece;
                boardWithMove[move[0], move[1]].rank = Rank.None;
                boardWithMove[move[0], move[1]].color = Color.None;

                if (printLevel == PrintLevel.All)
                {
                    PrintBoard(boardWithMove, $"White moved {movingPiece.rank} from ({move[0]},{move[1]}) to ({move[2]},{move[3]})");
                }

                bool result = CheckWhiteForWinner(boardWithMove, numberOfWhiteMovesLeft - 1, Color.Black);

                if(numberOfWhiteMovesLeft == initialNumberOfMoves)
                {
                    if (printLevel == PrintLevel.All || printLevel == PrintLevel.Selected)
                    {
                        PrintBoard(boardWithMove, $"Examined at {numberOfWhiteMovesLeft} white moves left: White moved {movingPiece.rank} from ({move[0]},{move[1]}) to ({move[2]},{move[3]})");
                        Console.WriteLine("Result of examination: " + result);
                    }
                }

                if (result) return true;
            }

            if (noMovesAvailable)
            {
                if (printLevel == PrintLevel.All || printLevel == PrintLevel.Selected)
                {
                    PrintBoard(board, "No moves available for white!");
                }
            }

            return false;

        }
        else
        {
            if (numberOfWhiteMovesLeft == 0)
            {
                return false;
            }
            bool noMovesAvailable = true;
            foreach (var move in mc.OrderBy(m => (board[m[2], m[3]].rank == Rank.Queen ? 0 : 1)))
            {
                noMovesAvailable = false;

                movesMade++;
                if (movesMade > 5 * 12)
                {
                    if (printLevel == PrintLevel.All || printLevel == PrintLevel.Selected)
                    {
                        Console.WriteLine("Large number of moves made for black");
                        Console.ReadLine();
                    }
                }

                Piece[,] boardWithMove = (Piece[,])board.Clone();

                Piece movingPiece = boardWithMove[move[0], move[1]];

                //Tjek om dronning bliver taget
                if (board[move[2], move[3]].rank == Rank.Queen)
                {
                    if (printLevel == PrintLevel.All)
                    {
                        PrintBoard(boardWithMove, $"White queen captured - Black moving {movingPiece.rank} from ({move[0]},{move[1]}) to ({move[2]},{move[3]})");
                    }
                    return false;
                }

                boardWithMove[move[2], move[3]] = movingPiece;
                boardWithMove[move[0], move[1]].rank = Rank.None;
                boardWithMove[move[0], move[1]].color = Color.None;

                if (printLevel == PrintLevel.All)
                {
                    PrintBoard(boardWithMove, $"Black moved {movingPiece.rank} from ({move[0]},{move[1]}) to ({move[2]},{move[3]})");
                }

                bool result = CheckWhiteForWinner(boardWithMove, numberOfWhiteMovesLeft, Color.White);

                if (numberOfWhiteMovesLeft == initialNumberOfMoves - 1)
                {
                    if (printLevel == PrintLevel.All || printLevel == PrintLevel.Selected)
                    {
                        PrintBoard(boardWithMove, $"Examined at {numberOfWhiteMovesLeft} white moves left: Black moved {movingPiece.rank} from ({move[0]},{move[1]}) to ({move[2]},{move[3]})");
                        Console.WriteLine("Result of examination: " + result);
                    }
                }

                if (!result) return false;
            }

            if (noMovesAvailable)
            {
                if (printLevel == PrintLevel.All || printLevel == PrintLevel.Selected)
                {
                    PrintBoard(board, "No moves available for black!");
                }
            }

            return true;
        }
    }




    private static void GetPieceOnBoard(char[] conf, out Rank rank, out int row, out int column)
    {
        switch (conf[0])
        {
            case 'Q':
                rank = Rank.Queen;
                break;
            case 'N':
                rank = Rank.Knight;
                break;
            case 'B':
                rank = Rank.Bishop;
                break;
            case 'R':
                rank = Rank.Rook;
                break;
            default:
                rank = Rank.None;
                break;
        }

        row = int.Parse(conf[2].ToString()) - 1;

        switch (conf[1])
        {
            case 'A':
                column = 0;
                break;
            case 'B':
                column = 1;
                break;
            case 'C':
                column = 2;
                break;
            case 'D':
                column = 3;
                break;
            default:
                column = -1;
                break;
        }
    }


    private static void PrintBoard(Piece[,] Board, string header)
    {
        Console.WriteLine("");
        Console.WriteLine("* " + header + " *");
        Console.WriteLine("  0  1  2  3");
        for (int i = 0; i < Board.GetLength(0); i++)
        {
            Console.Write(i + " ");
            for (int j = 0; j < Board.GetLength(1); j++)
            {
                Console.Write(Board[i, j] + " ");
            }

            Console.WriteLine();
        }
    }
}


enum Color
{
    None = 0,
    White = 87,
    Black = 66

}

enum Rank
{
    None = 0,
    Queen = 81,
    Rook = 82,
    Bishop = 66,
    Knight = 78
}

struct Piece
{
    public Color color;
    public Rank rank;

    public override string ToString()
    {
        if (color == Color.None && rank == Rank.None)
        {
            return "--";

        }
        return "" + (char)color + (char)rank;
    }
}

class MoveCalculator : IEnumerable<int[]>
{
    private readonly Piece[,] _board;
    private readonly Color _playerInTurn;

    public MoveCalculator(Piece[,] board, Color playerInTurn)
    {
        _board = board;
        _playerInTurn = playerInTurn;
    }

    public IEnumerator<int[]> GetEnumerator()
    {
        for (int i = 0; i < _board.GetLength(0); i++)
        {
            for (int j = 0; j < _board.GetLength(1); j++)
            {
                if (_board[i, j].color == _playerInTurn)
                {
                    MoveCalculatorSinglePiece moveCalculatorSinglePiece = new MoveCalculatorSinglePiece(_board, _playerInTurn, i, j);
                    int movesFound = 0;
                    //foreach (var move in moveCalculatorSinglePiece.OrderByDescending(m => m[5]))
                    foreach (var move in moveCalculatorSinglePiece)
                    {
                        movesFound++;
                        if (movesFound > 12)
                        {
                            if (Solution.printLevel == PrintLevel.All || Solution.printLevel == PrintLevel.Selected)
                            {
                                Console.WriteLine("Large number of moves found for piece on ({i},{j})");
                                Console.ReadLine();
                            }
                        }

                        yield return move;
                    }
                }
            }
        }
    }


    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

class MoveCalculatorSinglePiece : IEnumerable<int[]>
{
    private readonly Piece[,] _board;
    private readonly Color _playerInTurn;
    private readonly int _row;
    private readonly int _column;
    private static Dictionary<Rank, List<Tuple<int, int>>> _offsetsDictionary;

    static MoveCalculatorSinglePiece()
    {
        _offsetsDictionary = new Dictionary<Rank, List<Tuple<int, int>>>();
        _offsetsDictionary.Add(Rank.Knight, new List<Tuple<int, int>>());
        _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(1, 2));
        _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(1, -2));
        _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(-1, 2));
        _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(-1, -2));
        _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(2, 1));
        _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(2, -1));
        _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(-2, 1));
        _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(-2, -1));

        _offsetsDictionary.Add(Rank.Bishop, new List<Tuple<int, int>>());
        _offsetsDictionary[Rank.Bishop].Add(new Tuple<int, int>(1, 1));
        _offsetsDictionary[Rank.Bishop].Add(new Tuple<int, int>(1, -1));
        _offsetsDictionary[Rank.Bishop].Add(new Tuple<int, int>(-1, 1));
        _offsetsDictionary[Rank.Bishop].Add(new Tuple<int, int>(-1, -1));

        _offsetsDictionary.Add(Rank.Rook, new List<Tuple<int, int>>());
        _offsetsDictionary[Rank.Rook].Add(new Tuple<int, int>(1, 0));
        _offsetsDictionary[Rank.Rook].Add(new Tuple<int, int>(-1, 0));
        _offsetsDictionary[Rank.Rook].Add(new Tuple<int, int>(0, 1));
        _offsetsDictionary[Rank.Rook].Add(new Tuple<int, int>(0, -1));

        _offsetsDictionary.Add(Rank.Queen, new List<Tuple<int, int>>());
        _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(1, 1));
        _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(1, -1));
        _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(-1, 1));
        _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(-1, -1));
        _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(1, 0));
        _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(-1, 0));
        _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(0, 1));
        _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(0, -1));

        _offsetsDictionary.Add(Rank.None, new List<Tuple<int, int>>());
    }

    public MoveCalculatorSinglePiece(Piece[,] board, Color playerInTurn, int row, int column)
    {
        _board = board;
        _playerInTurn = playerInTurn;
        _row = row;
        _column = column;
    }

    public IEnumerator<int[]> GetEnumerator()
    {
        foreach (var offseTuple in _offsetsDictionary[_board[_row, _column].rank])
        {
            int moveToRow = _row;
            int moveToColumn = _column;

            while (true)
            {
                moveToRow = moveToRow + offseTuple.Item1;
                moveToColumn = moveToColumn + offseTuple.Item2;

                //Outside Board
                if (moveToRow < 0 || moveToRow > 3 || moveToColumn < 0 || moveToColumn > 3)
                {
                    break;
                }

                //Own piece
                if (_board[moveToRow, moveToColumn].color == _playerInTurn)
                {
                    break;
                }

                yield return new int[] { _row, _column, moveToRow, moveToColumn };
                
                //Opponents piece
                if (_board[moveToRow, moveToColumn].color != _playerInTurn && _board[moveToRow, moveToColumn].color != Color.None)
                {
                    break;
                }

                //Knight move
                if (_board[moveToRow, moveToColumn].rank == Rank.Knight)
                {
                    break;
                }


            }

        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

enum PrintLevel
{
    None,
    Selected,
    All
}
