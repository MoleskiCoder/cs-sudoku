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
        public bool Solve()
        {
            var coordinate = new Coordinate();
            if (!this.FindUnassignedLocation(coordinate))
            {
                return true; // success!
            }
            for (var number = 1; number < 10; ++number) // consider digits 1 to 9
            {
                if (this.IsAvailable(coordinate, number)) // if looks promising,
                {
                    this.grid.Set(coordinate, number); // make tentative assignment
                    if (this.Solve())
                    {
                        return true; // recur, if success, yay!
                    }
                    this.grid.Set(coordinate, SudokuGrid.UNASSIGNED); // failure, unmake & try again
                }
            }
            return false; // this triggers backtracking
        }

        /*
         * Function: FindUnassignedLocation
         * --------------------------------
         * Searches the grid to find an entry that is still unassigned. If found,
         * the reference parameters row, column will be set the location that is
         * unassigned, and true is returned. If no unassigned entries remain, false
         * is returned.
         */
        private bool FindUnassignedLocation(Coordinate coordinate) {
            for (var y = 0; y < this.height; ++y)
            {
                coordinate.Y = y;
                for (var x = 0; x < this.width; ++x)
                {
                    coordinate.X = x;
                    if (this.grid.Get(coordinate) == SudokuGrid.UNASSIGNED)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /*
         * Function: IsAvailable
         * ---------------------
         * Returns a boolean which indicates whether it will be legal to assign
         * number to the given row,column location. As assignment is legal if it that
         * number is not already used in the row, column, or box.
         */
        private bool IsAvailable(Coordinate coordinate, int number)
        {
            var x = coordinate.X;
            var y = coordinate.Y;
            return !this.IsUsedInRow(y, number)
                && !this.IsUsedInColumn(x, number)
                && !this.IsUsedInBox(x - x % 3, y - y % 3, number);
        }

        /*
         * Function: IsUsedInRow
         * -------------------
         * Returns a boolean which indicates whether any assigned entry
         * in the specified row matches the given number.
         */
        private bool IsUsedInRow(int y, int number)
        {
            for (var x = 0; x < this.width; ++x)
            {
                if (this.grid.Get(x, y) == number)
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
            for (var y = 0; y < this.height; ++y)
            {
                if (this.grid.Get(x, y) == number)
                {
                    return true;
                }
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
            for (var y = 0; y < 3; ++y)
            {
                for (var x = 0; x < 3; ++x)
                {
                    if (this.grid.Get(x + boxStartX, y + boxStartY) == number)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
