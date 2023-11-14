using EZCode.Variables;
using Microsoft.Win32;
using NAudio.Wave;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static EZ_IDE.Settings;

namespace EZ_IDE
{
    internal class TreeManager
    {
        public IDE ide;
        public TreeManager(IDE ide) => this.ide = ide;

        public string TreeViewDataFilePath
        {
            get
            {
                string appDataRoamingPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "EZCode_IDE");

                if (!Directory.Exists(appDataRoamingPath))
                    Directory.CreateDirectory(appDataRoamingPath);

                return Path.Combine(appDataRoamingPath, "TreeViewList.txt");
            }
        }

        public void SetTreeNodes()
        {
            var value = Open_Folder_Path;
            if (value != null)
                BuildTree(value, ide.Tree.Nodes);
        }
        public static bool SaveFile(IDE ide, bool dialog = false)
        {
            string path = ide.FileURLTextBox.Text;
            string contents = ide.fctb.Text;

            if (File.ReadAllText(path) != contents && dialog)
            {
                DialogResult result = MessageBox.Show("There are unsaved changes, do you want to save them?", "Unsaved changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    // nothing
                }
                else if (result == DialogResult.No)
                {
                    return true;
                }
                else if (result == DialogResult.Cancel)
                {
                    return false;
                }
            }

            File.WriteAllText(path, contents);

            return true;
        }
        public bool SaveFile(bool dialog = false)
        {
            return SaveFile(ide, dialog);
        }

        public void OpenFolder(string path = "")
        {
            if(path == "")
            {
                FolderBrowserDialog dialog = new FolderBrowserDialog();
                dialog.ShowDialog();
                path = dialog.SelectedPath;
            }
            ide.Tree.Nodes.Clear();
            BuildTree(path, ide.Tree.Nodes);

            Open_Folder_Path = path;
        }
        public void OpenFile(string path = "")
        {
            if(path == "")
            {
                OpenFileDialog dialog = new OpenFileDialog();
                dialog.ShowDialog();
                path = dialog.FileName;
            }

            DialogResult result = MessageBox.Show("Do you want to open the directory of this file?", "Open Folder", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                OpenFolder(new FileInfo(path).Directory.FullName);
            }
            else if (result == DialogResult.No)
            {
                ide.Tree.Nodes.Add(new TreeNode(path) { Name = path });
            }
            else if (result == DialogResult.Cancel)
            {
                return;
            }
        }
        public void OpenPath(string path)
        {
            if (!File.Exists(path)) return;
            
            FileAttributes attr = File.GetAttributes(path);

            if (!attr.HasFlag(FileAttributes.Directory))
            {
                OpenFolder(path);
            }
            else
            {
                OpenFile(path);
            }
        }

        private TreeNodeCollection BuildTree(string path, TreeNodeCollection addInMe)
        {
            try
            {
                DirectoryInfo directoryInfo = new DirectoryInfo(path);

                TreeNode curNode = addInMe.Add(directoryInfo.FullName, directoryInfo.Name);

                foreach (FileInfo file in directoryInfo.GetFiles())
                {
                    curNode.Nodes.Add(file.FullName, file.Name);
                }
                foreach (DirectoryInfo subdir in directoryInfo.GetDirectories())
                {
                    BuildTree(subdir.FullName, curNode.Nodes);
                }
            }
            catch
            {
                // nothing
            }
            return addInMe;
        }

        public void SelectedNode(TreeViewEventArgs args)
        {
            var name = args.Node.Name;
            try
            {
                StreamReader reader = new StreamReader(name);
                ide.fctb.ResetText();
                string s = reader.ReadToEnd();
                reader.Close();
                ide.FileURLTextBox.Text = name;

                if (name.EndsWith(".ezcode"))
                {
                    ide.fctb.DescriptionFile = "./EZCode_Syntax.xml";
                }
                else if (name.EndsWith(".ezproj"))
                {
                    ide.fctb.DescriptionFile = "./EZProj_Syntax.xml";
                }
                else if (name != "")
                {
                    ide.fctb.DescriptionFile = "";
                }
                else if (name == "")
                {
                    ide.fctb.DescriptionFile = "./EZCode_Syntax.xml";
                }
                ide.fctb.Text = s;
            }
            catch
            {
                SelectedCatchCheck(name);
            }
        }
        public void SelectedCatchCheck(string path)
        {
            try
            {
                if (path != "")
                {
                    FileAttributes attr = File.GetAttributes(path);

                    if (!attr.HasFlag(FileAttributes.Directory))
                    {
                        MessageBox.Show("Could not open the selected file", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch
            {

            }
        }
    }
}
