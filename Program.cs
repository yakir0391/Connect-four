using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EX02
{
    internal class Program
    {
        public static void Main()
        {
            StartConnectFourGame();
        }
        public static void StartConnectFourGame()
        {
            GameUI game = new GameUI();
            game.StartGame();
        }
    }
}