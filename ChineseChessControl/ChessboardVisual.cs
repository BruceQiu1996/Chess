using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ChineseChess
{
    internal class ChessboardVisual : DrawingVisual 
    {
        private Pen _boardPen;
        private Pen _lightPen;
        private Pen _darkPen;
        private Pen _crossPen;

        private DrawingContext _drawingContext;
        private double _cellWidth;

        public ChessboardVisual(double cellWidth)
        {
            InitializePens();

            Refresh(cellWidth);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="cellWidth">边长</param>
        public void Refresh(double cellWidth)
        {
            _cellWidth = cellWidth;
            using (_drawingContext = this.RenderOpen())
            {
                DrawBackGroudImage();
                DrawBoardBorder();
                DrawNumbers();
                DrawMiddleText();
                DrawMiddleLine();
                DrawWholeBoard();
            }
        }

        /// <summary>
        /// 初始化画笔
        /// </summary>
        void InitializePens()
        {
            _boardPen = new Pen(Brushes.Black, 3.5);
            _boardPen.Freeze();

            _lightPen = new Pen(Brushes.LightGray, 1.5);
            _lightPen.Freeze();

            _darkPen = new Pen(Brushes.Black, 1.5);
            _darkPen.Freeze();

            _crossPen = new Pen(Brushes.Black, 2);
            _crossPen.Freeze();
        }

        /// <summary>
        /// 背景图
        /// </summary>
        void DrawBackGroudImage()
        {
            BitmapImage backgroundImage = new BitmapImage();
            backgroundImage.BeginInit();
            backgroundImage.UriSource = new Uri(@"pack://application:,,,/ChineseChessControl;component/Images/woodDeskground.jpg", UriKind.RelativeOrAbsolute);
            backgroundImage.EndInit();
            backgroundImage.Freeze();


            _drawingContext.DrawImage(backgroundImage, new Rect(0, 0, _cellWidth * 10, _cellWidth * 11));
        }

        /// <summary>
        /// 画棋盘边框
        /// </summary>
        void DrawBoardBorder()
        {
            //最外框
            _drawingContext.DrawRectangle(null, _darkPen, new Rect(0, 0, _cellWidth * 10, _cellWidth * 11));
            //棋盘外框
            _drawingContext.DrawRectangle(null, _boardPen, new Rect(_cellWidth - 5, _cellWidth - 5, _cellWidth * 8 + 10, _cellWidth * 9 + 10));
        }

        /// <summary>
        /// 画数字
        /// </summary>
        void DrawNumbers()
        {
            for (int i = 1; i < 10; i++)
            {
                FormattedText formattedTextTop = new FormattedText(
                i.ToString(), System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                12,
                Brushes.Black);
                formattedTextTop.TextAlignment = TextAlignment.Center;
                _drawingContext.DrawText(formattedTextTop,
                    new Point(_cellWidth * i, formattedTextTop.Height / 2));


                FormattedText formattedTextBottom = new FormattedText(
                (10 - i).ToString(), System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                12,
                Brushes.Black);
                formattedTextBottom.TextAlignment = TextAlignment.Center;
                _drawingContext.DrawText(formattedTextBottom,
                    new Point(_cellWidth * i, _cellWidth * 10 + formattedTextBottom.Height));
            }
        }

        /// <summary>
        /// 画棋盘中间的文字
        /// </summary>
        void DrawMiddleText()
        {
             Action<string, Point, double, double> drawText =(Action<string, Point, double, double>)
                delegate(string text, Point location, double fontSize, double angle)
                {
                    FormattedText textFormat = new FormattedText(
                    text, System.Globalization.CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface("STLiti"),
                    fontSize,
                    Brushes.Black);

                    textFormat.MaxTextWidth = fontSize;
                    textFormat.TextAlignment = TextAlignment.Justify;

                    double subtract = (textFormat.Height - textFormat.Width) / 2;

                    _drawingContext.PushTransform(new RotateTransform(angle, (angle > 0 ? -textFormat.Width - subtract : subtract) + location.X + textFormat.Width / 2, location.Y - subtract - fontSize / 2 + textFormat.Height / 2));
                    _drawingContext.DrawText(textFormat,
                        new Point((angle > 0 ? -textFormat.Width - subtract : subtract) + location.X, location.Y - subtract - 15));
                    _drawingContext.Pop();

                };

              drawText(ChineseChessControl.Properties.Resources.ChuHe, new Point(_cellWidth * 2, _cellWidth * 5.5), 30, -90);
              drawText(ChineseChessControl.Properties.Resources.HanJie, new Point(_cellWidth * 8, _cellWidth * 5.5), 30, 90);
        }



        /// <summary>
        /// 画中线
        /// </summary>
        void DrawMiddleLine()
        {
             //纵中线上
            _drawingContext.DrawLine(_darkPen, new Point(_cellWidth * 5, _cellWidth), new Point(_cellWidth * 5, _cellWidth * 5));
            _drawingContext.DrawLine(_lightPen, new Point(_cellWidth * 5 + 1.5, _cellWidth), new Point(_cellWidth * 5 + 1.5, _cellWidth * 5));

            //纵中线下
            _drawingContext.DrawLine(_darkPen, new Point(_cellWidth * 5, _cellWidth * 6), new Point(_cellWidth * 5, _cellWidth * 10));
            _drawingContext.DrawLine(_lightPen, new Point(_cellWidth * 5 + 1.5, _cellWidth * 6), new Point(_cellWidth * 5 + 1.5, _cellWidth * 10));
        }

        /// <summary>
        /// 画四分之一棋盘
        /// </summary>
        void DrawQuarterChessBoard()
        {
            //皇宫
            _drawingContext.DrawLine(_darkPen, new Point(_cellWidth * 4, _cellWidth), new Point(_cellWidth * 5, _cellWidth * 2));
            _drawingContext.DrawLine(_lightPen, new Point(_cellWidth * 4, _cellWidth + 1.5), new Point(_cellWidth * 5, _cellWidth * 2 + 1.5));
            _drawingContext.DrawLine(_darkPen, new Point(_cellWidth * 4, _cellWidth * 3), new Point(_cellWidth * 5, _cellWidth * 2));
            _drawingContext.DrawLine(_lightPen, new Point(_cellWidth * 4 + 1.5, _cellWidth * 3), new Point(_cellWidth * 5 + 1.5, _cellWidth * 2));

            //竖线
            _drawingContext.DrawLine(_darkPen, new Point(_cellWidth, _cellWidth), new Point(_cellWidth, _cellWidth * 9 / 2 + _cellWidth));
            for (int i = 2; i < 5; i++)
            {
                _drawingContext.DrawLine(_darkPen, new Point(_cellWidth * i, _cellWidth), new Point(_cellWidth * i, _cellWidth * 10 / 2));
                _drawingContext.DrawLine(_lightPen, new Point(_cellWidth * i + 1.5, _cellWidth), new Point(_cellWidth * i + 1.5, _cellWidth * 10 / 2));
            }
            //横线
            for (int i = 1; i < 5; i++)
            {
                _drawingContext.DrawLine(_darkPen, new Point(_cellWidth, _cellWidth * i), new Point(_cellWidth * 8 / 2 + _cellWidth, _cellWidth * i));
                _drawingContext.DrawLine(_lightPen, new Point(_cellWidth, _cellWidth * i + 1.5), new Point(_cellWidth * 8 / 2 + _cellWidth, _cellWidth * i + 1.5));
            }
            //横中线
            _drawingContext.DrawLine(_lightPen, new Point(_cellWidth, _cellWidth * 5 + 2), new Point(_cellWidth * 8 / 2 + _cellWidth, _cellWidth * 5 + 2));
            _drawingContext.DrawLine(_darkPen, new Point(_cellWidth, _cellWidth * 5), new Point(_cellWidth * 8 / 2 + _cellWidth, _cellWidth * 5));

            //兵格
            drawCross(new Point(2 * _cellWidth + 1, 3 * _cellWidth + 1), true, true);
            drawCross(new Point(3 * _cellWidth + 1, 4 * _cellWidth + 1), true, true);
            drawCross(new Point(1 * _cellWidth + 1, 4 * _cellWidth + 1), false, true);
            drawCross(new Point(5 * _cellWidth, 4 * _cellWidth + 1), true, false);
        }

        /// <summary>
        /// 画整个棋盘
        /// </summary>
        void DrawWholeBoard()
        {
            //第一象限
            _drawingContext.PushTransform(new ScaleTransform(-1, 1, _cellWidth * 5, _cellWidth * 5.5));
            DrawQuarterChessBoard();
            _drawingContext.Pop();

            //第二象限
            DrawQuarterChessBoard();

            //第三象限
            _drawingContext.PushTransform(new ScaleTransform(1, -1, _cellWidth * 5, _cellWidth * 5.5));
            DrawQuarterChessBoard();
            _drawingContext.Pop();

            //第四象限
            _drawingContext.PushTransform(new ScaleTransform(-1, -1, _cellWidth * 5, _cellWidth * 5.5));
            DrawQuarterChessBoard();
            _drawingContext.Pop();
        }
        

        /// <summary>
        /// 画十字
        /// </summary>
        /// <param name="_drawingContext">画图对象</param>
        /// <param name="point">十字中间坐标点</param>
        /// <param name="left">是否要画左半边</param>
        /// <param name="right">是否要画右半边</param>
        void drawCross(Point point, bool left, bool right)
        {
            Action foldLine = (Action)delegate()
            {
                _drawingContext.DrawLine(_crossPen,
                    new Point(point.X + 4, point.Y + 4),
                    new Point(point.X + 4, point.Y + _cellWidth / 4));
                _drawingContext.DrawLine(_crossPen,
                    new Point(point.X + 4, point.Y + 4),
                    new Point(point.X + _cellWidth / 4, point.Y + 4));
            };

            if (left)
            {
                _drawingContext.PushTransform(new RotateTransform(90, point.X, point.Y));
                foldLine();
                _drawingContext.Pop();

                _drawingContext.PushTransform(new RotateTransform(180, point.X, point.Y));
                foldLine();
                _drawingContext.Pop();
            }

            if (right)
            {
                foldLine();

                _drawingContext.PushTransform(new RotateTransform(270, point.X, point.Y));
                foldLine();
                _drawingContext.Pop();
            }
        }
    }
}
