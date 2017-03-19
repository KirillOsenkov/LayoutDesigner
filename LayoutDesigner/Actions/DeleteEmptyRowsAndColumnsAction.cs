using System.Windows.Controls;
using GuiLabs.Undo;

namespace GuiLabs.LayoutDesigner
{
    public class DeleteEmptyRowsAndColumnsAction : AbstractAction
    {
        private readonly DragDropGrid grid;
        private int row;
        private int column;
        private ColumnDefinition columnDefinition;
        private RowDefinition rowDefinition;

        public DeleteEmptyRowsAndColumnsAction(DragDropGrid grid)
        {
            this.grid = grid;
        }

        protected override void ExecuteCore()
        {
            column = grid.FindEmptyColumn();
            if (column != -1 && grid.ColumnDefinitions.Count > 1)
            {
                columnDefinition = grid.ColumnDefinitions[column];
                grid.RemoveColumnDefinitionCore(column);
            }

            row = grid.FindEmptyRow();
            if (row != -1 && grid.RowDefinitions.Count > 1)
            {
                rowDefinition = grid.RowDefinitions[row];
                grid.RemoveRowDefinitionCore(row);
            }
        }

        protected override void UnExecuteCore()
        {
            if (row != -1 && rowDefinition != null)
            {
                grid.InsertRowDefinitionCore(rowDefinition, row);
            }
            if (column != -1 && columnDefinition != null)
            {
                grid.InsertColumnDefinitionCore(columnDefinition, column);
            }
        }
    }
}
