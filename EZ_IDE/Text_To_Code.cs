using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace EZ_IDE
{
    public partial class Text_To_Code : Form
    {
        public Text_To_Code()
        {
            InitializeComponent();
            AllowSpaces.Checked = Settings.TtC_Allow_Spaces;
            UseWrap.Checked = Settings.TtC_Use_Wrap;
            CharsBeforeWrap.Value = Settings.TtC_Chars_Before_Wrap;
            InputText.Text = Settings.TtC_Input_Text;
        }

        private void AllowSpaces_CheckedChanged(object sender, EventArgs e)
        {
            Settings.TtC_Allow_Spaces = AllowSpaces.Checked;

            Convert();
        }

        private void UseWrap_CheckedChanged(object sender, EventArgs e)
        {
            Settings.TtC_Use_Wrap = UseWrap.Checked;

            CharsBeforeWrap.Enabled = UseWrap.Checked;
            CBF_label.Enabled = UseWrap.Checked;

            Convert();
        }

        private void CharsBeforeWrap_ValueChanged(object sender, EventArgs e)
        {
            Settings.TtC_Chars_Before_Wrap = (int)CharsBeforeWrap.Value;

            Convert();
        }

        private void InputText_TextChanged(object sender, EventArgs e)
        {
            Settings.TtC_Input_Text = InputText.Text;

            Convert();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(OutputCode.Text);
        }

        void Convert() => OutputCode.Text = ConvertedCode(InputText.Text, AllowSpaces.Checked, UseWrap.Checked, (int)CharsBeforeWrap.Value);

        static string ConvertedCode(string text, bool allow_spaces, bool use_wrap, int max_char_val)
        {
            string code = text;
            string[] codeParts = code.Split(" ");
            for (int i = 0; i < codeParts.Length; i++)
            {
                codeParts[i] = codeParts[i].Replace("\\", "\\\\").Replace(Environment.NewLine, "\\n").Replace("'", "\\\"")
                    .Replace(":", "\\;").Replace("=", "\\=").Replace(",", "\\c").Replace("!", "\\e").Replace("|", "\\$");
            }
            code = string.Join(" ", codeParts);
            if (use_wrap)
            {
                if(code.Length > max_char_val)
                {
                    string[] parts = code.Split(new char[] { ' ' });
                    string newCode = "";
                    int a = 0;
                    for (int i = 0; i < parts.Length; i++) 
                    {
                        newCode += parts[i] + " ";
                        a += parts[i].Length;
                        if (a > max_char_val && i != parts.Length - 1)
                        {
                            newCode += $"->{Environment.NewLine}\t";
                            a = 0;
                        }
                    }
                    code = newCode;
                }
            }
            if (!allow_spaces)
            {
                code = code.Replace(" ", "\\_");
            }
            return code;
        }
    }
}
