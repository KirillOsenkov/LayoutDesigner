using System.Windows;

namespace GuiLabs.LayoutDesigner
{
    public class StackPanelDropLocation : IDropLocation
    {
        private readonly DragDropPanel panel;
        private readonly int index;
        private readonly PointSituationRelativeToRect classification;
        private readonly Rect insertionLineCoordinates;

        public StackPanelDropLocation(DragDropPanel panel, int index, PointSituationRelativeToRect classification, Rect coordinates)
        {
            this.panel = panel;
            this.index = index;
            this.classification = classification;
            this.insertionLineCoordinates = coordinates;
        }

        public bool IsNearby()
        {
            if (DragManager.DraggedElement == null || panel.Children.Count == 0)
            {
                return false;
            }

            if (panel.Children[index] == DragManager.DraggedElement
                || (index > 0
                    && panel.Children[index - 1] == DragManager.DraggedElement
                    && classification == PointSituationRelativeToRect.Before))
            {
                return true;
            }

            return false;
        }

        public void Drop(IDragSource dragSource, UIElement draggedElement)
        {
            // important to get the old index before dragSource.CompleteDrag() because 
            // the index of the deleted control will be "not found" after it's deleted
            int oldIndex = GetOldIndex(dragSource, draggedElement);

            dragSource.CompleteDrag();

            int index = this.index;
            if (oldIndex > -1 && oldIndex < index)
            {
                index--;
            }
            if (classification == PointSituationRelativeToRect.After)
            {
                index++;
            }
            Actions.InsertChild(panel, draggedElement, index);

            dragSource.AfterDropComplete();

            Actions.InvalidateLayout(panel);
        }

        private int GetOldIndex(IDragSource dragSource, UIElement draggedElement)
        {
            var sourcePanel = dragSource as StackPanelControl;
            int oldIndex = -1;
            if (sourcePanel != null && sourcePanel.Panel == panel)
            {
                oldIndex = sourcePanel.Panel.Children.IndexOf(draggedElement);
            }
            return oldIndex;
        }

        public void HideVisualCues()
        {
            Designer.Instance.Canvas.Children.Remove(Visuals.InsertionPointLine);
            Designer.Instance.Canvas.Children.Remove(Visuals.InsertionContainerRectangle);
        }

        public void ShowVisualCues()
        {
            Visuals.SetRectangle(Visuals.InsertionPointLine, insertionLineCoordinates);
            Visuals.SetRectangle(Visuals.InsertionContainerRectangle, Visuals.GetAbsoluteBounds(panel));

            if (classification != PointSituationRelativeToRect.Fill)
            {
                Designer.Instance.Canvas.Children.Add(Visuals.InsertionPointLine);
            }
            else
            {
                Designer.Instance.Canvas.Children.Add(Visuals.InsertionContainerRectangle);
            }
        }
    }
}
