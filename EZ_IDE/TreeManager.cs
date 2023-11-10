using Microsoft.Win32;
using System.IO;
using System.Text.Json;
using System.Xml;
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
            var value = GetKey(AddedKeys.OpenFolderPath);
            if (value != null)
                BuildTree(value, ide.Tree.Nodes);
            LoadTreeViewData();
        }

        public void OpenFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            BuildTree(dialog.SelectedPath, ide.Tree.Nodes);

            if (Save_folder)
            {
                SetKey(AddedKeys.OpenFolderPath, dialog.SelectedPath);
            }
        }
        public void OpenFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.ShowDialog();
            string[] files = dialog.FileNames;
            List<TreeNode> nodes = new List<TreeNode>();
            foreach (string file in files)
            {
                TreeNode node = new TreeNode(file) { Name = file };
                nodes.Add(node);
            }

            ide.Tree.Nodes.AddRange(nodes.ToArray());
        }
        public void OpenProject()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "EZProject|*.ezproj";
            dialog.ShowDialog();

            string code = File.ReadAllText(dialog.FileName);
            string[] lines = code.Split(new char[] { '\n', '|' });
            HashSet<string> hashFiles = new HashSet<string>();
            FileInfo fileI = new FileInfo(dialog.FileName);
            DirectoryInfo dirI = new DirectoryInfo(fileI.Directory.ToString());
            string rootDir = "";
            string name = fileI.Name;

            for (int i = 0; i < lines.Length; i++)
            {
                string[] parts = lines[i].Split(":");
                string before = parts[0];
                string after = string.Join(":", parts.Skip(1)).Replace("\"", "");
                rootDir = dirI.FullName;
                if (before == "startup")
                {
                    hashFiles.Add(after);
                }
                if (before == "include")
                {
                    if(after != "all")
                    {
                        hashFiles.Add(after);
                    }
                    else
                    {
                        foreach(FileInfo file in dirI.GetFiles("*", SearchOption.AllDirectories))
                        {
                            hashFiles.Add(file.FullName); 
                        }
                    }
                }
                if (before == "exclude")
                {
                    if(after != "all")
                    {
                        hashFiles.Remove(after);
                    }
                    else
                    {
                        foreach(FileInfo file in dirI.GetFiles("*", SearchOption.AllDirectories))
                        {
                            hashFiles.Remove(file.FullName); 
                        }
                    }
                }
                if(before == "name")
                {
                    name = after;
                }
            }
            string[] files = hashFiles.ToArray();
            for (int i = 0; i < files.Length; i++)
            {
                files[i] = files[i].Replace("~/", "").Replace("~\\", "").Trim();
                files[i] = Path.Combine(rootDir, files[i]);
            }

            List<TreeNode> nodes = new List<TreeNode>();
            TreeNode main = new TreeNode(name);
            
            foreach(string f in files)
            {
                nodes.Add(new TreeNode(f) { Name = f });
            }
            main.Nodes.AddRange(nodes.ToArray());

            ide.Tree.Nodes.Add(main);
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

        public void SaveTreeViewData()
        {
            TreeNodeCollection nodes = ide.Tree.Nodes;

            foreach (TreeNode node in nodes)
            {
                WriteDataToFile(node);
            }

            StreamWriter stream = new StreamWriter(TreeViewDataFilePath);
            stream.Write(FileData);
            stream.Close();
        }
        string FileData = "";
        private void WriteDataToFile(TreeNode node)
        {
            FileData += node.Name + Environment.NewLine;
            foreach (TreeNode child in node.Nodes)
            {
                WriteDataToFile(child);
            }
        }

        private void LoadTreeViewData()
        {

        }

        public void SelectedNode(TreeViewEventArgs args)
        {
            var name = args.Node.Name;
            try
            {
                ide.fctb.ResetText();
                StreamReader reader = new StreamReader(name);
                ide.fctb.Text = reader.ReadToEnd();
                reader.Close();
                ide.FileURLTextBox.Text = name;
            }
            catch
            {
                if (name != "")
                {
                    FileAttributes attr = File.GetAttributes(name);

                    if (!attr.HasFlag(FileAttributes.Directory))
                    {
                        MessageBox.Show("Could not open the selected file", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    }
}
