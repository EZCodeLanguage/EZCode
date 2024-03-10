namespace EZCodeLanguage
{
    public class Project
    {
        public string Name { get; set; }
        public string[] Files { get; set; }
        public Config Configuration { get; set; }

        public class Config
        {
            public bool Cache { get; set; } = false;
        }
    }
}
