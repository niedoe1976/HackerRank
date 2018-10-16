//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace SimplifiedChessEngine
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            char[][] white = new char[][]
//            {
//                new char[] {'N', 'B', '2'},
//                new char[] {'Q', 'B', '1'}
//            };

//            char[][] black = new char[][]
//            {
//                new char[] {'Q', 'A', '4'}
//            };

//            Console.WriteLine(simplifiedChessEngine(white, black, 1));

//            Console.ReadLine();
//        }

//        static string simplifiedChessEngine(char[][] whites, char[][] blacks, int moves)
//        {
//            Piece[,] board = new Piece[4, 4];

//            for (int i = 0; i < whites.GetLength(0); i++)
//            {
//                Rank rank;
//                int row;
//                int column;
//                GetPieceOnBoard(whites[i], out rank, out row, out column);
//                board[row, column].color = Color.White;
//                board[row, column].rank = rank;
//            }

//            for (int i = 0; i < blacks.GetLength(0); i++)
//            {
//                Rank rank;
//                int row;
//                int column;
//                GetPieceOnBoard(blacks[i], out rank, out row, out column);
//                board[row, column].color = Color.Black;
//                board[row, column].rank = rank;
//            }


//            return CheckWhiteForWinner(board, moves, Color.White) ? "YES" : "NO";

//        }

//        private static bool CheckWhiteForWinner(Piece[,] board, int numberOfWhiteMovesLeft, Color playerInTurn)
//        {
//            MoveCalculator mc = new MoveCalculator(board, playerInTurn);
//            bool resultSoFar;

//            if (playerInTurn == Color.White)
//            {
//                foreach (var move in mc)
//                {
//                    //Tjek om dronning bliver taget
//                    if (board[move[2], move[3]].rank == Rank.Queen)
//                    {
//                        return true;
//                    }

//                    Piece[,] boardWithMove = (Piece[,])board.Clone();

//                    Piece movingPiece = boardWithMove[move[0], move[1]];
//                    boardWithMove[move[2], move[3]] = movingPiece;
//                    boardWithMove[move[0], move[1]].rank = Rank.None;
//                    boardWithMove[move[0], move[1]].color = Color.None;

//                    //Console.WriteLine();
//                    //PrintBoard(boardWithMove);

//                    bool result = CheckWhiteForWinner(boardWithMove, numberOfWhiteMovesLeft - 1, Color.Black);
//                    if (result) return true;
//                }

//                return false;

//            }
//            else
//            {
//                if (numberOfWhiteMovesLeft == 0)
//                {
//                    return false;
//                }
//                foreach (var move in mc)
//                {
//                    //Tjek om dronning bliver taget
//                    if (board[move[2], move[3]].rank == Rank.Queen)
//                    {
//                        return false;
//                    }

//                    Piece[,] boardWithMove = (Piece[,])board.Clone();

//                    Piece movingPiece = boardWithMove[move[0], move[1]];
//                    boardWithMove[move[2], move[3]] = movingPiece;
//                    boardWithMove[move[0], move[1]].rank = Rank.None;
//                    boardWithMove[move[0], move[1]].color = Color.None;

//                    Console.WriteLine();
//                    PrintBoard(boardWithMove);

//                    bool result = CheckWhiteForWinner(boardWithMove, numberOfWhiteMovesLeft, Color.White);
//                    if (!result) return false;
//                }

//                return true;
//            }
//        }




//        private static void GetPieceOnBoard(char[] conf, out Rank rank, out int row, out int column)
//        {
//            switch (conf[0])
//            {
//                case 'Q':
//                    rank = Rank.Queen;
//                    break;
//                case 'N':
//                    rank = Rank.Knight;
//                    break;
//                case 'B':
//                    rank = Rank.Bishop;
//                    break;
//                case 'R':
//                    rank = Rank.Rook;
//                    break;
//                default:
//                    rank = Rank.None;
//                    break;
//            }

//            row = int.Parse(conf[2].ToString()) - 1;

//            switch (conf[1])
//            {
//                case 'A':
//                    column = 0;
//                    break;
//                case 'B':
//                    column = 1;
//                    break;
//                case 'C':
//                    column = 2;
//                    break;
//                case 'D':
//                    column = 3;
//                    break;
//                default:
//                    column = -1;
//                    break;
//            }
//        }


//        private static void PrintBoard(Piece[,] Board)
//        {
//            Console.WriteLine("  0  1  2  3");
//            for (int i = 0; i < Board.GetLength(0); i++)
//            {
//                Console.Write(i + " ");
//                for (int j = 0; j < Board.GetLength(1); j++)
//                {
//                    Console.Write(Board[i, j] + " ");
//                }

//                Console.WriteLine();
//            }
//        }



//    }

//    enum Color
//    {
//        None = 0,
//        White = 87,
//        Black = 66

//    }

//    enum Rank
//    {
//        None = 0,
//        Queen = 81,
//        Rook = 82,
//        Bishop = 66,
//        Knight = 78
//    }

//    struct Piece
//    {
//        public Color color;
//        public Rank rank;

//        public override string ToString()
//        {
//            if (color == Color.None && rank == Rank.None)
//            {
//                return "--";

