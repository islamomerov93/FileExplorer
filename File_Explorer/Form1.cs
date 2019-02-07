using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace File_Explorer
{
    public partial class Form1 : Form
    {
        public FolderBrowserDialog fbd { get; private set; }

        public Form1()
        {
            InitializeComponent();
            treeView1.ImageList = imageList1;
            splitContainer1.Dock = DockStyle.Fill;
            PopulateTreeView();
            timer1.Start();
        }

        private void PopulateTreeView()
        {
            TreeNode rootNode;

            DirectoryInfo info = new DirectoryInfo(@"\\DESKTOP-8BTETDB\Users\I_S_L_A_M");
            if (info.Exists)
            {
                rootNode = new TreeNode(info.Name);
                rootNode.Tag = info;
                GetDirectories(info.GetDirectories(), rootNode);
                treeView1.Nodes.Add(rootNode);
            }
        }
        private void GetDirectories(DirectoryInfo[] subDirs,
            TreeNode nodeToAddTo)
        {
            TreeNode aNode;
            DirectoryInfo[] subSubDirs;
            foreach (DirectoryInfo subDir in subDirs)
            {
                aNode = new TreeNode(subDir.Name, 0, 0);
                aNode.Tag = subDir;
                aNode.ImageKey = "folder";
                subSubDirs = subDir.GetDirectories();
                if (subSubDirs.Length != 0)
                {
                    GetDirectories(subSubDirs, aNode);
                }
                nodeToAddTo.Nodes.Add(aNode);
            }
        }

        void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode newSelected = e.Node;
            listView1.Items.Clear();
            DirectoryInfo nodeDirInfo = (DirectoryInfo)newSelected.Tag;
            ListViewItem.ListViewSubItem[] subItems;
            ListViewItem item = null;

            foreach (DirectoryInfo dir in nodeDirInfo.GetDirectories())
            {
                item = new ListViewItem(dir.Name, 0);
                subItems = new ListViewItem.ListViewSubItem[]
                    {new ListViewItem.ListViewSubItem(item, "Directory"),
             new ListViewItem.ListViewSubItem(item,
                dir.LastAccessTime.ToShortDateString())};
                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
            }
            foreach (FileInfo file in nodeDirInfo.GetFiles())
            {
                item = new ListViewItem(file.Name, 1);
                item.Tag = file.FullName;
                subItems = new ListViewItem.ListViewSubItem[]
                    { new ListViewItem.ListViewSubItem(item, "File"),
             new ListViewItem.ListViewSubItem(item,
                file.LastAccessTime.ToShortDateString())};

                item.SubItems.AddRange(subItems);
                listView1.Items.Add(item);
                
            }

            listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        public Icon LoadIconFromExtension(string extension)
        {
            string path = string.Format("dummy{0}", extension);
            using (File.Create(path)) { }
            Icon icon = Icon.ExtractAssociatedIcon(path);
            File.Delete(path);
            return icon;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                Process.Start(listView1.SelectedItems[0].Tag.ToString());
            }
        }
        int count = 0;
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            count++;
            switch (count)
            {
                case 1:
                    pictureBox2.Image = Properties.Resources.detail;
                    listView1.View = View.Details;
                    break;
                case 2:
                    pictureBox2.Image = Properties.Resources.grid;
                    listView1.View = View.LargeIcon;
                    break;
                case 3:
                    pictureBox2.Image = Properties.Resources.tile;
                    listView1.View = View.Tile;
                    break;
                case 4:
                    pictureBox2.Image = Properties.Resources.list;
                    listView1.View = View.List;
                    break;
                case 5:
                    pictureBox2.Image = Properties.Resources.small;
                    listView1.View = View.SmallIcon;
                    count = 0;
                    break;
            }
        }
    }
}
