namespace cs_sudoku
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal class SudokuGrid : Grid<int>
    {
        public const int UNASSIGNED = 0;
        public const int DIMENSION = 9;
        public const int CELL_COUNT = DIMENSION * DIMENSION;
        public const int WIDTH = DIMENSION;
        public const int HEIGHT = DIMENSION;
        public const int BOX_DIMENSION = 3;

        private List<HashSet<int>> possibles = new List<HashSet<int>>();
        private List<int> offsets = new List<int>();

        public SudokuGrid(int[] initial)
        : base(WIDTH, HEIGHT, initial)
        {
            var numbers = new HashSet<int>();
            for (var i = 1; i < DIMENSION + 1; ++i)
            {
                numbers.Add(i);
            }

            for (var offset = 0; offset < CELL_COUNT; ++offset)
            {
                if (this.Get(offset) == UNASSIGNED)
                {
                    this.possibles.Add(new HashSet<int>(numbers));
                }
                else
                {
                    this.possibles.Add(new HashSet<int>());
                }
            }
        }

        public HashSet<int> GetPossibilities(int offset)
        {
            return this.possibles[offset];
        }

        public int GetOffset(int index)
        {
            if (index + 1 > this.GetOffsetCount())
            {
                return -1;
            }
            return this.offsets[index];
        }

        public int GetOffsetCount()
        {
            return this.offsets.Count;
        }

        public void Eliminate()
        {
            do
            {
                this.EliminateAssigned();
                this.EliminateDangling();
            } while (this.TransferSingularPossibilities());

            for (var i = 0; i < CELL_COUNT; ++i)
            {
                var possible = this.possibles[i];
                if (possible.Count > 1)
                {
                    this.offsets.Add(i);
                }
            }
        }

        private void EliminateDangling()
        {
            this.EliminateRowDangling();
            this.EliminateColumnDangling();
            this.EliminateBoxDangling();
        }

        private void EliminateRowDangling()
        {
            for (var y = 0; y < HEIGHT; ++y)
            {
                var offset = y * DIMENSION;
                var counters = new Dictionary<int, List<int>>();
                for (var x = 0; x < WIDTH; ++x)
                {
                    this.AdjustPossibleCounters(counters, offset++);
                }
                this.TransferCountedEliminations(counters);
            }
        }

        private void EliminateColumnDangling()
        {
            for (var x = 0; x < WIDTH; ++x)
            {
                var offset = x;
                var counters = new Dictionary<int, List<int>>();
                for (var y = 0; y < HEIGHT; ++y)
                {
                    this.AdjustPossibleCounters(counters, offset);
                    offset += DIMENSION;
                }
                this.TransferCountedEliminations(counters);
            }
        }

        private void EliminateBoxDangling()
        {
            for (var y = 0; y < HEIGHT; y += BOX_DIMENSION)
            {
                for (var x = 0; x < WIDTH; x += BOX_DIMENSION)
                {
                    var counters = new Dictionary<int, List<int>>();

                    var boxStartX = x - x % BOX_DIMENSION;
                    var boxStartY = y - y % BOX_DIMENSION;

                    for (var yOffset = 0; yOffset < BOX_DIMENSION; ++yOffset)
                    {
                        var boxY = yOffset + boxStartY;
                        var offset = boxStartX + boxY * DIMENSION;
                        for (var xOffset = 0; xOffset < BOX_DIMENSION; ++xOffset)
                        {
                            this.AdjustPossibleCounters(counters, offset++);
                        }
                    }
                    this.TransferCountedEliminations(counters);
                }
            }
        }

        private void TransferCountedEliminations(Dictionary<int, List<int>> counters)
        {
            foreach (var counter in counters)
            {
                var cells = counter.Value;
                if (cells.Count == 1)
                {
                    var number = counter.Key;
                    var cell = cells[0];
                    var possible = this.possibles[cell];
                    possible.Clear();
                    possible.Add(number);
                }
            }
        }

        private void AdjustPossibleCounters(Dictionary<int, List<int>> counters, int offset)
        {
            foreach (var possible in this.possibles[offset])
            {
                List<int> counter;
                if (!counters.TryGetValue(possible, out counter))
                {
                    counter = new List<int>();
                    counters.Add(possible, counter);
                }
                counter.Add(offset);
            }
        }

        private void EliminateAssigned()
        {
            for (var y = 0; y < HEIGHT; ++y)
            {
                for (var x = 0; x < WIDTH; ++x)
                {
                    var number = this.Get(x, y);
                    if (number != UNASSIGNED)
                    {
                        this.ClearRowPossibles(y, number);
                        this.ClearColumnPossibles(x, number);
                        this.ClearBoxPossibilities(x - x % BOX_DIMENSION, y - y % BOX_DIMENSION, number);
                    }
                }
            }
        }

        private bool TransferSingularPossibilities()
        {
            var transfer = false;
            for (var offset = 0; offset < CELL_COUNT; ++offset)
            {
                var possible = this.possibles[offset];
                if (possible.Count == 1)
                {
                    var singular = possible.First();
                    this.Set(offset, singular);
                    possible.Clear();
                    transfer = true;
                }
            }
            return transfer;
        }

        private void ClearRowPossibles(int y, int number)
        {
            var offset = y * DIMENSION;
            for (var x = 0; x < WIDTH; ++x)
            {
                var possible = this.possibles[offset];
                possible.Remove(number);
                offset++;
            }
        }

        private void ClearColumnPossibles(int x, int number)
        {
            var offset = x;
            for (var y = 0; y < HEIGHT; ++y)
            {
                var possible = this.possibles[offset];
                possible.Remove(number);
                offset += DIMENSION;
            }
        }

        private void ClearBoxPossibilities(int boxStartX, int boxStartY, int number)
        {
            for (var yOffset = 0; yOffset < BOX_DIMENSION; ++yOffset)
            {
                var y = yOffset + boxStartY;
                var offset = boxStartX + y * DIMENSION;
                for (var xOffset = 0; xOffset < BOX_DIMENSION; ++xOffset)
                {
                    var possible = this.possibles[offset];
                    possible.Remove(number);
                    offset++;
                }
            }
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