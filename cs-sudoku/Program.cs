namespace cs_sudoku
{
    using System;
    using System.Globalization;

    class Program
    {
        static void Main(string[] args)
        {
            // http://www.telegraph.co.uk/news/science/science-news/9359579/Worlds-hardest-sudoku-can-you-crack-it.html
            int[] data = {
                8, 0, 0, 0, 0, 0, 0, 0, 0,
                    0, 0, 3, 6, 0, 0, 0, 0, 0,
                    0, 7, 0, 0, 9, 0, 2, 0, 0,
                    0, 5, 0, 0, 0, 7, 0, 0, 0,
                    0, 0, 0, 0, 4, 5, 7, 0, 0,
                    0, 0, 0, 1, 0, 0, 0, 3, 0,
                    0, 0, 1, 0, 0, 0, 0, 6, 8,
                    0, 0, 8, 5, 0, 0, 0, 1, 0,
                    0, 9, 0, 0, 0, 0, 4, 0, 0
            };

            var puzzle = new SudokuGrid(data);
            var solver = new Solver(puzzle);

            var start = DateTime.Now;
            var solved = solver.Solve();
            var finish = DateTime.Now;

            if (solved)
            {
                Console.Out.WriteLine(puzzle);

                var elapsedTime = finish - start;
                var elapsed = elapsedTime.Milliseconds;
                var seconds = elapsed / 1000.0 + (elapsed % 1000L) / 1000.0;

                Console.Out.WriteLine(string.Format(CultureInfo.CurrentCulture, "\n\nTime taken {0} seconds\n", seconds));
            }
            else
            {
                Console.Out.WriteLine("No solution exists");
            }
        }
    }
}