using System.Collections.Generic;
using System.Linq;

namespace GOL
{
    public class Board : Grid
    {
        public Board(int width, int height, bool random)
            : base(width, height, random) { }

        public override string ToString() {
            var str = "";
            IterateOverCells((x, y) => {
                const string on = " # ";
                const string off = " . ";
                str += (x == 0 ? "\n" : "") + (Cells[x, y].IsSet ? on : off);
            });
            return str;
        }

        private bool InBounds(Cell cell) {
            bool x = 0 <= cell.X && cell.X < Width;
            bool y = 0 <= cell.Y && cell.Y < Height;
            return x && y;
        }

        public List<Cell> Neighbors(Cell cell) {
            return cell.Neighbors.Where(InBounds).ToList();
        }
    }
}