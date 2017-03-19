using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace GuiLabs.LayoutDesigner
{
    public class Toolbox : StackPanelControl
    {
        public Toolbox()
        {
            this.Effect = new DropShadowEffect()
            {
                BlurRadius = 16,
                Color = Colors.Gray,
                ShadowDepth = 16,
                Opacity = 0.5,
            };
        }

        /// <summary>
        /// We only can start dragging, but we don't support dropping into the toolbox
        /// </summary>
        protected override void InitializeDragDropCapabilities()
        {
            new DragSourceCapability(this);
        }

        public void Add(UIElement element)
        {
            Panel.Children.Add(element);
        }

        public override void StartDrag(UIElement underCursor)
        {
            ToolboxItem item = underCursor as ToolboxItem;
            if (item == null)
            {
                return;
            }
            var copy = Factory.CreateControl(item.Title);
            DragManager.StartDrag(this, underCursor);
            DragManager.DraggedElement = copy;
        }

        /// <summary>
        /// Don't remove the dragged element from the toolbox. 
        /// We're going to need it again.
        /// </summary>
        public override void CompleteDrag()
        {
        }

        public override void AfterDropComplete()
        {
            Actions.Select(DragManager.DraggedElement);
        }

        /// <summary>
        /// We can't select toolbox items
        /// </summary>
        public override bool IsSelectionAllowed(UIElement candidateToBeSelected)
        {
            return false;
        }
    }
}
