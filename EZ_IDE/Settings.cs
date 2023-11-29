using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EZ_IDE
{
    public static class Settings
    {
        public static string keyName = @"JBrosDevelopment\EZCode\IDE";
        public static bool Save_Folder 
        {
            get => BoolParse(GetKey(nameof(Save_Folder), true)) == true; 
            set => SetKey(nameof(Save_Folder), value); 
        }
        public static string Open_Folder_Path 
        {
            get
            {
                string? val = GetKey(nameof(Open_Folder_Path));
                return val != null ? val : string.Empty;
            }
            set 
            {
                if (Save_Folder == true)
                    SetKey(nameof(Open_Folder_Path), value); 
            }
        }
        public static string Current_Project_File 
        {
            get
            {
                string? val = GetKey(nameof(Current_Project_File));
                return val != null ? val : string.Empty;
            }
            set => SetKey(nameof(Current_Project_File), value); 
        }
        public static int Default_Zoom 
        {
            get
            {
                int @default = 100;
                int? val = int.Parse(GetKey(nameof(Default_Zoom), @default));
                return val != null ? (int)val : @default;
            }
            set => SetKey(nameof(Default_Zoom), value);
        }
        public static int TtC_Chars_Before_Wrap
        {
            get
            {
                int @default = 35;
                int? val = int.Parse(GetKey(nameof(TtC_Chars_Before_Wrap), @default));
                return val != null ? (int)val : @default;
            }
            set => SetKey(nameof(TtC_Chars_Before_Wrap), value);
        }
        public static int Left_Splitter_Distance 
        {
            get
            {
                int @default = 300;
                int? val = int.Parse(GetKey(nameof(Left_Splitter_Distance), @default));
                return val != null ? (int)val : @default;
            }
            set => SetKey(nameof(Left_Splitter_Distance), value); 
        }
        public static int Bottom_Splitter_Distance 
        {
            get
            {
                int @default = 550;
                int? val = int.Parse(GetKey(nameof(Bottom_Splitter_Distance), @default));
                return val != null ? (int)val : @default;
            }
            set => SetKey(nameof(Bottom_Splitter_Distance), value); 
        }
        public static string New_Project_Default_Directory 
        {
            get
            {
                string? val = GetKey(nameof(New_Project_Default_Directory));
                return val != null ? val : string.Empty;
            }
            set => SetKey(nameof(New_Project_Default_Directory), value); 
        }
        public static string TtC_Input_Text 
        {
            get
            {
                string? val = GetKey(nameof(TtC_Input_Text));
                return val != null ? val : string.Empty;
            }
            set => SetKey(nameof(TtC_Input_Text), value); 
        }
        public static string Current_File 
        {
            get
            {
                string? val = GetKey(nameof(Current_File));
                return val != null ? val : string.Empty;
            }
            set => SetKey(nameof(Current_File), value); 
        }
        public static bool First_Open
        {
            get => BoolParse(GetKey(nameof(First_Open), true)) == true; 
            set => SetKey(nameof(First_Open), value);
        }
        public static bool Auto_Save
        {
            get => BoolParse(GetKey(nameof(Auto_Save), false)) == true; 
            set => SetKey(nameof(Auto_Save), value);
        }
        public static bool Save_On_Play
        {
            get => BoolParse(GetKey(nameof(Save_On_Play), true)) == true; 
            set => SetKey(nameof(Save_On_Play), value);
        }
        public static bool Debug_Pause
        {
            get => BoolParse(GetKey(nameof(Debug_Pause), true)) == true; 
            set => SetKey(nameof(Debug_Pause), value);
        }
        
        public static bool Higlight_Line
        {
            get => BoolParse(GetKey(nameof(Higlight_Line), true)) == true; 
            set => SetKey(nameof(Higlight_Line), value);
        }
        
        public static bool Always_Show_Highlight_Warning
        {
            get => BoolParse(GetKey(nameof(Always_Show_Highlight_Warning), true)) == true; 
            set => SetKey(nameof(Always_Show_Highlight_Warning), value);
        }
        
        public static bool TtC_Allow_Spaces
        {
            get => BoolParse(GetKey(nameof(TtC_Allow_Spaces), true)) == true; 
            set => SetKey(nameof(TtC_Allow_Spaces), value);
        }
        
        public static bool TtC_Use_Wrap
        {
            get => BoolParse(GetKey(nameof(TtC_Use_Wrap), false)) == true; 
            set => SetKey(nameof(TtC_Use_Wrap), value);
        }
        
        public static bool Play_In_Dedicated_Window
        {
            get => BoolParse(GetKey(nameof(Play_In_Dedicated_Window), false)) == true; 
            set => SetKey(nameof(Play_In_Dedicated_Window), value);
        }

        public static bool? BoolParse(string? value)
        {
            if (value == null) 
                return null;

            if(value.ToLower() == "true")
            {
                return true;
            }
            else if(value.ToLower() == "false")
            {
                return false;
            }
            else
            {
                return null;
            }
        }

        public static void Reset(bool dialog = false)
        {
            if (dialog)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to reset all IDE settings and values to default? This is irriversible.", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    // nothing
                }
                else
                {
                    return;
                }
            }

            First_Open = false;
            Save_Folder = true;
            Auto_Save = false;
            Open_Folder_Path = "";
            Current_Project_File = "";
            Left_Splitter_Distance = 300;
            Bottom_Splitter_Distance = 550;
            New_Project_Default_Directory = "";
            Default_Zoom = 100;
            Save_On_Play = true;
            Debug_Pause = true;
            Higlight_Line = true;
            Always_Show_Highlight_Warning = true;
            TtC_Allow_Spaces = true;
            TtC_Use_Wrap = false;
            TtC_Input_Text = "";
            Play_In_Dedicated_Window = false;
            Current_File = "";
        }

        public static void StartUp()
        {
            if (First_Open)
            {
                Reset();
            }
        }

        public static void SetKey(string key, object value)
        {
            using (var v = Registry.CurrentUser.CreateSubKey(keyName))
            {
                Type type = value.GetType();
                if (type == typeof(int))
                {
                    value = $"_ {value}";
                }
                v.SetValue(key, value);
            }
        }
        public static string? GetKey(string key, object defaultValue = null, bool create_if_null = true)
        {
            try
            {
                defaultValue ??= "";
                Type type = defaultValue.GetType();
                if (type == typeof(bool))
                {
                    bool val = (bool)defaultValue;
                    defaultValue = val ? "True" : "False";
                }
                using (var v = Registry.CurrentUser.OpenSubKey(keyName))
                {
                    if (v != null)
                    {
                        string retrievedData = v.GetValue(key) as string;
                        if (retrievedData != null)
                        {
                            if (type == typeof(int))
                            {
                                return retrievedData.Replace("_ ", "");
                            }
                            return retrievedData;
                        }
                        else if (create_if_null)
                        {
                            using (var _v = Registry.CurrentUser.CreateSubKey(keyName))
                            {
                                object original = defaultValue;
                                if (type == typeof(int))
                                {
                                    int inval = (int)defaultValue;
                                    string val = $"_ {inval}";
                                    defaultValue = val;
                                }
                                _v.SetValue(key, defaultValue);
                                return GetKey(key, type == typeof(int) ? original : null);
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return null;
        }
        public static bool SetArrayKey(string key, string[] value)
        {
            try
            {
                var v = Registry.CurrentUser.CreateSubKey(keyName);
                string val = string.Join("|", value);
                v.SetValue(key, val == "" ? "|" : val);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static string[]? GetArrayKey(string key, bool remove_empty = true)
        {
            try
            {
                var v = Registry.CurrentUser.OpenSubKey(keyName);
                if (v == null)
                    return null;
                string retrievedData = v.GetValue(key) as string;
                if (retrievedData != null)
                    return retrievedData.Split("|").Where(x => (remove_empty ? x != "" : x == x)).ToArray();
                else
                {
                    SetArrayKey(key, new string[0]);
                    return null;
                }
            }
            catch
            {
                return null;
            }
        }
        public static void Exit(IDE ide)
        {
            try
            {
                string path = ide.FileURLTextBox.Text;

                if (path == "") Application.Exit();

                string contents = ide.fctb.Text;
                string oldContents = File.ReadAllText(path);

                if (oldContents != contents)
                {
                    DialogResult result = MessageBox.Show("There are unsaved changes, do you want to save them?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        TreeManager.SaveFile(ide);
                        Application.Exit();
                    }
                    else if (result == DialogResult.No)
                    {
                        Application.Exit();
                    }
                }
            }
            catch
            {

            }
        }
        public static void Exit()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to quit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                Application.Exit();
        }
    }
}
