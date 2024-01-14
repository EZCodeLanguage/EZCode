using EZCode;
using System.Diagnostics;

namespace EZ_IDE
{
    public partial class EZText_To_EZCode : Form
    {
        public EZText_To_EZCode()
        {
            InitializeComponent();
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.SetText(OutputCode.Text);
            }
            catch 
            { 

            }
        }

        private void Translate(object sender, EventArgs e)
        {
            EZText eztext = new EZText();
            OutputCode.Text = eztext.Translate(InputText.Text);
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                var docs = new ProcessStartInfo
                {
                    FileName = "https://github.com/JBrosDevelopment/EZCode/wiki/EZText-Docs",
                    UseShellExecute = true
                };
                Process.Start(docs);
            }
            catch
            {
                MessageBox.Show("Error openning the link, https://github.com/JBrosDevelopment/EZCode/wiki/EZText-Docs", "EZ IDE", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
