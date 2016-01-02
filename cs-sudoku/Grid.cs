namespace cs_sudoku
{
    using System;

    internal class Grid<T>
    {
        private readonly T[] values;

        public Grid(int gridWidth, int gridHeight, T[] initial)
        {
            this.Width = gridWidth;
            this.Height = gridHeight;
            this.values = initial;
            int size = initial.Length;
            if (this.Width * this.Height != size)
            {
                throw new InvalidOperationException("initial array is the wrong size.");
            }
        }

        public int Height { get; }

        public int Width { get; }

        public void Set(int x, int y, T value)
        {
            this.Set(this.GetOffset(x, y), value);
        }

        public void Set(int offset, T value)
        {
            this.values[offset] = value;
        }

        public T Get(int x, int y)
        {
            return this.Get(this.GetOffset(x, y));
        }

        public T Get(int offset)
        {
            return this.values[offset];
        }

        private int GetOffset(int x, int y)
        {
            return x + y * this.Width;
        }
    }
}
