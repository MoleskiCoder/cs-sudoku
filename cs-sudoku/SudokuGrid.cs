﻿namespace cs_sudoku
{
    using System;
    using System.Text;

    internal class SudokuGrid : Grid<int>
    {
        public const int UNASSIGNED = 0;
        public const int DIMENSION = 9;
        public const int CELL_COUNT = DIMENSION * DIMENSION;
        public const int WIDTH = DIMENSION;
        public const int HEIGHT = DIMENSION;
        public const int BOX_DIMENSION = 3;

        public SudokuGrid(int[] initial)
        : base(WIDTH, HEIGHT, initial)
        {
        }

        public override string ToString() 
        {
            var output = new StringBuilder();
            output.Append('\n');
            var height = this.Height;
            for (var y = 0; y < height; ++y)
            {
                var width = this.Width;
                for (var x = 0; x < width; ++x)
                {
                    var number = this.Get(x, y);
                    output.Append(' ');
                    if (number == UNASSIGNED)
                    {
                        output.Append('-');
                    }
                    else
                    {
                        output.Append(number);
                    }
                    output.Append(' ');
                    if ((x + 1) % BOX_DIMENSION == 0 && x + 1 < width)
                    {
                        output.Append('|');
                    }
                }
                if ((y + 1) % BOX_DIMENSION == 0 && y + 1 < width)
                {
                    output.Append("\n --------+---------+--------");
                }
                output.Append('\n');
            }
            return output.ToString();
        }
    }
}