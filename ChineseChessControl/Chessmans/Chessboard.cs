using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ChineseChess
{
    public class Chessboard
    {
        public delegate void ChessPointHandler
            (ChessPoint newPoint, ChessPoint oldPoint, ChessmanBase newChessman, ChessmanBase oldChessman);
        public event ChessPointHandler ChessMoved;

        ChessmanBase[,] _chessmanMap;

        public Chessboard()
        {
            _chessmanMap = new ChessmanBase[9, 10];
        }

        public ChessmanBase GetChessman(ChessPoint point)
        {
            return _chessmanMap[point.X, point.Y];
        }

        internal void SetDefalutChessman(ChessPoint point, ChessmanBase chessman)
        {
            _chessmanMap[point.X, point.Y] = chessman;
        }

        internal void MoveChessman(ChessPoint point, ChessmanBase chessman)
        {
             ChessmanBase oldChessman = _chessmanMap[point.X, point.Y];

            _chessmanMap[chessman.Location.X, chessman.Location.Y] = null;
            _chessmanMap[point.X, point.Y] = chessman;

            if (ChessMoved != null)
                ChessMoved(point, chessman.Location, chessman, oldChessman);
        }
    }
}
