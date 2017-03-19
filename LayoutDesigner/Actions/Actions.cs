using System.Windows;
using System.Windows.Controls;
using GuiLabs.Undo;

namespace GuiLabs.LayoutDesigner
{
    public class Actions
    {
        private static ActionManager Manager
        {
            get
            {
                return Designer.Instance.ActionManager;
            }
        }

        public static Transaction Transaction()
        {
            return Manager.CreateTransaction();
        }

        private static void Record(IAction action)
        {
            Manager.RecordAction(action);
        }

        public static void InsertChild(Panel panel, UIElement element)
        {
            InsertChild(panel, element, panel.Children.Count);
        }

        public static void InsertChild(Panel panel, UIElement draggedElement, int index)
        {
            var action = new CallMethodAction(() =>
                {
                    panel.Children.Insert(index, draggedElement);
                },
                () =>
                {
                    panel.Children.Remove(draggedElement);
                });
            Record(action);
        }

        public static void InsertColumn(DragDropGrid grid, int columnIndex)
        {
            var action = new InsertColumnAction(grid, columnIndex);
            Record(action);
        }

        public static void RemoveColumn(DragDropGrid grid, int columnIndex)
        {
            var action = new RemoveColumnAction(grid, columnIndex);
            Record(action);
        }

        public static void InsertRow(DragDropGrid grid, int rowIndex)
        {
            var action = new InsertRowAction(grid, rowIndex);
            Record(action);
        }

        public static void RemoveRow(DragDropGrid grid, int rowIndex)
        {
            var action = new RemoveRowAction(grid, rowIndex);
            Record(action);
        }

        public static void AddNewCell(DragDropGrid grid, int columnIndex, int rowIndex, UIElement element)
        {
            var action = new InsertCellAction(grid, columnIndex, rowIndex, element);
            Record(action);
        }

        public static void RemoveCell(DragDropGrid grid, int columnIndex, int rowIndex)
        {
            var action = new RemoveCellAction(grid, columnIndex, rowIndex);
            Record(action);
        }

        public static void DeleteEmptyRowsAndColumns(DragDropGrid grid)
        {
            var action = new DeleteEmptyRowsAndColumnsAction(grid);
            Record(action);
        }

        public static void DeleteEmptyRowsAndColumns(ContentPanel contentPanel)
        {
            DragDropGrid grid = contentPanel.Parent as DragDropGrid;
            if (grid != null)
            {
                Actions.DeleteEmptyRowsAndColumns(grid);
            }
        }

        public static void RemoveElementFromPanel(Panel panel, UIElement element)
        {
            var action = new RemoveElementAction(panel, element);
            Record(action);
        }

        public static void DeleteContentPanelContents(ContentPanel contentPanel)
        {
            DragDropGrid grid = contentPanel.Parent as DragDropGrid;
            if (grid != null)
            {
                int columnIndex = Grid.GetColumn(contentPanel);
                int rowIndex = Grid.GetRow(contentPanel);
                Actions.RemoveCell(grid, columnIndex, rowIndex);
                return;
            }
            Actions.RemoveElementFromPanel(contentPanel, contentPanel.Children[0]);
        }

        public static void Delete(Panel panel, FrameworkElement toDelete)
        {
            using (Transaction())
            {
                ContentPanel contentPanel = panel as ContentPanel;
                if (contentPanel != null)
                {
                    DeleteContentPanelContents(contentPanel);
                    DeleteEmptyRowsAndColumns(contentPanel);
                }
                else
                {
                    RemoveElementFromPanel(panel, toDelete);
                }
                Actions.InvalidateLayout(panel);
            }
        }

        public static void InvalidateLayout(Panel panel)
        {
            var action = new CallMethodAction(() =>
            {
                panel.InvalidateMeasure();
                panel.InvalidateArrange();
            },
            () =>
            {
                panel.InvalidateMeasure();
                panel.InvalidateArrange();
            });
            Record(action);
        }

        public static void Select(UIElement element)
        {
            var action = new CallMethodAction(() =>
            {
                SelectionManager.Select(element);
            }, null);
            Record(action);
        }
    }
}
