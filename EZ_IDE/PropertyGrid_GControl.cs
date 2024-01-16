using System.ComponentModel;
using EZCode.Windows;
using EZCode.GControls;

namespace EZ_IDE
{
    public class PropertyGrid_GControl
    {
        public Control control;
        public Window window;
        public bool IsWindow = false;
        public PropertyGrid_GControl(Control con, bool iswindow = false, Window? wind = null)
        {
            window = wind ?? new Window();
            IsWindow = iswindow;
            control = con;
            if (!IsWindow)
            {
                X = con.Left;
                Y = con.Top;
                Width = con.Width;
                Height = con.Height;
                BackColor = con.BackColor;
                ForeColor = con.ForeColor;
                Text = con.Text;
                Name = con.Name;
                Id = con.AccessibleName;
                Enable = con.Enabled;
                AutoSize = con.AutoSize;
                Image = con.BackgroundImage;
                ImageLayout = con.BackgroundImageLayout;
                MainFont = con.Font;
                if (control is GShape a)
                {
                    Poly = a.Poly;
                    Points = a.Points ?? new PointF[0];
                }
                if (control is GButton b)
                {
                    Align = b.TextAlign;
                }
                else if (control is GLabel c)
                {
                    Align = c.TextAlign;
                }
                else if (control is GTextBox d)
                {
                    Multiline = d.Multiline;
                    ReadOnly = d.ReadOnly;
                    WordWrap = d.WordWrap;
                    ScrollBars = d.ScrollBars;
                }
            }
            else
            {
                BackColor = SystemColors.Control;
                Text = con.Text;
                Name = con.Name;
                Enable = con.Enabled;
                MinWidth = window.MinimumSize.Width;
                MaxWidth = window.MaximumSize.Width;
                MinHeight = window.MinimumSize.Height;
                MaxHeight = window.MaximumSize.Height;
                Opacity = window.Opacity;
                MinimizeBox = window.MinimizeBox;
                MaximizeBox = window.MaximizeBox;
                ShowIcon = window.ShowIcon;
                ShowInTaskbar = window.ShowInTaskbar;
                Icon = window.Icon;
                WindowState = window.WindowState;
                FormBorderStyle = window.FormBorderStyle;
                StartPosition = window.StartPosition;
            }

        }

        private int _x;
        [Category("_Properties")]
        [DisplayName("X")]
        [Description("The X property of the control. Centered Left.")]
        public int X
        {
            get => _x;
            set
            {
                _x = value;
                control.Left = value;
                window.Left = value;
            }
        }

        private int _y;
        [Category("_Properties")]
        [DisplayName("Y")]
        [Description("The Y property of the control. Centered Top.")]
        public int Y
        {
            get => _y;
            set
            {
                _y = value;
                control.Top = value;
                window.Top = value;
            }
        }

        private Color _backgroundcolor;
        [Category("_Properties")]
        [DisplayName("Background Color")]
        [Description("The Background Color property of the control.")]
        public Color BackColor
        {
            get => _backgroundcolor;
            set
            {
                _backgroundcolor = value;
                control.BackColor = value;
                window.BackColor = value;
            }
        }

        private Color _foregroundcolor;
        [Category("_Properties")]
        [DisplayName("Foreground Color")]
        [Description("The Foreground Color property of the control.")]
        public Color ForeColor
        {
            get => _foregroundcolor;
            set
            {
                _foregroundcolor = value;
                control.ForeColor = value;
                window.ForeColor = value;
            }
        }

        private int _width;
        [Category("_Properties")]
        [DisplayName("Width")]
        [Description("The Width property of the control.")]
        public int Width
        {
            get => _width;
            set
            {
                _width = value;
                control.Width = value;
                window.Width = value;
            }
        }

        private int _height;
        [Category("_Properties")]
        [DisplayName("Height")]
        [Description("The Height property of the control.")]
        public int Height
        {
            get => _height;
            set
            {
                _height = value;
                control.Height = value;
                window.Height = value;
            }
        }

