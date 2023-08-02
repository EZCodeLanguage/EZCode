using Variables;
using Objects;
using System.Windows.Forms;

namespace Groups;

class Group
{
    public List<Button> Buttons = new List<Button>();
    public List<Label> Labels = new List<Label>();
    public List<GObject> Objects = new List<GObject>();
    public List<TextBox> Textboxes = new List<TextBox>();
    public string Name { get; set; }
    public int X { get; set; }
    public int Y { get; set; }
    public int scaleX { get; set; }
    public int scaleY { get; set; }
    public Group(string name)
    {
        this.Name = name;
    }
}