using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EX02
{
    internal class GameLogic
    {
        public int FindEmptyCellAndDrop(GameBoard i_GameBoard, int i_ColumnChoice, Player i_Player, ref bool io_IsFoundOpenCell)
        {
            int row = -1;
            io_IsFoundOpenCell = false;

            for (int i = i_GameBoard.BoardHeight - 1; i >= 0; i--) 
            {
                if (i_GameBoard.Board[i, i_ColumnChoice] == '\0') 
                {
                    i_GameBoard.Board[i, i_ColumnChoice] = i_Player.Token;
                    row = i;
                    io_IsFoundOpenCell = true;
                    break;
                }
            }

            return row;
        }

        public int ComputerNextMove(Player i_Computer ,GameBoard i_GameBoard, ref int io_row)
        {
            Random random = new Random();
            int computerColumnChoice = -1;
            bool io_IsFoundOpenCell = false;

            do
            {
                computerColumnChoice = random.Next(1, i_GameBoard.BoardWidth + 1);
                io_row = FindEmptyCellAndDrop(i_GameBoard, computerColumnChoice - 1, i_Computer, ref io_IsFoundOpenCell);

            } while (!io_IsFoundOpenCell);

            computerColumnChoice -= 1;          

            return computerColumnChoice;
        }

        public bool CheckDiagonally(GameBoard i_GameBoard, int i_row, int i_col)
        {
            int count = 1;
            bool connect4 = false;
            int row = i_row + 1, col = i_col + 1;

            while (row < i_GameBoard.BoardHeight && col < i_GameBoard.BoardWidth &&
                   i_GameBoard.Board[i_row, i_col] == i_GameBoard.Board[row, col])
            {
                count++;
                row++;
                col++;
            }

            row = i_row - 1;
            col = i_col - 1;
            while (row >= 0 && col >= 0 && i_GameBoard.Board[i_row, i_col] == i_GameBoard.Board[row, col])
            {
                count++;
                row--;
                col--;
            }

            if (count >= 4)
            {
                connect4 = true;
            }

            count = 1;
            row = i_row + 1;
            col = i_col - 1;
            while (row < i_GameBoard.BoardHeight && col >= 0 &&
                   i_GameBoard.Board[i_row, i_col] == i_GameBoard.Board[row, col])
            {
                count++;
                row++;
                col--;
            }

            row = i_row - 1;
            col = i_col + 1;
            while (row >= 0 && col < i_GameBoard.BoardWidth &&
                   i_GameBoard.Board[i_row, i_col] == i_GameBoard.Board[row, col])
            {
                count++;
                row--;
                col++;
            }

            if (count >= 4)
            {
                connect4 = true;
            }

            return connect4;
        }

        public bool CheckHorizontally(GameBoard i_GameBoard, int i_row, int i_col)
        {
            int count = 1;
            int col = i_col + 1;

            while (col < i_GameBoard.BoardWidth && i_GameBoard.Board[i_row, i_col] == i_GameBoard.Board[i_row, col])
            {
                count++;
                col++;
            }

            col = i_col - 1;
            while (col >= 0 && i_GameBoard.Board[i_row, i_col] == i_GameBoard.Board[i_row, col])
            {
                count++;
                col--;
            }

            return count >= 4;
        }

        public bool CheckVertically(GameBoard i_GameBoard, int i_row, int i_col)
        {
            int count = 1;
            int row = i_row + 1;

            while (row < i_GameBoard.BoardHeight && i_GameBoard.Board[i_row, i_col] == i_GameBoard.Board[row, i_col])
            {
                count++;
                row++;
            }

            return count >= 4;
        }

        public bool CheckForWinner(GameBoard i_GameBoard, int i_row, int i_col)
        {
            return CheckDiagonally(i_GameBoard, i_row, i_col) ||
                CheckHorizontally(i_GameBoard, i_row, i_col) || CheckVertically(i_GameBoard, i_row, i_col);
        }
    }
}