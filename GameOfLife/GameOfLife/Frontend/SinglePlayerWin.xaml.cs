using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace GameOfLife.Frontend {
    /// <summary>
    /// Logique d'interaction pour SinglePlayerWin.xaml
    /// </summary>
    public partial class SinglePlayerWin : Window {
        public Rectangle[,] gridRect;
        private int width = 20;
        private int height = 20;
        Thread threadGame;
        public bool blBreak;

        public SinglePlayerWin() {
            InitializeComponent();
            CreateGrid(20,20);
        }

        public void CreateGrid(int width, int height) {
            gameGrid.Columns = width;
            gameGrid.Rows = height;

            int nbCases = width * height;
            gridRect = new Rectangle[width, height];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++)
                {
                    var r = new Rectangle();
                    r.Fill = Brushes.White;
                    gridRect[i,j] = r;
                    //Grid.SetColumn(r, i);
                    //Grid.SetColumn(r, j);
                    gameGrid.Children.Add(r);
                }

            }
        }

        void OnRectangleMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Rectangle r = sender as Rectangle;
            r.Fill = r.Fill == Brushes.White ? r.Fill = Brushes.Red : r.Fill = Brushes.White;
        }

        private List<GOL.Cell> GetCellsSelected() {
            List<GOL.Cell> cells = new List<GOL.Cell>();
            int count = 0;
            foreach (Rectangle r in gridRect) {
                if (r.Fill == Brushes.Red) {
                    cells.Add(new GOL.Cell(count / width, count % height));
                }
                count++;
            }
            return cells;
        }

        private GOL.GameOfLife InitStartGame()
        {
            blBreak = false;
            GOL.GameOfLife game;

            if (cbxMode.SelectedIndex == 0)
            {
                game = new GOL.GameOfLife(20, 20, false, 200);
                game.setCells(GetCellsSelected());
            }
            else
            {
                game = new GOL.GameOfLife(20, 20, true, 200);
            }


            var board = game.GetBoard();
            //foreach (var cell in board.Cells)
            //{
            //    if (cell.IsSet)
            //    {
            //        gridRect.Find((X, Y) => cell.X == X && cell.Y == Y).
            //        gridRect[cell.X*width+cell.Y*height].Fill = Brushes.Red;
            //    }
            //    else
            //    {
            //        gridRect[cell.X * width + cell.Y * height].Fill = Brushes.White;
            //    }
            //}
            return game;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e) {
            btnStart.IsEnabled = false;
            btnBreak.IsEnabled = true;
            var game = InitStartGame();
            threadGame = new Thread(() => GameRun(game));
            threadGame.Start();

        }

        private void GameRun(GOL.GameOfLife game)
        {
            while (true)
            {
                if (blBreak == false)
                {
                    foreach (var cell in game.iterate())
                    {
                        SolidColorBrush color = cell.IsSet ? Brushes.Red : Brushes.White;
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            //gridRect[cell.X * width + cell.Y * height].Fill = color;
                        }));
                    }
                    Thread.Sleep(200);
                }
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e) {
            threadGame.Abort();
        }

        private void btnBreak_Click(object sender, RoutedEventArgs e) {
            blBreak = !blBreak;
        }

        void SinglePlayerWin_Closing(object sender, CancelEventArgs e) {
            if (threadGame != null) {
                threadGame.Abort();

            }
            new MainWindow().Show();
        }

    }
}
