using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace BettaApp
{
    public sealed partial class ObjectDragging : Page
    {
        public ObjectDragging()
        {
            this.InitializeComponent();

            uiCanvas.Loaded += UiCanvas_Loaded;
        }

        private void UiCanvas_Loaded(object sender, RoutedEventArgs e)
        {
            SetCirclePosition(new Point(uiCanvas.ActualWidth / 2, uiCanvas.ActualHeight / 2));
        }

        public Point CirclesCurrentTLPosition
        {
            get { return new Point(Canvas.GetLeft(uiCircle), Canvas.GetTop(uiCircle)); }
        }

        public Point CirclesCurrentBRPosition
        {
            get { return new Point(Canvas.GetLeft(uiCircle) + uiCircle.ActualWidth, Canvas.GetTop(uiCircle) + uiCircle.ActualHeight); }
        }

        public bool IsDragging { get; set; }
        public Point BeforeManipulationCiclePosition { get; set; }

        private bool IsPositionOnCircle(Point position)
        {
            bool isPositionXGreaterThanCircleLeft = position.X > CirclesCurrentTLPosition.X;
            bool isPositionYGreaterThanCircleTop = position.Y > CirclesCurrentTLPosition.Y;

            bool isPositionXLessThanCircleLeft = position.X < CirclesCurrentBRPosition.X;
            bool isPositionYLessThanCircleTop = position.Y < CirclesCurrentBRPosition.Y;

            bool isPositionTLGreater = isPositionXGreaterThanCircleLeft && isPositionYGreaterThanCircleTop;
            bool isPositionBRLess = isPositionXLessThanCircleLeft && isPositionYLessThanCircleTop;

            return isPositionTLGreater && isPositionBRLess;
        }

        private double GetContainedDouble(double doubleToContain, double min, double max)
        {
            double containedDouble = doubleToContain;

            double minAmountLeft = doubleToContain - min;
            double maxAmountLeft = doubleToContain - max;

            if (minAmountLeft < 0)
                containedDouble = -minAmountLeft;
            if (maxAmountLeft > 0)
                containedDouble = max - maxAmountLeft;

            if (containedDouble < min || containedDouble > max)
                containedDouble = GetContainedDouble(containedDouble, min, max);

            return containedDouble;
        }

        private void SetCirclePosition(Point point)
        {
            point.X = GetContainedDouble(point.X, 0, uiCanvas.ActualWidth - uiCircle.ActualWidth);
            point.Y = GetContainedDouble(point.Y, 0, uiCanvas.ActualHeight - uiCircle.ActualHeight);

            Canvas.SetLeft(uiCircle, point.X);
            Canvas.SetTop(uiCircle, point.Y);
        }

        private void PositionCircleFromDelta(ManipulationDeltaRoutedEventArgs e)
        {
            var newX = BeforeManipulationCiclePosition.X + e.Cumulative.Translation.X;
            var newY = BeforeManipulationCiclePosition.Y + e.Cumulative.Translation.Y;

            Point newPosition = new Point(newX, newY);

            SetCirclePosition(newPosition);
        }

        private void ScaleCircleFromDelta(ManipulationDeltaRoutedEventArgs e)
        {
            double scalling = 10 / e.Velocities.Linear.ToVector2().LengthSquared();

            if (scalling < 1)
                SetCircleScaling(scalling);
        }

        private void SetCircleScaling(double scalling)
        {
            ((CompositeTransform)uiCircle.RenderTransform).ScaleX = scalling;
            ((CompositeTransform)uiCircle.RenderTransform).ScaleY = scalling;
        }

        private void uiCanvas_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
        {
            IsDragging = IsPositionOnCircle(e.Position);
            BeforeManipulationCiclePosition = CirclesCurrentTLPosition;
        }

        private void uiCanvas_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            if (IsDragging)
            {
                PositionCircleFromDelta(e);
                ScaleCircleFromDelta(e);
            }
        }

        private void uiCanvas_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            SetCircleScaling(1);
        }
    }
}
