﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using CSharpChess.ValidMoves;

namespace CSharpChess.TheBoard
{
    public class ChessBoard
    {
        private readonly BoardPiece[,] _boardPieces = new BoardPiece[9,9];

        public Chess.Colours ToPlay { get; private set; } = Chess.Colours.None;

        public ChessBoard(bool newGame)
        {
            if (newGame)
            {
                NewBoard();
                ToPlay = Chess.Colours.White;
            }
            else
                EmptyBoard();
        }

        public ChessBoard(IEnumerable<BoardPiece> pieces, Chess.Colours toPlay)
        {
            EmptyBoard();
            foreach (var boardPiece in pieces)
            {
                _boardPieces[(int) boardPiece.Location.File, boardPiece.Location.Rank] = boardPiece;
            }
            ToPlay = toPlay;
        }

        private void NewBoard()
        {
            _boardPieces[(int)Chess.ChessFile.A, 8] = new BoardPiece(1, 8, Chess.Pieces.Black.Rook);
            _boardPieces[(int)Chess.ChessFile.B, 8] = new BoardPiece(2, 8, Chess.Pieces.Black.Knight);
            _boardPieces[(int)Chess.ChessFile.C, 8] = new BoardPiece(3, 8, Chess.Pieces.Black.Bishop);
            _boardPieces[(int)Chess.ChessFile.D, 8] = new BoardPiece(4, 8, Chess.Pieces.Black.Queen);
            _boardPieces[(int)Chess.ChessFile.E, 8] = new BoardPiece(5, 8, Chess.Pieces.Black.King);
            _boardPieces[(int)Chess.ChessFile.F, 8] = new BoardPiece(6, 8, Chess.Pieces.Black.Bishop);
            _boardPieces[(int)Chess.ChessFile.G, 8] = new BoardPiece(7, 8, Chess.Pieces.Black.Knight);
            _boardPieces[(int)Chess.ChessFile.H, 8] = new BoardPiece(8, 8, Chess.Pieces.Black.Rook);

            _boardPieces[(int)Chess.ChessFile.A, 7] = new BoardPiece(1, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.B, 7] = new BoardPiece(2, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.C, 7] = new BoardPiece(3, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.D, 7] = new BoardPiece(4, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.E, 7] = new BoardPiece(5, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.F, 7] = new BoardPiece(6, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.G, 7] = new BoardPiece(7, 7, Chess.Pieces.Black.Pawn);
            _boardPieces[(int)Chess.ChessFile.H, 7] = new BoardPiece(8, 7, Chess.Pieces.Black.Pawn);

            for (int rank = 3; rank < 7; rank++)
            {
                _boardPieces[(int)Chess.ChessFile.A, rank] = new BoardPiece(1, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.B, rank] = new BoardPiece(2, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.C, rank] = new BoardPiece(3, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.D, rank] = new BoardPiece(4, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.E, rank] = new BoardPiece(5, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.F, rank] = new BoardPiece(6, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.G, rank] = new BoardPiece(7, rank, ChessPiece.NullPiece);
                _boardPieces[(int)Chess.ChessFile.H, rank] = new BoardPiece(8, rank, ChessPiece.NullPiece);
            }

            _boardPieces[(int)Chess.ChessFile.A, 2] = new BoardPiece(1, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.B, 2] = new BoardPiece(2, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.C, 2] = new BoardPiece(3, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.D, 2] = new BoardPiece(4, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.E, 2] = new BoardPiece(5, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.F, 2] = new BoardPiece(6, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.G, 2] = new BoardPiece(7, 2, Chess.Pieces.White.Pawn);
            _boardPieces[(int)Chess.ChessFile.H, 2] = new BoardPiece(8, 2, Chess.Pieces.White.Pawn);

            _boardPieces[(int) Chess.ChessFile.A, 1] = new BoardPiece(1,1, Chess.Pieces.White.Rook);
            _boardPieces[(int) Chess.ChessFile.B, 1] = new BoardPiece(2,1, Chess.Pieces.White.Knight);
            _boardPieces[(int) Chess.ChessFile.C, 1] = new BoardPiece(3,1, Chess.Pieces.White.Bishop);
            _boardPieces[(int) Chess.ChessFile.D, 1] = new BoardPiece(4,1, Chess.Pieces.White.Queen);
            _boardPieces[(int) Chess.ChessFile.E, 1] = new BoardPiece(5,1, Chess.Pieces.White.King);
            _boardPieces[(int) Chess.ChessFile.F, 1] = new BoardPiece(6,1, Chess.Pieces.White.Bishop);
            _boardPieces[(int) Chess.ChessFile.G, 1] = new BoardPiece(7,1, Chess.Pieces.White.Knight);
            _boardPieces[(int) Chess.ChessFile.H, 1] = new BoardPiece(8,1, Chess.Pieces.White.Rook);
        }

