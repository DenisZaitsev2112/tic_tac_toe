using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace tic_tac_toe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Button[,] buttons;
        private bool isPlayerTurn;
        private bool isGameOver;
        private Random random;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            buttons = new Button[3, 3]
            {
        { button1, button2, button3 },
        { button4, button5, button6 },
        { button7, button8, button9 }
            };

            isPlayerTurn = true;
            isGameOver = false;
            random = new Random();

            foreach (var button in buttons)
            {
                button.Content = string.Empty;
                button.IsEnabled = true;
                button.Click -= Button_Click; // Удаляем обработчик события
                button.Click += Button_Click; // Добавляем обработчик события
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;

            if (button.Content.ToString() != string.Empty || isGameOver)
                return;

            if (isPlayerTurn)
            {
                button.Content = "X";
            }
            else
            {
                button.Content = "O";
            }

            button.IsEnabled = false;
            isPlayerTurn = !isPlayerTurn;

            CheckWinConditions();

            if (!isPlayerTurn && !isGameOver)
                BotMove();
        }

        private void BotMove()
        {
            var availableButtons = new List<Button>();

            foreach (var button in buttons)
            {
                if (button.Content.ToString() == string.Empty)
                {
                    availableButtons.Add(button);
                }
            }

            if (availableButtons.Count > 0)
            {
                var randomButton = availableButtons[random.Next(availableButtons.Count)];
                randomButton.Content = "O";
                randomButton.IsEnabled = false;
                isPlayerTurn = true;

                CheckWinConditions();
            }
        }

        private void CheckWinConditions()
        {
            // Check rows
            for (int row = 0; row < 3; row++)
            {
                if (buttons[row, 0].Content.ToString() == buttons[row, 1].Content.ToString() &&
                    buttons[row, 1].Content.ToString() == buttons[row, 2].Content.ToString() &&
                    buttons[row, 0].Content.ToString() != string.Empty)
                {
                    GameOver(buttons[row, 0].Content.ToString());
                    return;
                }
            }

            // Check columns
            for (int col = 0; col < 3; col++)
            {
                if (buttons[0, col].Content.ToString() == buttons[1, col].Content.ToString() &&
                    buttons[1, col].Content.ToString() == buttons[2, col].Content.ToString() &&
                    buttons[0, col].Content.ToString() != string.Empty)
                {
                    GameOver(buttons[0, col].Content.ToString());
                    return;
                }
            }

            // Check diagonals
            if ((buttons[0, 0].Content.ToString() == buttons[1, 1].Content.ToString() &&
                 buttons[1, 1].Content.ToString() == buttons[2, 2].Content.ToString() &&
                 buttons[0, 0].Content.ToString() != string.Empty) ||
                (buttons[0, 2].Content.ToString() == buttons[1, 1].Content.ToString() &&
                 buttons[1, 1].Content.ToString() == buttons[2, 0].Content.ToString() &&
                 buttons[0, 2].Content.ToString() != string.Empty))
            {
                GameOver(buttons[1, 1].Content.ToString());
                return;
            }

            // Check for a draw
            bool isDraw = true;
            foreach (var button in buttons)
            {
                if (button.Content.ToString() == string.Empty)
                {
                    isDraw = false;
                    break;
                }
            }

            if (isDraw)
            {
                GameOver("draw");
            }
        }

        private void GameOver(string winner)
        {
            isGameOver = true;

            foreach (var button in buttons)
            {
                button.IsEnabled = false;
            }

            if (winner == "draw")
            {
                MessageBox.Show("Ничья!", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"Победитель - {winner}", "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            InitializeGame();
        }
    }
}
