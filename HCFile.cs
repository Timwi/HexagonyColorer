using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RT.Util;
using RT.Util.ExtensionMethods;
using RT.Util.Serialization;

namespace HexagonyColorer
{
    [ClassifyIgnoreIfDefault]
    sealed class HCFile
    {
        [ClassifyNotNull]
        public List<HCPath> Paths = new List<HCPath>();

        private string _source;
        public string HexagonySource { get { return _source; } set { _source = value; _grid = null; } }

        [ClassifyIgnore]
        private Grid _grid;
        public Grid Grid { get { return _grid ?? (_grid = Grid.Parse(HexagonySource)); } }

        public int GetX(PointAxial p) { return (2 * (p.Q + Grid.Size - 1) + p.R) * XTextSpacing + XPadding; }
        public int GetY(PointAxial p) { return (p.R + Grid.Size - 1) * YTextSpacing + YPadding; }

        public PointAxial FromScreen(int x, int y)
        {
            var r = (y - YPadding) / (double) YTextSpacing - (Grid.Size - 1);
            var q = ((x - XPadding) / (double) XTextSpacing - r) / 2 - (Grid.Size - 1);
            return new PointAxial((int) Math.Round(q), (int) Math.Round(r));
        }

        public int XTextSpacing = 15;
        public int YTextSpacing = 27;
        public int ArrowLength = 13;
        public int XPadding = 20;
        public int YPadding = 20;
    }
}
