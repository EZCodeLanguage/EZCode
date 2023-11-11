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
            get => BoolParse(GetKey(nameof(Save_Folder))) == true; 
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
        public static bool? First_Open
        {
            get => BoolParse(GetKey(nameof(First_Open))); 
            set => SetKey(nameof(First_Open), value);
        }
        public static bool Auto_Save
        {
            get => BoolParse(GetKey(nameof(Auto_Save))) == true; 
            set => SetKey(nameof(Auto_Save), value);
        }

        private static bool? BoolParse(string? value)
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

        public static void Reset()
        {
            First_Open = false;
            Save_Folder = true;
            Auto_Save = false;
            Open_Folder_Path = "";
        }

        public static void StartUp()
        {
            if (First_Open != null ? First_Open == true : false)
            {
                Reset();
            }
            else if (First_Open == null)
            {
                Reset();
            }
        }

        public static void SetKey(string key, object value)
        {
            using (var v = Registry.CurrentUser.CreateSubKey(keyName))
            {
                v.SetValue(key, value);
            }
        }
        public static string? GetKey(string key, bool create_if_null = true)
        {
            try
            {
                using (var v = Registry.CurrentUser.OpenSubKey(keyName))
                {
                    if (v != null)
                    {
                        string retrievedData = v.GetValue(key) as string;
                        if (retrievedData != null)
                        {
                            return retrievedData;
                        }
                    }
                    else if (create_if_null)
                    {
                        using (var _v = Registry.CurrentUser.CreateSubKey(keyName))
                        {
                            _v.SetValue(key, "");
                            return "";
                        }
                    }
                }
            }
            catch
            {

            }
            return null;
        }
        public static void Exit(IDE ide)
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
        public static void Exit()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to quit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                Application.Exit();
        }
    }
}
