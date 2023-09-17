using System.Globalization;
using System.Text.RegularExpressions;

namespace EZCode.Variables
{
    public interface Ivar
    {
        public enum Types
        {
            None,
            File,
            Bool,
            Array,
            Float,
            String
        }
        string Name { get; set; }
        float? number { get; set; }
        string? text { get; set; }
        object? accessible { get; set; }
        string[] array { get; set; }
        bool isSet { get; set; }
        Types Description { get; set; }
        void set(string? value = null, string[]? array = null);
        bool Multiply(string middle, string multiplier);
        bool Change(string adds, string mid);
        bool isNumber();
        bool isArray();
        bool isBool();
        bool isFile();
        bool isFile(string value);
        bool isString();
        string value();
        bool? returnBool();
    }
    public class Var : Ivar
    {
        public string Name { get; set; }
        public float? number { get; set; }
        public string? text { get; set; }
        public object? accessible { get; set; }
        public string[] array { get; set; }
        public bool isSet { get; set; }
        public Ivar.Types Description { get; set; }

        public Var(string name, string value = "", Ivar.Types type = Ivar.Types.None) 
        {
            Name = name;
            number = null;
            text = null;
            Description = type;
            set(value);
        }
        public void set(string? value = null, string[]? array = null)
        {
            if (value != null)
            {
                try
                {
                    number = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
                    text = value;
                    Description = Description == Ivar.Types.None ? Ivar.Types.Float : Description;
                }
                catch
                {
                    Description = Description == Ivar.Types.Float ? Ivar.Types.None : Description;
                    if (isFile(value))
                    {
                        Description = Description == Ivar.Types.None ? Ivar.Types.File : Description;
                    }
                    else if (returnBool() != null)
                    {
                        Description = Description == Ivar.Types.None ? Ivar.Types.Bool : Description;
                    }
                    else
                    {
                        Description = Description == Ivar.Types.None ? Ivar.Types.String : Description;
                    }
                    number = null;
                    text = value;
                }
                isSet = true;
            }
            else if (array != null)
            {
                Description = Description == Ivar.Types.None ? Ivar.Types.Array : Description;
                this.array = array;
                isSet = true;
            }
            else
            {
                this.text = value;
            }
        }
        public bool Multiply(string middle, string multiplier)
        {
            try
            {
                if (!isNumber()) return false;
                float value = float.Parse(multiplier, CultureInfo.InvariantCulture.NumberFormat);
                float final;
                if(middle == "+")
                {
                    final = (float)number + value;
                }
                else if(middle == "*")
                {
                    final = (float)number * value;
                }
                else if(middle == "-")
                {
                    final = (float)number - value;
                }
                else if(middle == "/")
                {
                    final = (float)number / value;
                }
                else if(middle == "=")
                {
                    final = value;
                }
                else
                {
                    final = (float)number;
                }
                number = final;
                return true;
            }
            catch
            {
                isSet = false;
                return false;
            }
        }
        public bool Change(string mid, string value)
        {
            try
            {
                bool setted = false;
                if (!isNumber())
                {
                    switch (mid)
                    {
                        case "+":
                            setted = true;
                            text += value;
                            break;
                        case "=":
                            setted = true;
                            text = value;
                            break;
                        case "-":
                            int v = 0;
                            try
                            {
                                v = int.Parse(value);
                            }
                            catch
                            {
                                isSet = false;
                                return false;
                            }
                            for (int i = 0; i < text.Length; i++)
                            {
                                if (i >= text.Length - v)
                                {
                                    text = text.Remove(i);
                                }
                            }
                            Description = returnBool() == true ? Ivar.Types.Bool : Description;
                            setted = true;
                            break;
                    }
                    if(!setted)
                    {
                        isSet = false;
                        return false;
                    }
                    return true;
                }
                else
                {
                    isSet = false;
                    return false;
                }
            }
            catch
            {
                isSet = false;
                return false;
            }
        }
        public bool isString()
        {
            if (Description == Ivar.Types.String)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isNumber()
        {
            if (Description == Ivar.Types.Float)
            {
                return true;
            }
            else
            {
                try
                {
                    float.Parse(text);
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }
        public bool isArray()
        {
            if (Description == Ivar.Types.Array)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isBool()
        {
            if (Description == Ivar.Types.Bool)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isFile()
        {
            if (Description == Ivar.Types.File)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isFile(string path)
        {
            Regex driveCheck = new Regex(@"^[a-zA-Z]:\\$");
            if (string.IsNullOrWhiteSpace(path) || path.Length < 3)
            {
                return false;
            }

            if (!driveCheck.IsMatch(path.Substring(0, 3)))
            {
                return false;
            }
            string strTheseAreInvalidFileNameChars = new string(Path.GetInvalidPathChars());
            strTheseAreInvalidFileNameChars += @":/?*" + "\"";
            Regex containsABadCharacter = new Regex("[" + Regex.Escape(strTheseAreInvalidFileNameChars) + "]");
            if (containsABadCharacter.IsMatch(path.Substring(3, path.Length - 3)))
            {
                return false;
            }

            return true;
        }
        public string value()
        {
            if (isNumber())
            {
                return number.ToString();
            }
            else if (isArray())
            {
                return string.Join(",", array);
            }
            else
            {
                return text.ToString();
            }
        }
        public static bool? staticReturnBool(string value)
        {
            if (value == "yes" || value == "Yes" || value == "Y" || value == "y" || value == "true" || value == "True" || value == "1")
            {
                return true;
            }
            else if (value == "no" || value == "No" || value == "N" || value == "n" || value == "false" || value == "False" || value == "0")
            {
                return false;
            }
            else
            {
                return null;
            }
        }
        public bool? returnBool()
        {
            return returnBool(text != null ? text : "");
        }
        public bool? returnBool(string value)
        {
            if(Description == Ivar.Types.Bool || Description == Ivar.Types.Float || Description == Ivar.Types.String)
            {
                if (value == "yes" || value == "Yes" || value == "Y" || value == "y" || value == "true" || value == "True" || value == "1")
                {
                    return true;
                }
                else if (value == "no" || value == "No" || value == "N" || value == "n" || value == "false" || value == "False" || value == "0")
                {
                    return false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
