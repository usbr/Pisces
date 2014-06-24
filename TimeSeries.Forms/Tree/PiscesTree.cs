using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Aga.Controls.Tree;
using Aga.Controls.Tree.NodeControls;
using SeriesCatalogRow = Reclamation.TimeSeries.TimeSeriesDatabaseDataSet.SeriesCatalogRow;
namespace Reclamation.TimeSeries.Forms
{
    /// <summary>
    /// UserControl wrapper containing a TreeView TreeViewAdv
    /// from http://sourceforge.net/projects/treeviewadv/
    /// http://www.codeproject.com/KB/tree/treeviewadv.aspx
    /// </summary>
    public partial class PiscesTree : UserControl
    {

        ITreeModel model;

        public PiscesTree(ITreeModel  model)
        {
            this.model = model;
            InitializeComponent();
            
            treeView1.SelectionChanged += new EventHandler(treeView1_SelectionChanged);
            treeView1.SelectionMode = TreeSelectionMode.Multi;
            treeView1.NodeControls.Clear();

            NodeStateIcon ni = new NodeStateIcon();
            ni.DataPropertyName = "Icon";
            treeView1.NodeControls.Add(ni);

            NodeTextBox tb = new NodeTextBox();
            treeView1.NodeControls.Add(tb);

            tb.DataPropertyName = "Name";
            tb.EditEnabled = true;
            tb.LabelChanged += new EventHandler<LabelEventArgs>(tb_LabelChanged);
            

            treeView1.Model = model;

            hydrometCommandLine1.OnSubmit += new EventHandler(hydrometCommandLine1_OnSubmit);
        }

        public event EventHandler<EventArgs> LabelChanged;

        void tb_LabelChanged(object sender, LabelEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                if (LabelChanged != null)
                    LabelChanged(this, EventArgs.Empty);
            }
        }

        public void RemoveCommandLine()
        {
            if (tabControl1.TabPages.Contains(tabPageCommand))
                tabControl1.TabPages.Remove(tabPageCommand);
        }

        public void SetModel(ITreeModel model)
        {
            treeView1.Model = null;
            this.model = model;
            treeView1.Model = model;
            ExpandRootNodes();
        }


        bool m_selectionChanged = false;

        void treeView1_SelectionChanged(object sender, EventArgs e)
        {
            Console.WriteLine("PiscesTree.SelectionChanged");
            m_selectionChanged = true;
            //OnSelectionChanged();
        }

        void hydrometCommandLine1_OnSubmit(object sender, EventArgs e)
        {
            //hydrometCommandLine1.
            OnSelectionChanged();
        }

        public bool IsCommandLine
        {
            get { return this.tabControl1.SelectedTab == tabPageCommand; }
        }


        public int SelectedCount
        {
            get {
                if (IsCommandLine)
                    return 0;
                return treeView1.SelectedNodes.Count; }
        }

        public IEnumerable<Series> GetSeriesRecursive()
        {
            TreeNodeAdv n = treeView1.SelectedNode;
            if (n.IsLeaf)
                yield break;
            if (!n.IsExpandedOnce)
            {
                n.Expand();
                Application.DoEvents();
            }
            TreeNodeAdv node = n;
            var folders = new List<TreeNodeAdv>();
            while (node != null)
            {
                foreach (var item in node.Children)
                {
                    if (item.IsLeaf && item.Tag is Series)
                        yield return item.Tag as Series;
                    else
                        if (!item.IsLeaf)// folder
                        {
                            if (!item.IsExpandedOnce)
                            {
                                item.Expand();
                                Application.DoEvents();
                            }
                            folders.Add(item);
                        }
                }
                if (folders.Count > 0)
                {
                    node = folders[0];
                    folders.Remove(node);
                }
                else
                {
                    node = null;
                }
            }

        }

        

        public Series[] GetSelectedSeries()
        {
                var rval = new List<Series>();

                if (IsCommandLine)
                {
                    return hydrometCommandLine1.SelectedSeries;
                }
                else
                {
                    for (int i = 0; i < treeView1.SelectedNodes.Count; i++)
                    {
                        PiscesObject o = treeView1.SelectedNodes[i].Tag as PiscesObject;
                        if (o is Series)
                            rval.Add(o as Series);
                    }
                }
                return rval.ToArray();
        }

