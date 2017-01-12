using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Collections;
#if UseCrystalReportDD
using CrystalDecisions.Windows.Forms;            
#endif

namespace Srvtools
{
    public partial class frmInfoSecurityAdd : Form
    {
        //public List<string> SelColbList = new List<string>();
        public ArrayList SelColbList = new ArrayList();

        public frmInfoSecurityAdd()
        {
            InitializeComponent();
        }

        private IContainer ict;
        IDesignerHost DesignerHost = null;
        private InfoSecurity infoS;
        public frmInfoSecurityAdd(InfoSecurity isy, IDesignerHost host, ArrayList ControlList)
        {
            InitializeComponent();
            DesignerHost = host;
            ict = host.Container;
            infoS = isy;
            SelColbList = ControlList;
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

        private void frmInfoSecurityAdd_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < ict.Components.Count; i++)
            {
                if (ict.Components[i] is InfoBindingSource)
                {
                    string str = ict.Components[i].ToString();
                    string strName = str.Substring(0, str.IndexOf(' '));
                    this.clstControlName.Items.Add(strName);
                }
                else if (ict.Components[i] is Panel)
                {
                    string str = ict.Components[i].ToString();
                    string strName = str.Substring(0, str.IndexOf(' '));
                    this.clstControlName.Items.Add(strName);
                }
                else if (ict.Components[i] is Button)
                {
                    string str = ict.Components[i].ToString();
                    string strName = str.Substring(0, str.IndexOf(' '));
                    this.clstControlName.Items.Add(strName);
                }
                else if (ict.Components[i] is ToolStripButton)
                {
                    string str = ict.Components[i].Site.Name.ToString();
                    if (str.Contains("bindingNavigator")) continue;
                    this.clstControlName.Items.Add(str);
                }
                else if (ict.Components[i] is InfoSecColumns)
                {
                    string str = ict.Components[i].Site.Name.ToString();
                    this.clstControlName.Items.Add(str);
                }
                else if (ict.Components[i] is TabControl)
                {
                    string str = ict.Components[i].Site.Name.ToString();
                    this.clstControlName.Items.Add(str);
                }

#if UseCrystalReportDD
                else if (ict.Components[i] is CrystalReportViewer)
                {
                    string str = ict.Components[i].Site.Name.ToString();
                    this.clstControlName.Items.Add(str);
                } 
#endif
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
    }
}