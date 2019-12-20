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
        Thread threadGame;
        public bool blBreak;

        public SinglePlayerWin() {
            InitializeComponent();
            CreateGrid(20, 20, GameGrid);
            
        }

        public void CreateGrid(int width, int height, Canvas parent) {
            gridRect = new Rectangle[20, 20];
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    var r = new Rectangle();
                    r.Fill = Brushes.White;
                    r.Stroke = Brushes.Black;
                    r.Width = 20;
                    r.Height = 20;
                    r.StrokeThickness = 1;
                    Canvas.SetLeft(r, 20 * i);
                    Canvas.SetTop(r, 20 * j);
                    r.MouseLeftButtonDown += OnRectangleMouseLeftButtonDown;
                    gridRect[i, j] = r;
                    parent.Children.Add(r);
                }

            }
        }

        void OnRectangleMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            Rectangle r = sender as Rectangle;
            if (r.Fill == Brushes.White) {
                r.Fill = Brushes.Red;

            } else {
                r.Fill = Brushes.White;
            }
            
        }
        private List<GOL.Cell> GetCellsSelected() {
            List<GOL.Cell> cells = new List<GOL.Cell>();            ;
            int count = 0;
            foreach (Rectangle r in gridRect) {
                
                if (r.Fill == Brushes.Red) {
                    cells.Add(new GOL.Cell(count / 20, count % 20));
                }
                count++;
            }
            return cells;
        }
        private GOL.GameOfLife InitStartGame() {
            blBreak = false;
            GOL.GameOfLife game;

            if (cbxMode.SelectedIndex == 0) {
                game = new GOL.GameOfLife(20, 20, false, 200);
                game.setCells(GetCellsSelected());
            } else {
                game = new GOL.GameOfLife(20, 20, true, 200);
            }


            var board = game.GetBoard();
            foreach (var cell in board.Cells) {
                if (cell.IsSet) {
                    gridRect[cell.X, cell.Y].Fill = Brushes.Red;
                } else {
                    gridRect[cell.X, cell.Y].Fill = Brushes.White;
                }
            }
            return game;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e) {
            btnStart.IsEnabled = false;
            btnBreak.IsEnabled = true;
            var game = InitStartGame();
            threadGame = new Thread(() => GameRun(game));
            threadGame.Start();

        }

        private void GameRun(GOL.GameOfLife game) {
            while (true) {
                if(blBreak == false) {
                    foreach (var cell in game.iterate()) {
                        SolidColorBrush color = cell.IsSet ? Brushes.Red : Brushes.White;
                        this.Dispatcher.BeginInvoke(new Action(() =>{
                            gridRect[cell.X, cell.Y].Fill = color;
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
