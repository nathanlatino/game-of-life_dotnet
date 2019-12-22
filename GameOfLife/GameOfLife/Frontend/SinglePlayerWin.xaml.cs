using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GameOfLife.Frontend {
    /// <summary>
    /// Logique d'interaction pour SinglePlayerWin.xaml
    /// </summary>
    public partial class SinglePlayerWin : Window {
        private Rectangle[][] gridRect;
        private int width = 20;
        private int height = 20;
        private int time = 200;
        private Thread threadGame;
        private bool blBreak;

        public SinglePlayerWin() {
            InitializeComponent();
            CreateGrid();
        }

        private void CreateGrid() {

            gridRect = new Rectangle[height][];
            for (int i = 0; i < width; i++) {
                gridRect[i] = new Rectangle[height];
                for (int j = 0; j < height; j++) {
                    var r = new Rectangle();
                    r.StrokeThickness = 1;
                    r.Stroke = Brushes.Black;
                    r.Fill = Brushes.White;
                    r.Width = 100;
                    r.Height = 100;
                    r.MouseLeftButtonDown += Rectangle_MouseLeftButtonDown;
                    gridRect[i][j] = r;
                }
            }
            gridGame.ItemsSource = gridRect;
        }


        private List<GOL.Cell> GetCellsSelected(GOL.GameOfLife game) {
            List<GOL.Cell> cells = new List<GOL.Cell>();
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (gridRect[i][j].Fill == Brushes.Red) {
                        cells.Add(game.GetBoard().Cells[i, j]);
                    }
                }

            }
            return cells;
        }

        private GOL.GameOfLife InitStartGame()
        {
            blBreak = false;
            GOL.GameOfLife game;

            if (cbxMode.SelectedIndex == 0)
            {
                game = new GOL.GameOfLife(width, height, false);
                game.SetCells(GetCellsSelected(game));
            }
            else
            {
                game = new GOL.GameOfLife(width, height, true);
            }
            return game;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e) {
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
                            gridRect[cell.X][cell.Y].Fill = color;
                        }));
                    }
                    Thread.Sleep(time);
                }
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e) {
            threadGame.Abort();
        }

        private void btnBreak_Click(object sender, RoutedEventArgs e) {
            blBreak = !blBreak;
        }

        private void SinglePlayerWin_Closing(object sender, CancelEventArgs e) {
            if (threadGame != null) {
                threadGame.Abort();
            }
            new MainWindow().Show();
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Rectangle r = sender as Rectangle;
            r.Fill = r.Fill == Brushes.White ? r.Fill = Brushes.Red : r.Fill = Brushes.White;
        }
    }
}
