using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GuiLabs.LayoutDesigner
{
    public class Visuals
    {
        public static Rect GetAbsoluteBounds(UIElement element)
        {
            var transform = element.TransformToVisual(Application.Current.RootVisual);
            var rect = transform.TransformBounds(new Rect(0, 0, element.RenderSize.Width, element.RenderSize.Height));
            return rect;
        }

        public static void SetRectangle(Rectangle shape, Rect bounds)
        {
            Canvas.SetLeft(shape, bounds.X);
            Canvas.SetTop(shape, bounds.Y);
            shape.Width = bounds.Width;
            shape.Height = bounds.Height;
        }

        public static Rectangle InsertionPointLine = new Rectangle()
        {
            Stroke = Brushes.InsertionPointLine,
            StrokeThickness = 1,
            Fill = Brushes.InsertionFill,
            Opacity = 0.5,
            IsHitTestVisible = false
        };

        public static Rectangle InsertionContainerRectangle = new Rectangle()
        {
            Stroke = Brushes.InsertionPointLine,
            StrokeThickness = 1,
            Fill = Brushes.InsertionFill,
            Opacity = 0.5,
            IsHitTestVisible = false
        };

        static SelectionCue Selection = new SelectionCue();

        public static void ShowSelectionCues(UIElement element)
        {
            HideSelectionCues();
            Selection.ShowAt(element);
        }

        public static void HideSelectionCues()
        {
            Selection.Hide();
        }

        public static Image GetImage(string fileName)
        {
            var bitmap = new BitmapImage();
            var stream = Application.GetResourceStream(new Uri("LayoutDesigner;component/Images/" + fileName, UriKind.Relative));
            bitmap.SetSource(stream.Stream);
            Image image = new Image() { Source = bitmap, Stretch = Stretch.None };
            return image;
        }
    }
}
