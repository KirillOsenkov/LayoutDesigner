using System.Windows.Controls;
using GuiLabs.Undo;

namespace GuiLabs.LayoutDesigner
{
    public class InsertColumnAction : AbstractAction
    {
        private readonly DragDropGrid grid;
        private readonly int columnIndex;
        private readonly ColumnDefinition columnDefinition;

        public InsertColumnAction(DragDropGrid grid, int columnIndex)
        {
            this.grid = grid;
            this.columnIndex = columnIndex;
            this.columnDefinition = new ColumnDefinition();
        }

        protected override void ExecuteCore()
        {
            this.grid.InsertColumnDefinitionCore(this.columnDefinition, this.columnIndex);
        }

        protected override void UnExecuteCore()
        {
            this.grid.RemoveColumnDefinitionCore(this.columnIndex);
        }
    }
}
