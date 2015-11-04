using System.Drawing;
using RT.Util.ExtensionMethods;
using RT.Util.Serialization;

namespace HexagonyColorer
{
    [ClassifyIgnoreIfDefault]
    sealed class HCPath
    {
        public string Name;
        public Color Color;
        public bool DrawStart;
        public bool DrawEnd;
        public PointAxial IpStartPos;
        [ClassifyNotNull]
        public Direction IpStartDirection = Direction.East;
        public int? NumInstructions;
        public bool? TakeBranch;
        public bool StopAtIpChanger;

        public override string ToString()
        {
            return @"{0}{1}{2}{3} {4}{5}{6}".Fmt(
                /* 0 */ DrawStart ? ">" : "|",
                /* 1 */ IpStartPos,
                /* 2 */ IpStartDirection,
                /* 3 */ DrawEnd ? ">" : "|",
                /* 4 */ Name,
                /* 5 */ NumInstructions == null ? "" : ", max=" + NumInstructions.Value,
                /* 6 */ TakeBranch == null ? "" : ", " + TakeBranch.Value
            );
        }
    }
}
