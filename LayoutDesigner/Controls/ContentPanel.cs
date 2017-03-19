using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace GuiLabs.LayoutDesigner
{
    public class ContentPanel : Grid, IDragSource, IDropTarget, IDropLocation, ISupportSelection, IXamlWriter
    {
        public ContentPanel()
        {
            MinWidth = 16;
            MinHeight = 16;
            HorizontalAlignment = HorizontalAlignment.Stretch;
            VerticalAlignment = VerticalAlignment.Stretch;
            this.Background = Brushes.ContentBackground;
            new DragSourceCapability(this);
            new DropTargetCapability(this);
        }

        public void HideVisualCues()
        {
            Designer.Instance.Canvas.Children.Remove(Visuals.InsertionContainerRectangle);
        }

        public void ShowVisualCues()
        {
            Visuals.SetRectangle(Visuals.InsertionContainerRectangle, Visuals.GetAbsoluteBounds(this));
            Designer.Instance.Canvas.Children.Add(Visuals.InsertionContainerRectangle);
        }

        public void CompleteDrag()
        {
            Actions.DeleteContentPanelContents(this);
        }

        public void AfterDropComplete()
        {
            Actions.DeleteEmptyRowsAndColumns(this);
        }

        public new void Drop(IDragSource dragSource, UIElement draggedElement)
        {
            dragSource.CompleteDrag();
            DragDropGrid grid = this.Parent as DragDropGrid;
            if (grid != null)
            {
                int column = Grid.GetColumn(this);
                int row = Grid.GetRow(this);
                Actions.AddNewCell(grid, column, row, draggedElement);
            }
            else
            {
                Actions.InsertChild(this, draggedElement);
            }
            dragSource.AfterDropComplete();
        }

        public void StartDrag(UIElement result)
        {
            DragManager.StartDrag(this, result);
        }

        public UIElement GetElementToDrag(Point cursor)
        {
            if (this.Children.Count == 0)
            {
                return null;
            }
            return this.Children[0] as UIElement;
        }

        public IDropLocation GetDropLocation(Point cursor)
        {
            var result = this.Children.Count == 0 ? this : null;
            if (DragManager.DraggedElement.IsElementInSubtree(this))
            {
                result = null;
            }
            return result;
        }

        public UIElement TargetContainer
        {
            get
            {
                return this;
            }
        }

        public UIElement SourceContainer
        {
            get
            {
                return this;
            }
        }

        public virtual bool IsSelectionAllowed(UIElement candidateToBeSelected)
        {
            return true;
        }

        public XElement WriteXaml()
        {
            if (this.Children.Count == 0)
            {
                return null;
            }

            var result = XamlWriter.Write(this.Children[0]);
            XamlWriter.WriteGridRowColumn(this, result);
            return result;
        }

        public void Select(UIElement element)
        {
            SelectionManager.Select(element);

            DragDropGrid grid = this.Parent as DragDropGrid;
            if (grid == null)
            {
                return;
            }

            var proxy = new GridItemProxy(grid, this);
            Designer.Instance.GridPropertyGrid.Show(proxy, Designer.Instance.ActionManager);
        }
    }
}
