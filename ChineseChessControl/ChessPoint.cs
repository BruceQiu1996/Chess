using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChineseChess
{
    [Serializable]
    public class ChessPoint
    {
        public ChessPoint()
        {
 
        }

        public ChessPoint(int x, int y) 
        {
            X = x;
            Y = y;
        }

        public int X { get; set; }
        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            ChessPoint  chessPoint = obj as ChessPoint;
            return chessPoint.X == X && chessPoint.Y == Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
