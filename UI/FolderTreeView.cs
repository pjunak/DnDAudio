using System;
using System.IO;
using System.Windows.Forms;
using MusicPlayerApp.Logging;

namespace MusicPlayerApp.UI
{
    public class FolderTreeView : TreeView
    {
        public FolderTreeView()
        {
            this.AfterSelect += OnFolderSelected;
            this.Dock = DockStyle.Left;
        }

        /// <summary>
        /// Loads the root folder structure and initializes tree nodes.
        /// </summary>
        /// <param name="rootPath">Root directory to load.</param>
        public void LoadRootFolder(string rootPath)
        {
            Nodes.Clear();
            DirectoryInfo rootDir = new DirectoryInfo(rootPath);
            TreeNode rootNode = new TreeNode(rootDir.Name) { Tag = rootDir };
            Nodes.Add(rootNode);
            LoadSubFolders(rootNode);
        }

        /// <summary>
        /// Recursively loads subfolders into the tree view.
        /// </summary>
        private void LoadSubFolders(TreeNode node)
        {
            DirectoryInfo dir = (DirectoryInfo)node.Tag;

            foreach (var subDir in dir.GetDirectories())
            {
                TreeNode subNode = new TreeNode(subDir.Name) { Tag = subDir };
                node.Nodes.Add(subNode);
                LoadSubFolders(subNode);
            }
        }

        /// <summary>
        /// Event handler for selecting a folder node.
        /// </summary>
        private void OnFolderSelected(object sender, TreeViewEventArgs e)
        {
            DirectoryInfo selectedDir = (DirectoryInfo)e.Node.Tag;
            Logger.LogInfo($"Folder selected: {selectedDir.FullName}");
        }
    }
}
