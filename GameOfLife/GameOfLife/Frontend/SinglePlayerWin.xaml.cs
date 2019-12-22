using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;

namespace GameOfLife.Frontend {
    /// <summary>
    /// Logique d'interaction pour SinglePlayerWin.xaml
    /// </summary>
    public partial class SinglePlayerWin : Window {
        private Rectangle[][] gridRect;
        private int width = 20;
        private int height = 20;
        private int speed = 200;
        private Thread threadGame;
        private bool isBreak = false;
        private bool isRunning = false;
        private SolidColorBrush saveColor;
        private ManualResetEvent stopEvent;
        private SolidColorBrush cellColor;

        public SinglePlayerWin() {
            InitializeComponent();
            CreateGrid();
            stopEvent = new ManualResetEvent(true);
            cellColor = Brushes.Red;
    }

        private void CreateGrid() {
            gridRect = new Rectangle[width][];
            for (int i = 0; i < width; i++) {
                gridRect[i] = new Rectangle[height];
                for (int j = 0; j < height; j++) {
                    var r = new Rectangle();
                    r.StrokeThickness = 1;
                    r.Stroke = Brushes.Black;
                    r.Fill = Brushes.White;
                    r.Width = 100;
                    r.Height = 100;
                    r.MouseDown += R_MouseDown;
                    r.MouseEnter += R_MouseEnter;
                    gridRect[i][j] = r;
                }
            }
            if(gridGame != null) {
                gridGame.ItemsSource = gridRect;
            }
            
        }

        private void R_MouseEnter(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !isRunning)
            {
                Rectangle r = sender as Rectangle;
                r.Fill = saveColor;
            }

        }

        private void R_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!isRunning)
            {
                Rectangle r = sender as Rectangle;
                saveColor = r.Fill == Brushes.White ? cellColor : Brushes.White;
                r.Fill = saveColor;
            }
        }

        private List<GOL.Cell> GetCellsSelected(GOL.GameOfLife game) {
            List<GOL.Cell> cells = new List<GOL.Cell>();
            for (int i = 0; i < width; i++) {
                for (int j = 0; j < height; j++) {
                    if (gridRect[i][j].Fill == cellColor) {
                        cells.Add(game.GetBoard().Cells[i, j]);
                    }
                }

            }
            return cells;
        }

        private GOL.GameOfLife InitStartGame()
        {
            isBreak = false;
            GOL.GameOfLife game;
            if (rbnRandom.IsChecked ?? true)
            {
                game = new GOL.GameOfLife(width, height, true);
                CreateGrid();
            } else {
                game = new GOL.GameOfLife(width, height, false);
                game.SetCells(GetCellsSelected(game));
            }
            return game;
        }



        private void GameRun(GOL.GameOfLife game)
        {
            while (true)
            {
                stopEvent.WaitOne();
                foreach (var cell in game.iterate())
                    {
                        SolidColorBrush color = cell.IsSet ? cellColor : Brushes.White;
                        this.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            gridRect[cell.X][cell.Y].Fill = color;
                        }));
                    }
                    Thread.Sleep(speed);
            }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = false;
            btnBreak.IsEnabled = true;
            btnStop.IsEnabled = true;
            gpbMode.IsEnabled = false;
            gpbSize.IsEnabled = false;
            isRunning = true;
            var game = InitStartGame();
            threadGame = new Thread(() => GameRun(game));
            threadGame.Start();

        }

        private void btnStop_Click(object sender, RoutedEventArgs e) {
            if(threadGame != null)
            {
                threadGame.Abort();
            }
            btnStart.IsEnabled = true;
            btnBreak.IsEnabled = false;
            btnStop.IsEnabled = false;
            gpbMode.IsEnabled = true;
            gpbSize.IsEnabled = true;
            isRunning = false;
            isBreak = false;
        }

        private void btnBreak_Click(object sender, RoutedEventArgs e) {
            if (isBreak) {
                stopEvent.Set();
            } else {
                stopEvent.Reset();
            }
            isBreak = !isBreak;
        }

        private void IudSize_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            var iud = sender as IntegerUpDown;
            IntUpDownValid(iud);
            if (iud.Name == "iudWidth") {
                width = iudWidth.Value ?? 20;
                iud.Value = width;
            }
            if (iud.Name == "iudHeigth") {
                height = iudHeigth.Value ?? 20;
                iud.Value = height;
            }
            
            CreateGrid();
        }

        private void IudSpeed_ValueChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
            var iud = sender as IntegerUpDown;
            IntUpDownValid(iud);
            speed = iud.Value ?? 200;
            iud.Value = speed;
        }

        private void IntUpDownValid(IntegerUpDown iud)
        {
            iud.Value = iud.Value < iud.Minimum ? iud.Minimum : iud.Value > iud.Maximum ? iud.Value = iud.Maximum : iud.Value;
        }

        private void SinglePlayerWin_Closing(object sender, CancelEventArgs e)
        {
            if (threadGame != null)
            {
                threadGame.Abort();
            }
        }



        private void ClpCell_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            var newColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString(clpCell.SelectedColor.ToString()));
            ChangeColor(newColor);
            cellColor = newColor;
        }

        private void ChangeColor(SolidColorBrush newColor)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    if (gridRect[i][j].Fill == cellColor)
                    {
                        gridRect[i][j].Fill = newColor;
                    }
                }

            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = true;
            btnBreak.IsEnabled = false;
            btnStop.IsEnabled = false;
            gpbMode.IsEnabled = true;
            gpbSize.IsEnabled = true;
            isRunning = false;
            isBreak = false;
            if (threadGame != null)
            {
                threadGame.Abort();
            }
            CreateGrid();
        }
    }
}
