using System.Windows;
using System.Windows.Shapes;

namespace GuiLabs.LayoutDesigner
{
    public class SelectionCue
    {
        private Rectangle topLeft = new Rectangle();
        private Rectangle topRight = new Rectangle();
        private Rectangle bottomLeft = new Rectangle();
        private Rectangle bottomRight = new Rectangle();

        public SelectionCue()
        {
            topLeft = new Rectangle();
            topRight = new Rectangle();
            bottomLeft = new Rectangle();
            bottomRight = new Rectangle();

            SetStyle(topLeft);
            SetStyle(topRight);
            SetStyle(bottomLeft);
            SetStyle(bottomRight);
        }

        private void SetStyle(Rectangle r)
        {
            r.Fill = Brushes.SelectionCue;
            r.IsHitTestVisible = false;
        }

        public void ShowAt(UIElement element)
        {
            Designer.Instance.Canvas.Children.Add(topLeft);
            Designer.Instance.Canvas.Children.Add(topRight);
            Designer.Instance.Canvas.Children.Add(bottomLeft);
            Designer.Instance.Canvas.Children.Add(bottomRight);

            var bounds = Visuals.GetAbsoluteBounds(element);
            var size = 8;

            Visuals.SetRectangle(topLeft, new Rect(bounds.Left - size, bounds.Top - size, size, size));
            Visuals.SetRectangle(topRight, new Rect(bounds.Right, bounds.Top - size, size, size));
            Visuals.SetRectangle(bottomLeft, new Rect(bounds.Left - size, bounds.Bottom, size, size));
            Visuals.SetRectangle(bottomRight, new Rect(bounds.Right, bounds.Bottom, size, size));
        }

        public void Hide()
        {
            if (topLeft.Parent != null)
            {
                Designer.Instance.Canvas.Children.Remove(topLeft);
            }
            if (topRight.Parent != null)
            {
                Designer.Instance.Canvas.Children.Remove(topRight);
            }
            if (bottomLeft.Parent != null)
            {
                Designer.Instance.Canvas.Children.Remove(bottomLeft);
            }
            if (bottomRight.Parent != null)
            {
                Designer.Instance.Canvas.Children.Remove(bottomRight);
            }
        }
    }
}
