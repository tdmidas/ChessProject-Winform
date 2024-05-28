﻿using ChessSharp.SquareData;
using System;
using System.Linq;

namespace ChessSharp.Pieces
{
    /// <summary>Represents a pawn <see cref="Piece"/>.</summary>
    public class Pawn : Piece
    {
        internal Pawn(Player player) : base(player) { }


        internal static PawnMoveType GetPawnMoveType(Move move)
        {
            // No need to do null check here, this method isn't public and isn't annotated with nullable.
            // If the caller try to pass a possible null reference, the compiler should issue a warning.
            // TODO: Should I add [NotNull] attribute to the argument? What's the benefit?
            // The argument is already non-nullable.

            int deltaY = move.GetDeltaY();
            int absDeltaX = move.GetAbsDeltaX();

            // Check normal one step pawn move.
            if ((move.Player == Player.White && deltaY == 1 && absDeltaX == 0) ||
                move.Player == Player.Black && deltaY == -1 && absDeltaX == 0)
            {
                if ((move.Player == Player.White && move.Destination.Rank == Rank.Eighth) ||
                    (move.Player == Player.Black && move.Destination.Rank == Rank.First))
                {
                    return PawnMoveType.OneStep | PawnMoveType.Promotion;
                }
                return PawnMoveType.OneStep;
            }

            // Check two step move from starting position.
            if ((move.Player == Player.White && deltaY == 2 && absDeltaX == 0 && move.Source.Rank == Rank.Second) ||
                (move.Player == Player.Black && deltaY == -2 && absDeltaX == 0 && move.Source.Rank == Rank.Seventh))
            {
                return PawnMoveType.TwoSteps;
            }

            // Check capture (Enpassant is special case from capture).
            if ((move.Player == Player.White && deltaY == 1 && absDeltaX == 1) ||
                (move.Player == Player.Black && deltaY == -1 && absDeltaX == 1))
            {
                if ((move.Player == Player.White && move.Destination.Rank == Rank.Eighth) ||
                    (move.Player == Player.Black && move.Destination.Rank == Rank.First))
                {
                    return PawnMoveType.Capture | PawnMoveType.Promotion;
                }
                return PawnMoveType.Capture;
            }

            return PawnMoveType.Invalid;
        }

        internal override bool IsValidGameMove(Move move, ChessGame board)
        {
            // No need to do null checks here, this method isn't public and isn't annotated with nullable.
            // If the caller try to pass a possible null reference, the compiler should issue a warning.
            // TODO: Should I add [NotNull] attribute to the arguments? What's the benefit?
            // The arguments are already non-nullable.

            var moveType = GetPawnMoveType(move);

            if (moveType == PawnMoveType.Invalid)
            {
                return false;
            }

            if (moveType.Contains(PawnMoveType.Promotion) && move.PromoteTo == null)
            {
                return false;
            }

            if (moveType.Contains(PawnMoveType.TwoSteps))
            {
                return !board.IsTherePieceInBetween(move.Source, move.Destination) && board[move.Destination.File, move.Destination.Rank] == null;
            }

            if (moveType.Contains(PawnMoveType.OneStep))
            {
                return board[move.Destination.File, move.Destination.Rank] == null;
            }

            if (moveType.Contains(PawnMoveType.Capture))
            {
                // Capture isn't possible as first move.
                // This prevents exception when getting board.Moves.Last() later.
                if (board.Moves.Count == 0)
                {
                    return false;
                }
                // Check regular capture.
                if (board[move.Destination.File, move.Destination.Rank] != null)
                {
                    return true;
                }

                // Check enpassant.
                Move lastMove = board.Moves.Last();
                Piece? lastMovedPiece = board[lastMove.Destination.File, lastMove.Destination.Rank];

                if (lastMovedPiece is Pawn ||
                    !GetPawnMoveType(lastMove).Contains(PawnMoveType.TwoSteps) || lastMove.Destination.File != move.Destination.File ||
                    lastMove.Destination.Rank != move.Source.Rank)
                {
                    return false;
                }
                // Two Step pawn move ( white from rank 2 to 4 ) ( black from rank 7 to 5 )
                // SHOULDN'T REMOVE CAPTURED PAWN FROM THE BOARD HERE!! THIS IS ONLY FOR CHECKING IF MOVE IS LEGAL OR NOT!!
                // PAWN REMOVAL SHOULD BE DONE IN MAKEMOVE METHOD!!!
                ChessGame clone = board.DeepClone();
                clone.Board[(int)move.Destination.Rank][(int)move.Destination.File] = null;
                clone.Board[((int)move.Destination.Rank + (int)move.Source.Rank) / 2][(int)move.Destination.File] = lastMovedPiece;
                return !clone.PlayerWillBeInCheck(move);
            }

            throw new Exception("Unexpected PawnMoveType."); // Should never happen. If it happened, this it's a bug.


        }
    }
}