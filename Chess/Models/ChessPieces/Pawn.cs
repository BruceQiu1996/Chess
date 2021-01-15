using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Chess.Models.ChessPieces
{
    public class Pawn : ChessBase
    {
        public string Name { get; set; } = "兵";

        protected override bool Action()
        {
            return false;
        }
    }
}
