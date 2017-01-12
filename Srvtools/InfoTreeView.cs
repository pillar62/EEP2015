using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing.Design;
using System.Text;
using System.Windows.Forms;
using System.Drawing;


namespace Srvtools
{
    [ToolboxBitmap(typeof(InfoTreeView), "Resources.InfoTreeView.png")]
    public class InfoTreeView:TreeView,IGetValues
    {
        private ArrayList lstkey = new ArrayList();
        private ArrayList lstparent = new ArrayList();
        private ArrayList lsttext = new ArrayList();        
        
        public InfoTreeView()
            : base()
        { }

              
        private InfoBindingSource _bindingsource = null;
        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to")]
        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.All)]
        public InfoBindingSource BindingSource
        {
            get
            {
                return _bindingsource;
            }
            set
            {
                if (value != _bindingsource)
                {
                    _bindingsource = value;

                }
            }
        }

        private string _parentfiled;
        [Category("Infolight"),
        Description("Specifies the column stroring the data of parent node")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ParentField
        {
            get
            {
                return _parentfiled;
            }
            set
            {
                _parentfiled = value;
            }
        }

        private string _keyfiled;
        [Category("Infolight"),
        Description("Specifies the column stroring the data of node's key")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string KeyField
        {
            get
            {
                return _keyfiled;
            }
            set
            {
                _keyfiled = value;
            }
        }

        private string _textfiled;
        [Category("Infolight"),
        Description("Specifies the column stroring the data of node's text")]
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string TextField
        {
            get
            {
                return _textfiled;
            }
            set
            {
                _textfiled = value;
            }
        }

        private bool _autoApply = false;
        [Category("Infolight"), DefaultValue(false)]
        public bool AutoApply
        {
            get
            {
                return _autoApply;
            }
            set
            {
                _autoApply = value;
            }
        }

        public void Initial()
        {
            ArrayList lstmainkey = new ArrayList();
            ArrayList lstmaintext = new ArrayList();
            ArrayList lstchildkey = new ArrayList();
            ArrayList lstchildparent = new ArrayList();
            ArrayList lstchildtext = new ArrayList();

            lstkey.Clear();
            lstparent.Clear();
            lsttext.Clear();
            this.Nodes.Clear();

            InfoBindingSource ibs = this.BindingSource;
            if (ibs != null)
            {
                DataView view = ibs.List as DataView;
                if (view == null)
                {
                    return;
                }
                DataTable dt = view.ToTable();
                if (dt == null)
                {
                    return;
                }
                int nodecount = dt.Rows.Count;
                for (int i = 0; i < nodecount; i++)
                {
                    lstkey.Add(dt.Rows[i][this.KeyField]);
                    lstparent.Add(dt.Rows[i][this.ParentField]);
                    lsttext.Add(dt.Rows[i][this.TextField]);
                }
                for (int i = 0; i < nodecount; i++)
                {
                    if (lstkey[i].ToString() != lstparent[i].ToString())
                    {
                        if (lstparent[i].ToString() == string.Empty)
                        {
                            lstmainkey.Add(lstkey[i]);
                            lstmaintext.Add(lsttext[i]);
                        }
                        else
                        {
                            lstchildkey.Add(lstkey[i]);
                            lstchildparent.Add(lstparent[i]);
                            lstchildtext.Add(lsttext[i]);
                        }
                    }
                }
                int mainnodecount = lstmainkey.Count;
                TreeNode[] nodemain = new TreeNode[mainnodecount];

                for (int i = 0; i < mainnodecount; i++)
                {
                    nodemain[i] = new TreeNode();
                    nodemain[i].Text = lstmaintext[i].ToString();
                    nodemain[i].Name = lstmainkey[i].ToString();
                    this.Nodes.Add(nodemain[i]);
                }

                int childnodecount = lstchildkey.Count;
                TreeNode[] nodechild = new TreeNode[childnodecount];
                for (int i = 0; i < childnodecount; i++)
                {
                    nodechild[i] = new TreeNode();
                    nodechild[i].Text = lstchildtext[i].ToString();
                    nodechild[i].Name = lstchildkey[i].ToString();
                }

                for (int i = 0; i < childnodecount; i++)
                {
                    for (int j = 0; j < mainnodecount; j++)
                    {
                        if (lstchildparent[i].ToString() == lstmainkey[j].ToString())
                        {
                            nodemain[j].Nodes.Add(nodechild[i]);
                        }
                    }
                    for (int k = 0; k < childnodecount; k++)
                    {
                        if (lstchildparent[i].ToString() == lstchildkey[k].ToString())
                        {
                            nodechild[k].Nodes.Add(nodechild[i]);
                        }
                    }
                }
                this.ExpandAll();
            }

            this.AllowDrop = true;
            this.DragEnter += new DragEventHandler(InfoTreeView_DragEnter);
            this.DragDrop += new DragEventHandler(InfoTreeView_DragDrop);
            this.ItemDrag += new ItemDragEventHandler(InfoTreeView_ItemDrag);
        }

        void InfoTreeView_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private Point Position = new Point(0, 0);
        void InfoTreeView_DragDrop(object sender, DragEventArgs e)
        {
            TreeNode myNode = null;
            if (e.Data.GetDataPresent(typeof(TreeNode)))
            {
                myNode = (TreeNode)(e.Data.GetData(typeof(TreeNode)));
            }
            else
            {
                MessageBox.Show("error");
            }
            Position.X = e.X;
            Position.Y = e.Y;
            Position = this.PointToClient(Position);
            TreeNode DropNode = this.GetNodeAt(Position);
            if (DropNode != null && DropNode.Parent != myNode && DropNode != myNode)
            {
                TreeNode DragNode = myNode;
                myNode.Remove();
                DropNode.Nodes.Add(DragNode);
            }
            if (DropNode == null)
            {
                TreeNode DragNode = myNode;
                myNode.Remove();
                this.Nodes.Add(DragNode);
            }

            InfoBindingSource ibs = this.BindingSource;
            DataTable dt = ((InfoDataSet)ibs.DataSource).RealDataSet.Tables[ibs.DataMember];
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][this.KeyField].ToString() == myNode.Name)
                    dt.Rows[i][this.ParentField] = DropNode.Name;
            }

            if (AutoApply)
                ((InfoDataSet)ibs.DataSource).ApplyUpdates();
        }

        public void Apply()
        {
            InfoBindingSource ibs = this.BindingSource;
            ((InfoDataSet)ibs.DataSource).ApplyUpdates();
        }

        void InfoTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }

        public void InsertItem()
        {
            frmTreeView frmAddNode = new frmTreeView(this, true);
            if (frmAddNode.ShowDialog() == DialogResult.OK) 
            {
                this.Nodes.Clear();
                this.Initial();
            }

            this.SelectedNode = null;
        }

        public void DeleteItem()
        {
            TreeNode nodedelete = this.SelectedNode;
            InfoBindingSource ibs = this.BindingSource;
            DataTable dt = ((InfoDataSet)ibs.DataSource).RealDataSet.Tables[ibs.DataMember];

            if (nodedelete != null)
            {
                if (nodedelete.Nodes.Count > 0)
                {
                    throw new Exception("delete the childnode first!");
                    //CliSysMegLag.GetClientLanguage()                 
                }
                else
                {
                    for(int i = 0; i < dt.Rows.Count; i ++)
                    {
                        if(dt.Rows[i][this.KeyField].ToString() == nodedelete.Name)
                        {
                            dt.Rows[i].Delete();
                        }
                    }
                    ((InfoDataSet)ibs.DataSource).ApplyUpdates();
                }
            }
            this.Nodes.Clear();
            this.Initial();
            this.SelectedNode = null;
        }

        public void UpdateItem()
        {
            if (this.SelectedNode != null)
            {
                frmTreeView frmUpdateNode = new frmTreeView(this, false);
                if (frmUpdateNode.ShowDialog() == DialogResult.OK)
                {
                    this.Nodes.Clear();
                    this.Initial();
                }
                this.SelectedNode = null;
            }
        }



        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (string.Compare(sKind, "parentfield", true) == 0 || string.Compare(sKind, "keyfield", true) == 0 
                || string.Compare(sKind, "textfield", true) == 0)//IgnoreCase
            {
                if (this is InfoTreeView)
                {
                    InfoTreeView itv = (InfoTreeView)this;
                    if (itv.BindingSource != null && itv.BindingSource.DataSource != null )
                    {

                        int colNum =((InfoDataSet)itv.BindingSource.DataSource).RealDataSet.Tables[itv.BindingSource.DataMember].Columns.Count;

                        for (int i = 0; i < colNum; i++)
                        {
                            values.Add(((InfoDataSet)itv.BindingSource.DataSource).RealDataSet.Tables[itv.BindingSource.DataMember].Columns[i].ColumnName); 
                        }

                        if (values.Count > 0)
                        {
                            int i = values.Count;
                            retList = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                retList[j] = values[j];
                            }
                        }
                    }
                }
            }
            return retList;
        }

        #endregion
    }
}
