using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Shapes;

namespace GOL
{
    public delegate void Del(int x, int y);

    public class Grid
    {
        public readonly Cell[,] Cells;
        protected readonly int Width;
        protected readonly int Height;
        private readonly bool IsRandom;

        public Grid(int width, int height, bool random) {
            Width = width;
            Height = height;
            IsRandom = random;
            Cells = new Cell[this.Width, this.Height];
            InitCells();
        }

        public Cell this[int x, int y] => Cells[x, y];

        protected void IterateOverCells(Del f) {
            List<int> Range(int n) => Enumerable.Range(0, n).ToList();
            Range(Height).ForEach(y => Range(Width).ForEach(x => f(x, y)));
        }

        private void InitCells() {
            Random rand = new Random();
            IterateOverCells((x, y) => {
                Cells[x, y] = new Cell(x, y);
                Cells[x, y].SetNeighbors();
                if (IsRandom && rand.Next(3) == 0)
                    Cells[x, y].UpdateState();
            });
        }
        
    }
}
