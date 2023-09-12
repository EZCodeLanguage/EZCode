using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ezcode_Viewer
{
    internal interface Ivar
    {
        string Name { get; set; }
        float number { get; set; }
        string text { get; set; }
        bool isSet { get; set; }
        int stack { get; set; }
        void set(string value);
        void change(string middle, string multiplier);
        void stringChange(string adds, string mid);
        bool isNumber();
        string value();
    }
    public class Var : Ivar
    {
        public string Name { get; set; }
        public float number { get; set; }
        public string text { get; set; }
        public int stack { get; set; }
        public bool isSet { get; set; }

        string StringStandered = "IF YOU GET THIS. IT IS AN ERROR MESSAGE - (23dsffdsf86dg45b64ytu7578566434654fg4g4fhjd) = just some random text";
        float FloatStatendered = 1111.111100015684465464864f;

        public Var(string name)
        {
            Name = name;
            number = FloatStatendered;
            text = StringStandered;
        }
        public void set(string value)
        {
            try
            {
                number = float.Parse(value, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch
            {
                text = value;
            }
        }
        public void change(string middle, string multiplier)
        {
            try
            {
                float value = float.Parse(multiplier, CultureInfo.InvariantCulture.NumberFormat);
                float final;
                if (middle == "+")
                {
                    final = number + value;
                }
                else if (middle == "*")
                {
                    final = number * value;
                }
                else if (middle == "-")
                {
                    final = number - value;
                }
                else if (middle == "/")
                {
                    final = number / value;
                }
                else if (middle == "=")
                {
                    final = value;
                }
                else
                {
                    final = number;
                }
                number = final;
            }
            catch
            {
                isSet = false;
            }
        }
        public void stringChange(string value, string mid)
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
                                return;
                            }
                            for (int i = 0; i < text.Length; i++)
                            {
                                if (i >= text.Length - v)
                                {
                                    text = text.Remove(i);
                                }
                            }
                            setted = true;
                            break;
                    }
                    if (!setted)
                    {
                        isSet = false;
                    }
                }
                else
                {
                    isSet = false;
                }
            }
            catch
            {
                isSet = false;
            }
        }
        public bool isNumber()
        {
            if (number != FloatStatendered)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public string value()
        {
            if (isNumber())
            {
                return number.ToString();
            }
            else
            {
                return text.ToString();
            }
        }

    }
}
