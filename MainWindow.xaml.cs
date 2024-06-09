using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace TicTacToe
{
    public partial class MainWindow : Window
    {
        private char currentPlayer = 'X';
        private char[,] board;
        private int rows;
        private int columns;
        private int winCondition;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(RowsTextBox.Text, out rows) && int.TryParse(ColumnsTextBox.Text, out columns) && int.TryParse(WinConditionTextBox.Text, out winCondition))
            {
                board = new char[rows, columns];
                MenuPanel.Visibility = Visibility.Collapsed;
                InitializeBoard();
            }
            else
            {
                MessageBox.Show("Please enter valid numbers for rows, columns, and win condition.");
            }
        }

        private void InitializeBoard()
        {
            MainGrid.Children.Clear();
            MainGrid.RowDefinitions.Clear();
            MainGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < rows; i++)
            {
                MainGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < columns; j++)
            {
                MainGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    Button button = new Button
                    {
                        FontSize = 24,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        MinWidth = 50,
                        MinHeight = 50
                    };
                    button.Click += Button_Click;
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    MainGrid.Children.Add(button);
                }
            }

            currentPlayer = 'X';
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            if (button == null) return;

            int row = Grid.GetRow(button);
            int column = Grid.GetColumn(button);

            if (button.Content == null || button.Content.ToString() == "")
            {
                button.Content = currentPlayer;
                button.Foreground = currentPlayer == 'X' ? Brushes.Blue : Brushes.Red;
                board[row, column] = currentPlayer;
                if (CheckWin(currentPlayer, row, column))
                {
                    MessageBox.Show($"Player {currentPlayer} wins!");
                    ResetBoard();
                }
                else if (IsBoardFull())
                {
                    MessageBox.Show("It's a draw!");
                    ResetBoard();
                }
                else
                {
                    currentPlayer = currentPlayer == 'X' ? 'O' : 'X';
                }
            }
        }

        private bool CheckWin(char player, int lastRow, int lastCol)
        {
            // Check row
            int count = 0;
            for (int col = 0; col < columns; col++)
            {
                if (board[lastRow, col] == player) count++;
                else count = 0;
                if (count >= winCondition) return true;
            }

            // Check column
            count = 0;
            for (int row = 0; row < rows; row++)
            {
                if (board[row, lastCol] == player) count++;
                else count = 0;
                if (count >= winCondition) return true;
            }

            // Check diagonals
            count = 0;
            for (int i = -winCondition; i < winCondition; i++)
            {
                int r = lastRow + i, c = lastCol + i;
                if (r >= 0 && r < rows && c >= 0 && c < columns && board[r, c] == player) count++;
                else count = 0;
                if (count >= winCondition) return true;
            }

            count = 0;
            for (int i = -winCondition; i < winCondition; i++)
            {
                int r = lastRow + i, c = lastCol - i;
                if (r >= 0 && r < rows && c >= 0 && c < columns && board[r, c] == player) count++;
                else count = 0;
                if (count >= winCondition) return true;
            }

            return false;
        }

        private bool IsBoardFull()
        {
            foreach (char cell in board)
            {
                if (cell == '\0')
                {
                    return false;
                }
            }
            return true;
        }

        private void ResetBoard()
        {
            MenuPanel.Visibility = Visibility.Visible;
            MainGrid.Children.Clear();
            board = new char[rows, columns];
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}