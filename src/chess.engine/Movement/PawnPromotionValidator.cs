﻿using chess.engine.Board;
using chess.engine.Chess;
using chess.engine.Game;

namespace chess.engine.Movement
{
    public class PawnPromotionValidator : IMoveValidator
    {

        public bool ValidateMove(ChessMove move, BoardState boardState)
        {
            var piece = boardState.Entities[move.From];
            var moveOk = new DestinationIsEmptyValidator().ValidateMove(move, boardState);

            var destinationIsEndRank = move.To.Rank == ChessGame.EndRankFor(piece.Player);
            var destinationIsEmpty = boardState.GetEntityOrNull(move.To) == null;

            return moveOk && destinationIsEndRank && destinationIsEmpty;
        }


        private bool CheckPawnUsedDoubleMove(BoardLocation moveTo)
        {
            // ************************
            // TODO: Need to check move count/history to confirm that the pawn we passed did it's double move last turn
            // ************************
            return true;
        }
    }
}