using System;
using System.Windows;
using System.Windows.Media;

namespace GuiLabs.LayoutDesigner
{
    public static class Utilities
    {
        public static double Distance(this Point source, Point other)
        {
            return Math.Sqrt((source.X - other.X) * (source.X - other.X) + (source.Y - other.Y) * (source.Y - other.Y));
        }

        public static bool IsElementInSubtree(this DependencyObject root, DependencyObject element)
        {
            if (root == null || element == null)
            {
                return false;
            }

            if (root == element)
            {
                return true;
            }

            int count = VisualTreeHelper.GetChildrenCount(root);
            if (count == 0)
            {
                return false;
            }

            for (int i = 0; i < count; i++)
            {
                var child = VisualTreeHelper.GetChild(root, i);
                if (child != null && child.IsElementInSubtree(element))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
