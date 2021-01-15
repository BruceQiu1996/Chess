using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChineseChessControl.Properties;

namespace ChineseChess
{
    public class Pawn:ChessmanBase
    {
        List<Func<ChessPoint,bool>> canMoveFunc;

        public Pawn(ChessPoint point, Chessboard board, bool isRed)
            : base(point, board,isRed)
        {
            canMoveFunc = new List<Func<ChessPoint, bool>>(5);

            canMoveFunc.Add(MoveInStraight);
            canMoveFunc.Add(ContainSize);
            canMoveFunc.Add(MoveStep);
            canMoveFunc.Add(MoveFrontOnly);
            canMoveFunc.Add(CanHorizontalMove);
        }
        protected sealed override bool CheckPoint(ChessPoint chessPoint)
        {
            foreach (var item in canMoveFunc)
            {
                if (!item(chessPoint))
                    return false;
            }

            return true;
        }

        bool MoveFrontOnly(ChessPoint chessPoint)
        {
            if (chessPoint.Y == Location.Y)
                return true;

            return defalutPoint.Y > 4 ? chessPoint.Y < Location.Y : chessPoint.Y > Location.Y;
        }

        bool CanHorizontalMove(ChessPoint chessPoint)
        {
            if(chessPoint.X == Location.X)
                return true;

            return defalutPoint.Y > 4 ? Location.Y < 5 : Location.Y > 4;
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
            return x < 0 || x > 8;
        }

        protected sealed override bool YIsBeyond(double y)
        {
            return y < 0 || y > 9;
        }

        public override string Name
        {
            get
            {
                return Resources.Pawn;
            }
            protected set
            {
                //base.Name = value;
            }
        }
    }
}
