using System.Xml.Linq;

namespace GuiLabs.LayoutDesigner
{
    public interface IXamlWriter
    {
        XElement WriteXaml();
    }
}
