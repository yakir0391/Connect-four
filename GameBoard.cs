using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EX02
{
    internal class GameBoard
    {
        private char[,] m_Board;
        private readonly int r_BoardHeight;
        private readonly int r_BoardWidth;

        public GameBoard(int rows, int columns)
        {
            m_Board = new char[rows, columns];
            r_BoardWidth = columns;
            r_BoardHeight = rows;
        }

        public char[,] Board
        {
            get { return m_Board; }

            set { m_Board = value; }
        }

        public int BoardWidth
        {
            get { return r_BoardWidth; }
        }

        public int BoardHeight
        {
            get { return r_BoardHeight; }
        }

        public void ClearGameBoard()
        {
            for (int i = 0; i < r_BoardHeight; i++)
            {
                for (int j = 0; j < r_BoardWidth; j++)
                {
                    m_Board[i, j] = '\0';
                }
            }
        }

        public bool IsBoardFull()
        {
            int countFullColumns = 0;

            for (int i = 0; i < r_BoardWidth; i++)
            {
                if (m_Board[0, i] != '\0')
                {
                    countFullColumns++;
                }
            }

            return countFullColumns == r_BoardWidth;
        }
    }
}