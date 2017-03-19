using System;
using System.Collections.Generic;
using System.Windows.Controls;

namespace GuiLabs.LayoutDesigner
{
    public class ProxyManager
    {
        static Dictionary<Type, Type> proxies = new Dictionary<Type, Type>();

        static ProxyManager()
        {
            Add<TextBox, TextBoxProxy>();
            Add<Button, ButtonProxy>();
            Add<CheckBox, CheckBoxProxy>();
            Add<RadioButton, RadioButtonProxy>();
            Add<TextBlock, TextBlockProxy>();
            Add<ComboBox, ComboBoxProxy>();
            Add<StackPanelControl, StackPanelControlProxy>();
            Add<GridControl, GridControlProxy>();
        }

        private static void Add<T1, T2>()
        {
            proxies.Add(typeof(T1), typeof(T2));
        }

        public static object GetProxy(object element)
        {
            Type result = null;
            if (proxies.TryGetValue(element.GetType(), out result))
            {
                return Activator.CreateInstance(result, element);
            }
            return element;
        }
    }
}
