using System;
using System.Collections.Generic;
using System.Linq;
using RT.Util;
using RT.Util.ExtensionMethods;

namespace HexagonyColorer
{
    class Grid
    {
        public int Size { get; private set; }
        private char[][] _grid;

        public IEnumerable<Tuple<PointAxial, char>> Everything
        {
            get
            {
                for (int i = 0; i < _grid.Length; i++)
                    for (int j = 0; j < _grid[i].Length; j++)
                        yield return new Tuple<PointAxial, char>(indexToAxial(i, j), _grid[i][j]);
            }
        }

        private Grid(int size, int fileLength, char[] data)
        {
            Size = size;
            var i = 0;
            _grid = Ut.NewArray(2 * size - 1, j => Ut.NewArray(2 * size - 1 - Math.Abs(size - 1 - j), _ => i >= data.Length ? '.' : data[i++]));
        }

        public static Grid Parse(string input)
        {
            var bare = input.Where(c => c != '`' && !char.IsWhiteSpace(c)).ToArray();
            var size = 1;
            while (3 * size * (size - 1) + 1 < bare.Length)
                size++;
            return new Grid(size, input.Length, bare);
        }

        public char this[PointAxial coords]
        {
            get
            {
                var tup = axialToIndex(coords);
                return tup == null ? '.' : _grid[tup.Item1][tup.Item2];
            }
            set
            {
                var tup = axialToIndex(coords);
                if (tup != null)
                    _grid[tup.Item1][tup.Item2] = value;
            }
        }

        private Tuple<int, int> axialToIndex(PointAxial coords)
        {
            var x = coords.Q;
            var z = coords.R;
            var y = -x - z;
            if (Ut.Max(Math.Abs(x), Math.Abs(y), Math.Abs(z)) >= Size)
                return null;

            var i = z + Size - 1;
            var j = x + Math.Min(i, Size - 1);
            return Tuple.Create(i, j);
        }

        private PointAxial indexToAxial(int i, int j)
        {
            return new PointAxial(j - Math.Min(i, Size - 1), i - Size + 1);
        }

        public override string ToString()
        {
            return _grid.Select(line => new string(' ', 2 * Size - line.Length) + line.JoinString(" ")).JoinString(Environment.NewLine);
        }
    }
}
