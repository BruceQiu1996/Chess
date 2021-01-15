using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChineseChessControl.Properties;

namespace ChineseChess
{
    public class Rook:ChessmanBase
    {
        public Rook(ChessPoint point, Chessboard board, bool isRed)
            : base(point, board,isRed)
        {
            
        }

        protected sealed override bool CheckPoint(ChessPoint chessPoint)
        {
            if (!MoveInStraight(chessPoint))
                return false;

            if (!ContainSize(chessPoint))
                return false;

            if (AnyOneInRoad(chessPoint))
                return false;
            return true;
        }

        private bool MoveInStraight(ChessPoint chessPoint)
        {
            return !(chessPoint.X != Location.X && chessPoint.Y != Location.Y);
        }

        public override string Name
        {
            get
            {
                return Resources.Rook;
            }
            protected set
            {
                //base.Name = value;
            }
        }
    }
}


