using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChineseChessControl.Properties;

namespace ChineseChess
{
    public class Elephant : Minister
    {
        public Elephant(ChessPoint point, Chessboard board,bool isRed)
            : base(point, board,isRed)
        {
            
        }

        public override string Name
        {
            get
            {
                return Resources.Elephant;
            }
            protected set
            {
                //base.Name = value;
            }
        }
    }
}
