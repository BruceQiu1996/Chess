using GalaSoft.MvvmLight.Command;
using System;
using Microsoft.AspNetCore.SignalR.Client;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess.Models
{
    public class User
    {
        public string UserId { get; set; } = Guid.NewGuid().ToString();

        public Status Status { get; set; }

        public string Ip { get; set; }

        public RelayCommand GameRequestCommand { get; set; }

        public User()
        {
            GameRequestCommand = new RelayCommand(RequestGame);
        }

        private async void RequestGame()
        {
            if (UserId == Const.User.UserId) return;
            var result = await Const.Connection.InvokeAsync<string>("GameRequest", Const.User.UserId, UserId);
            MessageBox.Show(result, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    public enum Status
    {
        FREE = 0,
        WORKING = 1,
    }
}
