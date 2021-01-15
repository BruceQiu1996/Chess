using Chess.Models;
using ChineseChess;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class Const
    {
        internal static User User;
        /// <summary>
        /// 连接对象唯一
        /// </summary>
        private static HubConnection connection;

        public static HubConnection Connection => connection ?? (connection = new HubConnectionBuilder() //mysignalr address you can replace yours
                .WithUrl("http://47.101.171.149:20001/chathub", option =>
                {
                    option.CloseTimeout = TimeSpan.FromSeconds(60);
                })
                .Build());
    }
}
