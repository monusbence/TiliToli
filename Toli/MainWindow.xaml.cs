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
        private int rowCount;
        private int columnCount;

        public MainWindow()
        {
            InitializeComponent();
            btnGenerál_Click();
            GenerateAndShuffleButtons();
        }

        

        private void GenerateAndShuffleButtons()
        {
            GenerateButtons();
            ShuffleButtons();
        }

        private void GenerateButtons()
        {
            buttons = new Button[rowCount, columnCount];
            int buttonNumber = 1;

            for (int i = 0; i < rowCount; i++)
            {
                for (int j = 0; j < columnCount; j++)
                {
                    Button button = new Button { Content = $"{buttonNumber++}", Margin = new Thickness(5) };
                    button.Click += Button_Click;
                    ToliGrid.Children.Add(button);
                    Grid.SetRow(button, i);
                    Grid.SetColumn(button, j);
                    buttons[i, j] = button;
                }
            }

            emptyButton = buttons[rowCount - 1, columnCount - 1];
            emptyButton.Content = "";
        }

        private void ShuffleButtons()
        {
            Random rand = new Random();
            int[] numbers;

            do
            {
                numbers = Enumerable.Range(1, rowCount * columnCount - 1).OrderBy(x => rand.Next()).ToArray();
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
                SwapButtons(clickedButton, emptyButton);
                if (IsGameSolved())
                {
                    MessageBox.Show("Gratulálok, sikeresen megoldottad a TiliToli játékot!");
                }
            }
        }

        private void SwapButtons(Button button1, Button button2)
        {
            int row1 = Grid.GetRow(button1);
            int col1 = Grid.GetColumn(button1);
            int row2 = Grid.GetRow(button2);
            int col2 = Grid.GetColumn(button2);

            Grid.SetRow(button1, row2);
            Grid.SetColumn(button1, col2);
            Grid.SetRow(button2, row1);
            Grid.SetColumn(button2, col1);
        }

        private void BtnUjraKezd_Click(object sender, RoutedEventArgs e)
        {
            ToliGrid.Children.Clear();
            btnGenerál_Click();
            GenerateAndShuffleButtons();
        }

        private void btnGenerál_Click()
        {
            int rowCount = int.Parse(txtRowCount.Text);
            int columnCount = int.Parse(txtColumnCount.Text);

            ToliGrid.ColumnDefinitions.Clear();
            ToliGrid.RowDefinitions.Clear();
            for (int i = 0; i < rowCount; i++)
            {
                ToliGrid.RowDefinitions.Add(new RowDefinition());
            }
            for (int j = 0; j < columnCount; j++)
            {
                ToliGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }
    }
}