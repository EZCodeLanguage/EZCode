using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EZ_IDE
{
    public partial class Breakpoints_Form : Form
    {
        public DebugSettings debugSettings;
        public Breakpoints_Form()
        {
            InitializeComponent();
            debugSettings = new DebugSettings();
            propertyGrid1.SelectedObject = debugSettings;
        }

        private void Breakpoints_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            debugSettings.Save();
        }
    }
}
