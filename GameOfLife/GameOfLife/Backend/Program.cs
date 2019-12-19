using System;

namespace GOL
{
    class Program
    {
        static void Main(string[] args) {
            
            const int width = 30;
            const int height = 20;
            const bool random = true;
            const int updateSpeed = 200;
            
            new GameOfLife(width, height, random, updateSpeed).Start();
        }
    }
}