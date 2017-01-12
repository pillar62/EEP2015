using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Web.UI.Design;
using System.Web.UI;
using System.Collections;
#if UseCrystalReportDD
using CrystalDecisions.Web;
#endif

namespace JQClientTools
{
    public partial class JQSecurityAddForm : Form
    {
        //public List<string> SelColbList = new List<string>();
        public ArrayList SelColbList = new ArrayList();

        public JQSecurityAddForm()
        {
            InitializeComponent();
        }

        private IContainer ict;
        IDesignerHost DesignerHost = null;
        private JQSecurity ws;
        public JQSecurityAddForm(JQSecurity isy, IDesignerHost host, ArrayList ControlList)
        {
            InitializeComponent();
            DesignerHost = host;
            ict = host.Container;
            ws = isy;
            SelColbList = ControlList;
        }

        private void frmWebSecurityAdd_Load(object sender, EventArgs e)
        {
            foreach (System.Web.UI.Control c in ict.Components)
            {
                if (c is JQDataGrid)
                {
                    string strName = (c as JQDataGrid).ID;
                    this.clstControlName.Items.Add(strName);

                    foreach (JQToolItem item in (c as JQDataGrid).TooItems)
                    {
                        if (item.Icon == "icon-add" || item.Icon == "icon-edit" || item.Icon == "icon-remove"
                            || item.Icon == "icon-save" || item.Icon == "icon-undo")
                            continue;

                        this.clstControlName.Items.Add(item.Text + "_" + strName);
                    }
                }
                else if (c is System.Web.UI.WebControls.Panel)
                {
                    string strName = ((System.Web.UI.WebControls.Panel)c).ID;
                    this.clstControlName.Items.Add(strName);
                }
                else if (c is System.Web.UI.WebControls.Button)
                {
                    string strName = ((System.Web.UI.WebControls.Button)c).ID;
                    this.clstControlName.Items.Add(strName);
                }
                else if (c is System.Web.UI.WebControls.ImageButton)
                {
                    string strName = ((System.Web.UI.WebControls.ImageButton)c).ID;
                    this.clstControlName.Items.Add(strName);
                }
                else if (c is System.Web.UI.WebControls.LinkButton)
                {
                    string strName = ((System.Web.UI.WebControls.LinkButton)c).ID;
                    this.clstControlName.Items.Add(strName);
                }
                else if (c is System.Web.UI.WebControls.HyperLink)
                {
                    string strName = ((System.Web.UI.WebControls.HyperLink)c).ID;
                    this.clstControlName.Items.Add(strName);
                }
                else if (c is JQSecColumns)
                {
                    string strName = ((JQSecColumns)c).ID;
                    this.clstControlName.Items.Add(strName);
                }
            }

            for (int j = 0; j < SelColbList.Count; j++)
            {
                if (this.clstControlName.Items.Contains(SelColbList[j]))
                {
                    int m = this.clstControlName.Items.IndexOf(SelColbList[j]);
                    this.clstControlName.SetItemChecked(m, true);
                }
            }
            bHasLoaded = true;
        }

        private bool bHasLoaded = false;
        private void clstControlName_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (bHasLoaded)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    this.SelColbList.Add(this.clstControlName.SelectedItem.ToString());
                }
                else
                {
                    this.SelColbList.Remove(this.clstControlName.SelectedItem.ToString());
                }
            }
        }
    }
}