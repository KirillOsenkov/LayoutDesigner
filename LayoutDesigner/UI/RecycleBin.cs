using System.Windows;
using System.Windows.Controls;

namespace GuiLabs.LayoutDesigner
{
    public class RecycleBin : Grid, IDropTarget, IDropLocation
    {
        private readonly Image image;

        public RecycleBin()
        {
            this.Background = Brushes.DesignerBackground;
            image = Visuals.GetImage("recyclebin_empty.png");
            image.Margin = new Thickness(16);
            this.Children.Add(image);
            this.MouseLeftButtonDown += RecycleBin_MouseLeftButtonDown;
            new DropTargetCapability(this);
        }

        void RecycleBin_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            SelectionManager.Delete();
        }

        public void UpdateOpacity()
        {
            if (SelectionManager.HasSelection)
            {
                this.Opacity = 1;
            }
            else
            {
                this.Opacity = 0.5;
            }
        }

        public IDropLocation GetDropLocation(Point cursor)
        {
            if (DragManager.DragSource is Toolbox)
            {
                return null;
            }
            return this;
        }

        public UIElement TargetContainer
        {
            get { return image; }
        }

        public new void Drop(IDragSource dragSource, UIElement draggedElement)
        {
            dragSource.CompleteDrag();
            dragSource.AfterDropComplete();
        }

        public void ShowVisualCues()
        {
            this.Opacity = 1;
            Visuals.SetRectangle(Visuals.InsertionContainerRectangle, Visuals.GetAbsoluteBounds(this));
            Designer.Instance.Canvas.Children.Add(Visuals.InsertionContainerRectangle);
        }

        public void HideVisualCues()
        {
            UpdateOpacity();
            Designer.Instance.Canvas.Children.Remove(Visuals.InsertionContainerRectangle);
        }
    }
}
