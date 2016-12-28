﻿using System;
using ConsoleStuff;
using ConsoleStuff.Panels;
using ConsoleStuff.Tests;
using ConsoleStuff.Tests.Commands;
using CSharpChess.System.Extensions;
using CSharpChess.TheBoard;

namespace CsChess
{
    class Program
    {
        private static readonly ConsoleCellColour ErrorTextColour = new ConsoleCellColour(ConsoleColor.White, ConsoleColor.Red);
        private const int ScreenWidth = 100;
        private static bool _debug;
        private static bool _exiting;

        public Program()
        {
        }

        static void Main(string[] args)
        {
            var board = new ChessBoard();

            MoveResult moveResult = null;
            bool first = true;

            Action<ConsolePanel> initialiseConsoleWindow = (screen) =>
            {
                if (Console.WindowWidth < screen.Width) Console.WindowWidth = screen.Width;
                if (Console.WindowHeight < screen.Height) Console.WindowHeight = screen.Height + 1;
                Console.CursorTop = 0;
                first = false;
            };

            var options = new Options();

            var commandMenu = BuildMenu(options);
            while (!_exiting)
            {
                var screen = DrawScreen(board, commandMenu, options, moveResult);
                if (first) initialiseConsoleWindow(screen);

                Console.CursorTop = screen.Height - 1;

                var cmd = GetCommand(board);

                moveResult = null;
                try
                {
                    if (string.IsNullOrEmpty(cmd))
                    {
                        moveResult = MoveResult.Success(ChessMove.Null);
                    }
                    else if (!commandMenu.Execute(cmd))
                    {
                        try
                        {
                            moveResult = board.Move(cmd);
                        }
                        catch (Exception e)
                        {
                            moveResult = InvalidMoveOrCommand(cmd, e);
                        }
                    }
                }
                catch (Exception e)
                {
                    moveResult = InvalidMoveOrCommand(cmd, e);
                }
            }

            Console.WriteLine("Exiting...");
            Environment.Exit(0);
        }

        private static CommandMenu BuildMenu(Options options)
        {
            CommandMenu menu = new CommandMenuBuilder()
                .WithItem("debug", (cargs) => _debug = !_debug, $"Set debug = {!_debug}", visible:false)
                .WithItem("quit", (s) => _exiting = true) // TODO: Move to an alias approach
                .WithItem("exit", (s) => _exiting = true) // TODO: Move to an alias approach5
                .WithItem("colour", (s) => options.ColouredSquares = !options.ColouredSquares, "Toggle coloured board")
                .WithItem("coords", (s) => options.ShowRanksAndFiles = !options.ShowRanksAndFiles, "Toggle show ranks and files")
                .WithItem("size", (s) => options.Size = (BoardSize)Enum.Parse(typeof(BoardSize), s.ToLower()), "small | medium | large")
                .Build();
            return menu;
        }

        private static MoveResult InvalidMoveOrCommand(string cmd, Exception e)
        {
            var debugInfo = _debug
                ? e.StackTrace
                : "";
            return MoveResult.Failure($"Invalid Move or Command: {cmd}\n" + e.Message + debugInfo, ChessMove.Null);
        }

        private static string GetCommand(ChessBoard board)
        {
            if (board.GameOver())
            {
                Console.WriteLine($"Game over: {board.GameState}");
                Console.ReadLine();
                return "quit";
            }
            Console.Write($"{board.WhoseTurn} to play: ");
            var cmd = Console.ReadLine();
            return cmd;
        }
        private static ConsolePanel DrawScreen(ChessBoard board, CommandMenu commandMenu, Options options, MoveResult moveResult)
        {
            Console.Clear();
            var boardPanel = new MediumConsoleBoard(board).Build(options);
            var screen = new ConsolePanel(ScreenWidth, boardPanel.Height);

            var y = AddTitlePanel(screen, boardPanel.Width);
            AddErrorPanel(screen, moveResult, boardPanel.Width+ 3, y + 2);
            AddMenuPanel(screen, commandMenu, boardPanel.Width + 3, boardPanel.Height, options);

            // Check current view states to show
            //  Movelist
            //  About

            screen.PrintAt(1, 1, boardPanel);
            
            if (options.ColouredSquares)
            {
                screen.ToColouredConsole()();
            }
            else
            {
                Console.WriteLine(screen.ToString());
            }
            Console.CursorLeft = boardPanel.Width + 2;

            return screen;
        }

        private static int AddTitlePanel(ConsolePanel screen, int boardPanelWidth)
        {
            var y = 1;
            var x = (ScreenWidth - boardPanelWidth + 2) / 3;
            screen.PrintAt(boardPanelWidth +2 + x, y, "---=== CsChess V0.1 ===---");
            return y;
        }

        private static void AddMenuPanel(ConsolePanel screen, CommandMenu commandMenu, int x, int yOffset, Options options)
        {
            if (options.ShowMenu)
            {
                var menuPanel = new TextConsolePanel(commandMenu.HelpText());
                screen.PrintAt(x, yOffset - (menuPanel.Height + 1), menuPanel);
            }
        }

        private static void AddErrorPanel(ConsolePanel screen, MoveResult moveResult, int x, int y)
        {
            if (moveResult != null && !moveResult.Succeeded)
            {
                var errorPanel = CreateErrorPanel(moveResult.Message);
                if (errorPanel != null)
                {
                    screen.PrintAt(x, y, errorPanel);
                }
            }
        }

        private static ConsolePanel CreateErrorPanel(string error)
        {

            var textConsolePanel = new TextConsolePanel(error, 60, ErrorTextColour);
            var borderPanel = new ConsolePanel(textConsolePanel.Width + 4, textConsolePanel.Height + 4);
            borderPanel.Fill('*', ErrorTextColour);
            borderPanel.PrintAt(2, 1, "ERROR");
            borderPanel.PrintAt(3, 3, textConsolePanel);

            return borderPanel;
        }
    }
}
