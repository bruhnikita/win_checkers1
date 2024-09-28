using win_checkers;

namespace win_checkers;

public partial class Form1 : Form
{
    private Board Board;
    private Checker selectedChecker = null;
    private int selectedRow = -1;
    private int selectedCol = -1;
    private Color currentPlayer = Color.White;

    public Form1()
    {
        InitializeComponent();
        Board = new Board();
        Board.InitializeCheckers();
        DrawBoard();
        RefreshBoard();
    }

    private void DrawBoard()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                Button btn = new Button();
                btn.BackColor = Board.Squares[i, j].Color;
                btn.Location = new Point(j * 50, i * 50);
                btn.Size = new Size(50, 50);

                int row = i;
                int col = j;

                btn.Click += (sender, e) => OnSquareClick(row, col);
                this.Controls.Add(btn);
            }
        }
    }

    private void RefreshBoard()
    {
        foreach (Control control in this.Controls)
        {
            if (control is Button btn)
            {
                int row = btn.Location.Y / 50;
                int col = btn.Location.X / 50;
                btn.BackColor = Board.Squares[row, col].Color;

                if (Board.Squares[row, col].Checker != null)
                {
                    btn.Text = Board.Squares[row, col].Checker.IsKing ? "K" : "O";
                    btn.ForeColor = Board.Squares[row, col].Checker.Color;
                }
                else
                {
                    btn.Text = string.Empty;
                }
            }
        }
    }

    private void CheckForVictory()
    {
        bool whiteCheckers = false;
        bool redCheckers = false;

        foreach (var square in Board.Squares)
        {
            if (square.Checker != null)
            {
                if (square.Checker.Color == Color.White)
                    whiteCheckers = true;
                else
                    redCheckers = true;
            }
        }

        if (!whiteCheckers)
        {
            MessageBox.Show("Победа черных!");
            Application.Exit();
        }
        else if (!redCheckers)
        {
            MessageBox.Show("Победа белых!");
            Application.Exit();
        }
    }

    private void OnSquareClick(int i, int j)
    {
        Square square = Board.Squares[i, j];

        if (selectedChecker == null)
        {
            if (square.Checker != null && square.Checker.Color == currentPlayer)
            {
                selectedChecker = square.Checker;
                selectedRow = i;
                selectedCol = j;
            }
        }
        else
        {
            if (CanCapture(selectedRow, selectedCol, i, j))
            {
                CaptureChecker(selectedRow, selectedCol, i, j);
            }
            else if (IsValidMove(selectedRow, selectedCol, i, j))
            {
                MoveChecker(selectedRow, selectedCol, i, j);
            }
            else
            {
                MessageBox.Show("Недопустимый ход!");

                if (!HasValidMoves(selectedChecker))
                {
                    MessageBox.Show("У вас нет доступных ходов для этой шашки. Пожалуйста, выберите другую шашку.");
                    CustomResetSelection();
                }
            }
        }

        CheckForVictory();
    }

    private bool HasValidMoves(Checker checker)
    {
        int row = -1;
        int col = -1;

        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Board.Squares[i, j].Checker == checker)
                {
                    row = i;
                    col = j;
                    break;
                }
            }
            if (row != -1) break;
        }

        return IsValidMove(row, col, row + 1, col + 1) || IsValidMove(row, col, row + 1, col - 1) ||
               CanCapture(row, col, row + 2, col + 2) || CanCapture(row, col, row + 2, col - 2) ||
               CanCapture(row, col, row - 2, col + 2) || CanCapture(row, col, row - 2, col - 2);
    }

    private void MoveChecker(int fromRow, int fromCol, int toRow, int toCol)
    {
        Board.Squares[toRow, toCol].Checker = selectedChecker;
        Board.Squares[fromRow, fromCol].Checker = null;

        if ((currentPlayer == Color.White && toRow == 0) || (currentPlayer == Color.Red && toRow == 7))
        {
            selectedChecker.IsKing = true;
        }

        ResetSelection();
        SwitchPlayer();
        RefreshBoard();
    }

    private bool HasCaptureMoves(Checker checker)
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (Board.Squares[i, j].Checker == checker)
                {
                    if (CanCapture(i, j, i + 2, j + 2) || CanCapture(i, j, i + 2, j - 2) ||
                        CanCapture(i, j, i - 2, j + 2) || CanCapture(i, j, i - 2, j - 2))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void CustomResetSelection()
    {
        selectedChecker = null;
        selectedRow = -1;
        selectedCol = -1;
    }

    private void CaptureChecker(int fromRow, int fromCol, int toRow, int toCol)
    {
        int capturedRow = (fromRow + toRow) / 2;
        int capturedCol = (fromCol + toCol) / 2;

        Board.Squares[capturedRow, capturedCol].Checker = null;

        Board.Squares[toRow, toCol].Checker = selectedChecker;
        Board.Squares[fromRow, fromCol].Checker = null;

        if ((currentPlayer == Color.White && toRow == 0) || (currentPlayer == Color.Red && toRow == 7))
        {
            selectedChecker.IsKing = true;
        }

        ResetSelection();
        SwitchPlayer();
        RefreshBoard();
    }

    private void ResetSelection()
    {
        selectedChecker = null;
        selectedRow = -1;
        selectedCol = -1;
    }

    private void SwitchPlayer()
    {
        currentPlayer = (currentPlayer == Color.White) ? Color.Red : Color.White;
    }

    private bool CanCapture(int fromRow, int fromCol, int toRow, int toCol)
    {
        if (toRow < 0 || toRow >= 8 || toCol < 0 || toCol >= 8)
            return false;

        if (Math.Abs(fromRow - toRow) == 2 && Math.Abs(fromCol - toCol) == 2)
        {
            int capturedRow = (fromRow + toRow) / 2;
            int capturedCol = (fromCol + toCol) / 2;

            if (capturedRow < 0 || capturedRow >= 8 || capturedCol < 0 || capturedCol >= 8)
                return false;

            Checker capturedChecker = Board.Squares[capturedRow, capturedCol].Checker;
            return capturedChecker != null && capturedChecker.Color != currentPlayer;
        }
        return false;
    }
    private bool IsValidMove(int fromRow, int fromCol, int toRow, int toCol)
    {
        if (toRow < 0 || toRow >= 8 || toCol < 0 || toCol >= 8)
            return false;

        return Math.Abs(fromRow - toRow) == 1 && Math.Abs(fromCol - toCol) == 1 && Board.Squares[toRow, toCol].Checker == null;
    }
}
