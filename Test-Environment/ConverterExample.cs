namespace TestEnvironment
{
    using EZCode.Converter;
    using System.Windows.Forms;
    using RegestrySettings;

    public partial class ConverterExample : Form
    {
        public Converter converter = new Converter();
        public ConverterExample()
        {
            InitializeComponent();
            Settings.keyName = @"JBrosDevelopment\EZCode\TestEnvironment";
            Language.DataSource = Enum.GetValues(typeof(Converter.ProgrammingLanguage));
            main.Text = Settings.GetKey("ConverterText", "") != null ? Settings.GetKey("ConverterText", "")!.ToString() : "";
            string Lang = Settings.GetKey("ConverterLanguage", "") != null ? Settings.GetKey("ConverterLanguage", "")!.ToString() : "";

            Converter.ProgrammingLanguage language;
            Enum.TryParse(Lang, out language);
            Language.SelectedIndex = Language.Items.Cast<Converter.ProgrammingLanguage>().ToList().IndexOf(language);
        }
        private void generate_Click(object sender, EventArgs e)
        {
            Converter.ProgrammingLanguage language;
            Enum.TryParse(Language.SelectedValue.ToString(), out language);

            converted.Text = converter.Convert(main.Text, language);
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (msg.Msg == 256)
            {
                if (keyData == (Keys.G | Keys.Control))
                {
                    generate.PerformClick();
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        protected override void OnClosed(EventArgs e)
        {
            Settings.SetKey("ConverterText", main.Text);
            Settings.SetKey("ConverterLanguage", Language.SelectedValue.ToString());
            base.OnClosed(e);
        }
    }
}
