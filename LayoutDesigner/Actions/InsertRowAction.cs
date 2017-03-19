using System.Windows.Controls;
using GuiLabs.Undo;

namespace GuiLabs.LayoutDesigner
{
    public class InsertRowAction : AbstractAction
    {
        private readonly DragDropGrid grid;
        private readonly int rowIndex;
        private readonly RowDefinition rowDefinition;

        public InsertRowAction(DragDropGrid grid, int rowIndex)
        {
            this.grid = grid;
            this.rowIndex = rowIndex;
            this.rowDefinition = new RowDefinition();
        }

        protected override void ExecuteCore()
        {
            grid.InsertRowDefinitionCore(this.rowDefinition, this.rowIndex);
        }

        protected override void UnExecuteCore()
        {
            grid.RemoveRowDefinitionCore(this.rowIndex);
        }
    }
}
