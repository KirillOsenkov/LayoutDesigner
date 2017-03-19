using System.Windows;

namespace GuiLabs.LayoutDesigner
{
    public class InsertRowGridDropLocation : IDropLocation
    {
        private readonly DragDropGrid panel;
        private readonly int column;
        private readonly int row;
        private readonly Rect insertionLineCoordinates;

        public InsertRowGridDropLocation(DragDropGrid panel, int column, int row, bool insertAtTheEnd, Rect insertionLineCoordinates)
        {
            this.panel = panel;
            if (insertAtTheEnd)
            {
                row++;
            }
            this.column = column;
            this.row = row;
            this.insertionLineCoordinates = insertionLineCoordinates;
        }

        public void Drop(IDragSource dragSource, UIElement draggedElement)
        {
            dragSource.CompleteDrag();
            Actions.InsertRow(panel, row);
            Actions.AddNewCell(panel, column, row, draggedElement);
            dragSource.AfterDropComplete();
            Actions.InvalidateLayout(panel);
        }

        public void HideVisualCues()
        {
            Designer.Instance.Canvas.Children.Remove(Visuals.InsertionPointLine);
        }

        public void ShowVisualCues()
        {
            Visuals.SetRectangle(Visuals.InsertionPointLine, insertionLineCoordinates);
            Designer.Instance.Canvas.Children.Add(Visuals.InsertionPointLine);
        }
    }
}
