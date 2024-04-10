using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Toli
{
    public partial class MainWindow : Window
    {
        private Button emptyButton;
        private Button[,] buttons;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGrid();
            GenerateAndShuffleButtons();
        }

        private void InitializeGrid()
        {
            ToliGrid.ColumnDefinitions.Clear();
            ToliGrid.RowDefinitions.Clear();
            for (int i = 0; i < 3; i++)
            {
                ToliGrid.RowDefinitions.Add(new RowDefinition());
                ToliGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void GenerateAndShuffleButtons()
        {
            GenerateButtons();
            ShuffleButtons();
        }

        private void GenerateButtons()
        {
            buttons = new Button[3, 3];
            int buttonNumber = 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    Button button = new Button
                    {
                        Content = $"{buttonNumber++}",
                        Margin = new Thickness(5)
                    };
                    button.Click += Button_Click;
                    ToliGrid.Children.Add(button);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    buttons[i, j] = button;
                }
            }

            emptyButton = buttons[2, 2];
            emptyButton.Content = "";
        }

        private void ShuffleButtons()
        {
            Random rand = new Random();
            int[] numbers;

            do
            {
                numbers = Enumerable.Range(1, 8).OrderBy(x => rand.Next()).ToArray();
            } while (!IsSolvable(numbers));

            int index = 0;
            foreach (Button button in buttons)
            {
                if (button != emptyButton)
                {
                    button.Content = numbers[index++];
                }
            }
        }

        private bool IsSolvable(int[] numbers)
        {
            int inversionCount = numbers.SelectMany((x, i) => numbers.Skip(i + 1).Where(y => y < x && y != 0)).Count();
            return inversionCount % 2 == 0;
        }

        private bool IsGameSolved()
        {
            return buttons.Cast<Button>().Select((button, index) => button.Content.ToString() == (index + 1).ToString()).All(x => x);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            int row = Grid.GetRow(clickedButton);
            int col = Grid.GetColumn(clickedButton);

            if ((row == Grid.GetRow(emptyButton) && Math.Abs(col - Grid.GetColumn(emptyButton)) == 1) || (col == Grid.GetColumn(emptyButton) && Math.Abs(row - Grid.GetRow(emptyButton)) == 1))
            {
                Grid.SetRow(clickedButton, Grid.GetRow(emptyButton));
                Grid.SetColumn(clickedButton, Grid.GetColumn(emptyButton));
                Grid.SetRow(emptyButton, row);
                Grid.SetColumn(emptyButton, col);

                if (IsGameSolved())
                {
                    MessageBox.Show("Gratulálok, sikeresen megoldottad a TiliToli játékot!");
                }
            }
        }

        private void BtnUjraKezd_Click(object sender, RoutedEventArgs e)
        {
            ToliGrid.Children.Clear();
            InitializeGrid();
            GenerateAndShuffleButtons();
        }
    }
}

