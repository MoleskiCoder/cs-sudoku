﻿namespace cs_sudoku
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

        public void Set(Coordinate coordinate, T value)
        {
            this.values[this.GetOffset(coordinate)] = value;
        }

        public void Set(int x, int y, T value)
        {
            this.values[this.GetOffset(x, y)] = value;
        }

        public T Get(Coordinate coordinate)
        {
            return this.values[this.GetOffset(coordinate)];
        }

        public T Get(int x, int y)
        {
            return this.values[this.GetOffset(x, y)];
        }

        private int GetOffset(Coordinate coordinate)
        {
            return this.GetOffset(coordinate.X, coordinate.Y);
        }

        private int GetOffset(int x, int y)
        {
            return x + y * this.Width;
        }
    }
}