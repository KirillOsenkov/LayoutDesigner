using System.Windows;
using GuiLabs.Undo;

namespace GuiLabs.LayoutDesigner
{
    public class InsertCellAction : AbstractAction
    {
        private readonly DragDropGrid grid;
        private readonly int columnIndex;
        private readonly int rowIndex;
        private readonly UIElement element;

        public InsertCellAction(DragDropGrid grid, int columnIndex, int rowIndex, UIElement element)
        {
            this.grid = grid;
            this.columnIndex = columnIndex;
            this.rowIndex = rowIndex;
            this.element = element;
        }

        protected override void ExecuteCore()
        {
            var contentPanel = grid.FindContentPanel(columnIndex, rowIndex);
            contentPanel.Children.Add(element);
        }

        protected override void UnExecuteCore()
        {
            var contentPanel = grid.FindContentPanel(columnIndex, rowIndex);
            contentPanel.Children.RemoveAt(0);
        }
    }
}
