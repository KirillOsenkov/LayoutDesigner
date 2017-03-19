using System.Windows;
using System.Windows.Input;

namespace GuiLabs.LayoutDesigner
{
    public sealed class DragSourceCapability
    {
        private readonly IDragSource DragSource;
        private Point mouseDownCoordinates;

        public DragSourceCapability(IDragSource dragSource)
        {
            DragSource = dragSource;
            Subscribe();
        }

        private UIElement DragSourceContainer
        {
            get
            {
                return DragSource.SourceContainer;
            }
        }

        private void Subscribe()
        {
            DragSourceContainer.MouseLeftButtonDown += DragSourceContainer_MouseLeftButtonDown;
            DragSourceContainer.MouseMove += DragSourceContainer_MouseMove;
            DragSourceContainer.MouseLeftButtonUp += DragSourceContainer_MouseLeftButtonUp;
            DragSourceContainer.MouseLeave += DragSourceContainer_MouseLeave;
        }

        private Point Coordinates(MouseEventArgs e)
        {
            return e.GetPosition(DragSourceContainer);
        }

        private void DragSourceContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mouseDownCoordinates = Coordinates(e);
            if (DragManager.DragCandidate == null)
            {
                DragManager.DragCandidate = DragSource.GetElementToDrag(mouseDownCoordinates);
                if (DragManager.DragCandidate != null)
                {
                    DragManager.DragCandidateSponsor = DragSource;
                }
            }
        }

        private void DragSourceContainer_MouseMove(object sender, MouseEventArgs e)
        {
            var coordinates = Coordinates(e);
            if (DragManager.DragCandidate != null
                && DragManager.DragCandidateSponsor == DragSource
                && !DragManager.IsDragging
                && coordinates.Distance(mouseDownCoordinates) > 15)
            {
                SelectionManager.ClearSelection();
                DragSource.StartDrag(DragManager.DragCandidate);
                DragManager.MouseMove(e.GetPosition(Designer.Instance.Canvas));
            }
        }

        private void DragSourceContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // this entire handler only makes sense if we have a drag candidate
            if (DragManager.DragCandidate == null || DragManager.DragCandidateSponsor == null)
            {
                return;
            }

            var coordinates = Coordinates(e);
            if (DragSource is ISupportSelection
                && (DragSource as ISupportSelection).IsSelectionAllowed(DragManager.DragCandidate)
                && coordinates.Distance(mouseDownCoordinates) < 15
                && !DragManager.IsDragging)
            {
                DragManager.DragCandidateSponsor.Select(DragManager.DragCandidate);
            }

            DragManager.ResetDragCandidate();
        }

        private void DragSourceContainer_MouseLeave(object sender, MouseEventArgs e)
        {
            var coordinates = Coordinates(e);
            if (DragManager.DragCandidate != null
                && DragManager.DragCandidateSponsor == DragSource
                && !DragManager.IsDragging)
            {
                SelectionManager.ClearSelection();
                DragSource.StartDrag(DragManager.DragCandidate);
                DragManager.MouseMove(e.GetPosition(Designer.Instance.Canvas));
            }
        }
    }
}
