namespace EZ_IDE
{
    using EZCode.GControls;
    using EZCode.Groups;
    using EZCode.Windows;
    using System.Drawing;

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
            Code += $"window {MAIN.Name} new : " +
                $"{(win.MinimumSize.Width != w.MinimumSize.Width ? $"minwidth:{win.MinimumSize.Width}, " : "")}" +
                $"{(win.MinimumSize.Height != w.MinimumSize.Height ? $"minheight:{win.MinimumSize.Height}, " : "")}" +
                $"{(win.MaximumSize.Width != w.MaximumSize.Width ? $"maxwidth:{win.MaximumSize.Width}, " : "")}" +
                $"{(win.MaximumSize.Height != w.MaximumSize.Height ? $"maxheight:{win.MaximumSize.Height}, " : "")}" +
                $"{(win.Enabled != w.Enabled ? $"enabled:{win.Enabled}, " : "")}";
            Control[] Stay_controls = controls.ToArray();
            controls.Remove(MAIN);
            controls.OrderBy(GetZIndex);
            foreach (Control control in controls)
            {
                Code += Environment.NewLine + Environment.NewLine;
                Code += $"// Control {control.Name}{Environment.NewLine}";
                Code += $"";
                Code += $"{MAIN.Name} display {control.Name}";
            }
            OutputCode.Text = Code;
            controls = Stay_controls.ToList();
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
                property.X += e.X - point.X;
                property.Y += e.Y - point.Y;
            }
            if (e.Button == MouseButtons.Right)
            {
                property.X += e.X - point.X;
                property.Width -= e.X - point.X;
                property.Y += e.Y - point.Y;
                property.Height -= e.Y - point.Y;
                if (property.Width <= 1) property.Width = 3;
                if (property.Height <= 1) property.Height = 3;
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
