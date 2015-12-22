/**
 * From: https://see.stanford.edu/materials/icspacs106b/H19-RecBacktrackExamples.pdf
 *
 */
namespace cs_sudoku
{
    internal class Solver
    {
        private readonly Grid<int> grid;
        private readonly int width;
        private readonly int height;

        public Solver(Grid<int> start)
        {
            this.grid = start;
            this.width = this.grid.Width;
            this.height = this.grid.Height;
        }

        /*
         * Function: Solve
         * ---------------------
         * Takes a partially filled-in grid and attempts to assign values to all
         * unassigned locations in such a way to meet the requirements for sudoku
         * solution (non-duplication across rows, columns, and boxes). The function
         * operates via recursive backtracking: it finds an unassigned location with
         * the grid and then considers all digits from 1 to 9 in a loop. If a digit
         * is found that has no existing conflicts, tentatively assign it and recur
         * to attempt to fill in rest of grid. If this was successful, the puzzle is
         * solved. If not, unmake that decision and try again. If all digits have
         * been examined and none worked out, return false to backtrack to previous
         * decision point.
         */
        public bool Solve(int offset)
        {

            if (offset == SudokuGrid.CELL_COUNT)
                return true; // success!

            if (this.grid.Get(offset) != SudokuGrid.UNASSIGNED)
                return this.Solve(offset + 1);

            var x = offset%SudokuGrid.DIMENSION;
            var y = offset/SudokuGrid.DIMENSION;
            for (var number = 0; number<SudokuGrid.DIMENSION + 1; ++number) // consider digits 1 to DIMENSION
            {
                if (this.IsAvailable(x, y, number)) // if looks promising,
                {
                    this.grid.Set(offset, number); // make tentative assignment
                    if (this.Solve(offset + 1))
                    {
                        return true; // recur, if success, yay!
                    }
                }
            }
            this.grid.Set(offset, SudokuGrid.UNASSIGNED); // failure, unmake & try again
            return false; // this triggers backtracking
        }

        /*
         * Function: IsAvailable
         * ---------------------
         * Returns a boolean which indicates whether it will be legal to assign
         * number to the given row,column location. As assignment is legal if it that
         * number is not already used in the row, column, or box.
         */
        private bool IsAvailable(int x, int y, int number)
        {
            return !this.IsUsedInRow(y, number)
                && !this.IsUsedInColumn(x, number)
                && !this.IsUsedInBox(x - x%SudokuGrid.BOX_DIMENSION, y - y%SudokuGrid.BOX_DIMENSION, number);
        }

        /*
         * Function: IsUsedInRow
         * -------------------
         * Returns a boolean which indicates whether any assigned entry
         * in the specified row matches the given number.
         */
        private bool IsUsedInRow(int y, int number)
        {
            var offset = y*SudokuGrid.DIMENSION;
            for (var x = 0; x < this.width; ++x)
            {
                if (this.grid.Get(offset++) == number)
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Function: IsUsedInColumn
         * -------------------
         * Returns a boolean which indicates whether any assigned entry
         * in the specified column matches the given number.
         */
        private bool IsUsedInColumn(int x, int number)
        {
            var offset = x;
            for (var y = 0; y < this.height; ++y)
            {
                if (this.grid.Get(offset) == number)
                {
                    return true;
                }
                offset += SudokuGrid.DIMENSION;
            }
            return false;
        }

        /*
         * Function: IsUsedInBox
         * -------------------
         * Returns a boolean which indicates whether any assigned entry
         * within the specified 3x3 box matches the given number.
         */
        private bool IsUsedInBox(int boxStartX, int boxStartY, int number) {
            for (var yOffset = 0; yOffset < SudokuGrid.BOX_DIMENSION; ++yOffset)
            {
                var x = boxStartX;
                var y = yOffset + boxStartY;
                var offset = x + y*SudokuGrid.DIMENSION;
                for (var xOffset = 0; xOffset < SudokuGrid.BOX_DIMENSION; ++xOffset)
                {
                    if (this.grid.Get(offset++) == number)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
