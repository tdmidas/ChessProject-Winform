using ChessSharp.Pieces;
using ChessSharp.SquareData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessSharp
{
    /// <summary>Represents the chess game.</summary>
    public class ChessGame : IDeepCloneable<ChessGame>
    {
        /// <summary>Gets <see cref="Piece"/> in a specific square.</summary>
        /// <param name="file">The <see cref="File"/> of the square.</param>
        /// <param name="rank">The <see cref="Rank"/> of the square.</param>
        public Piece? this[File file, Rank rank] => Board[(int)rank][(int)file];

        /// <summary>Gets a list of the game moves.</summary>
        public List<Move> Moves { get; private set; } // TODO: BAD! Investigate why the class consumer would even need this. Make it a private field if appropriate. And make it some kind of interface (`IEnumerable` for example).

        /// <summary>Gets a 2D array of <see cref="Piece"/>s in the board.</summary>
        public Piece?[][] Board { get; private set; } // TODO: It's bad idea to expose this to public.

        /// <summary>Gets the <see cref="Player"/> who has turn.</summary>
        public Player WhoseTurn { get; set; }

        /// <summary>Gets the current <see cref="ChessSharp.GameState"/>.</summary>
        public GameState GameState { get; private set; }


        internal bool CanWhiteCastleKingSide { get; set; } = true;
        internal bool CanWhiteCastleQueenSide { get; set; } = true;
        internal bool CanBlackCastleKingSide { get; set; } = true;
        internal bool CanBlackCastleQueenSide { get; set; } = true;

        /// <summary>Initializes a new instance of <see cref="ChessGame"/>.</summary>
        public ChessGame()
        {
            Moves = new List<Move>();
            var whitePawn = new Pawn(Player.White);
            var whiteRook = new Rook(Player.White);
            var whiteKnight = new Knight(Player.White);
            var whiteBishop = new Bishop(Player.White);
            var whiteQueen = new Queen(Player.White);
            var whiteKing = new King(Player.White);

            var blackPawn = new Pawn(Player.Black);
            var blackRook = new Rook(Player.Black);
            var blackKnight = new Knight(Player.Black);
            var blackBishop = new Bishop(Player.Black);
            var blackQueen = new Queen(Player.Black);
            var blackKing = new King(Player.Black);
            Board = new Piece?[][]
            {
            new Piece?[] { whiteRook, whiteKnight, whiteBishop, whiteQueen, whiteKing, whiteBishop, whiteKnight, whiteRook },
            new Piece?[] { whitePawn, whitePawn, whitePawn, whitePawn, whitePawn, whitePawn, whitePawn, whitePawn },
            new Piece?[] { null, null, null, null, null, null, null, null },
            new Piece?[] { null, null, null, null, null, null, null, null },
            new Piece?[] { null, null, null, null, null, null, null, null },
            new Piece?[] { null, null, null, null, null, null, null, null },
            new Piece?[] { blackPawn, blackPawn, blackPawn, blackPawn, blackPawn, blackPawn, blackPawn, blackPawn},
            new Piece?[] { blackRook, blackKnight, blackBishop, blackQueen, blackKing, blackBishop, blackKnight, blackRook}
            };
        }

        /// <summary>Makes a move in the game.</summary>
        /// <param name="move">The <see cref="Move"/> you want to make.</param>
        /// <param name="isMoveValidated">Only pass true when you've already checked that the move is valid.</param>
        /// <returns>Returns true if the move is made; false otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        ///     The <c>move</c> is null.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     The <see cref="Move.Source"/> square of the <c>move</c> doesn't contain a piece.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///    The <c>move.PromoteTo</c> is null and the move is a pawn promotion move.
        /// </exception>
        public bool MakeMove(Move move, bool isMoveValidated)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            Piece? piece = this[move.Source.File, move.Source.Rank];
            if (piece == null)
            {
                throw new InvalidOperationException("Source square has no piece.");
            }

            if (!isMoveValidated && !IsValidMove(move))
            {
                return false;
            }

            SetCastleStatus(move, piece);

            if (piece is King && move.GetAbsDeltaX() == 2)
            {
                // Queen-side castle
                if (move.Destination.File == File.C)
                {
                    var rook = this[File.A, move.Source.Rank];
                    Board[(int)move.Source.Rank][(int)File.A] = null;
                    Board[(int)move.Source.Rank][(int)File.D] = rook;
                }

                // King-side castle
                if (move.Destination.File == File.G)
                {
                    var rook = this[File.H, move.Source.Rank];
                    Board[(int)move.Source.Rank][(int)File.H] = null;
                    Board[(int)move.Source.Rank][(int)File.F] = rook;
                }
            }

            if (piece is Pawn)
            {
                if ((move.Player == Player.White && move.Destination.Rank == Rank.Eighth) ||
                    (move.Player == Player.Black && move.Destination.Rank == Rank.First))
                {
                    piece = move.PromoteTo switch
                    {
                        PawnPromotion.Knight => new Knight(piece.Owner),
                        PawnPromotion.Bishop => new Bishop(piece.Owner),
                        PawnPromotion.Rook => new Rook(piece.Owner),
                        PawnPromotion.Queen => new Queen(piece.Owner),
                        _ => throw new ArgumentException($"A promotion move should have a valid {move.PromoteTo} property.", nameof(move)),
                    };
                }
                // Enpassant
                if (Pawn.GetPawnMoveType(move) == PawnMoveType.Capture &&
                    this[move.Destination.File, move.Destination.Rank] == null)
                {
                    Board[(int)Moves.Last().Destination.Rank][(int)Moves.Last().Destination.File] = null;
                }

            }
            Board[(int)move.Source.Rank][(int)move.Source.File] = null;
            Board[(int)move.Destination.Rank][(int)move.Destination.File] = piece;
            Moves.Add(move);
            WhoseTurn = ChessUtilities.RevertPlayer(move.Player);
            SetGameState();
            return true;
        }

        private void SetCastleStatus(Move move, Piece piece)
        {
            if (piece.Owner == Player.White && piece is King)
            {
                CanWhiteCastleKingSide = false;
                CanWhiteCastleQueenSide = false;
            }

            if (piece.Owner == Player.White && piece is Rook &&
                move.Source.File == File.A && move.Source.Rank == Rank.First)
            {
                CanWhiteCastleQueenSide = false;
            }

            if (piece.Owner == Player.White && piece is Rook &&
                move.Source.File == File.H && move.Source.Rank == Rank.First)
            {
                CanWhiteCastleKingSide = false;
            }

            if (piece.Owner == Player.Black && piece is King)
            {
                CanBlackCastleKingSide = false;
                CanBlackCastleQueenSide = false;
            }

            if (piece.Owner == Player.Black && piece is Rook &&
                move.Source.File == File.A && move.Source.Rank == Rank.Eighth)
            {
                CanBlackCastleQueenSide = false;
            }

            if (piece.Owner == Player.Black && piece is Rook &&
                move.Source.File == File.H && move.Source.Rank == Rank.Eighth)
            {
                CanBlackCastleKingSide = false;
            }
        }

        /// <summary>Checks if a given move is valid or not.</summary>
        /// <param name="move">The <see cref="Move"/> to check its validity.</param>
        /// <returns>Returns true if the given <c>move</c> is valid; false otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        ///     The given <c>move</c> is null.
        /// </exception>
        public bool IsValidMove(Move move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            Piece? pieceSource = this[move.Source.File, move.Source.Rank];
            Piece? pieceDestination = this[move.Destination.File, move.Destination.Rank];
            return (WhoseTurn == move.Player && pieceSource != null && pieceSource.Owner == move.Player &&
                    !Equals(move.Source, move.Destination) &&
                    (pieceDestination == null || pieceDestination.Owner != move.Player) &&
                    !PlayerWillBeInCheck(move) && pieceSource.IsValidGameMove(move, this));
        }

        internal bool PlayerWillBeInCheck(Move move)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            ChessGame clone = DeepClone(); // Make the move on this board to keep original board as is.
            Piece? piece = clone[move.Source.File, move.Source.Rank]; // TODO: throwing causes un-intended behavior. ?? throw new ArgumentException("Invalid move", nameof(move));
            clone.Board[(int)move.Source.Rank][(int)move.Source.File] = null;
            clone.Board[(int)move.Destination.Rank][(int)move.Destination.File] = piece;

            return ChessUtilities.IsPlayerInCheck(move.Player, clone);
        }

        internal void SetGameState()
        {
            Player opponent = WhoseTurn;
            Player lastPlayer = ChessUtilities.RevertPlayer(opponent);
            bool isInCheck = ChessUtilities.IsPlayerInCheck(opponent, this);
            var hasValidMoves = ChessUtilities.GetValidMoves(this).Count > 0;

            if (isInCheck && !hasValidMoves)
            {
                GameState = lastPlayer == Player.White ? GameState.WhiteWinner : GameState.BlackWinner;
                return;
            }

            if (!hasValidMoves)
            {
                GameState = GameState.Stalemate;
                return;
            }

            if (isInCheck)
            {
                GameState = opponent == Player.White ? GameState.WhiteInCheck : GameState.BlackInCheck;
                return;
            }
            GameState = IsInsufficientMaterial() ? GameState.Draw : GameState.NotCompleted;
        }

        internal bool IsInsufficientMaterial() // TODO: Much allocations seem to happen here? (LINQ)
        {
            IEnumerable<Piece?> pieces = Board.SelectMany(x => x); // https://stackoverflow.com/questions/32588070/flatten-jagged-array-in-c-sharp

            var whitePieces = pieces.Select((p, i) => new { Piece = p, SquareColor = (i % 8 + i / 8) % 2 })
                .Where(p => p.Piece?.Owner == Player.White).ToArray();

            var blackPieces = pieces.Select((p, i) => new { Piece = p, SquareColor = (i % 8 + i / 8) % 2 })
                .Where(p => p.Piece?.Owner == Player.Black).ToArray();

            switch (whitePieces.Length)
            {
                // King vs King
                case 1 when blackPieces.Length == 1:
                // White King vs black king and (Bishop|Knight)
                case 1 when blackPieces.Length == 2 && blackPieces.Any(p => p.Piece is Bishop ||
                                                                            p.Piece is Knight):
                // Black King vs white king and (Bishop|Knight)
                case 2 when blackPieces.Length == 1 && whitePieces.Any(p => p.Piece is Bishop ||
                                                                            p.Piece is Knight):
                    return true;
                // King and bishop vs king and bishop
                case 2 when blackPieces.Length == 2:
                    {
                        var whiteBishop = whitePieces.First(p => p.Piece is Bishop);
                        var blackBishop = blackPieces.First(p => p.Piece is Bishop);
                        return whiteBishop != null && blackBishop != null &&
                               whiteBishop.SquareColor == blackBishop.SquareColor;
                    }
                default:
                    return false;
            }
        }
        public string GetFen()
        {
            StringBuilder fen = new StringBuilder();

            // Piece placement
            for (int rank = 7; rank >= 0; rank--)
            {
                int emptySquares = 0;
                for (int file = 0; file < 8; file++)
                {
                    var piece = this[(File)file, (Rank)rank];
                    if (piece == null)
                    {
                        emptySquares++;
                    }
                    else
                    {
                        if (emptySquares > 0)
                        {
                            fen.Append(emptySquares);
                            emptySquares = 0;
                        }
                        char pieceChar = GetFenCharacter(piece);
                        fen.Append(pieceChar);
                    }
                }
                if (emptySquares > 0)
                {
                    fen.Append(emptySquares);
                }
                if (rank > 0)
                {
                    fen.Append('/');
                }
            }

            // Active color
            fen.Append(' ');
            fen.Append(WhoseTurn == Player.White ? 'w' : 'b');

            // Castling availability
            fen.Append(' ');
            bool hasCastlingRights = false;
            if (CanWhiteCastleKingSide)
            {
                fen.Append('K');
                hasCastlingRights = true;
            }
            if (CanWhiteCastleQueenSide)
            {
                fen.Append('Q');
                hasCastlingRights = true;
            }
            if (CanBlackCastleKingSide)
            {
                fen.Append('k');
                hasCastlingRights = true;
            }
            if (CanBlackCastleQueenSide)
            {
                fen.Append('q');
                hasCastlingRights = true;
            }
            if (!hasCastlingRights)
            {
                fen.Append('-');
            }

            // En passant target square
            fen.Append(' ');
            fen.Append('-'); // For simplicity, assuming no en passant target square

            // Halfmove clock and fullmove number
            fen.Append(' ');
            fen.Append(0); // For simplicity, assuming 0 halfmoves
            fen.Append(' ');
            fen.Append(Moves.Count / 2 + 1);

            return fen.ToString();
        }

        private char GetFenCharacter(Piece piece)
        {
            char pieceChar = piece.GetType().Name[0]; // Use the first letter of the piece type
            if (piece.Owner == Player.Black)
            {
                pieceChar = char.ToLower(pieceChar); // Lowercase for black pieces
            }
            return pieceChar;
        }

        internal static bool IsValidMove(Move move, ChessGame board)
        {
            if (move == null)
            {
                throw new ArgumentNullException(nameof(move));
            }

            Piece? pieceSource = board[move.Source.File, move.Source.Rank];
            Piece? pieceDestination = board[move.Destination.File, move.Destination.Rank];

            return (pieceSource != null && pieceSource.Owner == move.Player &&
                    !Equals(move.Source, move.Destination) &&
                    (pieceDestination == null || pieceDestination.Owner != move.Player) &&
                    !board.PlayerWillBeInCheck(move) && pieceSource.IsValidGameMove(move, board));
        }

        internal bool IsTherePieceInBetween(Square square1, Square square2)
        {
            int xStep = Math.Sign(square2.File - square1.File);
            int yStep = Math.Sign(square2.Rank - square1.Rank);

            Rank rank = square1.Rank;
            File file = square1.File;
            while (true) // TODO: Prevent possible infinite loop (by throwing an exception) when passing un-logical squares (two squares not on same file, rank, or diagonal).
            {
                rank += yStep;
                file += xStep;
                if (rank == square2.Rank && file == square2.File)
                {
                    return false;
                }

                if (Board[(int)rank][(int)file] != null)
                {
                    return true;
                }
            }

        }
        public List<Move> MoveHistory()
        {
            return Moves.ToList();
        }

        public ChessGame DeepClone()
        {

            return new ChessGame
            {
                Board = new Piece?[][]
                {
                new Piece?[] { Board[0][0], Board[0][1], Board[0][2], Board[0][3], Board[0][4], Board[0][5], Board[0][6], Board[0][7] },
                new Piece?[] { Board[1][0], Board[1][1], Board[1][2], Board[1][3], Board[1][4], Board[1][5], Board[1][6], Board[1][7] },
                new Piece?[] { Board[2][0], Board[2][1], Board[2][2], Board[2][3], Board[2][4], Board[2][5], Board[2][6], Board[2][7] },
                new Piece?[] { Board[3][0], Board[3][1], Board[3][2], Board[3][3], Board[3][4], Board[3][5], Board[3][6], Board[3][7] },
                new Piece?[] { Board[4][0], Board[4][1], Board[4][2], Board[4][3], Board[4][4], Board[4][5], Board[4][6], Board[4][7] },
                new Piece?[] { Board[5][0], Board[5][1], Board[5][2], Board[5][3], Board[5][4], Board[5][5], Board[5][6], Board[5][7] },
                new Piece?[] { Board[6][0], Board[6][1], Board[6][2], Board[6][3], Board[6][4], Board[6][5], Board[6][6], Board[6][7] },
                new Piece?[] { Board[7][0], Board[7][1], Board[7][2], Board[7][3], Board[7][4], Board[7][5], Board[4][6], Board[7][7] },
                },
                Moves = Moves.Select(m => m.DeepClone()).ToList(),
                GameState = GameState,
                WhoseTurn = WhoseTurn,
                CanBlackCastleKingSide = CanBlackCastleKingSide,
                CanBlackCastleQueenSide = CanBlackCastleQueenSide,
                CanWhiteCastleKingSide = CanWhiteCastleKingSide,
                CanWhiteCastleQueenSide = CanWhiteCastleQueenSide
            };
        }
    }
}