using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChineseChess
{
    public class ChessmanControl : FrameworkElement
    {
        public ChessmanControl(ChessmanBase chessman)
        {
            _chessman = chessman;
            if (!chessman.IsRed)
                ChessColor = Brushes.Blue;
        }

        ChessmanBase _chessman;
        public ChessmanBase Chessman
        {
            get
            {
                return _chessman;
            }
        }

        public Brush ChessColor
        {
            get { return (Brush)GetValue(ChessColorProperty); }
            set { SetValue(ChessColorProperty, value); }
        }

        public static readonly DependencyProperty ChessColorProperty =
            DependencyProperty.Register("ChessColor",
            typeof(Brush),
            typeof(ChessmanControl),
            new FrameworkPropertyMetadata(Brushes.Red, FrameworkPropertyMetadataOptions.AffectsRender));

        public bool isSelected
        {
            get { return (bool)GetValue(isSelectedProperty); }
            set { SetValue(isSelectedProperty, value); }
        }

        public static readonly DependencyProperty isSelectedProperty =
            DependencyProperty.Register("isSelected", typeof(bool),
            typeof(ChessmanControl),
            new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.AffectsRender));


        public static readonly DependencyProperty FontFamilyProperty = ChessControl.FontFamilyProperty.AddOwner(typeof(ChessmanControl),
            new FrameworkPropertyMetadata(SystemFonts.MessageFontFamily, FrameworkPropertyMetadataOptions.Inherits));

        public FontFamily FontFamily
        {
            get { return (FontFamily)GetValue(FontFamilyProperty); }
            set { SetValue(FontFamilyProperty, value); }
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            Pen pen = new Pen(isSelected ? Brushes.White : ChessColor, 1);
            pen.Freeze();

            Point centerPoint = new Point(this.DesiredSize.Width / 2, this.DesiredSize.Height / 2);
            drawingContext.DrawEllipse(isSelected ? ChessColor : Brushes.White, pen,
                centerPoint,
                this.DesiredSize.Width / 2, this.DesiredSize.Height / 2);

            FormattedText formattedText = new FormattedText(
                _chessman.Name, System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(FontFamily.ToString()),//"Verdana"
                20,
                isSelected ? Brushes.White : ChessColor);

            formattedText.TextAlignment = TextAlignment.Center;

            drawingContext.DrawText(formattedText,
                new Point(centerPoint.X, centerPoint.Y - formattedText.Height / 2));
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size(30, 30);
        }

    }
}