        private string _text;
        [Category("_Properties")]
        [DisplayName("Text")]
        [Description("The Text property of the control.")]
        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                control.Text = value;
                window.Text = value;
            }
        }

        private string _name;
        [Category("_Properties")]
        [DisplayName("Name")]
        [Description("The Name of the control.")]
        public string Name
        {
            get => _name;
            set
            {
                string v = value.Replace(" ", "_");
                _name = v;
                control.Name = v;
                window.Name = v;
            }
        }

        private string _id;
        [Category("_Properties")]
        [DisplayName("Id")]
        [Description("The Id of the control.")]
        public string Id
        {
            get => _id;
            set
            {
                _id = value;
                control.AccessibleName = value;
            }
        }

        private bool _enable;
        [Category("_Properties")]
        [DisplayName("Enable")]
        [Description("If the control is Enabled.")]
        public bool Enable
        {
            get => _enable;
            set
            {
                _enable = value;
                control.Enabled = value;
                window.Enabled = value;
            }
        }

        private bool _autosize;
        [Category("_Properties")]
        [DisplayName("AutoSize")]
        [Description("The AutoSize property of the control.")]
        public bool AutoSize
        {
            get => _autosize;
            set
            {
                _autosize = value;
                control.AutoSize = value;
                window.AutoSize = value;
            }
        }

        private Image? _image;
        [Category("_Properties")]
        [DisplayName("Image")]
        [Description("The background Image of the control.")]
        public Image? Image
        {
            get => _image;
            set
            {
                _image = value;
                control.BackgroundImage = value;
                window.BackgroundImage = value;
            }
        }

        private ImageLayout _imagelayout;
        [Category("_Properties")]
        [DisplayName("Image Layout")]
        [Description("The background Image Layout of the control.")]
        public ImageLayout ImageLayout
        {
            get => _imagelayout;
            set
            {
                _imagelayout = value;
                control.BackgroundImageLayout = value;
                window.BackgroundImageLayout = value;
            }
        }

        private Font _mainFont;
        [Category("_Properties")]
        [DisplayName("Font")]
        [Description("The control's Font.")]
        public Font MainFont
        {
            get => _mainFont;
            set
            {
                _mainFont = value;
                control.Font = value;
                window.Font = value;
            }
        }

        private int _poly;
        [Category("Shape")]
        [DisplayName("Poly")]
        [Description("The shape's poly count (at least 3).")]
        public int Poly
        {
            get => _poly;
            set
            {
                int v = value;
                if (value < 3) v = 3;
                _poly = v;

                if (control is GShape a)
                {
                    Points = new PointF[0];
                    if (Poly == 3) a.type = GShape.Type.Triangle;
                    else if (Poly == 4) a.type = GShape.Type.Square;
                    else a.type = GShape.Type.Polygon;
                    a.Poly = v;
                    a.Refresh();
                }
                else _poly = 4;
            }
        }

        private PointF[] _points;
        [Category("Shape")]
        [DisplayName("Custom Points")]
        [Description("The shape's custom points (at least 3).")]
        public PointF[] Points
        {
            get => _points;
            set
            {
                _points = value;

                if (control is GShape a)
                {
                    if (Points.Length < 3 && Points.Length != 0)
                    {
                        MessageBox.Show("Expected at least 3 points.", "IDE - Window Designer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (Points.Length != 0)
                    {
                        Poly = Points.Length;
                        a.type = GShape.Type.Custom;
                        a.Points = value;
                        _points = value;
                        a.Refresh();
                    }
                }
                else _points = new PointF[0];
            }
        }

        private ContentAlignment _align;
        [Category("Button and Label")]
        [DisplayName("Text Align")]
        [Description("A button's or label's text alignment.")]
        public ContentAlignment Align
        {
            get => _align;
            set
            {
                _align = value;
                if (control is GButton a)
                {
                    a.TextAlign = _align;
                }
                else if(control is GLabel b)
                {
                    b.TextAlign = _align;
                }
            }
        }

        private bool _multiline;
        [Category("Textbox")]
        [DisplayName("Multiline")]
        [Description("The multiline property for a textbox.")]
        public bool Multiline
        {
            get => _multiline;
            set
            {
                _multiline = value;
                if (control is TextBox a)
                {
                    a.Multiline = _multiline;
                }
            }
        }

        private bool _readonly;
        [Category("Textbox")]
        [DisplayName("ReadOnly")]
        [Description("If the textbox is Readonly.")]
        public bool ReadOnly
        {
            get => _readonly;
            set
            {
                _readonly = value;
                if (control is TextBox a)
                {
                    a.ReadOnly = _readonly;
                }
            }
        }

        private bool _wordwrap;
        [Category("Textbox")]
        [DisplayName("WordWrap")]
        [Description("The wordwrap property for a textbox.")]
        public bool WordWrap
        {
            get => _wordwrap;
            set
            {
                _wordwrap = value;
                if (control is TextBox a)
                {
                    a.WordWrap = _wordwrap;
                    a.AcceptsReturn = !_wordwrap;
                    a.AcceptsTab = !_wordwrap;
                    a.AllowDrop = !_wordwrap;
                }
            }
        }

        private ScrollBars _scrollbar;
        [Category("Textbox")]
        [DisplayName("VerticalScrollBar")]
        [Description("The Scrollbars for the textbox.")]
        public ScrollBars ScrollBars
        {
            get => _scrollbar;
            set
            {
                _scrollbar = value;
                if (control is TextBox a)
                {
                    a.ScrollBars = _scrollbar;
                }
            }
        }

        private int _minwidth;
        [Category("Window")]
        [DisplayName("Min Width")]
        [Description("The Minumum Width of the Window.")]
        public int MinWidth
        {
            get => _minwidth;
            set
            {
                _minwidth = value;
                window.MinimumSize = new Size(value, window.MinimumSize.Height);
            }
        }

        private int _minheight;
        [Category("Window")]
        [DisplayName("Min Height")]
        [Description("The Minumum Height of the Window.")]
        public int MinHeight
        {
            get => _minheight;
            set
            {
                _minheight = value;
                window.MinimumSize = new Size(window.MinimumSize.Width, value);
            }
        }

        private int _maxwidth;
        [Category("Window")]
        [DisplayName("Max Width")]
        [Description("The maximum Width of the Window.")]
        public int MaxWidth
        {
            get => _maxwidth;
            set
            {
                _maxwidth = value;
                window.MaximumSize = new Size(value, window.MaximumSize.Height);
            }
        }

        private int _maxheight;
        [Category("Window")]
        [DisplayName("Max Height")]
        [Description("The Maximum Height of the Window.")]
        public int MaxHeight
        {
            get => _maxheight;
            set
            {
                _maxheight = value;
                window.MaximumSize = new Size(window.MaximumSize.Width, value);
            }
        }

        private double _opacity;
        [Category("Window")]
        [DisplayName("Opacity")]
        [Description("The Opacity of the Window.")]
        public double Opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                window.Opacity = value;
            }
        }

        private bool _minimizebox;
        [Category("Window")]
        [DisplayName("Show Minimize Box")]
        [Description("If the Minimize Box should show on the window.")]
        public bool MinimizeBox
        {
            get => _minimizebox;
            set
            {
                _minimizebox = value;
                window.MinimizeBox = value;
            }
        }

        private bool _maximizebox;
        [Category("Window")]
        [DisplayName("Show Maximize Box")]
        [Description("If the Maximize Box should show on the window.")]
        public bool MaximizeBox
        {
            get => _maximizebox;
            set
            {
                _maximizebox = value;
                window.MaximizeBox = value;
            }
        }

        private bool _showicon;
        [Category("Window")]
        [DisplayName("Show Icon")]
        [Description("If the Icon should show on the window.")]
        public bool ShowIcon
        {
            get => _showicon;
            set
            {
                _showicon = value;
                window.ShowIcon = value;
            }
        }

        private bool _showintaskbar;
        [Category("Window")]
        [DisplayName("Show In Taskbar")]
        [Description("If the Icon should show in the Taskbar.")]
        public bool ShowInTaskbar
        {
            get => _showintaskbar;
            set
            {
                _showintaskbar = value;
                window.ShowInTaskbar = value;
            }
        }

        private Icon _icon;
        [Category("Window")]
        [DisplayName("Icon")]
        [Description("The Window's Icon.")]
        public Icon? Icon
        {
            get => _icon;
            set
            {
                _icon = value;
                window.Icon = value;
            }
        }

        private FormWindowState _windowstate;
        [Category("Window")]
        [DisplayName("Window State")]
        [Description("The Window's State")]
        public FormWindowState WindowState
        {
            get => _windowstate;
            set
            {
                _windowstate = value;
                window.WindowState = value;
            }
        }

        private FormBorderStyle _formborderstyle;
        [Category("Window")]
        [DisplayName("Window State")]
        [Description("The Window's State")]
        public FormBorderStyle FormBorderStyle
        {
            get => _formborderstyle;
            set
            {
                _formborderstyle = value;
                window.FormBorderStyle = value;
            }
        }

        private FormStartPosition _startposition;
        [Category("Window")]
        [DisplayName("Window State")]
        [Description("The Window's State")]
        public FormStartPosition StartPosition
        {
            get => _startposition;
            set
            {
                _startposition = value;
                window.StartPosition = value;
            }
        }
    }
}
