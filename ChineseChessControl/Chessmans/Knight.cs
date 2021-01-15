using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ChineseChessControl.Properties;

namespace ChineseChess
{
    public class Knight : ChessmanBase
    {
        List<Func<ChessPoint>> canMovePoint;

        public Knight(ChessPoint point, Chessboard board, bool isRed)
            : base(point, board,isRed)
        {
            canMovePoint = new List<Func<ChessPoint>>(8);

            canMovePoint.Add(Left_Buttom);
            canMovePoint.Add(Left_Up);

            canMovePoint.Add(Up_Left);
            canMovePoint.Add(Up_Right);

            canMovePoint.Add(Right_Up);
            canMovePoint.Add(Right_Bottom);

            canMovePoint.Add(Bottom_Right);
            canMovePoint.Add(Bottom_Left);
        }

        ChessPoint Left_Buttom()
        {
            ChessPoint currentPoint  = this.Location;

            int x = currentPoint.X - 2;
            int y = currentPoint.Y + 1;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;

            if(Board.GetChessman(new ChessPoint(currentPoint.X-1,currentPoint.Y))!=null)
                return null;

            return new ChessPoint(x, y);
        }

        ChessPoint Left_Up()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X - 2;
            int y = currentPoint.Y - 1;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;

            if (Board.GetChessman(new ChessPoint(currentPoint.X - 1, currentPoint.Y)) != null)
                return null;

            return new ChessPoint(x, y);
        }

        ChessPoint Up_Left()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X + 1;
            int y = currentPoint.Y - 2;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;
            if (Board.GetChessman(new ChessPoint(currentPoint.X, currentPoint.Y - 1)) != null)
                return null;

            return new ChessPoint(x, y);
        }

        ChessPoint Up_Right()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X - 1;
            int y = currentPoint.Y - 2;

            if (XIsBeyond(x)||YIsBeyond(y))
                return null;

            if (Board.GetChessman(new ChessPoint(currentPoint.X , currentPoint.Y-1)) != null)
                return null;

            return new ChessPoint(x, y);
        }

        ChessPoint Right_Up()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X + 2;
            int y = currentPoint.Y - 1;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;

            if (Board.GetChessman(new ChessPoint(currentPoint.X + 1, currentPoint.Y)) != null)
                return null;

            return new ChessPoint(x, y);
        }

        ChessPoint Right_Bottom()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X + 2;
            int y = currentPoint.Y + 1;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;
            if (Board.GetChessman(new ChessPoint(currentPoint.X + 1, currentPoint.Y)) != null)
                return null;

            return new ChessPoint(x, y);
        }

        ChessPoint Bottom_Right()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X + 1;
            int y = currentPoint.Y + 2;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;
            if (Board.GetChessman(new ChessPoint(currentPoint.X, currentPoint.Y + 1)) != null)
                return null;

            return new ChessPoint(x, y);
        }

        ChessPoint Bottom_Left()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X - 1;
            int y = currentPoint.Y + 2;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;
            if (Board.GetChessman(new ChessPoint(currentPoint.X, currentPoint.Y + 1)) != null)
                return null;

            return new ChessPoint(x, y);
        }

        protected sealed override bool CheckPoint(ChessPoint chessPoint)
        {
            foreach (var item in canMovePoint)
            {
                ChessPoint result = item();
                if (result != null && result.Equals(chessPoint))
                    return true;
            }

            return false;
        }

        public override string Name
        {
            get
            {
                return Resources.Knight;
            }
            protected set
            {
                //base.Name = value;
            }
        }
    }
}
