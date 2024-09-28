using System.Drawing; 

namespace win_checkers
{
    public class Board
    {
        public Square[,] Squares { get; set; }

        public Board()
        {
            Squares = new Square[8, 8];
            InitializeBoard();
            InitializeCheckers(); 
        }

        private void InitializeBoard()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Squares[i, j] = new Square((i + j) % 2 == 0 ? Color.White : Color.Black);
                }
            }
        }

        public void InitializeCheckers()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (i < 3 && (i + j) % 2 != 0) 
                    {
                        Squares[i, j].Checker = new Checker(Color.Red);
                    }
                    else if (i > 4 && (i + j) % 2 != 0) 
                    {
                        Squares[i, j].Checker = new Checker(Color.White);
                    }
                }
            }
        }

    }
}
