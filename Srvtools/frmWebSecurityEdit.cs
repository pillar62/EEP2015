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
using CrystalDecisions.Web;
#endif

namespace Srvtools
{
    public partial class frmWebSecurity : Form
    {
        public frmWebSecurity()
        {
            InitializeComponent();
        }

        //public ClientQuery cqeditor = new ClientQuery();
        private IContainer ict;
        IDesignerHost DesignerHost = null;
        private WebSecurity ws;
        public frmWebSecurity(WebSecurity isy, IDesignerHost host)
        {
            InitializeComponent();
            DesignerHost = host;
            ict = host.Container;
            ws = isy;
            //InitializeColumn(infoS);

        }

        private void frmWebSecurity_Load(object sender, EventArgs e)
        {
            object[] param = new object[1];
            param[0] = ws.MenuID;
            CliUtils.fLoginDB = ws.DBAlias;
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
                            infoDataGridView1.Rows[i].Cells["MenuID"].Value = ws.MenuID;
                            infoDataGridView1.Rows[i].Cells["ControlName"].Value = listControlName[i];
                            infoDataGridView1.Rows[i].Cells["Descriptions"].Value = listDescription[i];
                            infoDataGridView1.Rows[i].Cells["Type"].Value = listType[i];
                        }
                    }
                }
            }
            CliUtils.fLoginDB = "";
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                if (ws.webExportControls != null)
                    ws.webExportControls.Clear();

                object[] paramDown = new object[1];
                paramDown[0] = ws.MenuID;
                CliUtils.fLoginDB = ws.DBAlias;
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
                        if (infoDataGridView1.Rows[i].Cells["ControlName"].Value == null || infoDataGridView1.Rows[i].Cells["ControlName"].Value.ToString() == "" || str == infoDataGridView1.Rows[i].Cells["ControlName"].Value.ToString())
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
                    if (infoDataGridView1.Rows[i].Cells["ControlName"].Value != null && infoDataGridView1.Rows[i].Cells["ControlName"].Value.ToString() != "")
                    {
                        WebExportControl wec = new WebExportControl();
                        wec.ControlName = infoDataGridView1.Rows[i].Cells["ControlName"].Value == null ? "" : infoDataGridView1.Rows[i].Cells["ControlName"].Value.ToString();
                        wec.Description = infoDataGridView1.Rows[i].Cells["Descriptions"].Value == null ? "" : infoDataGridView1.Rows[i].Cells["Descriptions"].Value.ToString();
                        wec.Type = infoDataGridView1.Rows[i].Cells["Type"].Value == null ? "" : infoDataGridView1.Rows[i].Cells["Type"].Value.ToString();
                        ws.webExportControls.Add(wec);
                    }
                }
                CliUtils.fLoginDB = ws.DBAlias;
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
                if (ict.Components[i] is WebDataSource)
                {
                    string strName = ((WebDataSource)ict.Components[i]).ID;
                    string strDes = (ict.Components[i] as WebDataSource).DataMember;
                    string strType = "WebDataSource";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is System.Web.UI.WebControls.Panel)
                {
                    string strName = ((System.Web.UI.WebControls.Panel)ict.Components[i]).ID;
                    string strDes = "";
                    string strType = "Panel";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is System.Web.UI.WebControls.Button)
                {
                    string strName = ((System.Web.UI.WebControls.Button)ict.Components[i]).ID;
                    string strDes = ((System.Web.UI.WebControls.Button)ict.Components[i]).Text;
                    string strType = "Button";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is System.Web.UI.WebControls.ImageButton)
                {
                    string strName = ((System.Web.UI.WebControls.ImageButton)ict.Components[i]).ID;
                    string strDes = "";
                    string strType = "ImageButton";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is System.Web.UI.WebControls.LinkButton)
                {
                    string strName = ((System.Web.UI.WebControls.LinkButton)ict.Components[i]).ID;
                    string strDes = ((System.Web.UI.WebControls.LinkButton)ict.Components[i]).Text;
                    string strType = "LinkButton";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is System.Web.UI.WebControls.HyperLink)
                {
                    string strName = ((System.Web.UI.WebControls.HyperLink)ict.Components[i]).ID;
                    string strDes = ((System.Web.UI.WebControls.HyperLink)ict.Components[i]).Text;
                    string strType = "LinkButton";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is WebSecColumns)
                {
                    string strName = ((WebSecColumns)ict.Components[i]).ID;
                    string strDes = "";
                    string strType = "WebSecColumns";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }

