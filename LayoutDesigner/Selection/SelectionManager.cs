using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GuiLabs.LayoutDesigner
{
    public class SelectionManager
    {
        private static readonly List<UIElement> selected = new List<UIElement>();

        public static void Select(UIElement element)
        {
            ClearSelection();
            selected.Add(element);
            var proxy = ProxyManager.GetProxy(element);
            Designer.Instance.ShowProperties(proxy);
            UpdateSelectionCues(element);
            Designer.Instance.RecycleBin.UpdateOpacity();
        }

        private static void UpdateSelectionCues(UIElement element)
        {
            Visuals.ShowSelectionCues(element);
        }

        public static void ClearSelection()
        {
            if (!HasSelection)
            {
                return;
            }
            selected.Clear();
            Designer.Instance.ShowProperties(null);
            Designer.Instance.GridPropertyGrid.Show(null, null);
            Visuals.HideSelectionCues();
            Designer.Instance.RecycleBin.UpdateOpacity();
        }

        public static IEnumerable<UIElement> GetSelection()
        {
            return selected.ToArray();
        }

        public static void UpdateSelection()
        {
            if (HasSelection)
            {
                UpdateSelectionCues(selected[0]);
            }
        }

        public static bool HasSelection
        {
            get
            {
                return selected.Count > 0;
            }
        }

        public static void Delete()
        {
            if (!HasSelection)
            {
                return;
            }

            FrameworkElement toDelete = selected[0] as FrameworkElement;
            if (toDelete != null)
            {
                Panel parent = toDelete.Parent as Panel;
                if (parent != null)
                {
                    ClearSelection();
                    Actions.Delete(parent, toDelete);
                }
            }
        }
    }
}