        public PiscesFolder SelectedFolder
        {
            get {
                if (SelectedFolders.Length == 0)
                    return null;

                return SelectedFolders[0];
            }
        }

        public PiscesFolder RootFolder
        {
            get
            {
               if( treeView1.Root.Children.Count >0)
                  return treeView1.Root.Children[0].Tag as PiscesFolder;
                return null;
            }
        }

        public PiscesFolder[] SelectedFolders
        {
            get
            {
                var rval = new List<PiscesFolder>();
                if (IsCommandLine)
                    return rval.ToArray();

                for (int i = 0; i < treeView1.SelectedNodes.Count; i++)
                {
                    PiscesObject o = treeView1.SelectedNodes[i].Tag as PiscesObject;
                    if( o is PiscesFolder)
                        rval.Add(o as PiscesFolder);
                }
                return rval.ToArray();
            }
        }

        enum SelectionTypes { Folder, Series };

        private int[] GetSelectedIndices(SelectionTypes t)
        {
            List<int> rval = new List<int>();

            if (IsCommandLine)
                return rval.ToArray();

            for (int i = 0; i < treeView1.SelectedNodes.Count; i++)
            {
                 SeriesCatalogRow si = treeView1.SelectedNodes[i].Tag as SeriesCatalogRow;
                if(t == SelectionTypes.Series && !si.IsFolder)
                rval.Add(si.id);
                if (t == SelectionTypes.Folder && si.IsFolder)
                    rval.Add(si.id);
            }
            return rval.ToArray();
        }


        public PiscesObject SelectedObject
        {
            get
            {
                return treeView1.SelectedNode.Tag as PiscesObject;
            }
        }

        public bool IsFolderSelected
        {
            get
            {
                if (treeView1.SelectedNode == null)
                    return false;
                return !treeView1.SelectedNode.IsLeaf;
            }                
        }

        internal void SelectParent()
        {

            treeView1.SelectedNode = treeView1.SelectedNode.Parent;
        }

        public void ExpandRootNodes()
        {
            if (treeView1.Root.Children.Count > 0)
            {
                for (int i = 0; i < treeView1.Root.Children.Count; i++)
                {
                    treeView1.Root.Children[i].Expand();
                }
            }
        }

         void OnSelectionChanged()
        {
            if (SelectionChanged != null)
            {
                SelectionChanged(this, EventArgs.Empty);
            }
        }
        public event EventHandler SelectionChanged;


        private void treeViewAdv1_ItemDrag(object sender, ItemDragEventArgs e)
        {
           treeView1.DoDragDropSelectedNodes(DragDropEffects.Move);
        }

