using System.Windows;
using System.Windows.Input;

namespace GuiLabs.LayoutDesigner
{
    public sealed class DropTargetCapability
    {
        private readonly IDropTarget dropTarget;

        public DropTargetCapability(IDropTarget dropTarget)
        {
            this.dropTarget = dropTarget;
            Subscribe();
        }

        UIElement DropTargetContainer
        {
            get
            {
                return this.dropTarget.TargetContainer;
            }
        }

        private void Subscribe()
        {
            DropTargetContainer.MouseMove += DropTargetContainer_MouseMove;
            DropTargetContainer.MouseLeftButtonUp += DropTargetContainer_MouseLeftButtonUp;
            DropTargetContainer.MouseLeave += DropTargetContainer_MouseLeave;
        }

        private void DropTargetContainer_MouseMove(object sender, MouseEventArgs e)
        {
            if (!DragManager.IsDragging || e.OriginalSource != sender)
            {
                return;
            }
            var dropTarget = this.dropTarget.GetDropLocation(e.GetPosition(DropTargetContainer));
            DragManager.UpdateDropTarget(dropTarget);
        }

        private void DropTargetContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DragManager.Drop();
            e.Handled = true;
        }

        private void DropTargetContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            if (!DragManager.IsDragging)
            {
                return;
            }
            DragManager.UpdateDropTarget(null);
        }
    }
}
