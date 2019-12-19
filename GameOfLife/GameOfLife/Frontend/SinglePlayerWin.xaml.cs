using System;
using System.Collections.Generic;
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

        public SinglePlayerWin() {
            InitializeComponent();
            gridRect = new Rectangle[20, 20];
            for(int i =0; i < 20; i++) {
                for (int j = 0; j < 20; j++) {
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
                    GameGrid.Children.Add(r);
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

        private void BtnStart_Click(object sender, RoutedEventArgs e) {
            var game = new GOL.GameOfLife(20, 20, true, 200);
            var board = game.GetBoard();
            foreach(var cell in board.Cells) {
                if (cell.IsSet) {
                        gridRect[cell.X, cell.Y].Fill = Brushes.Red;
                } else {
                        gridRect[cell.X, cell.Y].Fill = Brushes.White;
                }
            }
            threadGame = new Thread(() => GameRun(game));
            threadGame.Start();

        }

        private void GameRun(GOL.GameOfLife game) {
            while (true) {

                foreach (var cell in game.iterate()) {
                    if (cell.IsSet) {
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            gridRect[cell.X, cell.Y].Fill = Brushes.Red;
                        }));
                    } else {
                        this.Dispatcher.BeginInvoke(new Action(() => {
                            gridRect[cell.X, cell.Y].Fill = Brushes.White;
                        }));
                    }
                }
                Thread.Sleep(200);
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e) {
            threadGame.Abort();
        }
    }
}
