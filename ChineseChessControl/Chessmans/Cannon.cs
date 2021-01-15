using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChineseChessControl.Properties;

namespace ChineseChess
{
    public class Cannon:ChessmanBase
    {
        public Cannon(ChessPoint point, Chessboard board, bool isRed)
            : base(point, board,isRed)
        {
            
        }

        protected sealed override bool CheckPoint(ChessPoint chessPoint)
        {
            if (!MoveInStraight(chessPoint))
                return false;

            if (!ContainSize(chessPoint))
                return false;

            if (this.Board.GetChessman(chessPoint) != null)
            {
                if (chessPoint.X != Location.X)
                {
                    if (!canXEat(chessPoint))
                        return false;
                }
                else
                {
                    if (!canYEat(chessPoint))
                        return false;
                }
            }
            else
            {
                if (AnyOneInRoad(chessPoint))
                    return false;
            }

            return true;
        }

        bool canXEat(ChessPoint chessPoint)
        {
            int maxX = Math.Max(chessPoint.X, Location.X);
            int minX = Math.Min(chessPoint.X, Location.X);
            int i = 0;
            for (minX++; minX < maxX && i < 2; minX++)
            {
                if (this.Board.GetChessman(new ChessPoint(minX, chessPoint.Y)) != null)
                {
                    i++;
                }
            }

            return i==1;
        }

        bool canYEat(ChessPoint chessPoint)
        {
            int maxY = Math.Max(chessPoint.Y, Location.Y);
            int minY = Math.Min(chessPoint.Y, Location.Y);
            int i = 0;
            for (minY++; minY < maxY && i < 2; minY++)
            {
                if (this.Board.GetChessman(new ChessPoint(chessPoint.X, minY)) != null)
                {
                    i++;
                }
            }

            return i == 1;
        }

        private bool MoveInStraight(ChessPoint chessPoint)
        {
            return !(chessPoint.X != Location.X && chessPoint.Y != Location.Y);
        }

        public override string Name
        {
            get
            {
                return Resources.Cannon;
            }
            protected set
            {
                //base.Name = value;
            }
        }
    }
}
