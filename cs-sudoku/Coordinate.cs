namespace cs_sudoku
{
    internal class Coordinate
    {
        public Coordinate()
        {
            this.X = -1;
            this.Y = -1;
        }

        public Coordinate(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; set; }

        public int Y { get; set; }
    }
}