        private void treeViewAdv1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNodeAdv[])) && treeView1.DropPosition.Node != null)
            {
                TreeNodeAdv[] nodes = e.Data.GetData(typeof(TreeNodeAdv[])) as TreeNodeAdv[];
                TreeNodeAdv parent = treeView1.DropPosition.Node;
                if (treeView1.DropPosition.Position != NodePosition.Inside)
                    parent = parent.Parent;
                foreach (TreeNodeAdv node in nodes)
                    if (!CheckNodeParent(parent, node))
                    {
                        e.Effect = DragDropEffects.None;
                        return;
                    }

                e.Effect = e.AllowedEffect;
            }
        }
        private bool CheckNodeParent(TreeNodeAdv parent, TreeNodeAdv node)
        {
            while (parent != null)
            {
                if (node == parent)
                    return false;
                else
                    parent = parent.Parent;
            }
            return true;
        }


        public event EventHandler<ParentChangedEventArgs> TreeNodeParentChanged;

        private void OnNodeParentChanged(ParentChangedEventArgs e)
        {
            EventHandler<ParentChangedEventArgs> handler = TreeNodeParentChanged;
            if (TreeNodeParentChanged != null)
                handler(this, e);
        }

        public event EventHandler<SortChangedEventArgs> TreeNodeSortChanged;
        private void OnNodeSortOrderChanged(SortChangedEventArgs e)
        {
            EventHandler<SortChangedEventArgs> handler = TreeNodeSortChanged;
            if (TreeNodeParentChanged != null)
                handler(this, e);
        }


        private void treeView1_DragDrop(object sender, DragEventArgs e)
        {
            treeView1.BeginUpdate();

            TreeNodeAdv[] nodes = (TreeNodeAdv[])e.Data.GetData(typeof(TreeNodeAdv[]));
            TreeNodeAdv dropNodeAdv = treeView1.DropPosition.Node;
            PiscesObject dropNode = treeView1.DropPosition.Node.Tag as PiscesObject;

            if (treeView1.DropPosition.Position == NodePosition.Inside && dropNode is PiscesFolder)
            {
                foreach (TreeNodeAdv n in nodes)
                {
                    PiscesObject o = n.Tag as PiscesObject;
                    OnNodeParentChanged(new ParentChangedEventArgs(o,dropNode as PiscesFolder));
                }
                treeView1.DropPosition.Node.IsExpanded = true;
            }
            else
            {
                PiscesFolder parent = dropNodeAdv.Parent.Tag as PiscesFolder;
                PiscesObject nextItem = dropNode;
                int sortOrder = nextItem.SortOrder;

                if (treeView1.DropPosition.Position == NodePosition.Before)
                {
                    sortOrder--;
                }

                foreach (TreeNodeAdv node in nodes)
                {
                    OnNodeParentChanged(new ParentChangedEventArgs(node.Tag as PiscesObject, parent));
                }

                
                Console.WriteLine("sort order for dragged object = "+sortOrder);

                foreach (TreeNodeAdv node in nodes)
                {
                    PiscesObject o = node.Tag as PiscesObject;
                    OnNodeSortOrderChanged(new SortChangedEventArgs(o, sortOrder));
                }
            }

            treeView1.EndUpdate();

        }


        public void ExpandSelected()
        {
            treeView1.SelectedNode.Expand();
        }

        private void treeView1_KeyPress(object sender, KeyPressEventArgs e)
        {
            Console.WriteLine("piscesTree.KeyPress");
            Console.WriteLine(e.KeyChar);
            //Keys.Delete 
           
        }

        public event EventHandler<EventArgs> Delete;

        private void treeView1_KeyUp(object sender, KeyEventArgs e)
        {
            Console.WriteLine("piscesTree.KeyUp");
            if (e.KeyCode == Keys.Delete)
            {
                if (Delete != null)
                    Delete(this, EventArgs.Empty);
                Console.WriteLine("Delete");
            }
            else
            {
                if (m_selectionChanged)
                {
                    OnSelectionChanged();
                    m_selectionChanged = false;
                }
            }
        }

        private void treeView1_Expanding(object sender, TreeViewAdvEventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
        }

        private void treeView1_Expanded(object sender, TreeViewAdvEventArgs e)
        {
            this.Cursor = Cursors.Default;
        }


        MouseButtons mouseClickButtons = System.Windows.Forms.MouseButtons.None;

        public MouseButtons MouseClickButtons
        {
            get { return mouseClickButtons; }
            //set { mouseClickButtons = value; }
        }
       

        private void treeView1_MouseDown(object sender, MouseEventArgs e)
        {
            Console.WriteLine("MouseDown");
            mouseClickButtons = e.Button;
            Console.WriteLine(mouseClickButtons.ToString());
        }

        private void treeView1_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("treeView1_MouseClick");
        }

        private void treeView1_Click(object sender, EventArgs e)
        {
            Console.WriteLine("treeView1_Click");
        }

        private void treeView1_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine("treeView1_MouseUp");
            if (m_selectionChanged)
            {
                OnSelectionChanged();
                m_selectionChanged = false;
            }
        }

        
        /// <summary>
        /// Filter set in textbox above the tree control
        /// </summary>
        public string Filter = "";

        /// <summary>
        /// When the filter is changed the main program can set the database filter 
        /// at a higher level.
        /// </summary>
        public event EventHandler<EventArgs> FilterChanged;


        private void textBoxTreeFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                Filter = this.textBoxTreeFilter.Text;

                EventHandler<EventArgs> handler = FilterChanged;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }

                SetModel(model);
                if (Filter.Trim() != "")
                    treeView1.ExpandAll();
            }
        }

    }
}
