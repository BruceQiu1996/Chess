using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess.Models
{
    public abstract class ChessBase
    {
        public Point Location { get; set; }

        public Point LastLocation { get; set; }

        public bool IsAlive { get; set; }

        public bool IsBlack { get; set; }

        protected abstract bool Action();
    }
}
