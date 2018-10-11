﻿using System;
using System.Windows;
using System.Windows.Media;

namespace Suggeritore_Cisco.Controls
{
    /// <summary>
    /// Interaction logic for CircularProgressBar.xaml
    /// </summary>
    public partial class CircularProgressBar
    {
        #region Properties
        public CircularProgressBar()
        {
            InitializeComponent();
            Angle = (Percentage * 360) / 100;
            RenderArc();
        }

        public int Radius
        {
            get => (int)GetValue(RadiusProperty);
            set => SetValue(RadiusProperty, value);
        }

        public Brush SegmentColor
        {
            get => (Brush)GetValue(SegmentColorProperty);
            set => SetValue(SegmentColorProperty, value);
        }

        public int StrokeThickness
        {
            get { if (GetValue(StrokeThicknessProperty) is int) return (int)GetValue(StrokeThicknessProperty); else return 0; }
            set => SetValue(StrokeThicknessProperty, value);
        }

        public double Percentage
        {
            get => (double)GetValue(PercentageProperty);
            set => SetValue(PercentageProperty, value);
        }

        public double Angle
        {
            get => (double)GetValue(AngleProperty);
            set => SetValue(AngleProperty, value);
        }

        #endregion

        #region DependencyProperties

        public static readonly DependencyProperty PercentageProperty =
            DependencyProperty.Register("Percentage", typeof(double), typeof(CircularProgressBar), new PropertyMetadata(65d, OnPercentageChanged));

        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(int), typeof(CircularProgressBar), new PropertyMetadata(5));

        public static readonly DependencyProperty SegmentColorProperty =
            DependencyProperty.Register("SegmentColor", typeof(Brush), typeof(CircularProgressBar), new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(int), typeof(CircularProgressBar), new PropertyMetadata(25, OnPropertyChanged));

        public static readonly DependencyProperty AngleProperty =
            DependencyProperty.Register("Angle", typeof(double), typeof(CircularProgressBar), new PropertyMetadata(120d, OnPropertyChanged));

        #endregion

        #region Callbacks

        private static void OnPercentageChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgressBar circle = sender as CircularProgressBar;
            circle.Angle = (circle.Percentage * 360) / 100;
        }

        private static void OnPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            CircularProgressBar circle = sender as CircularProgressBar;
            circle.RenderArc();
        }

        #endregion

        public void RenderArc()
        {
            var startPoint =  new Point(Radius, 0);
            var endPoint = ComputeCartesianCoordinate(Angle, Radius);
            endPoint.X += Radius;
            endPoint.Y += Radius;

            pathRoot.Width = Radius * 2 + StrokeThickness;
            pathRoot.Height = Radius * 2 + StrokeThickness;
            pathRoot.Margin = new Thickness(StrokeThickness, StrokeThickness, 0, 0);

            var largeArc = Angle > 180.0;

            var outerArcSize = new Size(Radius, Radius);

            pathFigure.StartPoint = startPoint;

            if (startPoint.X == Math.Round(endPoint.X) && startPoint.Y == Math.Round(endPoint.Y))
                endPoint.X -= 0.01;

            arcSegment.Point = endPoint;
            arcSegment.Size = outerArcSize;
            arcSegment.IsLargeArc = largeArc;
        }

        private static Point ComputeCartesianCoordinate(double angle, double radius)
        {
            // convert to radians
            var angleRad = (Math.PI / 180.0) * (angle - 90);

            var x = radius * Math.Cos(angleRad);
            var y = radius * Math.Sin(angleRad);

            return new Point(x, y);
        }
    }
}