#if UseCrystalReportDD
                else if (ict.Components[i] is CrystalReportViewer)
                {
                    string strName = ((CrystalReportViewer)ict.Components[i]).ID;
                    string strDes = "CrystalDecisions.Web.CrystalReportViewer";
                    string strType = "CrystalReportViewer";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }  
#endif
                else if (ict.Components[i].GetType().Name == "WebEasilyReport")
                {
                    string strName = ((System.Web.UI.WebControls.WebControl)ict.Components[i]).ID;
                    string strDes = "";
                    string strType = "WebEasilyReport";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i].GetType().Name == "ExtSecColumns")
                {
                    string strName = ict.Components[i].GetType().GetProperty("ID").GetValue(ict.Components[i], null).ToString();
                    string strDes = "";
                    string strType = "ExtSecColumns";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i].GetType().Name == "AjaxSecColumns")
                {
                    string strName = ict.Components[i].GetType().GetProperty("ID").GetValue(ict.Components[i], null).ToString();
                    string strDes = "";
                    string strType = "AjaxSecColumns";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
            }
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
            if (infoDataGridView1.Rows[x].Cells["ControlName"].Value != null)
            {
                object[] param = new object[1];
                param[0] = ws.MenuID + ";" + infoDataGridView1.Rows[x].Cells["ControlName"].Value;
                CliUtils.fLoginDB = ws.DBAlias;
                object[] myRet = CliUtils.CallMethod("GLModule", "DelMenu", param);
                infoDataGridView1.Rows.Remove(infoDataGridView1.Rows[x]);
                CliUtils.fLoginDB = "";
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
            frmWebSecurityAdd fisa = new frmWebSecurityAdd(ws, DesignerHost, ControlList);
            fisa.ShowDialog();
            if (fisa.DialogResult == DialogResult.OK)
            {
                foreach (string controlName in fisa.SelColbList)
                {
                    int rowCount = infoDataGridView1.Rows.Count;
                    rowCount = rowCount - 1;
                    for (int i = 0; i < ict.Components.Count; i++)
                    {
                        if (ict.Components[i] is WebDataSource)
                        {
                            string strName = ((WebDataSource)ict.Components[i]).ID;
                            string strDes = ((WebDataSource)ict.Components[i]).DataMember;
                            string strType = "WebDataSource";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == strName)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is System.Web.UI.WebControls.Panel)
                        {
                            string strName = ((System.Web.UI.WebControls.Panel)ict.Components[i]).ID;
                            string strDes = "";
                            string strType = "Panel";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == strName)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is System.Web.UI.WebControls.Button)
                        {
                            string strName = ((System.Web.UI.WebControls.Button)ict.Components[i]).ID;
                            string strDes = ((System.Web.UI.WebControls.Button)ict.Components[i]).Text;
                            string strType = "Button";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == strName)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is System.Web.UI.WebControls.ImageButton)
                        {
                            string strName = ((System.Web.UI.WebControls.ImageButton)ict.Components[i]).ID;
                            string strDes = "";
                            string strType = "ImageButton";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is System.Web.UI.WebControls.LinkButton)
                        {
                            string strName = ((System.Web.UI.WebControls.LinkButton)ict.Components[i]).ID;
                            string strDes = ((System.Web.UI.WebControls.LinkButton)ict.Components[i]).Text;
                            string strType = "LinkButton";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is System.Web.UI.WebControls.HyperLink)
                        {
                            string strName = ((System.Web.UI.WebControls.HyperLink)ict.Components[i]).ID;
                            string strDes = ((System.Web.UI.WebControls.HyperLink)ict.Components[i]).Text;
                            string strType = "LinkButton";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is WebSecColumns)
                        {
                            string strName = ((WebSecColumns)ict.Components[i]).ID;
                            string strDes = "";
                            string strType = "WebSecColumns";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i].GetType().Name == "ExtSecColumns")
                        {
                            string strName = ict.Components[i].GetType().GetProperty("ID").GetValue(ict.Components[i], null).ToString();
                            string strDes = "";
                            string strType = "ExtSecColumns";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i].GetType().Name == "AjaxSecColumns")
                        {
                            string strName = ict.Components[i].GetType().GetProperty("ID").GetValue(ict.Components[i], null).ToString();
                            string strDes = "";
                            string strType = "AjaxSecColumns";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
#if UseCrystalReportDD
                        else if (ict.Components[i] is CrystalReportViewer)
                        {
                            string strName = ((CrystalReportViewer)ict.Components[i]).ID;
                            string strDes = "CrystalDecisions.Web.CrystalReportViewer";
                            string strType = "CrystalReportViewer";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = ws.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
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
    }
}