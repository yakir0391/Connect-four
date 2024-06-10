using System;
using System.Diagnostics;
using System.Security.AccessControl;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace EX02
{
    internal class GameUI
    {
        private GameBoard m_Board;
        private Player[] m_PlayersArr;
        private GameLogic m_Logic;
        private readonly bool r_IsAgainstComputer;
        public const int MaxBoardSize = 8;
        public const int MinBoardSize = 4;

        public GameUI()
        {
            string nameForPlayer2;
            int height = 0, width = 0;

            WelcomePlayers();
            SetupGameBoardDimensions(ref height , ref width);
            m_Board = new GameBoard(height, width);
            r_IsAgainstComputer = IsAgainstComputer();
            nameForPlayer2 = (r_IsAgainstComputer ? "Computer" : "Player 2");
            Player m_Player1 = new Player('X', "Player 1", false);
            Player m_Player2 = new Player('O', nameForPlayer2, r_IsAgainstComputer);
            m_PlayersArr = new Player[] { m_Player1, m_Player2 };
            m_Logic = new GameLogic();
        }

        public void StartGame()
        {
            while (true)
            {
                if (Play())
                {
                    Ex02.ConsoleUtils.Screen.Clear();
                    m_Board.ClearGameBoard();
                    PrintBoard();
                    continue;
                }
                else
                {
                    DisplayStatus(m_PlayersArr);
                    EndGame();
                    break;
                }
            }
        }

        public bool Play()
        {
            int column, row = 0, currentPlayerIndex = 0;
            bool isQuitGame = false, isFoundOpenCell = false;

            Ex02.ConsoleUtils.Screen.Clear();
            PrintBoard();
            while (true)
            {
                if (m_PlayersArr[currentPlayerIndex].IsComputer)
                {
                    column = m_Logic.ComputerNextMove(m_PlayersArr[currentPlayerIndex] , m_Board , ref row);
                }
                else
                {
                    column = GetUserColumnOrQuit(m_Board, m_PlayersArr[currentPlayerIndex] , ref isQuitGame);

                    if (!isQuitGame)
                    {
                        row = m_Logic.FindEmptyCellAndDrop(m_Board, column, m_PlayersArr[currentPlayerIndex], ref isFoundOpenCell);
                        if (!isFoundOpenCell)
                        {
                            FullColumnMessage();
                            continue;
                        }
                    }
                    else
                    {
                        m_PlayersArr[(currentPlayerIndex + 1) % 2].Points++;
                        DisplayStatus(m_PlayersArr);
                        break;
                    }
                }

                Ex02.ConsoleUtils.Screen.Clear();
                PrintBoard();

                if (m_Logic.CheckForWinner(m_Board, row, column))
                {
                    m_PlayersArr[currentPlayerIndex].Points++;
                    DisplayWinner(m_PlayersArr[currentPlayerIndex]);
                    DisplayStatus(m_PlayersArr);
                    break;
                }
                else if (m_Board.IsBoardFull())
                {
                    DisplayTie();
                    DisplayStatus(m_PlayersArr);
                    break;
                }

                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }

            return ChooseToStartNewGame();
        }

        public bool ChooseToStartNewGame()
        {
            Console.WriteLine("\nPress Y to start new game or any other key to exit: ");
            string userAnswer = Console.ReadLine();

            return userAnswer.ToUpper() == "Y";
        }

        public void DisplayStatus(Player[] i_PlayerArr)
        {
            Console.WriteLine("Player 1 points : {0} . {1} points : {2}", i_PlayerArr[0].Points, i_PlayerArr[1].Name ,i_PlayerArr[1].Points);
        }

        public void DisplayWinner(Player i_Player)
        {
            Console.WriteLine("The winner of this round is {0}", i_Player.Name);

        }

        public void DisplayTie()
        {
            Console.WriteLine("It's a TIE!");
        }

        public void EndGame()
        {
            Console.WriteLine("Thank you for playing!");
        }

        public void WelcomePlayers()
        {
            Console.WriteLine("Hello! Welcome to Connect 4.");
            Console.WriteLine("============================");
        }

        public bool IsAgainstComputer()
        {
            bool isAgainstComputer = false;

            Console.WriteLine("\nPress 1 to play against the computer or 0 to play against another user:");
            while (true)
            {
                if (int.TryParse(Console.ReadLine(), out int selectedOption))
                {
                    if (selectedOption == 1 || selectedOption == 0)
                    {
                        isAgainstComputer = (selectedOption == 1);
                        break;
                    }
                }

                Console.WriteLine("Invalid input. Press 1 to play against the computer or 0 to play against another user:");
            }

            return isAgainstComputer;
        }

        public void SetupGameBoardDimensions(ref int io_Height, ref int io_Width)
        {
            io_Height = GetDimensionFromUser("height");
            io_Width = GetDimensionFromUser("width");
        }

        private int GetDimensionFromUser(string i_DimensionName)
        {
            int dimension;

            do
            {
                Console.Write($"Enter the {i_DimensionName} of the board (between {MinBoardSize} and {MaxBoardSize}): ");
                if (!int.TryParse(Console.ReadLine(), out dimension) || dimension < MinBoardSize || dimension > MaxBoardSize) 
                {
                    Console.WriteLine($"Invalid input. Please enter a valid number {MinBoardSize} between and {MaxBoardSize}.");
                }
            } while (dimension < MinBoardSize || dimension > MaxBoardSize);

            return dimension;
        }

        public int GetUserColumnOrQuit(GameBoard i_Board, Player i_Player , ref bool io_IsQuitGame)
        {
            Console.WriteLine($"{i_Player.Name} , enter the column where you want to make a move. To quit, press 'Q':");
            string userInput = Console.ReadLine();
            int userColumnChoice;

            while (true)
            {
                if (userInput.ToUpper() == "Q")
                {
                    io_IsQuitGame = true;
                    userColumnChoice = -1;
                    break;
                }
                if (!int.TryParse(userInput, out userColumnChoice) || userColumnChoice > i_Board.BoardWidth || userColumnChoice <= 0)
                {
                    Console.WriteLine("Please enter a valid number.");
                    userInput = Console.ReadLine();
                }
                else
                {
                    io_IsQuitGame = false;
                    break;
                }
            }

            userColumnChoice -= 1;

            return userColumnChoice;
        }

        public void PrintBoard()
        {
            StringBuilder board = new StringBuilder();

            board.Append("  ");

            for (int i = 1; i <= m_Board.BoardWidth; i++)
            {
                board.Append(i);
                board.Append("   ");
            }

            board.Append("\n");

            for (int i = 0; i < m_Board.BoardHeight; i++)
            {
                for (int j = 0; j < m_Board.BoardWidth; j++)
                {
                    if (m_Board.Board[i, j] == '\0')
                    {
                        board.Append("|   ");
                    }
                    else if (m_Board.Board[i, j] == m_PlayersArr[0].Token)
                    {
                        board.Append($"| {m_PlayersArr[0].Token} ");
                    }
                    else
                    {
                        board.Append($"| {m_PlayersArr[1].Token} ");
                    }
                }

                board.Append("|\n");

                for (int j = 0; j < m_Board.BoardWidth; j++)
                {
                    board.Append("====");
                }
                board.Append("=\n");
            }

            Console.WriteLine(board);
        }

        public void FullColumnMessage()
        {
            Console.WriteLine("Sorry, the selected column is already full.");
        }
    }
}