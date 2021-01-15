using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChessServer
{
    public class Item
    {
        public string  Id { get; set; }

        public User user { get; set; }
    }

    public class User
    {
        public string UserId { get; set; }

        public Status Status { get; set; }

        public string Ip { get; set; }
    }

    public enum Status 
    {
        FREE=0,
        WORKING=1,
    }
}
