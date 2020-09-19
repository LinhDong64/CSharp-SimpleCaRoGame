using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace TicTacToe.VeBanCoVaTinhToaDoa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        const int startX = 30;
        const int startY = 30;
        const int width = 50;
        const int height = 50;
        const int Cols = 6;
        const int Rows = 6;

        int[,] _a;
        Image[,]_images;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _a = new int[Rows, Cols];
            _images = new Image[Rows, Cols];
            //Ve cac truc doc
            for (int i = 0; i < 7; i++)
            {
                var line = new Line();
                line.StrokeThickness = 1;
                line.Stroke = new SolidColorBrush(Colors.Red);
                canvas.Children.Add(line);

                line.X1 = startX + i*width;
                line.Y1 = startY;

                line.X2 = startX + i*width;
                line.Y2 = startY + 6 * height;
            }

            //Ve cac truc ngang
            for(int j=0;j<7;j++)
            {
                var line = new Line();
                line.StrokeThickness = 1;
                line.Stroke = new SolidColorBrush(Colors.Red);
                canvas.Children.Add(line);

                line.X1 = startX;
                line.Y1 = startY +j*height;

                line.X2 = startX + 6 * width;
                line.Y2 = startY + j*height;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
          
        }

        bool isXTurn = true;
        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var positon = e.GetPosition(this);


            int j = ((int)positon.X - startX) / width;
            int i = ((int)positon.Y - startY) / height;

            this.Title = $"{positon.X}-{positon.Y},a[{i}][{j}]";

            
            if (_a[i, j] == 0)
            {
                var img = new Image();
                img.Width = 30;
                img.Height = 30;
                if (isXTurn)
                {
                    img.Source = new BitmapImage(
                        new Uri("X.png", UriKind.Relative));
                    canvas.Children.Add(img);
                    _a[i, j] = 1;
                    _images[i, j] = img;
                }
                else
                {
                    img.Source = new BitmapImage(
                        new Uri("O.png", UriKind.Relative));
                    canvas.Children.Add(img);
                    _a[i, j] = 2;
                    _images[i, j] = img;
                }

                Canvas.SetLeft(img, startX + j * width + 10);
                Canvas.SetTop(img, startY + i * height + 10);

                isXTurn = !isXTurn;
                
                var (gameOver, xWin) = checkWin(_a, i, j);

                if (gameOver == 1)
                {
                    if (xWin)
                    {
                        MessageBox.Show("X won!");
                    }
                    else
                    {
                        MessageBox.Show("O won!");
                    }
                    reset(_a, _images);
                    return;
                }
                if (gameOver == 2)
                {
                    MessageBox.Show("Bat phan thang bai!");
                }
            }
            
        }

        //Kiem tra theo dong
        private int checkRow(int[,] a, int i, int j)
        {
            int count = 1;
            int dj = -1;
            int startJ = j;

            while (-1 != startJ + dj)//con qua duoc ben trai
            {
                startJ += dj;//di qua trai
                if (_a[i, j] == a[i, startJ])//tang bien dem
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            startJ = j;
            dj = 1;
            while (startJ + dj != 6)
            {
                startJ += dj;//di qua phai
                if (_a[i, j] == a[i, startJ])//tang bien dem
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        //Kiem tra theo cot
        private int checkCol(int[,] a, int i, int j)
        {
            int count = 1;
            int di = -1;
            int startI = i;

            while (-1 != startI + di)//con di len duoc
            {
                startI += di;//di len
                if (_a[i, j] == a[startI, j])//tang bien dem
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            startI = i;
            di = 1;
            while (startI + di != 6)
            {
                startI += di;//di qua phai
                if (_a[i, j] == a[startI, j])//tang bien dem
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        //Kiem tra duong cheo chinh
        private int checkCheoChinh(int[,] a, int i, int j)
        {
            int count = 1;
            int di = -1;
            int dj = -1;
            int startI = i;
            int startJ = j;

            while ((-1 != startI + di) && (-1 != startJ + dj))//con di len chech ve ben trai duoc
            {
                startI += di;//di len
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])//tang bien dem
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            startI = i;
            startJ = j;
            di = 1;
            dj = 1;
            while ((startI + di != 6) && (startJ + dj != 6))
            {
                startI += di;//di cheo xuong phai
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])//tang bien dem
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }

        //Kiem tra duong dheo phu
        private int checkCheoPhu(int[,] a, int i, int j)
        {
            int count = 1;
            int di = -1;
            int dj = 1;
            int startI = i;
            int startJ = j;

            while ((-1 != startI + di) && (startJ + dj != 6))//con di len chech ve ben phai duoc
            {
                startI += di;//di len 
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])//tang bien dem
                {
                    count++;
                }
                else
                {
                    break;
                }
            }

            startI = i;
            startJ = j;
            di = 1;
            dj = -1;
            while ((startI + di != 6) && (startJ + dj != -1))
            {
                startI += di;//di cheo xuong ben trai
                startJ += dj;
                if (_a[i, j] == a[startI, startJ])//tang bien dem
                {
                    count++;
                }
                else
                {
                    break;
                }
            }
            return count;
        }
        private (int, bool) checkWin(int[,] a, int i, int j)
        {
            const int WinConditon = 5;
            var countRow = 0;
            var countCol = 0;
            var countCheoChinh = 0;
            var countCheoPhu = 0;
            var hoa = 0;
            /*Loang theo chieu ngang*/
            //Loang ben trai

            countRow = checkRow(a, i, j);
            countCol = checkCol(a, i, j);
            countCheoChinh = checkCheoChinh(a, i, j);
            countCheoPhu = checkCheoPhu(a, i, j);
            var xWin = false;
            var gameOver = 0;
            if (countRow >= WinConditon || countCol >= WinConditon || countCheoChinh >= WinConditon || countCheoPhu >= WinConditon)
            {
                gameOver = 1;
                xWin = _a[i, j] == 1;
            }
            else
            {
                for (int k = 0; k < 6; k++)
                {
                    for (int m = 0; m < 6; m++)
                    {
                        if (_a[k, m] != 0)
                        {
                            hoa++;
                        }
                    }
                }

                if (hoa == 36)
                {
                    gameOver = 2;
                }
            }

            return (gameOver, xWin);
        }

        //Reset
        private void reset(int[,] a, Image[,] images)
        {
            for(int i=0;i<Rows;i++)
            {
                for(int j=0;j<Cols;j++)
                {
                    _a[i, j] = 0;
                    canvas.Children.Remove(_images[i, j]);
                }
            }
            isXTurn = true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            const string filename = "save.txt";

            var writer = new StreamWriter(filename);
            //Dong dau tien la luot di hien tai
            writer.WriteLine(isXTurn ? "X" : "O");

            //Sau do la ma tran bieu dien game
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    writer.Write($"{_a[i, j]}");
                    if (j != 5)
                    {
                        writer.Write(" ");
                    }
                }
                writer.WriteLine("");
            }

            writer.Close();
            MessageBox.Show("Game is saved");
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            reset(_a, _images);
            var screen = new OpenFileDialog();
            if (screen.ShowDialog() == true)
            {
                var filename = screen.FileName;

                var reader = new StreamReader(filename);
                var firstLine = reader.ReadLine();
                isXTurn = firstLine == "X";

                for (int i = 0; i < 6; i++)
                {
                    var tokens = reader.ReadLine().Split(new string[] { " " }, StringSplitOptions.None);

                    for (int j = 0; j < 6; j++)
                    {
                        _a[i, j] = int.Parse(tokens[j]);

                        if (_a[i, j] == 1)
                        {
                            _images[i,j].Source= new BitmapImage(
                        new Uri("X.png", UriKind.Relative));
                            canvas.Children.Add(_images[i,j]);
                        }

                        if (_a[i, j] == 2)
                        {
                            _images[i, j].Source = new BitmapImage(
                        new Uri("O.png", UriKind.Relative));
                            canvas.Children.Add(_images[i, j]);
                        }
                    }
                }

                MessageBox.Show("Game is loaded!");
            }

        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            reset(_a, _images);
        }
    }
}
