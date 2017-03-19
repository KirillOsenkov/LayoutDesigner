using System.Windows.Controls;
using GuiLabs.Undo;

namespace GuiLabs.LayoutDesigner
{
    public class RemoveColumnAction : AbstractAction
    {
        private readonly DragDropGrid grid;
        private readonly int columnIndex;
        private ColumnDefinition columnDefinition;

        public RemoveColumnAction(DragDropGrid grid, int columnIndex)
        {
            this.grid = grid;
            this.columnIndex = columnIndex;
            this.columnDefinition = grid.ColumnDefinitions[columnIndex];
        }

        protected override void ExecuteCore()
        {
            grid.RemoveColumnDefinitionCore(this.columnIndex);
        }

        protected override void UnExecuteCore()
        {
            grid.InsertColumnDefinitionCore(this.columnDefinition, this.columnIndex);
        }
    }
}
