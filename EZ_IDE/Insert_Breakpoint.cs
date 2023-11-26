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
    public partial class Insert_Breakpoint : Form
    {
        private IDE ide;
        public Insert_Breakpoint(IDE Ide, string text, int charNum, string file, bool showDialog = true)
        {
            InitializeComponent();

            try
            {
                PopulateControls(text, GetLineIndex(text, charNum), file);
            }
            catch
            {

            }

            ide = Ide;

            if (showDialog)
                ShowDialog();
        }

        private void PopulateControls(string text, int lineIndex, string file)
        {
            string[] lines = text.Split('\n', '|');

            if (lineIndex >= 0 && lineIndex < lines.Length)
            {
                string line = lines[lineIndex].Trim();
                string[] parts = line.Split(" ");

                FilePathTextBox.Text = parts[0];

                int methodLineIndex = lineIndex;
                while (methodLineIndex >= 0 && !lines[methodLineIndex].StartsWith("method"))
                {
                    methodLineIndex--;
                }

                if (methodLineIndex >= 0)
                {
                    string methodLine = lines[methodLineIndex].Trim();
                    string[] methodParts = methodLine.Split(" ");

                    MethodTextBox.Text = methodParts[1];
                }

                SegmentNumericUpDown.Value = SegmentValue(lines, lineIndex, methodLineIndex);
            }

            FilePathTextBox.Text = file;
        }

        static int SegmentValue(string[] lines, int lineIndex, int methodLineIndex)
        {
            if (methodLineIndex > lineIndex) 
                return 0;

            int subtract = 0;
            for (int i = methodLineIndex; i < lineIndex; i++)
            {
                string line = lines[i].Trim();
                if (line.Equals("") || line.Contains("->"))
                    subtract++;
            }
            return lineIndex - methodLineIndex - subtract;
        }

        static int GetLineIndex(string text, int charNum)
        {
            string[] lines = text.Split(new[] { "\r\n", "\r", "\n", "|" }, StringSplitOptions.None);

            int currentPosition = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                currentPosition += lines[i].Length + Environment.NewLine.Length;

                if (currentPosition > charNum)
                {
                    return i;
                }
            }

            return -1;
        }

        private void Insert_Click(object sender, EventArgs e)
        {
            List<EZCode.Debug.Breakpoint> breakpoints = ide.debugSettings.Breakpoints.ToList();
            breakpoints.Add(new EZCode.Debug.Breakpoint(FilePathTextBox.Text, (int)SegmentNumericUpDown.Value, RemoveOnHitCheckbox.Checked, MethodTextBox.Text, true));
            ide.debugSettings.Breakpoints = breakpoints.ToArray();
            ide.debugSettings.Save();
            Close();
        }
    }
}
