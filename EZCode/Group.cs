namespace EZCode.Groups
{
    using EZCode.GControls;
    using System.Drawing;
    using System.Windows.Forms;

    public class Group
    {
        public string Method { get; set; }
        public HashSet<GButton> Buttons = new HashSet<GButton>();
        public HashSet<GLabel> Labels = new HashSet<GLabel>();
        public HashSet<GShape> Shapes = new HashSet<GShape>();
        public HashSet<GTextBox> Textboxes = new HashSet<GTextBox>();
        public List<Control> Controls
        {
            get
            {
                List<Control> _controls = new List<Control>();
                _controls.AddRange(Buttons);
                _controls.AddRange(Labels);
                _controls.AddRange(Shapes);
                _controls.AddRange(Textboxes);
                return _controls;
            }
            set
            {
                foreach (Control control in Controls)
                {
                    if (control is GShape g) Shapes.Add(g);
                    if (control is GLabel l) Labels.Add(l);
                    if (control is GButton b) Buttons.Add(b);
                    if (control is GTextBox t) Textboxes.Add(t);
                }
            }
        }
        public string Name { get; set; }
        int _X;
        public int X
        {
            get
            {
                return _X;
            }
            set
            {
                _X = value;
                foreach (Control control in Controls)
                {
                    control.Left = Absolute ? _X : control.Left + _X;
                }
            }
        }
        int _Y;
        public int Y
        {
            get
            {
                return _Y;
            }
            set
            {
                _Y = value;
                foreach (Control control in Controls)
                {
                    control.Top = Absolute ? _Y : control.Top + _Y;
                }
            }
        }
        int _Width;
        public int Width
        {
            get
            {
                return _Width;
            }
            set
            {
                _Width = value;
                foreach (Control control in Controls)
                {
                    control.Width = Absolute ? _Width : control.Width + _Width;
                }
            }
        }
        int _Height;
        public int Height
        {
            get
            {
                return _Height;
            }
            set
            {
                _Height = value;
                foreach (Control control in Controls)
                {
                    control.Height = Absolute ? _Height : control.Height + _Height;
                }
            }
        }
        int _bgR;
        public int bgR
        {
            get
            {
                return _bgR;
            }
            set
            {
                _bgR = value;
                foreach (Control control in Controls)
                {
                    int r = Absolute ? _bgR : control.BackColor.R + _bgR;
                    int g = Absolute ? _bgG : control.BackColor.G + _bgG;
                    int b = Absolute ? _bgB : control.BackColor.B + _bgB;
                    r = r >= 0 && r <= 255 ? r : r > 255 ? 255 : 0;
                    g = g >= 0 && g <= 255 ? g : g > 255 ? 255 : 0;
                    b = b >= 0 && b <= 255 ? b : b > 255 ? 255 : 0;
                    control.BackColor = System.Drawing.Color.FromArgb(r, g, b);
                }
            }
        }
        int _bgG;
        public int bgG
        {
            get
            {
                return _bgG;
            }
            set
            {
                _bgG = value;
                foreach (Control control in Controls)
                {
                    int r = Absolute ? _bgR : control.BackColor.R + _bgR;
                    int g = Absolute ? _bgG : control.BackColor.G + _bgG;
                    int b = Absolute ? _bgB : control.BackColor.B + _bgB;
                    r = r >= 0 && r <= 255 ? r : r > 255 ? 255 : 0;
                    g = g >= 0 && g <= 255 ? g : g > 255 ? 255 : 0;
                    b = b >= 0 && b <= 255 ? b : b > 255 ? 255 : 0;
                    control.BackColor = System.Drawing.Color.FromArgb(r, g, b);
                }
            }
        }
        int _bgB;
        public int bgB
        {
            get
            {
                return _bgB;
            }
            set
            {
                _bgB = value;
                foreach (Control control in Controls)
                {
                    int r = Absolute ? _bgR : control.BackColor.R + _bgR;
                    int g = Absolute ? _bgG : control.BackColor.G + _bgG;
                    int b = Absolute ? _bgB : control.BackColor.B + _bgB;
                    r = r >= 0 && r <= 255 ? r : r > 255 ? 255 : 0;
                    g = g >= 0 && g <= 255 ? g : g > 255 ? 255 : 0;
                    b = b >= 0 && b <= 255 ? b : b > 255 ? 255 : 0;
                    control.BackColor = System.Drawing.Color.FromArgb(r, g, b);
                }
            }
        }
        int _fcR;
        public int fcR
        {
            get
            {
                return _fcR;
            }
            set
            {
                _fcR = value;
                foreach (Control control in Controls)
                {
                    int r = Absolute ? _fcR : control.ForeColor.R + _fcR;
                    int g = Absolute ? _fcG : control.ForeColor.G + _fcG;
                    int b = Absolute ? _fcB : control.ForeColor.B + _fcB;
                    r = r >= 0 && r <= 255 ? r : r > 255 ? 255 : 0;
                    g = g >= 0 && g <= 255 ? g : g > 255 ? 255 : 0;
                    b = b >= 0 && b <= 255 ? b : b > 255 ? 255 : 0;
                    control.ForeColor = System.Drawing.Color.FromArgb(r, g, b);
                }
            }
        }
        int _fcG;
        public int fcG
        {
            get
            {
                return _fcG;
            }
            set
            {
                _fcG = value;
                foreach (Control control in Controls)
                {
                    int r = Absolute ? _fcR : control.ForeColor.R + _fcR;
                    int g = Absolute ? _fcG : control.ForeColor.G + _fcG;
                    int b = Absolute ? _fcB : control.ForeColor.B + _fcB;
                    r = r >= 0 && r <= 255 ? r : r > 255 ? 255 : 0;
                    g = g >= 0 && g <= 255 ? g : g > 255 ? 255 : 0;
                    b = b >= 0 && b <= 255 ? b : b > 255 ? 255 : 0;
                    control.ForeColor = System.Drawing.Color.FromArgb(r, g, b);
                }
            }
        }
        int _fcB;
        public int fcB
        {
            get
            {
                return _fcB;
            }
            set
            {
                _fcB = value;
                foreach (Control control in Controls)
                {
                    int r = Absolute ? _fcR : control.ForeColor.R + _fcR;
                    int g = Absolute ? _fcG : control.ForeColor.G + _fcG;
                    int b = Absolute ? _fcB : control.ForeColor.B + _fcB;
                    r = r >= 0 && r <= 255 ? r : r > 255 ? 255 : 0;
                    g = g >= 0 && g <= 255 ? g : g > 255 ? 255 : 0;
                    b = b >= 0 && b <= 255 ? b : b > 255 ? 255 : 0;
                    control.ForeColor = System.Drawing.Color.FromArgb(r, g, b);
                }
            }
        }
        string _Text;
        public string Text
        {
            get
            {
                return _Text;
            }
            set
            {
                _Text = value;
                foreach (Control control in Controls)
                {
                    control.Text = Absolute ? _Text : control.Text + _Text;
                }
            }
        }
        public Color BackColor
        {
            get
            {
                return Color.FromArgb(_bgR, _bgG, _bgB);
            }
        }
        public Color ForeColor
        {
            get
            {
                return Color.FromArgb(_fcR, _fcG, _fcB);
            }
        }
        public bool Absolute { get; set; }
        public bool isSet { get; set; }
        public Group(string name)
        {
            this.Name = name;
            this.Controls = new List<Control>();
        }
        public void set(List<Control> controls)
        {
            isSet = true;
            clear();
            foreach (Control control in controls)
            {
                if (control is GShape g) Shapes.Add(g);
                if (control is GLabel l) Labels.Add(l);
                if (control is GButton b) Buttons.Add(b);
                if (control is GTextBox t) Textboxes.Add(t);
            }
        }
        public void SetRelativeChenges(Control control)
        {
            X = control.Left;
            Y = control.Top;
            Width = control.Width;
            Height = control.Height;
            bgR = control.BackColor.R;
            bgG = control.BackColor.G;
            bgB = control.BackColor.B;
            fcR = control.ForeColor.R;
            fcG = control.ForeColor.G;
            fcB = control.ForeColor.B;
            Text = control.Text;
        }
        public void SetAbsoluteChenges(Control oldc, Control newc)
        {
            if (oldc.Left != newc.Left) X = newc.Left;
            if (oldc.Top != newc.Top) Y = newc.Top;
            if (oldc.Width != newc.Width) Width = newc.Width;
            if (oldc.Height != newc.Height) Height = newc.Height;
            if (oldc.BackColor.R != newc.BackColor.R) bgR = newc.BackColor.R;
            if (oldc.BackColor.G != newc.BackColor.G) bgG = newc.BackColor.G;
            if (oldc.BackColor.B != newc.BackColor.B) bgB = newc.BackColor.B;
            if (oldc.ForeColor.R != newc.ForeColor.R) fcR = newc.ForeColor.R;
            if (oldc.ForeColor.G != newc.ForeColor.G) fcG = newc.ForeColor.G;
            if (oldc.ForeColor.B != newc.ForeColor.B) fcB = newc.ForeColor.B;
            if (oldc.Text != newc.Text) Text = newc.Text;
        }
        public void clear()
        {
            Buttons.Clear();
            Textboxes.Clear();
            Shapes.Clear();
            Labels.Clear();
        }

        public static explicit operator Control(Group v)
        {
            Control control = new Control();

            control.Text = v.Text;
            control.Top = v.Y;
            control.Left = v.X;
            control.Width = v.Width;
            control.Height = v.Height;
            control.BackColor = Color.FromArgb(v.bgR, v.bgG, v.bgB);
            control.ForeColor = Color.FromArgb(v.fcR, v.fcG, v.fcB);

            return control;
        }
    }
}