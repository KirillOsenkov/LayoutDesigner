using System.Windows;
using System.Windows.Controls;
using GuiLabs.Undo;

namespace GuiLabs.LayoutDesigner
{
    public class RemoveElementAction : AbstractAction
    {
        private readonly Panel panel;
        private readonly UIElement child;
        private int index;

        public RemoveElementAction(Panel panel, UIElement child)
        {
            this.panel = panel;
            this.child = child;
        }

        protected override void ExecuteCore()
        {
            SelectionManager.ClearSelection();
            index = panel.Children.IndexOf(child);
            panel.Children.RemoveAt(index);
        }

        protected override void UnExecuteCore()
        {
            panel.Children.Insert(index, child);
        }
    }
}
