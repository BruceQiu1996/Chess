using ChineseChess;
using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Command;

namespace Chess.ViewModels
{
    public class ChessWindowViewModel : ViewModelBase
    {

        public RelayCommand ClosedCommandAsync { get; set; }

        private readonly string ReceiveLocation = nameof(ReceiveLocation);
        private readonly string OtherExit = nameof(OtherExit);

        private bool _isRedTurn;
        public bool IsRedTurn
        {
            get
            {
                return _isRedTurn;
            }
            set
            {
                _isRedTurn = value;
                RaisePropertyChanged(nameof(IsRedTurn));
            }
        }

        private bool _isRedSelected;
        public bool IsRedSelected
        {
            get
            {
                return _isRedSelected;
            }
            set
            {
                _isRedSelected = value;
                RaisePropertyChanged(nameof(IsRedSelected));
            }
        }

        private bool _isRedReady;
        public bool IsRedReady
        {
            get
            {
                return _isRedReady;
            }
            set
            {
                _isRedReady = value;
                RaisePropertyChanged(nameof(IsRedReady));
            }
        }

        private bool _isBlueReady;
        public bool IsBlueReady
        {
            get
            {
                return _isBlueReady;
            }
            set
            {
                _isBlueReady = value;
                RaisePropertyChanged(nameof(IsBlueReady));
            }
        }

        private Chessboard _board;

        private string _receiver;

        public ChessWindowViewModel(bool isRed, string reveiver, Chessboard board)
        {
            _board = board;
            _board.ChessMoved += _board_ChessMoved;
            IsRedSelected = isRed;
            _receiver = reveiver;

            if (isRed)
            {
                IsRedReady = true;
                IsBlueReady = true;
            }
            else
            {
                IsBlueReady = true;
                IsRedReady = true;
            }

            ClosedCommandAsync = new RelayCommand(CloseAsync);
            //监听落子
            Const.Connection.On<ChessPoint, ChessPoint>(ReceiveLocation, (oldPoint, newPoint) =>
            {
                var old = new ChessPoint(oldPoint.X, 9 - oldPoint.Y);
                var neW = new ChessPoint(newPoint.X, 9 - newPoint.Y);
                _board.GetChessman(old).Location = neW;

                IsRedTurn = IsRedSelected;
            });

            //对方逃跑或投降
            Const.Connection.On(OtherExit, () =>
            {
                MessageBox.Show("对方逃跑或者投降了,你赢了！", "恭喜", MessageBoxButton.OK, MessageBoxImage.Information);
                Messenger.Default.Send(true, "Close");
            });
        }

        async void _board_ChessMoved(ChessPoint newPoint, ChessPoint oldPoint, ChessmanBase newChessman, ChessmanBase oldChessman)
        {
            if (newChessman.IsRed == IsRedSelected)
            {
                //发送落子坐标
                var result = await Const.Connection.InvokeAsync<string>("Action", oldPoint, newPoint, _receiver);

                if (result != "SUCCESS")
                    MessageBox.Show(result, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (oldChessman != null && oldChessman is King)
            {
                IsBlueReady = false;
                IsRedReady = false;
                await Const.Connection.InvokeAsync("GameOver");

                if (oldChessman.IsRed == IsRedSelected)
                    MessageBox.Show("你输了", "Info", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("你赢了", "Info", MessageBoxButton.OK, MessageBoxImage.Information);

                Messenger.Default.Send(true, "Close");
            }
        }

        private async void CloseAsync()
        {
            await Const.Connection.InvokeAsync("Exit",_receiver);
        }
    }
}
