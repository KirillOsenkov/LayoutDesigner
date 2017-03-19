using System.Windows;
using System.Windows.Controls;

namespace GuiLabs.LayoutDesigner
{
    public abstract class DragDropControl : ContentControl, IDragSource, IDropTarget, ISupportSelection
    {
        public Border Border { get; set; }
        public DragDropPanel Panel { get; set; }

        public DragDropControl()
        {
            Initialize();
        }

        protected virtual void Initialize()
        {
            this.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            this.VerticalContentAlignment = VerticalAlignment.Stretch;

            Panel = CreatePanel();

            Border = new Border();
            Border.BorderThickness = new Thickness(1);
            Border.BorderBrush = Brushes.Border;
            Border.Child = Panel;
            
            this.Content = Border;

            InitializeDragDropCapabilities();
        }

        protected virtual void InitializeDragDropCapabilities()
        {
            new DragSourceCapability(this);
            new DropTargetCapability(this);
        }

        protected virtual DragDropPanel CreatePanel()
        {
            return new DragDropStackPanel();
        }

        public virtual void CompleteDrag()
        {
            Actions.RemoveElementFromPanel(Panel, DragManager.DraggedElement);
        }

        public virtual void AfterDropComplete()
        {

        }

        public UIElement GetElementToDrag(Point cursor)
        {
            var result = Panel.HitTest(cursor);
            if (result is ContentPanel)
            {
                result = null;
            }
            return result;
        }

        public virtual void StartDrag(UIElement result)
        {
            DragManager.StartDrag(this, result);
        }

        public IDropLocation GetDropLocation(Point cursor)
        {
            return Panel.GetDropLocation(cursor);
        }

        public UIElement SourceContainer
        {
            get
            {
                return Panel;
            }
        }

        public UIElement TargetContainer
        {
            get
            {
                return Panel;
            }
        }

        public virtual bool IsSelectionAllowed(UIElement candidateToBeSelected)
        {
            return true;
        }

        public virtual void Select(UIElement element)
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
