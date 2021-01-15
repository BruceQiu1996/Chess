using Chess.ViewModels;
using ChineseChess;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Chess
{
    /// <summary>
    /// ChessWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChessWindow
    {
        public ChessWindow()
        {
            InitializeComponent();
            Messenger.Default.Register<bool>(this, "Close", (flag) =>
            {
                 if (flag)
                     this.Close();
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("棋品呢？不许反悔！", "等待开发..", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
