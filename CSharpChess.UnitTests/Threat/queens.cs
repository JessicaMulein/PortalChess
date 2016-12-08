﻿using CSharpChess.UnitTests.Helpers;
using NUnit.Framework;

namespace CSharpChess.UnitTests.Threat
{
    [TestFixture]
    public class queens : BoardAssertions
    {
        [Test]
        public void queens_generate_vertical_threat()
        {
            var customBoard = BoardBuilder.CustomBoard(NoPawnBoard, Chess.Colours.White);

            AssertPiecesGeneratesVerticalThreat(customBoard, Chess.PieceNames.Queen, BuildVerticalThreats);
        }
    }
}