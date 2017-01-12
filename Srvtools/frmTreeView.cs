using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class frmTreeView : Form
    {
        private InfoTreeView tvCopy;
        private bool isAddNode;
        InfoBindingSource ibs = new InfoBindingSource();
        
        public frmTreeView(InfoTreeView tv, bool isAdd)
        {
            InitializeComponent();

            tvCopy = tv;
            ibs = tvCopy.BindingSource;
            isAddNode = isAdd;
            if (isAdd)
            {
                this.Text = "Add TreeNode";
            }
            else
            {
                this.Text = "Update TreeNode";
            }

            InitialText();
        }

        private void frmTreeView_Load(object sender, EventArgs e)
        {
            //SYS_LANGUAGE language = CliSysMegLag.GetClientLanguage();
            SYS_LANGUAGE language = CliUtils.fClientLang;
            string caption = SysMsg.GetSystemMessage(language, "Srvtools", "InfoNavigator", "NavText");

            for (int i = 0; i < 7; i++)
            {
                caption = caption.Substring(caption.IndexOf(";") + 1);
            }
            btnOk.Text = caption.Substring(0, caption.IndexOf(";"));
            caption = caption.Substring(caption.IndexOf(";") + 1);
            btnCancel.Text = caption.Substring(0, caption.IndexOf(";"));
            
        }

        private void InitialText()
        { 
            //initial combobox
            //cbbParentField.DataSource = tvCopy.BindingSource;
            //cbbParentField.DisplayMember = tvCopy.KeyField;
            //cbbParentField.ValueMember = tvCopy.TextField;

        
            







            DataTable dt = ((InfoDataSet)ibs.DataSource).RealDataSet.Tables[ibs.DataMember];

            ArrayList lstnode = new ArrayList();
            lstnode.Add(new nodeinfo("","(None)"));
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (isAddNode || dt.Rows[i][tvCopy.KeyField].ToString() != tvCopy.SelectedNode.Name )
                {
                    lstnode.Add(new nodeinfo(dt.Rows[i][tvCopy.KeyField].ToString(), dt.Rows[i][tvCopy.TextField].ToString()));
                }
            }

          
            cbbParentField.DataSource = lstnode;
            cbbParentField.DisplayMember = "NodeKey";
            cbbParentField.ValueMember = "NodeText";

            
            //Add Node
            if (isAddNode)
            {               
               // tbKeyField.Text = 
              
                ArrayList lstkey = new ArrayList();

                for (int i = 0; i < dt.Rows.Count; i ++ )
                {
                    lstkey.Add(dt.Rows[i][tvCopy.KeyField]);
                }

                tbKeyField.Text = FindMaxKey(lstkey).ToString();

                if (tvCopy.SelectedNode != null)
                {
                    cbbParentField.SelectedValue = tvCopy.SelectedNode.Text;
                }
                else
                {
                    cbbParentField.SelectedValue = "(None)";
                }
            }
            else
            {
                tbKeyField.Text = tvCopy.SelectedNode.Name;

                if (tvCopy.SelectedNode.Parent == null)
                {
                    cbbParentField.SelectedValue = "(None)";
                }
                else
                {
                    cbbParentField.SelectedValue = tvCopy.SelectedNode.Parent.Text;
                }
                tbTextField.Text = tvCopy.SelectedNode.Text;
            
            }

            tbTextField.Focus();
        }

        private int FindMaxKey(ArrayList arrlst)
        {
            int maxkey = -1;
            if (arrlst.Count > 0)
            {
                try
                {
                    maxkey = Convert.ToInt32(arrlst[0]);

                    for (int i = 1; i < arrlst.Count; i++)
                    {
                        int key = Convert.ToInt32(arrlst[i]);
                        if (key > maxkey)
                        {
                            maxkey = key;
                        }
                    }
                }
                catch
                {
                    throw new Exception("Can't Convert Key to int32");
                }
            }

            return (maxkey + 1);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
          
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string strkey = "";
            string strparent = "";
            string strtext = "";
            DataTable dt = ((InfoDataSet)ibs.DataSource).RealDataSet.Tables[ibs.DataMember];

            strkey = tbKeyField.Text;
            if (cbbParentField.SelectedIndex == -1)
            {
                strparent = "";
            }
            else
            {
                strparent = cbbParentField.Text;
            }
            if (tbTextField.Text == "")
            {
                throw new Exception("the value of text can not be null");
            }
            else
            {
                strtext = tbTextField.Text;
            }

           
            if (isAddNode)
            {
                DataRow drAdd = dt.NewRow();
                drAdd[tvCopy.KeyField] = strkey;
                drAdd[tvCopy.ParentField] = strparent;
                drAdd[tvCopy.TextField] = strtext;
                dt.Rows.Add(drAdd);
                ((InfoDataSet)ibs.DataSource).ApplyUpdates();
            }

            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][tvCopy.KeyField].ToString() == strkey)
                    {
                        dt.Rows[i][tvCopy.ParentField] = strparent;
                        dt.Rows[i][tvCopy.TextField] = strtext;
                    }
                }
                ((InfoDataSet)ibs.DataSource).ApplyUpdates();

             }

        }
    }

    public class nodeinfo
    {
        private string nodekey;
        private string nodetext;
        
        public nodeinfo(string strkey, string strtext)
        {
            nodekey = strkey;
            nodetext = strtext;
        }


        public string NodeKey
        {
            get
            {
                return nodekey;
            }
        }

        public string NodeText
        {
            get
            {
                return nodetext;
            }
        }
    }
}