namespace GOL
{
    public class Point2D {
        public int X { get; }
        public int Y { get; }


        public Point2D(int x, int y) {
            X = x;
            Y = y;
        }

        public static bool operator ==(Point2D obj1, Point2D obj2) {
            return obj1.X == obj2.X && obj1.Y == obj2.Y;
        }

        public static bool operator !=(Point2D obj1, Point2D obj2) {
            return obj1.X != obj2.X || obj1.Y != obj2.Y;
        }
    }

}