using System.Windows.Forms;

namespace EZCode.Windows
{
    public class Window : Form
    {
        public string click { get; set; }
        public string mousehover { get; set; }
        public string move { get; set; }
        public string backcolor { get; set; }
        public string forecolor { get; set; }
        public string font { get; set; }
        public string image { get; set; }
        public string imagelayout { get; set; }
        public string text { get; set; }
        public string scroll { get; set; }
        public string focused { get; set; }
        public string controladded { get; set; }
        public string controlremoved { get; set; }
        public string defocused { get; set; }
        public string close { get; set; }
        public string open { get; set; }
        public string enabledchanged { get; set; }
        public string keydown { get; set; }
        public string keyup { get; set; }
        public string keypress { get; set; }
        public string resized { get; set; }
        public string resizedstart { get; set; }
        public string resizedend { get; set; }
        public string BGImageFile { get; set; }
        public string IconImageFile { get; set; }

        public Window()
        {
            setsuff("");
        }
        public Window(string name)
        {
            setsuff(name);
        }

        void setsuff(string name)
        {
            KeyPreview = true;
            StartPosition = FormStartPosition.Manual;
            Name = name;
            Width = 600;
            Height = 400;
            Text = name;

        }
    }
}
