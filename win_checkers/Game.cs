using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace win_checkers
{
    public class Game
    {
        private Board Board;
        private Checker[] Checkers {  get; set; }
        private State currentState { get; set; }

        private enum State
        {
            GameOver,
            WhiteTurn,
            BlackTurn
        }

        public Game() 
        {
            Board = new Board();
            Checkers = new Checker[24];
            currentState = State.WhiteTurn; 
        }
    }
}
