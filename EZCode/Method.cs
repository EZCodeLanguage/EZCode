using EZCode.Variables;

namespace EZCode.Methods
{
    public class Method
    {
        string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                IsSet = value == "" ? false : true;
            }
        }
        public int Line {  get; set; }
        public int Length {  get; set; }
        public Var[]? Paremters { get; set; }
        public string[]? Contents { get; set; }
        public bool IsSet = false;
        
        public Method() => Set();
        public Method(string name) => Set(name);
        public Method(string name, Var[]? paremters, string[] contents, int line, int length) => Set(name, paremters, true, line, length, contents);

        public void Set(string name = "", Var[]? paremters = null,  bool isSet = false, int line = 0, int length = 0, string[]? contents = null)
        {
            this.Name = name;
            this.Paremters = paremters;
            this.IsSet = isSet;
            this.Line = line;
            this.Length = length;
            this.Contents = contents;
        }
    }
}
