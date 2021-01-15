using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChineseChessControl.Properties;

namespace ChineseChess
{
    public class  Minister: ChessmanBase
    {
        List<Func<ChessPoint>> canMovePoint;

        public Minister(ChessPoint point, Chessboard board, bool isRed)
            : base(point, board,isRed)
        {
            canMovePoint = new List<Func<ChessPoint>>(8);

            canMovePoint.Add(Northwest);
            canMovePoint.Add(Northeast);

            canMovePoint.Add(Southeast);
            canMovePoint.Add(Southwest);
        }

        ChessPoint Northwest()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X - 2;
            int y = currentPoint.Y - 2;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;
            if (Board.GetChessman(new ChessPoint(currentPoint.X - 1, currentPoint.Y - 1)) != null)
                return null;

            return new ChessPoint(x, y);
        }

        ChessPoint Northeast()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X + 2;
            int y = currentPoint.Y - 2;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;
            if (Board.GetChessman(new ChessPoint(currentPoint.X + 1, currentPoint.Y - 1)) != null)
                return null;

            return new ChessPoint(x, y);
        }

        ChessPoint Southeast()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X + 2;
            int y = currentPoint.Y + 2;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;
            if (Board.GetChessman(new ChessPoint(currentPoint.X + 1, currentPoint.Y + 1)) != null)
                return null;

            return new ChessPoint(x, y);
        }

        ChessPoint Southwest()
        {
            ChessPoint currentPoint = this.Location;

            int x = currentPoint.X - 2;
            int y = currentPoint.Y + 2;

            if (XIsBeyond(x) || YIsBeyond(y))
                return null;
            if (Board.GetChessman(new ChessPoint(currentPoint.X - 1, currentPoint.Y + 1)) != null)
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

        protected sealed override bool YIsBeyond(double y)
        {
            return defalutPoint.Y > 4 ? y < 4 || y > 9 : y > 5 || y < 0;
        }

        public override string Name
        {
            get
            {
                return Resources.Minister;
            }
            protected set
            {
                //base.Name = value;
            }
        }

    }
}
