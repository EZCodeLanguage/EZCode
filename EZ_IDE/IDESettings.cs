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
        private static bool _save_folder;
        public static bool Save_folder 
        {
            get { return _save_folder; }
            set { _save_folder = value; Save(); }
        }

        public static void Reset()
        {
            Save_folder = true;
        }

        public static void StartUp()
        {
            string? val = GetKey(AddedKeys.FirstOpen);
            if (bool.Parse(val != null ? val : "false"))
            {
                Reset();
            }
            else if (val == null)
            {
                SetKey(AddedKeys.FirstOpen, "true");
                Reset();
            }
        }

        public static void Open()
        {
            using (var key = Registry.CurrentUser.OpenSubKey(keyName))
            {
                if (key != null)
                {
                    bool retrievedData = bool.Parse(key.GetValue("save_folder") as string);
                    if (retrievedData != null)
                    {
                        Save_folder = retrievedData;
                    }
                }
            }
        }

        public static void Save()
        {
            using (var key = Registry.CurrentUser.CreateSubKey(keyName))
            {
                key.SetValue("save_folder", Save_folder.ToString());
            }
        }

        public enum AddedKeys
        {
            OpenFolderPath,
            FirstOpen
        }

        public static void SetKey(AddedKeys key, string value)
        {
            using (var v = Registry.CurrentUser.CreateSubKey(keyName))
            {
                switch (key)
                {
                    case AddedKeys.OpenFolderPath:
                        v.SetValue("open_folder_path", value);
                        break;
                }
            }
        }
        public static string? GetKey(AddedKeys key)
        {
            using (var v = Registry.CurrentUser.OpenSubKey(keyName))
            {
                if (v != null)
                {
                    switch (key)
                    {
                        case AddedKeys.OpenFolderPath:
                            string retrievedData = v.GetValue("open_folder_path") as string;
                            if (retrievedData != null)
                            {
                                return retrievedData;
                            }
                            break;
                    }
                }
            }
            return null;
        }
    }
}
