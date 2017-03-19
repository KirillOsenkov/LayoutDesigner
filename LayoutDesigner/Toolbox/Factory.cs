using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace GuiLabs.LayoutDesigner
{
    public class Factory
    {
        static Factory()
        {
            factories.Add("StackPanel", () => new StackPanelControl());
            factories.Add("Grid", () => new GridControl() { });
            factories.Add("TextBox", () => new TextBox() { Text = "Text", MinWidth = 73, IsHitTestVisible = false });
            factories.Add("TextBlock", () => new TextBlock() { Text = "Text", IsHitTestVisible = false });
            factories.Add("Button", () => new Button() { Content = "Button", MinWidth = 73, MinHeight = 21, IsHitTestVisible = false });
            factories.Add("Checkbox", () => new CheckBox() { Content = "Checkbox", MinWidth = 73, IsHitTestVisible = false });
            factories.Add("RadioButton", () => new RadioButton() { Content = "RadioButton", IsHitTestVisible = false });
            factories.Add("ComboBox", () => new ComboBox() { MinWidth = 73, IsHitTestVisible = false });
        }

        private static Dictionary<string, Func<UIElement>> factories = new Dictionary<string, Func<UIElement>>();

        public static UIElement CreateControl(string title)
        {
            UIElement result = null;
            Func<UIElement> factory = null;
            if (factories.TryGetValue(title, out factory))
            {
                result = factory();
            }
            return result;
        }

        public static void FillToolbox(Toolbox toolBox)
        {
            foreach (var str in factories.Keys)
            {
                if (str == "StackPanel" || str == "Grid")
                {
                    continue;
                }
                AddToolBoxItem(toolBox, str);
            }
        }

        public static void AddToolBoxItem(Toolbox toolBox, string str)
        {
            toolBox.Add(new ToolboxItem(string.Format("Control_{0}.png", str), str));
        }
    }
}
