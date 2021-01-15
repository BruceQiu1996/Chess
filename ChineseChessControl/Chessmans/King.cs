using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChineseChessControl.Properties;

namespace ChineseChess
{
    public class King:ChessmanBase
    {
        public King(ChessPoint point, Chessboard board, bool isRed)
            : base(point, board, isRed)
        {
            
        }

        protected sealed override bool CheckPoint(ChessPoint chessPoint)
        {
            if (!IsEnemyKing(chessPoint))
            {
                if (!MoveInStraight(chessPoint))
                    return false;

                if (!ContainSize(chessPoint))
                    return false;

                if (!MoveStep(chessPoint))
                    return false;
            }
            else
            {
                if (AnyOneInRoad(chessPoint))
                    return false;
            }

            return true;
        }

        bool IsEnemyKing(ChessPoint chessPoint)
        {
            return Board.GetChessman(chessPoint) is King;
        }


        bool MoveStep(ChessPoint chessPoint)
        {
            return Math.Abs(chessPoint.X-Location.X)<2&&Math.Abs(chessPoint.Y-Location.Y)<2;
        }

        private bool MoveInStraight(ChessPoint chessPoint)
        {
            return !(chessPoint.X != Location.X && chessPoint.Y != Location.Y);
        }

        protected sealed override bool XIsBeyond(double x)
        {
            return x < 3 || x > 5;
        }

        protected sealed override bool YIsBeyond(double y)
        {
            return defalutPoint.Y > 4 ? (y < 7 || y > 9) : (y < 0 || y > 2);
        }

        public override string Name
        {
            get
            {
                return Resources.King;
            }
            protected set
            {
                //base.Name = value;
            }
        }
    }
}
