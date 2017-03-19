using System.Windows;
using GuiLabs.Undo;

namespace GuiLabs.LayoutDesigner
{
    public class RemoveCellAction : AbstractAction
    {
        private readonly DragDropGrid grid;
        private readonly int columnIndex;
        private readonly int rowIndex;
        private UIElement element;

        public RemoveCellAction(DragDropGrid grid, int columnIndex, int rowIndex)
        {
            this.grid = grid;
            this.columnIndex = columnIndex;
            this.rowIndex = rowIndex;
        }

        protected override void ExecuteCore()
        {
            var contentPanel = grid.FindContentPanel(columnIndex, rowIndex);
            element = contentPanel.Children[0];
            contentPanel.Children.RemoveAt(0);
        }

        protected override void UnExecuteCore()
        {
            var contentPanel = grid.FindContentPanel(columnIndex, rowIndex);
            contentPanel.Children.Add(element);
        }
    }
}