//            }
//            return "" + (char)color + (char)rank;
//        }
//    }

//    class MoveCalculator : IEnumerable<int[]>
//    {
//        private readonly Piece[,] _board;
//        private readonly Color _playerInTurn;

//        public MoveCalculator(Piece[,] board, Color playerInTurn)
//        {
//            _board = board;
//            _playerInTurn = playerInTurn;
//        }

//        public IEnumerator<int[]> GetEnumerator()
//        {
//            for (int i = 0; i < _board.GetLength(0); i++)
//            {
//                for (int j = 0; j < _board.GetLength(1); j++)
//                {
//                    if (_board[i, j].color == _playerInTurn)
//                    {
//                        MoveCalculatorSinglePiece moveCalculatorSinglePiece = new MoveCalculatorSinglePiece(_board, _playerInTurn, i, j);
//                        foreach (var move in moveCalculatorSinglePiece)
//                        {
//                            yield return move;
//                        }
//                    }
//                }
//            }
//        }


//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
//    }

//    class MoveCalculatorSinglePiece : IEnumerable<int[]>
//    {
//        private readonly Piece[,] _board;
//        private readonly Color _playerInTurn;
//        private readonly int _row;
//        private readonly int _column;
//        private static Dictionary<Rank, List<Tuple<int, int>>> _offsetsDictionary;

//        static MoveCalculatorSinglePiece()
//        {
//            _offsetsDictionary = new Dictionary<Rank, List<Tuple<int, int>>>();
//            _offsetsDictionary.Add(Rank.Knight, new List<Tuple<int, int>>());
//            _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(1, 2));
//            _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(1, -2));
//            _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(-1, 2));
//            _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(-1, -2));
//            _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(2, 1));
//            _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(2, -1));
//            _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(-2, 1));
//            _offsetsDictionary[Rank.Knight].Add(new Tuple<int, int>(-2, -1));

//            _offsetsDictionary.Add(Rank.Bishop, new List<Tuple<int, int>>());
//            _offsetsDictionary[Rank.Bishop].Add(new Tuple<int, int>(1, 1));
//            _offsetsDictionary[Rank.Bishop].Add(new Tuple<int, int>(1, -1));
//            _offsetsDictionary[Rank.Bishop].Add(new Tuple<int, int>(-1, 1));
//            _offsetsDictionary[Rank.Bishop].Add(new Tuple<int, int>(-1, -1));

//            _offsetsDictionary.Add(Rank.Rook, new List<Tuple<int, int>>());
//            _offsetsDictionary[Rank.Rook].Add(new Tuple<int, int>(1, 0));
//            _offsetsDictionary[Rank.Rook].Add(new Tuple<int, int>(-1, 0));
//            _offsetsDictionary[Rank.Rook].Add(new Tuple<int, int>(0, 1));
//            _offsetsDictionary[Rank.Rook].Add(new Tuple<int, int>(0, -1));

//            _offsetsDictionary.Add(Rank.Queen, new List<Tuple<int, int>>());
//            _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(1, 1));
//            _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(1, -1));
//            _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(-1, 1));
//            _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(-1, -1));
//            _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(1, 0));
//            _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(-1, 0));
//            _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(0, 1));
//            _offsetsDictionary[Rank.Queen].Add(new Tuple<int, int>(0, -1));

//            _offsetsDictionary.Add(Rank.None, new List<Tuple<int, int>>());
//        }

//        public MoveCalculatorSinglePiece(Piece[,] board, Color playerInTurn, int row, int column)
//        {
//            _board = board;
//            _playerInTurn = playerInTurn;
//            _row = row;
//            _column = column;

//        }

//        public IEnumerator<int[]> GetEnumerator()
//        {
//            foreach (var offseTuple in _offsetsDictionary[_board[_row, _column].rank])
//            {
//                int moveToRow = _row;
//                int moveToColumn = _column;

//                while (true)
//                {
//                    moveToRow = moveToRow + offseTuple.Item1;
//                    moveToColumn = moveToColumn + offseTuple.Item2;

//                    //Outside Board
//                    if (moveToRow < 0 || moveToRow > 3 || moveToColumn < 0 || moveToColumn > 3)
//                    {
//                        break;
//                    }

//                    //Own piece
//                    if (_board[moveToRow, moveToColumn].color == _playerInTurn)
//                    {
//                        break;
//                    }

//                    //Opponents piece
//                    if (_board[moveToRow, moveToColumn].color != _playerInTurn && _board[moveToRow, moveToColumn].color != Color.None)
//                    {
//                        yield return new int[] { _row, _column, moveToRow, moveToColumn };
//                        break;
//                    }

//                    //Knight move
//                    if (_board[moveToRow, moveToColumn].rank == Rank.Knight)
//                    {
//                        yield return new int[] { _row, _column, moveToRow, moveToColumn };
//                        break;
//                    }

//                    yield return new int[] { _row, _column, moveToRow, moveToColumn };

//                }

//            }
//        }

//        IEnumerator IEnumerable.GetEnumerator()
//        {
//            return GetEnumerator();
//        }
//    }
//}