        private void EmptyBoard()
        {
            foreach (var rank in Chess.Ranks)
            {
                foreach (var file in Chess.Files)
                {
                    if (file != 0 && rank != 0)
                        this[file, rank] = new BoardPiece(file, rank, Chess.Pieces.Blank);
                    else
                    {
                        this[file, rank] = null;
                    }
                }
            }
        }

        public IEnumerable<BoardPiece> Pieces {
            get
            {
                foreach (var file in Chess.Files)
                {
                    foreach (var rank in Chess.Ranks)
                    {
                        yield return this[file, rank];
                    }
                }
            }
        }

        public IEnumerable<BoardRank> Ranks
        {
            get
            {
                foreach (var rank in Chess.Ranks)
                {
                    yield return new BoardRank(rank, Rank((int)rank).ToArray());
                }
            }
        }

        public IEnumerable<BoardPiece> Rank(int rank)
        {
            Chess.Validations.ThrowInvalidRank(rank);
            foreach (var file in Chess.Files)
            {
                yield return this[file, rank];
            }
        }

        public IEnumerable<BoardFile> Files
        {
            get
            {
                foreach (var file in Chess.Files)
                {
                    yield return new BoardFile(file, File(file).ToArray());
                }
            }
        }

        public IEnumerable<BoardPiece> File(Chess.ChessFile file)
        {
            Chess.Validations.ThrowInvalidFile(file);
            foreach (var rank in Chess.Ranks)
            {
                yield return this[file, rank];
            }
        }

        public BoardPiece this[Chess.ChessFile file, int rank]
        {
            get { return GetPiece(file, rank); }
            private set { this[(int) file, rank] = value; }
        }

        public BoardPiece this[int file, int rank]
        {
            get { return GetPiece((Chess.ChessFile) file, rank); }
            private set { _boardPieces[file, rank] = value; }
        }

        public BoardPiece this[BoardLocation location]
        {
            get { return GetPiece(location); }
            private set { this[location.File, location.Rank] = value; }
        }

        public bool IsEmptyAt(BoardLocation location) => this[location].Piece.Equals(ChessPiece.NullPiece);

        public bool IsNotEmptyAt(BoardLocation location) => !IsEmptyAt(location);

        public MoveResult Move(ChessMove move)
        {
            BoardPiece from = this[move.From];
            var to = this[move.To];

            if (from.Piece.Colour != ToPlay) // This is also caters from empty from square
                return MoveResult.IncorrectPlayer(move);

            var validMovesForPiece = GetValidMoves(this, move.From);

            if (validMovesForPiece.Any(vm => vm.Equals(move)))
            {
                if (IsEmptyAt(move.To))
                {
                    from.MoveTo(move.To);
                    this[move.To] = from;
                    _boardPieces[(int) move.From.File, move.From.Rank] = BoardPiece.Empty(from.Location);
                    return MoveResult.Success(move);
                }

            }

            throw new InvalidOperationException("Move() still a WIP");
        }

        private IEnumerable<ChessMove> GetValidMoves(ChessBoard board, BoardLocation at)
        {
            var pieceName = board[at].Piece.Name;
            if(pieceName != Chess.PieceNames.Pawn) throw new NotImplementedException($"ValidMoveGenerator for {pieceName} not yet implemented.");
            return new PawnValidMoveGenerator().For(board, at);
        }

        private BoardPiece GetPiece(Chess.ChessFile file, int rank)
        {
            Chess.Validations.ThrowInvalidRank(rank);
            Chess.Validations.ThrowInvalidFile(file);
            return _boardPieces[(int)file, rank];
        }

        private BoardPiece GetPiece(BoardLocation location) => GetPiece(location.File, location.Rank);
    }

    public class MoveResult
    {
        public bool Succeeded { get; }
        public MoveType MoveType { get; }
        public ChessMove Move { get; set; }

        public MoveResult(bool success, MoveType moveType, ChessMove move)
        {
            Succeeded = success;
            MoveType = moveType;
            Move = move;
        }

        public static MoveResult IncorrectPlayer(ChessMove move)
        {
            return new MoveResult(false, TheBoard.MoveType.Move, move);
        }

        public static MoveResult Success(ChessMove move)
        {
            return new MoveResult(true, TheBoard.MoveType.Move, move);
        }
    }

    public enum MoveType
    {
        Move, Take, TakeEnPassant, Castle, Check, Checkmate
    }
}