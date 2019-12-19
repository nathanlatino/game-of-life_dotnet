using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace GOL
{
    public class GameOfLife
    {
        private readonly Board Board;
        private readonly List<Cell> marked;
        private readonly int updateSpeed;
        private int iterations;
        private bool isRunning;

        public GameOfLife(int width,
                          int height,
                          bool random = false,
                          int updateSpeed = 300) {
            int Side(int side) => side > 10 ? side : 10;
            Board = new Board(Side(width), Side(height), random);
            this.updateSpeed = updateSpeed;
            marked = new List<Cell>();
        }

        public Board GetBoard() {
            return Board;    
        }

        public void Start() {
            isRunning = true;
            MainLoop();
        }

        public List<Cell> iterate() {
            MarkCells();
            UpdateValues();
            UpdateCells();
            return marked;
        }

        private void MainLoop() {
            while (isRunning) {
                Console.WriteLine(this);
                MarkCells();
                UpdateValues();
                UpdateCells();
                Thread.Sleep(updateSpeed);
            }
            Console.WriteLine("Simulation ended.");
        }

        private void MarkCells() {
            marked.Clear();
            foreach (var cell in Board.Cells) {
                var neighborsAlive = NeighborsAlive(cell);
                if (cell.IsSet) {
                    if (neighborsAlive < 2 || neighborsAlive > 3)
                        marked.Add(cell);
                }
                else {
                    if (neighborsAlive == 3)
                        marked.Add(cell);
                }
            }
        }
        
        private void UpdateValues() {
            isRunning = marked.Count > 0;
            iterations++;
        }

        private void UpdateCells() {
            marked.ForEach(i => i.UpdateState());
        }

        private int NeighborsAlive(Cell cell) {
            return Board.Neighbors(cell).Count(c => Board[c.X, c.Y].IsSet);
        }

        public override string ToString() {
            var alives = $"Alive: {Cell.Count}";
            var it = $"Iterations: {iterations}";
            return $"\n{Board}\n{alives}\t{it}";
        }
    }
}