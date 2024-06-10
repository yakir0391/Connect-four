using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EX02
{
    internal class Player
    {
        private readonly char m_Token;
        private int m_Points;
        private readonly string r_Name;
        private readonly bool r_IsComputer;

        public Player(char symbol, string name, bool isComputer)
        {
            this.m_Token = symbol;
            this.m_Points = 0;
            this.r_Name = name;
            this.r_IsComputer = isComputer;
        }

        public char Token
        {
            get { return m_Token; }
        }

        public int Points
        {
            get { return m_Points; }

            set { m_Points = value; }
        }

        public string Name
        {
            get { return r_Name; }
        }

        public bool IsComputer
        {
            get { return r_IsComputer; }
        }
    }
}