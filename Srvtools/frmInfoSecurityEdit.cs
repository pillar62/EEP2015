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
    public partial class frmInfoSecurityEdit : Form
    {
        public frmInfoSecurityEdit()
        {
            InitializeComponent();
        }

        //public ClientQuery cqeditor = new ClientQuery();
        private IContainer ict;
        IDesignerHost DesignerHost = null;
        private InfoSecurity infoS;
        public frmInfoSecurityEdit(InfoSecurity isy, IDesignerHost host)
        {
            InitializeComponent();
            DesignerHost = host;
            ict = host.Container;
            infoS = isy;
            //InitializeColumn(infoS);

        }

        private void frmInfoSecurityEdit_Load(object sender, EventArgs e)
        {
            object[] param = new object[1];
            param[0] = infoS.MenuID;
            CliUtils.fLoginDB = infoS.DBAlias;
            object[] myRet = CliUtils.CallMethod("GLModule", "GetMenu", param);
            if ((myRet != null) && (0 == (int)myRet[0]))
            {
                //List<string> listControlName = (List<string>)myRet[1];
                //List<string> listDescription = (List<string>)myRet[2];
                //List<string> listType = (List<string>)myRet[3];
                ArrayList listControlName = (ArrayList)myRet[1];
                ArrayList listDescription = (ArrayList)myRet[2];
                ArrayList listType = (ArrayList)myRet[3];
                if (listControlName.Count > 0)
                {
                    infoDataGridView1.Rows.Add(listControlName.Count);
                    for (int i = 0; i < listControlName.Count; i++)
                    {
                        if (listControlName[i].ToString() != "" && listControlName[i] != null)
                        {
                            infoDataGridView1.Rows[i].Cells["MenuID"].Value = infoS.MenuID;
                            infoDataGridView1.Rows[i].Cells["ControlName"].Value = listControlName[i];
                            infoDataGridView1.Rows[i].Cells["Descriptions"].Value = listDescription[i];
                            infoDataGridView1.Rows[i].Cells["Type"].Value = listType[i];
                        }
                    }
                }
            }
            CliUtils.fLoginDB = "";
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddAll_Click(object sender, EventArgs e)
        {
            int rowCount = infoDataGridView1.Rows.Count;
            rowCount = rowCount - 1;
            for (int i = 0; i < ict.Components.Count; i++)
            {
                if (ict.Components[i] is InfoBindingSource)
                {
                    string str = ict.Components[i].ToString();
                    string strName = str.Substring(0, str.IndexOf(' '));
                    string strDes = (ict.Components[i] as InfoBindingSource).DataMember;
                    string strType = "InfoBindingSource";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is Panel)
                {
                    if (ict.Components[i] is TabPage) continue;
                    string str = ict.Components[i].ToString();
                    string strName = str.Substring(0, str.IndexOf(' '));
                    string strDes = "";
                    string strType = "Panel";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is TabControl)
                {
                    string str = ict.Components[i].ToString();
                    string strName = str.Substring(0, str.IndexOf(' '));
                    string strDes = "";
                    string strType = "TabControl";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is Button)
                {
                    string str = ict.Components[i].ToString();
                    string strName = str.Substring(0, str.IndexOf(' '));
                    string strDes = (ict.Components[i] as Button).Text;
                    string strType = "Button";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is ToolStripButton)
                {
                    string str = ict.Components[i].Site.Name.ToString();
                    if (str.Contains("bindingNavigator")) continue;
                    string strDes = "ToolStripButton";
                    string strType = "ToolStripButton";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && str == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = str;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is InfoSecColumns)
                {
                    IComponentChangeService ComponentChangeService =
                            this.DesignerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                    object oldValue = null;
                    object newValue = null;
                    string str = ict.Components[i].Site.Name.ToString();
                    PropertyDescriptor descName = TypeDescriptor.GetProperties((ict.Components[i] as InfoSecColumns))["Name"];
                    ComponentChangeService.OnComponentChanging((ict.Components[i] as InfoSecColumns), descName);
                    oldValue = (ict.Components[i] as InfoSecColumns).Name;
                    (ict.Components[i] as InfoSecColumns).Name = str;
                    newValue = str;
                    ComponentChangeService.OnComponentChanged((ict.Components[i] as InfoSecColumns), descName, oldValue, newValue);

                    string strDes = "";
                    string strType = "InfoSecColumns";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && str == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = str;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
#if UseCrystalReportDD
                else if (ict.Components[i] is CrystalReportViewer)
                {
                    string str = ict.Components[i].Site.Name.ToString();
                    string strDes = "CrystalDecisions.Windows.Forms.CrystalReportViewer";
                    string strType = "CrystalReportViewer";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && str == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = str;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                } 
#endif
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //List<string> ControlList = new List<string>();
            ArrayList ControlList = new ArrayList();
            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
            {
                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null)
                    ControlList.Add(infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString());
            }
            frmInfoSecurityAdd fisa = new frmInfoSecurityAdd(infoS, DesignerHost, ControlList);
            fisa.ShowDialog();
            if (fisa.DialogResult == DialogResult.OK)
            {
                foreach (string controlName in fisa.SelColbList)
                {
                    int rowCount = infoDataGridView1.Rows.Count;
                    rowCount = rowCount - 1;
                    for (int i = 0; i < ict.Components.Count; i++)
                    {
                        if (ict.Components[i] is InfoBindingSource)
                        {
                            string str = ict.Components[i].ToString();
                            string strName = str.Substring(0, str.IndexOf(' '));
                            string strDes = (ict.Components[i] as InfoBindingSource).DataMember;
                            string strType = "InfoBindingSource";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null
                                    && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == strName)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is Panel)
                        {
                            string str = ict.Components[i].ToString();
                            string strName = str.Substring(0, str.IndexOf(' '));
                            string strDes = "";
                            string strType = "Panel";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == strName)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is TabControl)
                        {
                            string str = ict.Components[i].ToString();
                            string strName = str.Substring(0, str.IndexOf(' '));
                            string strDes = "";
                            string strType = "TabControl";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == strName)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is Button)
                        {
                            string str = ict.Components[i].ToString();
                            string strName = str.Substring(0, str.IndexOf(' '));
                            string strDes = (ict.Components[i] as Button).Text;
                            string strType = "Button";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == strName)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is ToolStripButton)
                        {
                            string str = ict.Components[i].Site.Name.ToString();
                            string strDes = "ToolStripButton";
                            string strType = "ToolStripButton";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && str == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == str)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = str;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is InfoSecColumns)
                        {
                            IComponentChangeService ComponentChangeService =
                                    this.DesignerHost.GetService(typeof(IComponentChangeService)) as IComponentChangeService;
                            object oldValue = null;
                            object newValue = null;
                            string str = ict.Components[i].Site.Name.ToString();
                            PropertyDescriptor descName = TypeDescriptor.GetProperties((ict.Components[i] as InfoSecColumns))["Name"];
                            ComponentChangeService.OnComponentChanging((ict.Components[i] as InfoSecColumns), descName);
                            oldValue = (ict.Components[i] as InfoSecColumns).Name;
                            (ict.Components[i] as InfoSecColumns).Name = str;
                            newValue = str;
                            ComponentChangeService.OnComponentChanged((ict.Components[i] as InfoSecColumns), descName, oldValue, newValue);

                            string strDes = "";
                            string strType = "InfoSecColumns";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && str == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == str)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = str;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                       
#if UseCrystalReportDD
                        else if (ict.Components[i] is CrystalReportViewer)
                        {
                            string str = ict.Components[i].Site.Name.ToString();
                            string strDes = "CrystalDecisions.Windows.Forms.CrystalReportViewer";
                            string strType = "CrystalReportViewer";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && str == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = infoS.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = str;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        } 
#endif
                    }

                }
            }
        }

        private void btnRef_Click(object sender, EventArgs e)
        {
            btnAddAll_Click(sender, e);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            int x = 0;
            for (int i = 0; i < infoDataGridView1.Rows.Count; i++)
                for (int j = 0; j < infoDataGridView1.Rows[i].Cells.Count; j++)
                    if (infoDataGridView1.Rows[i].Selected == true || infoDataGridView1.Rows[i].Cells[j].Selected == true)
                    {
                        x = i;
                        break;
                    }
            if (infoDataGridView1.Rows[x].Cells[1].Value != null)
            {
                object[] param = new object[1];
                param[0] = infoS.MenuID + ";" + infoDataGridView1.Rows[x].Cells["ControlName"].Value;
                CliUtils.fLoginDB = infoS.DBAlias;
                object[] myRet = CliUtils.CallMethod("GLModule", "DelMenu", param);
                infoDataGridView1.Rows.Remove(infoDataGridView1.Rows[x]);
                CliUtils.fLoginDB = "";
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                infoS.ExportControls.Clear();

                object[] paramDown = new object[1];
                paramDown[0] = infoS.MenuID;
                CliUtils.fLoginDB = infoS.DBAlias;
                //List<string> listControlName = new List<string>();
                ArrayList listControlName = new ArrayList();
                object[] myRetDown = CliUtils.CallMethod("GLModule", "GetMenu", paramDown);
                if ((myRetDown != null) && (0 == (int)myRetDown[0]))
                {
                    //listControlName = (List<string>)myRetDown[1];
                    listControlName = (ArrayList)myRetDown[1];
                }
                int count = infoDataGridView1.Rows.Count;
                object[] paramUp = new object[count];
                for (int i = 0; i < count; i++)
                {
                    bool flag = false;
                    foreach (string str in listControlName)
                    {
                        if (infoDataGridView1.Rows[i].Cells["ControlName"].Value == null || str == infoDataGridView1.Rows[i].Cells["ControlName"].Value.ToString())
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (flag == false && infoDataGridView1.Rows[i].Cells["ControlName"].Value != null)
                        paramUp[i] = infoDataGridView1.Rows[i].Cells["MenuID"].Value + ";"
                                      + infoDataGridView1.Rows[i].Cells["ControlName"].Value + ";"
                                      + infoDataGridView1.Rows[i].Cells["Descriptions"].Value + ";"
                                      + infoDataGridView1.Rows[i].Cells["Type"].Value;
                    if (infoDataGridView1.Rows[i].Cells["ControlName"].Value != null)
                    {
                        ExportControl ec = new ExportControl();
                        ec.ControlName = infoDataGridView1.Rows[i].Cells["ControlName"].Value == null ? "" : infoDataGridView1.Rows[i].Cells["ControlName"].Value.ToString();
                        ec.Description = infoDataGridView1.Rows[i].Cells["Descriptions"].Value == null ? "" : infoDataGridView1.Rows[i].Cells["Descriptions"].Value.ToString();
                        ec.Type = infoDataGridView1.Rows[i].Cells["Type"].Value == null ? "" : infoDataGridView1.Rows[i].Cells["Type"].Value.ToString();
                        infoS.ExportControls.Add(ec);
                    }
                }
                CliUtils.fLoginDB = infoS.DBAlias;
                object[] myRetUp = CliUtils.CallMethod("GLModule", "InsertToMenu", paramUp);
                object[] paramUpdate = new object[count];
                for (int i = 0; i < count; i++)
                {
                    if (infoDataGridView1.Rows[i].Cells["ControlName"].Value != null)
                        paramUpdate[i] = infoDataGridView1.Rows[i].Cells["MenuID"].Value + ";"
                                      + infoDataGridView1.Rows[i].Cells["ControlName"].Value + ";"
                                      + infoDataGridView1.Rows[i].Cells["Descriptions"].Value + ";"
                                      + infoDataGridView1.Rows[i].Cells["Type"].Value;
                }
                myRetUp = CliUtils.CallMethod("GLModule", "UpdateMenu", paramUpdate);
                CliUtils.fLoginDB = "";
                this.Close();
            }
        }
    }
}