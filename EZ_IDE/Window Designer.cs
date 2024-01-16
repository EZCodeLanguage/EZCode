namespace EZ_IDE
{
    using EZCode.GControls;
    using EZCode.Groups;
    using EZCode.Windows;
    using System.Drawing;
    using System.Windows.Forms;

    public partial class Window_Designer : Form
    {
        public static Window_Designer? Instance;
        private Group controlGroup = new Group("WINDOW_DESIGNER_GROUP");
        private List<Control> controls = new List<Control>();
        private List<PropertyGrid_GControl> properies = new List<PropertyGrid_GControl>();
        private int selectedIndex = -1;
        public Window_Designer()
        {
            Instance = this;
            InitializeComponent();
        }

        private void Window_Designer_Load(object sender, EventArgs e)
        {
            Dropdown.Items.Add("MAIN");
            controls.Add(MAIN);
            properies.Add(new PropertyGrid_GControl(MAIN, true));
            MAIN.Click += Object_Clicked;
            Dropdown.SelectedIndex = 0;
        }

        private void Generate_Code(object sender, EventArgs e)
        {
            string Code = $"// Window {MAIN.Name}{Environment.NewLine}";
            Window w = new Window();
            Window win = properies.FirstOrDefault(x => x.IsWindow).window;
            Code += $"window {MAIN.Name} new :" +
                $"{(win.Text != w.Text ? $", text:{win.Text}" : "")}" +
                $"{(win.Width != w.Width ? $", width:{win.Width}" : "")}" +
                $"{(win.Height != w.Height ? $", height:{win.Height}" : "")}" +
                $"{(win.MinimumSize.Width != w.MinimumSize.Width ? $", minwidth:{win.MinimumSize.Width}" : "")}" +
                $"{(win.MinimumSize.Height != w.MinimumSize.Height ? $", minheight:{win.MinimumSize.Height}" : "")}" +
                $"{(win.MaximumSize.Width != w.MaximumSize.Width ? $", maxwidth:{win.MaximumSize.Width}" : "")}" +
                $"{(win.MaximumSize.Height != w.MaximumSize.Height ? $", maxheight:{win.MaximumSize.Height}" : "")}" +
                $"{(win.BackColor != w.BackColor ? $", bg:[{win.BackColor.R};{win.BackColor.G};{win.BackColor.B}]" : "")}" +
                $"{(win.ForeColor != w.ForeColor ? $", fg:[{win.ForeColor.R};{win.ForeColor.G};{win.ForeColor.B}]" : "")}" +
                $"{(win.Left != w.Left ? $", x:{win.Left}" : "")}" +
                $"{(win.Top != w.Top ? $", y:{win.Top}" : "")}" +
                $"{(win.Opacity != w.Opacity ? $", opacity:{win.Opacity}" : "")}" +
                $"{(win.Enabled != w.Enabled ? $", enable:{win.Enabled}" : "")}" +
                $"{(win.AutoSize != w.AutoSize ? $", auto:{win.AutoSize}" : "")}" +
                $"{(win.MinimizeBox != w.MinimizeBox ? $", minimizebox:{win.MinimizeBox}" : "")}" +
                $"{(win.MaximizeBox != w.MaximizeBox ? $", maximizebox:{win.MaximizeBox}" : "")}" +
                $"{(win.ShowIcon != w.ShowIcon ? $", showicon:{win.ShowIcon}" : "")}" +
                $"{(win.ShowInTaskbar != w.ShowInTaskbar ? $", showintaskbar:{win.ShowInTaskbar}" : "")}" +
                $"{(win.WindowState != w.WindowState ? $", state:{win.WindowState.ToString().ToLower()}" : "")}" +
                $"{(win.FormBorderStyle != w.FormBorderStyle ? $", type:{win.FormBorderStyle.ToString().ToLower()}" : "")}" +
                $"{(win.StartPosition != w.StartPosition ? $", startposition:{win.StartPosition.ToString().ToLower().Replace("centerscreen", "enter").Replace("window", "")}" : "")}" +
                $"{(win.Font != w.Font ? $", font:[{win.Font.Name};{win.Font.Size};{win.Font.Style}]" : "")}" +
                $"{(win.BackgroundImageLayout != w.BackgroundImageLayout ? $", imagelayout:{win.BackgroundImageLayout.ToString().ToLower()}" : "")}" +
                $"{(win.IconImageFile != w.IconImageFile ? $", icon:{win.IconImageFile.Replace("\\", "/").Replace(":", "\\;")}" : "")}" +
                $"{(win.BGImageFile != w.BGImageFile ? $", image:{win.BGImageFile.Replace("\\", "/").Replace(":", "\\;")}" : "")}";
            Code = Code.Replace(" new :,", " new :");
            if (Code.EndsWith(" new :")) Code = Code.Replace(" :", "");
            Control[] Stay_controls = controls.ToArray();
            controls.Remove(MAIN);
            controls = controls.OrderBy(GetZIndex).Reverse().ToList();
            Control c = new Control();
            foreach (Control con in controls)
            {
                PropertyGrid_GControl prop = properies.FirstOrDefault(x => x.control.Name == con.Name);
                prop.Image ??= new ImageFile[0];
                string type = con.GetType().Name.ToLower().Replace("g", "");
                string line = Environment.NewLine + Environment.NewLine;
                line += $"// Control {con.Name}{Environment.NewLine}";
                line += $"{type} {con.Name}" +
                    $"{(con.Text != c.Text ? $", text:{con.Text}" : "")}" +
                    $"{(con.AccessibleName != c.AccessibleName ? $", id:{con.AccessibleName}" : "")}" +
                    $"{(con.Left != c.Left ? $", x:{con.Left}" : "")}" +
                    $"{(con.Top != c.Top ? $", y:{con.Top}" : "")}" +
                    $"{(con.Width != c.Width ? $", width:{con.Width}" : "")}" +
                    $"{(con.Height != c.Height ? $", height:{con.Height}" : "")}" +
                    $"{(con.BackColor != c.BackColor ? $", bg:[{con.BackColor.R};{con.BackColor.G};{con.BackColor.B}]" : "")}" +
                    $"{(con.ForeColor != c.ForeColor ? $", fg:[{con.ForeColor.R};{con.ForeColor.G};{con.ForeColor.B}]" : "")}" +
                    $"{(con.Enabled != c.Enabled ? $", enable:{con.Enabled}" : "")}" +
                    $"{(con.Visible != c.Visible ? $", visible:{con.Visible}" : "")}" +
                    $"{(con.Anchor != c.Anchor? $", anchor:[{con.Anchor.ToString().Replace(",", ";").Replace(" ", "")}]" : "")}" +
                    $"{(con.AutoSize != c.AutoSize ? $", auto:{con.AutoSize}" : "")}" +
                    $"{(con.Font != c.Font ? $", font:[{con.Font.Name};{con.Font.Size};{con.Font.Style}]" : "")}" +
                    $"{(con.BackgroundImageLayout != c.BackgroundImageLayout ? $", imagelayout:{con.BackgroundImageLayout.ToString().ToLower()}" : "")}" +
                    $"{(prop.Image != null && prop.Image.Length > 0 ? $", image:{prop.Image[0].FilePath.Replace("\\", "/").Replace(":", "\\;")}" : "")}";
                if (con is GShape cS)
                {
                    string points = "";
                    cS.Points ??= new PointF[0];
                    c = new GShape();
                    if (cS.Points != null && cS.Points.Length > 0)
                    {
                        foreach (PointF point in cS.Points)
                        {
                            points += $"({point.X}*{point.Y});";
                        }
                        if (points.EndsWith(";")) points = points.Substring(0, points.Length - 2);
                    }
                    line += $"{(cS.Poly != 4 ? $", poly:{cS.Poly}" : "")}" +
                        $"{(points != "" ? $", points:[{points}]" : "")}";
                }
                if (con is GLabel cL)
                {
                    c = new GLabel();
                    line += $"{(cL.TextAlign != (c as GLabel).TextAlign ? $", align:{cL.TextAlign.ToString().ToLower()}" : "")}";
                }
                if (con is GButton cB)
                {
                    c = new GButton();
                    line += $"{(cB.TextAlign != (c as GButton).TextAlign ? $", align:{cB.TextAlign.ToString().ToLower()}" : "")}";
                }
                if (con is GTextBox cT)
                {
                    c = new GTextBox();
                    line += $"{(cT.ReadOnly != (c as GTextBox).ReadOnly ? $", readonly:{cT.ReadOnly}" : "")}" +
                        $"{(cT.WordWrap != (c as GTextBox).WordWrap ? $", wordwrap:{cT.WordWrap}" : "")}" +
                        $"{(cT.Multiline != (c as GTextBox).Multiline ? $", multiline:{cT.Multiline}" : "")}" +
                        $"{(cT.ScrollBars != (c as GTextBox).ScrollBars ? 
                            $", verticalscrollbar:{(cT.ScrollBars == ScrollBars.Vertical || cT.ScrollBars == ScrollBars.Both ? "true" : "false")}" : "") +
                            $", horizantalscrollbar:{(cT.ScrollBars == ScrollBars.Horizontal || cT.ScrollBars == ScrollBars.Both ? "true" : "false")}"}";
                }
                string[] parts = line.Split(" ");
                if (parts[3].EndsWith(","))
                {
                    parts[3] = parts[3].Replace(",", "");
                    line = string.Join(" ", parts);
                }
                line += $"{Environment.NewLine}{MAIN.Name} display {con.Name}";
                Code += line;
            }
            Code += Environment.NewLine + Environment.NewLine;
            Code += $"// Open {MAIN.Name}{Environment.NewLine}";
            Code += $"{MAIN.Name} open";
            OutputCode.Text = Code;
            controls = Stay_controls.ToList();
        }

        private void Designer_Resize_Begin(object sender, EventArgs e)
        {
            properies[0].Size = new Size(MAIN.Width, MAIN.Height);
        }

        private void Add_Shape(object sender, EventArgs e)
        {
            GShape control = new GShape();
            control.Left = new Random().Next(0, 500);
            control.Top = new Random().Next(0, 250);
            control.Name = $"Shape{controlGroup.Shapes.Count}";
            controls.Add(control);
            controlGroup.Shapes.Add(control);
            MAIN.Controls.Add(control);
            Dropdown.Items.Add(control.Name);
            Dropdown.SelectedIndex = Dropdown.Items.Count - 1;
            PropertyGrid_GControl property = new PropertyGrid_GControl(control);
            properies.Add(property);
            propertyGrid.SelectedObject = property;
            control.Click += Object_Clicked;
            control.MouseDown += MouseDown;
            control.MouseMove += MouseMove;
            control.BringToFront();
        }

        private void Add_Button(object sender, EventArgs e)
        {
            GButton control = new GButton();
            control.Left = new Random().Next(0, 500);
            control.Top = new Random().Next(0, 250);
            control.Name = $"Button{controlGroup.Buttons.Count}";
            control.Text = control.Name;
            controls.Add(control);
            controlGroup.Buttons.Add(control);
            MAIN.Controls.Add(control);
            Dropdown.Items.Add(control.Name);
            Dropdown.SelectedIndex = Dropdown.Items.Count - 1;
            PropertyGrid_GControl property = new PropertyGrid_GControl(control);
            properies.Add(property);
            propertyGrid.SelectedObject = property;
            control.Click += Object_Clicked;
            control.MouseDown += MouseDown;
            control.MouseMove += MouseMove;
            control.BringToFront();
        }

        private void Add_Label(object sender, EventArgs e)
        {
            GLabel control = new GLabel();
            control.Left = new Random().Next(0, 500);
            control.Top = new Random().Next(0, 250);
            control.Name = $"Label{controlGroup.Labels.Count}";
            control.Text = control.Name;
            controls.Add(control);
            controlGroup.Labels.Add(control);
            MAIN.Controls.Add(control);
            Dropdown.Items.Add(control.Name);
            Dropdown.SelectedIndex = Dropdown.Items.Count - 1;
            PropertyGrid_GControl property = new PropertyGrid_GControl(control);
            properies.Add(property);
            propertyGrid.SelectedObject = property;
            control.Click += Object_Clicked;
            control.MouseDown += MouseDown;
            control.MouseMove += MouseMove;
            control.BringToFront();
        }

        private void Add_Textbox(object sender, EventArgs e)
        {
            GTextBox control = new GTextBox();
            control.Left = new Random().Next(0, 500);
            control.Top = new Random().Next(0, 250);
            control.Name = $"Textbox{controlGroup.Textboxes.Count}";
            controls.Add(control);
            controlGroup.Textboxes.Add(control);
            MAIN.Controls.Add(control);
            Dropdown.Items.Add(control.Name);
            Dropdown.SelectedIndex = Dropdown.Items.Count - 1;
            PropertyGrid_GControl property = new PropertyGrid_GControl(control);
            properies.Add(property);
            propertyGrid.SelectedObject = property;
            control.Click += Object_Clicked;
            control.MouseDown += MouseDown;
            control.MouseMove += MouseMove;
            control.BringToFront();
        }

        private void Delete_Selected(object sender, EventArgs e)
        {
            if (propertyGrid.SelectedObject == null)
            {
                MessageBox.Show("No Object Selected", "IDE - Window Designer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (Dropdown.SelectedIndex == 0)
            {
                MessageBox.Show("Can not delete 'MAIN'", "IDE - Window Designer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                object obj = propertyGrid.SelectedObject;
                PropertyGrid_GControl property = (PropertyGrid_GControl)obj;
                Control control = property.control;
                properies.Remove(property);
                controls.Remove(control);
                controlGroup.set(controls);
                Dropdown.Items.Remove(control.Name);
                propertyGrid.SelectedObject = properies[0];
                control.Dispose();
                Dropdown.SelectedIndex = 0;
            }
        }

        private void Object_Clicked(object? sender, EventArgs e)
        {
            Control control = (Control)sender;
            Dropdown.SelectedIndex = controls.IndexOf(control);
            propertyGrid.SelectedObject = properies[controls.IndexOf(control)];
        }

        private void Copy_Code(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(OutputCode.Text);
            }
            catch
            {

            }
        }

        private void Object_Selection_Changed(object sender, EventArgs e)
        {
            Control control = controls[Dropdown.SelectedIndex] is not Panel ? controls[Dropdown.SelectedIndex] : MAIN;
            propertyGrid.SelectedObject = properies.FirstOrDefault(x => x.control == control);
            selectedIndex = Dropdown.SelectedIndex;
        }

        private void Property_Value_Changed(object s, PropertyValueChangedEventArgs e)
        {
            string[] names = controls.Select(x => x.Name).ToArray();
            string[] dropnames = Dropdown.Items.Cast<string>().ToArray();
            if (!names.SequenceEqual(dropnames))
            {
                Dropdown.Items.Clear();
                Dropdown.Items.AddRange(names);
                Control control = controls.FirstOrDefault(x => x.Name == controls[selectedIndex].Name);
                Dropdown.SelectedIndex = controls.IndexOf(control);
            }
            (propertyGrid.SelectedObject as PropertyGrid_GControl).control.Refresh();
        }

        private void To_Front(object sender, EventArgs e)
        {
            int i = Dropdown.SelectedIndex;
            if (i == 0)
            {
                MessageBox.Show("Can not send Window to front", "IDE - Window Designer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                controls[i].BringToFront();
            }
        }

        private void To_Back(object sender, EventArgs e)
        {
            int i = Dropdown.SelectedIndex;
            if (i == 0)
            {
                MessageBox.Show("Can not send Window to back", "IDE - Window Designer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                controls[i].SendToBack();
            }
        }

        Point point = new Point();
        public void MouseDown(object sender, MouseEventArgs e)
        {
            point = e.Location;
        }

        public void MouseMove(object sender, MouseEventArgs e)
        {
            Control control = sender as Control;
            PropertyGrid_GControl property = properies[controls.IndexOf(control)];
            if (e.Button == MouseButtons.Left)
            {
                property.Location = new Point(property.Location.X + e.X - point.X, property.Location.Y + e.Y - point.Y);
            }
            if (e.Button == MouseButtons.Right)
            {
                int X = property.Location.X, Y = property.Location.Y, width = property.Size.Width, height = property.Size.Height;
                X += e.X - point.X;
                width -= e.X - point.X;
                Y += e.Y - point.Y;
                height -= e.Y - point.Y;
                if (width <= 1) width = 3;
                if (height <= 1) height = 3;

                property.Location = new Point(X, Y);
                property.Size = new Size(width, height);
            }
            if (e.Button == MouseButtons.Middle)
            {
                Color a = property.BackColor;
                Random rand = new Random();
                int r = a.R, g = a.G, b = a.B;
                r += e.X - point.X - rand.Next(0, 10);
                g += ((e.X + e.Y) / 2) - ((point.X + point.Y) / 2) - rand.Next(0, 3);
                b += e.Y - point.Y - rand.Next(0, 10);
                if (r < 0) r = 0; if (r > 224) r = 224;
                if (g < 0) g = 0; if (g > 224) g = 224;
                if (b < 0) b = 0; if (b > 224) b = 224;
                property.BackColor = Color.FromArgb(r, g, b);
            }
            control.Refresh();
        }

        private void Window_Designer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (controls.Count != 1)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to exit? It will not save.", "IDE - Window Designer", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    Instance = null;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                Instance = null;
            }
        }
        static int GetZIndex(Control control)
        {
            Control parent = control.Parent;

            if (parent != null)
            {
                for (int i = 0; i < parent.Controls.Count; i++)
                {
                    if (parent.Controls[i] == control)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }
    }
}