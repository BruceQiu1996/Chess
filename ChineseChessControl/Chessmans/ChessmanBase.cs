using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ChineseChess
{
    public class ChessmanBase
    {
        protected ChessPoint defalutPoint;
        public Chessboard Board { set; protected get; }
        public  bool IsRed { get; set; }
        public virtual string Name { get; protected set; }

        public ChessmanBase(ChessPoint point, Chessboard board,bool isRed)
        {
            IsRed = isRed;
            Board = board;
            this.Board.SetDefalutChessman(point, this);
            _location = point;
            defalutPoint = point;
            
        }

       private ChessPoint _location;
       public ChessPoint Location
       {
           get
           {
               return _location;
           }
           set
           {
               ChessmanBase tempChessmanBase = this.Board.GetChessman(value);
               //只能移动到空位或吃异色棋子
               if (tempChessmanBase==null||tempChessmanBase.IsRed != this.IsRed)
               {
                   if (CheckPoint(value))
                   {
                       this.Board.MoveChessman(value, this);
                       _location = value;
                   }
               }
               
           }
       }

       protected virtual bool CheckPoint(ChessPoint chessPoint)
       {
           return true;
       }

       protected bool ContainSize(ChessPoint chessPoint)
       {
           if (XIsBeyond(chessPoint.X) || YIsBeyond(chessPoint.Y))
               return false;
           return true;
       }

       protected bool canXMove(ChessPoint chessPoint)
       {
           int maxX = Math.Max(chessPoint.X, Location.X);
           int minX = Math.Min(chessPoint.X, Location.X);

           for (minX++; minX < maxX; minX++)
           {
               if (this.Board.GetChessman(new ChessPoint(minX, chessPoint.Y)) != null)
                   return false;
           }

           return true;
       }

       protected bool AnyOneInRoad(ChessPoint chessPoint)
       {
           if (chessPoint.X != Location.X)
           {
               if (!canXMove(chessPoint))
                   return true;
           }
           else
           {
               if (!canYMove(chessPoint))
                   return true;
           }

           return false;
       }

       protected bool canYMove(ChessPoint chessPoint)
       {
           int maxY = Math.Max(chessPoint.Y, Location.Y);
           int minY = Math.Min(chessPoint.Y, Location.Y);

           for (minY++; minY < maxY; minY++)
           {
               if (this.Board.GetChessman(new ChessPoint(chessPoint.X, minY)) != null)
                   return false;
           }

           return true;
       }

       protected virtual bool XIsBeyond(double x)
       {
           return x < 0 || x > 8;
       }

       protected virtual bool YIsBeyond(double y)
       {
           return y < 0 || y > 9;
       }

    }
}
