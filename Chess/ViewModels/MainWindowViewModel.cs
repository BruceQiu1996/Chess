using Chess.Models;
using ChineseChess;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Chess.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly string Login = nameof(Login);
        private readonly string GameRequest = nameof(GameRequest);
        private readonly string ReceiveRequest = nameof(ReceiveRequest);
        private readonly string BeginGame = nameof(BeginGame);
        private readonly string RequestList = nameof(RequestList);
        public RelayCommand LoadCommand { get; set; }
        public RelayCommand RefreshListCommand { get; set; }

        private ObservableCollection<User> users;
        public ObservableCollection<User> Users
        {
            get { return this.users; }
            set
            {
                users = value;
                RaisePropertyChanged(nameof(Users));
            }
        }

        private string message;
        public string Message
        {
            get { return this.message; }
            set
            {
                message = value;
                RaisePropertyChanged(nameof(Message));
            }
        }

        public MainWindowViewModel()
        {
            LoadCommand = new RelayCommand(Load);
            RefreshListCommand = new RelayCommand(Refresh);
        }

        private async void Load()
        {
            try
            {
                await Const.Connection.StartAsync();
                //登陆成功
                var users = await Const.Connection.InvokeAsync<IEnumerable<User>>(Login, Const.User = new User()
                {
                    Status = Status.FREE,
                });

                Users = new ObservableCollection<User>(users);

                Message = $"登陆成功 id:[{Const.User.UserId}]";

                //开启接收游戏请求接口
                Const.Connection.On<string>(GameRequest, async (str) =>
                {
                    var result = MessageBox.Show("有人邀请你来一局,是否接受?", str, MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        var tempResult = await Const.Connection.InvokeAsync<string>(ReceiveRequest, Const.User.UserId, str);
                        if (tempResult != "SUCCESS")
                        {
                            MessageBox.Show(tempResult, "WARNING", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                });

                Const.Connection.On<string, string>(BeginGame, (beginner, receiver) =>
                 {
                     ChessWindow window = new ChessWindow();
                     window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                     var vm = new ChessWindowViewModel(beginner == Const.User.UserId, beginner == Const.User.UserId ? receiver : beginner,window.chesscontrol.Chessboard);
                     window.DataContext = vm;
                     window.Show();
                 });
            }
            catch (Exception ex)
            {
                Message = ex.Message;
            }
        }

        private async void Refresh()
        {
            Users = new ObservableCollection<User>(
                    await Const.Connection.InvokeAsync<IEnumerable<User>>(RequestList));
        }
    }
}
