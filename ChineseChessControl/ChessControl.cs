using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Media;
using System.Windows.Documents;

namespace ChineseChess
{
    public class ChessControl : FrameworkElement
    {
        public ChessControl()
        {
            boardVisual = new ChessboardVisual(CellWidth);
            AddVisualChild(boardVisual);
            Chessboard = new Chessboard();
            Chessboard.ChessMoved += chessboard_ChessMoved;
            InitializeReminder();
            MakeSound();

        }

        public static readonly DependencyProperty FontFamilyProperty =
                 DependencyProperty.Register("FontFamily",
            typeof(FontFamily),
            typeof(ChessControl),
            new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits));

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        public Chessboard Chessboard;

        static object ChessboardCoerceValueCallback(DependencyObject d, object baseValue)
        {
            ChessControl chineseChessboard = d as ChessControl;

            if (baseValue != null)
            {
                (baseValue as Chessboard).ChessMoved += chineseChessboard.chessboard_ChessMoved;
            }
            return baseValue;
        }

        static void ChessboardPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChessControl chineseChessboard = d as ChessControl;
            if (e.OldValue != null)
            {
                (e.OldValue as Chessboard).ChessMoved -= chineseChessboard.chessboard_ChessMoved;
            }
        }

        public bool IsRedReady
        {
            get { return (bool)GetValue(IsRedReadyProperty); }
            set { SetValue(IsRedReadyProperty, value); }
        }

        public static readonly DependencyProperty IsRedReadyProperty =
            DependencyProperty.Register("IsRedReady",
                typeof(bool),
                typeof(ChessControl),
                new FrameworkPropertyMetadata(false, 
                    FrameworkPropertyMetadataOptions.AffectsMeasure, 
                    new PropertyChangedCallback(IsRedReadyPropertyChangedCallback)));

        static void IsRedReadyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChessControl chineseChessboard = d as ChessControl;
            if (e.NewValue.Equals(true))
            {
                chineseChessboard.ReadForRedChessman();
            }
            else
            {
                if (chineseChessboard._dispatcherTimer != null)
                    chineseChessboard._dispatcherTimer.Stop();
            }
        }

        public bool IsBlueReady
        {
            get { return (bool)GetValue(IsBlueReadyProperty); }
            set { SetValue(IsBlueReadyProperty, value); }
        }

        public static readonly DependencyProperty IsBlueReadyProperty =
            DependencyProperty.Register("IsBlueReady",
                typeof(bool),
                typeof(ChessControl),
                new FrameworkPropertyMetadata(false, 
                    FrameworkPropertyMetadataOptions.AffectsMeasure, 
                    IsBlueReadyPropertyChangedCallback));

        static void IsBlueReadyPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChessControl chineseChessboard = d as ChessControl;
            if (e.NewValue.Equals(true))
            {
                chineseChessboard.ReadForBlueChessman();
            }
            else
            {
                if (chineseChessboard._dispatcherTimer != null)
                    chineseChessboard._dispatcherTimer.Stop();
            }
        }
        //格子边长
        public double CellWidth
        {
            get { return (double)GetValue(CellWidthProperty); }
            set { SetValue(CellWidthProperty, value); }
        }

        public static readonly DependencyProperty CellWidthProperty =
            DependencyProperty.Register
            ("CellWidth",
            typeof(double),
            typeof(ChessControl),
            new FrameworkPropertyMetadata(40.0, FrameworkPropertyMetadataOptions.AffectsMeasure,(o,e)=>
                {
                    if ((o as ChessControl).boardVisual!=null)
                        (o as ChessControl).boardVisual.Refresh((double)e.NewValue);
                }));


        public bool IsRedSelected
        {
            get { return (bool)GetValue(IsRedSelectedProperty); }
            set { SetValue(IsRedSelectedProperty, value); }
        }

        public static readonly DependencyProperty IsRedSelectedProperty =
            DependencyProperty.Register("IsRedSelected", typeof(bool), typeof(ChessControl), new UIPropertyMetadata(true));


        public bool IsRedTurn
        {
            get { return (bool)GetValue(IsRedTurnProperty); }
            set { SetValue(IsRedTurnProperty, value); }
        }

        public static readonly DependencyProperty IsRedTurnProperty =
            DependencyProperty.Register("IsRedTurn",
            typeof(bool),
            typeof(ChessControl),
            new UIPropertyMetadata(true, IsRedTurnPropertyChangedCallback));

        static void IsRedTurnPropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ChessControl chineseChessboard = d as ChessControl;
            if (e.NewValue.Equals(chineseChessboard.IsRedSelected))
            {
                chineseChessboard._dispatcherTimer.Start();
            }
            else
            {
                chineseChessboard._dispatcherTimer.Stop();
            }
        }

        /// <summary>
        /// 初始化蓝棋
        /// </summary>
        /// <param name="currentColorIsRed">自己是否为红色一方</param>
        void InitializeBlueChessman(bool currentColorIsRed)
        {
            int defaultLocation = currentColorIsRed ? 0 : 9;

            _chessmanList.Add(new ChessmanControl(
                new Rook(new ChessPoint(0, defaultLocation), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Knight(new ChessPoint(1, defaultLocation), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Elephant(new ChessPoint(2, defaultLocation), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Guard(new ChessPoint(3, defaultLocation), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new General(new ChessPoint(4, defaultLocation), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Guard(new ChessPoint(5, defaultLocation), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Elephant(new ChessPoint(6, defaultLocation), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Knight(new ChessPoint(7, defaultLocation), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                 new Rook(new ChessPoint(8, defaultLocation), Chessboard, false)));

            _chessmanList.Add(new ChessmanControl(
                new Cannon(new ChessPoint(1, Math.Abs(defaultLocation - 2)), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Cannon(new ChessPoint(7, Math.Abs(defaultLocation - 2)), Chessboard, false)));

            _chessmanList.Add(new ChessmanControl(
                new Pawn(new ChessPoint(0, Math.Abs(defaultLocation - 3)), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Pawn(new ChessPoint(2, Math.Abs(defaultLocation - 3)), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Pawn(new ChessPoint(4, Math.Abs(defaultLocation - 3)), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Pawn(new ChessPoint(6, Math.Abs(defaultLocation - 3)), Chessboard, false)));
            _chessmanList.Add(new ChessmanControl(
                new Pawn(new ChessPoint(8, Math.Abs(defaultLocation - 3)), Chessboard, false)));
        }
        /// <summary>
        /// 初始化红棋
        /// </summary>
        /// <param name="currentColorIsRed">自己是否为红色一方</param>
        void InitializeRedChessman(bool currentColorIsRed)
        {
            int defaultLocation = currentColorIsRed ? 9 : 0;

            _chessmanList.Add(new ChessmanControl(
                new Rook(new ChessPoint(0, defaultLocation), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Knight(new ChessPoint(1, defaultLocation), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Minister(new ChessPoint(2, defaultLocation), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Mandarin(new ChessPoint(3, defaultLocation), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new King(new ChessPoint(4, defaultLocation), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Mandarin(new ChessPoint(5, defaultLocation), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Minister(new ChessPoint(6, defaultLocation), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Knight(new ChessPoint(7, defaultLocation), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                 new Rook(new ChessPoint(8, defaultLocation), Chessboard, true)));

            _chessmanList.Add(new ChessmanControl(
                new Cannon(new ChessPoint(1, Math.Abs(defaultLocation - 2)), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Cannon(new ChessPoint(7, Math.Abs(defaultLocation - 2)), Chessboard, true)));

            _chessmanList.Add(new ChessmanControl(
                new Soldier(new ChessPoint(0, Math.Abs(defaultLocation - 3)), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Soldier(new ChessPoint(2, Math.Abs(defaultLocation - 3)), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Soldier(new ChessPoint(4, Math.Abs(defaultLocation - 3)), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Soldier(new ChessPoint(6, Math.Abs(defaultLocation - 3)), Chessboard, true)));
            _chessmanList.Add(new ChessmanControl(
                new Soldier(new ChessPoint(8, Math.Abs(defaultLocation - 3)), Chessboard, true)));
        }

        /// <summary>
        /// 呈现红色棋子
        /// </summary>
        void ReadForRedChessman()
        {
            if (_chessmanList == null)
                InitalChessman();

            foreach (var item in _chessmanList)
            {
                if (item.Chessman.IsRed)
                {
                    ChessmanCollection.Add(item);
                }
            }
        }

        /// <summary>
        /// 呈现蓝色棋子
        /// </summary>
        void ReadForBlueChessman()
        {
            if (_chessmanList == null)
                InitalChessman();

            foreach (var item in _chessmanList)
            {
                if (!item.Chessman.IsRed)
                {
                    ChessmanCollection.Add(item);
                }
            }
        }

        /// <summary>
        /// 初始化提示器
        /// </summary>
        void InitializeReminder()
        {
            int frameCount = 0;
            CompositionTargetRenderingListener _renderingListener = new CompositionTargetRenderingListener();
            ChessmanControl tempChessmanControl = null;
            _renderingListener.Rendering += delegate
            {
                if (tempChessmanControl == null)
                    tempChessmanControl = _lastMoveChessman;
                if (tempChessmanControl != null)
                {
                    if (frameCount % 20 == 0)
                    {
                        tempChessmanControl.Opacity = tempChessmanControl.Opacity > 0 ? 0 : 1;
                    }

                    frameCount++;

                    if (tempChessmanControl != _lastMoveChessman //已经换了棋子
                        || frameCount > 120 //提醒大于规定
                        || !IsRedReady     //红方没有开始，或已经结束
                        || !IsBlueReady)   //蓝方没有开始，或已经结束
                    {
                        tempChessmanControl.Opacity = 1;
                        frameCount = 0;
                        tempChessmanControl = null;
                        _renderingListener.StopListening();
                    }
                }
            };

            _dispatcherTimer.Interval = TimeSpan.FromSeconds(10);
            _dispatcherTimer.Tick += delegate
            {
                _renderingListener.StartListening();
            };
        }

        /// <summary>
        /// 初始化棋子
        /// </summary>
        void InitalChessman()
        {
            _chessmanBaseDic = new Dictionary<ChessmanBase, ChessmanControl>(32);
            _chessmanList = new List<ChessmanControl>(32);


            InitializeBlueChessman(IsRedSelected);
            InitializeRedChessman(IsRedSelected);

            foreach (var item in _chessmanList)
            {
                _chessmanBaseDic.Add(item.Chessman, item);
            }
        }


        void chessboard_ChessMoved(ChessPoint newPoint, ChessPoint oldPoint, ChessmanBase newChessman, ChessmanBase oldChessman)
        {
            _lastMoveChessman = _chessmanBaseDic[newChessman];

            soundPlayer.Play();
            //置换
            IsRedTurn = !IsRedTurn;

            //移动动画
            _chessmanBaseDic[newChessman].RenderTransform = _moveTransform;

            DoubleAnimation xAnimation = new DoubleAnimation((newPoint.X - oldPoint.X) * CellWidth, _moveDuration, FillBehavior.Stop);
            _moveTransform.BeginAnimation(TranslateTransform.XProperty, xAnimation);
           
            DoubleAnimation yAnimation = new DoubleAnimation((newPoint.Y - oldPoint.Y) * CellWidth, _moveDuration, FillBehavior.Stop);

            EventHandler tempAction = default(EventHandler);
            tempAction = delegate
            {
                _chessmanBaseDic[newChessman].ClearValue(UIElement.RenderTransformProperty);
                _chessmanBaseDic[newChessman].isSelected = false;
                _currentChessmanControl = null;
                if (oldChessman != null)
                {
                    ChessmanCollection.Remove(_chessmanBaseDic[oldChessman]);
                }

                //更新
                this.InvalidateArrange();
                //移除本身
                yAnimation.Completed -= tempAction;
            };

            yAnimation.Completed += tempAction;
            
            _moveTransform.BeginAnimation(TranslateTransform.YProperty, yAnimation);
        }

        void MakeSound()
        {
            Uri uri = new Uri("pack://application:,,,/ChineseChessControl;Component/Sound/fall.wav", UriKind.RelativeOrAbsolute);
            Stream stream = Application.GetResourceStream(uri).Stream;
            stream.Position = 0;

            soundPlayer = new SoundPlayer();

            soundPlayer.Stream = null;
            soundPlayer.Stream = stream;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            //是否自己下
            if (IsRedTurn != IsRedSelected)
                return;
            //判断棋局是否开始
            if (!IsRedReady || !IsBlueReady)
                return;

            Point location = e.GetPosition(this);
            HitTestResult result = VisualTreeHelper.HitTest(this, location);
            ChessmanControl chessmanControl = result.VisualHit as ChessmanControl;

            //如果选中的和之前的一样或没有选中任何棋子
            if (chessmanControl == null && _currentChessmanControl == null ||
                chessmanControl == _currentChessmanControl)
                return;

            if (_currentChessmanControl == null)
            {
                //判断是否选中自己的棋子
                if (chessmanControl.Chessman.IsRed == IsRedSelected)
                {
                    chessmanControl.isSelected = true;
                    _currentChessmanControl = chessmanControl;
                }
            }
            else //判断是否选择其他子 
                if (chessmanControl != null && IsRedSelected == chessmanControl.Chessman.IsRed)
            {
                _currentChessmanControl.isSelected = false;
                chessmanControl.isSelected = true;
                _currentChessmanControl = chessmanControl;
            }
            else
            {
                //得到实际需要移动的位置
                int xActual = (int)((Math.Round((location.X - CellWidth) / CellWidth)));
                int yActual = (int)(Math.Round((location.Y - CellWidth) / CellWidth));
                //判断是否和上次选中的一样
                if (_currentChessmanControl.Chessman.Location.X != xActual ||
                    _currentChessmanControl.Chessman.Location.Y != yActual)
                    _currentChessmanControl.Chessman.Location = new ChessPoint(xActual, yActual);
            }
        }

        protected override int VisualChildrenCount
        {
            get
            {
                // +1 是为棋盘
                return (ChessmanCollection != null ? ChessmanCollection.Count : 0) + 1;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            //第一个为棋盘其他为棋子
            if (index == 0)
                return boardVisual;

            return ChessmanCollection[index - 1];
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            for (int i = 0; ChessmanCollection != null && i < ChessmanCollection.Count; i++)
            {
                ChessmanControl item = ChessmanCollection[i] as ChessmanControl;
                item.Arrange(new Rect(
                 new Point(item.Chessman.Location.X * CellWidth - item.DesiredSize.Width / 2+CellWidth,
                           item.Chessman.Location.Y * CellWidth - item.DesiredSize.Height / 2+CellWidth),
                 item.DesiredSize));
            }

            return this.DesiredSize;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            for (int i = 0; ChessmanCollection != null && i < ChessmanCollection.Count; i++)
            {
                ChessmanCollection[i].Measure(availableSize);
            }

            return new Size(CellWidth * 10, CellWidth * 11);
        }

        SoundPlayer soundPlayer;
        Dictionary<ChessmanBase, ChessmanControl> _chessmanBaseDic;
        List<ChessmanControl> _chessmanList;
        ChessboardVisual boardVisual;
        ChessmanControl _currentChessmanControl;
        ChessmanControl _lastMoveChessman;
       
        readonly TranslateTransform _moveTransform = new TranslateTransform();
        readonly Duration _moveDuration = new Duration(TimeSpan.FromMilliseconds(300));
        public readonly DispatcherTimer _dispatcherTimer = new DispatcherTimer(DispatcherPriority.ApplicationIdle);

        UIElementCollection _chessmanCollection;
        protected UIElementCollection ChessmanCollection
        {
            get
            {
                if (_chessmanCollection == null)
                    _chessmanCollection = new UIElementCollection(this, null);

                return _chessmanCollection;
            }
        }
    }
}
