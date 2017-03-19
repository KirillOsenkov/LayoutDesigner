using System.Windows.Controls;
using GuiLabs.Undo;

namespace GuiLabs.LayoutDesigner
{
    public class RemoveRowAction : AbstractAction
    {
        private readonly DragDropGrid grid;
        private readonly int rowIndex;
        private RowDefinition rowDefinition;

        public RemoveRowAction(DragDropGrid grid, int rowIndex)
        {
            this.grid = grid;
            this.rowIndex = rowIndex;
            this.rowDefinition = grid.RowDefinitions[rowIndex];
        }

        protected override void ExecuteCore()
        {
            grid.RemoveRowDefinitionCore(this.rowIndex);
        }

        protected override void UnExecuteCore()
        {
            grid.InsertRowDefinitionCore(this.rowDefinition, this.rowIndex);
        }
    }
}
