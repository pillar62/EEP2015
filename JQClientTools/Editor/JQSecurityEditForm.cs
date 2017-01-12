using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Collections;
using System.Linq;

namespace JQClientTools
{
    public partial class JQSecurityForm : Form
    {
        public JQSecurityForm()
        {
            InitializeComponent();
        }

        private IContainer ict;
        IDesignerHost DesignerHost = null;
        private JQSecurity jqSecurity;
        public JQSecurityForm(JQSecurity jqs, IDesignerHost host)
        {
            InitializeComponent();
            DesignerHost = host;
            ict = host.Container;
            jqSecurity = jqs;
        }

        private void frmWebSecurity_Load(object sender, EventArgs e)
        {
            EFClientTools.DesignClientUtility.ClientInfo.Database = jqSecurity.DBAlias;
            EFClientTools.DesignClientUtility.ClientInfo.UseDataSet = true;
            var menutableControls = EFClientTools.DesignClientUtility.GetAllDataByTableName("MENUTABLECONTROL").Cast<EFClientTools.EFServerReference.MENUTABLECONTROL>().Where(m => m.MENUID.Equals(jqSecurity.MenuID)).ToList();
            EFClientTools.DesignClientUtility.ClientInfo.Database = String.Empty;
            if (menutableControls != null && menutableControls.Count() > 0)
            {
                infoDataGridView1.Rows.Add(menutableControls.Count());
                for (int i = 0; i < menutableControls.Count(); i++)
                {
                    infoDataGridView1.Rows[i].Cells["MenuID"].Value = menutableControls[i].MENUID;
                    infoDataGridView1.Rows[i].Cells["ControlName"].Value = menutableControls[i].CONTROLNAME;
                    infoDataGridView1.Rows[i].Cells["Descriptions"].Value = menutableControls[i].DESCRIPTION;
                    infoDataGridView1.Rows[i].Cells["Type"].Value = menutableControls[i].TYPE;
                }
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!this.DesignMode)
            {
                EFClientTools.DesignClientUtility.ClientInfo.Database = jqSecurity.DBAlias;
                EFClientTools.DesignClientUtility.ClientInfo.UseDataSet = true;
                int count = infoDataGridView1.Rows.Count;
                for (int i = 0; i < count; i++)
                {
                    if (infoDataGridView1.Rows[i].Cells["ControlName"].Value != null)
                    {
                        List<object> menuIds = new List<object>();
                        EFClientTools.EFServerReference.MENUTABLECONTROL aMENUTABLECONTROL = new EFClientTools.EFServerReference.MENUTABLECONTROL();
                        aMENUTABLECONTROL.MENUID = jqSecurity.MenuID; //infoDataGridView1.Rows[i].Cells["MenuID"].Value.ToString();
                        if (infoDataGridView1.Rows[i].Cells["ControlName"].Value == null)
                        {
                            MessageBox.Show("You have to set the ControlName first");
                            return;
                        }
                        aMENUTABLECONTROL.CONTROLNAME = infoDataGridView1.Rows[i].Cells["ControlName"].Value.ToString();
                        if (infoDataGridView1.Rows[i].Cells["Descriptions"].Value != null)
                            aMENUTABLECONTROL.DESCRIPTION = infoDataGridView1.Rows[i].Cells["Descriptions"].Value != null ? infoDataGridView1.Rows[i].Cells["Descriptions"].Value.ToString() : "";
                        if (infoDataGridView1.Rows[i].Cells["Type"].Value != null)
                            aMENUTABLECONTROL.TYPE = infoDataGridView1.Rows[i].Cells["Type"].Value.ToString();
                        if (String.IsNullOrEmpty(aMENUTABLECONTROL.TYPE))
                            aMENUTABLECONTROL.TYPE = "HTML";
                        menuIds.Add(aMENUTABLECONTROL);
                        EFClientTools.DesignClientUtility.SaveDataToTable(menuIds, "MENUTABLECONTROL");
                    }
                }
                EFClientTools.DesignClientUtility.ClientInfo.Database = String.Empty;

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
                if (ict.Components[i] is JQDataGrid)
                {
                    string strName = ((JQDataGrid)ict.Components[i]).ID;
                    string strDes = (ict.Components[i] as JQDataGrid).DataMember;
                    if (String.IsNullOrEmpty(strDes)) strDes = strName;
                    string strType = "JQDataGrid";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                    foreach (JQToolItem item in (ict.Components[i] as JQDataGrid).TooItems)
                    {
                        if (item.Icon == "icon-add" || item.Icon == "icon-edit" || item.Icon == "icon-remove"
                            || item.Icon == "icon-save" || item.Icon == "icon-undo")
                            continue;
                        strName = ((JQDataGrid)ict.Components[i]).ID;
                        strName = item.Text + "_" + strName;
                        strDes = strName;
                        strType = "JQDataGrid-ToolItem";
                        flag = false;
                        for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                            if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                flag = true;
                        if (flag != true)
                        {
                            infoDataGridView1.Rows.Add(1);
                            infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                            infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                            infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                            infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                            rowCount++;
                        }
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
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
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
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
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
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
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
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
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
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                        infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                        infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                        infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                        rowCount++;
                    }
                }
                else if (ict.Components[i] is JQSecColumns)
                {
                    string strName = ((JQSecColumns)ict.Components[i]).ID;
                    string strDes = "";
                    string strType = "JQSecColumns";
                    bool flag = false;
                    for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                        if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                            flag = true;
                    if (flag != true)
                    {
                        infoDataGridView1.Rows.Add(1);
                        infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
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
            EFClientTools.DesignClientUtility.ClientInfo.Database = jqSecurity.DBAlias;
            EFClientTools.DesignClientUtility.ClientInfo.UseDataSet = true;
            int count = infoDataGridView1.Rows.Count;
            for (int i = 0; i < count; i++)
            {
                for (int j = 0; j < infoDataGridView1.Rows[i].Cells.Count; j++)
                {
                    if (infoDataGridView1.Rows[i].Selected == true || infoDataGridView1.Rows[i].Cells[j].Selected == true)
                    {
                        EFClientTools.EFServerReference.MENUTABLECONTROL aMENUTABLECONTROL = new EFClientTools.EFServerReference.MENUTABLECONTROL();
                        aMENUTABLECONTROL.MENUID = infoDataGridView1.Rows[i].Cells["MenuID"].Value.ToString();
                        aMENUTABLECONTROL.CONTROLNAME = infoDataGridView1.Rows[i].Cells["ControlName"].Value.ToString();
                        //aMENUTABLECONTROL.DESCRIPTION = infoDataGridView1.Rows[i].Cells["Descriptions"].Value.ToString();
                        //aMENUTABLECONTROL.TYPE = infoDataGridView1.Rows[i].Cells["Type"].Value.ToString();
                        EFClientTools.DesignClientUtility.DeleteDataFromTable(aMENUTABLECONTROL, "MENUTABLECONTROL");
                    }
                }
            }
            EFClientTools.DesignClientUtility.ClientInfo.Database = String.Empty;

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
                infoDataGridView1.Rows.Remove(infoDataGridView1.Rows[x]);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ArrayList ControlList = new ArrayList();
            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
            {
                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null)
                    ControlList.Add(infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString());
            }
            JQSecurityAddForm fisa = new JQSecurityAddForm(jqSecurity, DesignerHost, ControlList);
            fisa.ShowDialog();
            if (fisa.DialogResult == DialogResult.OK)
            {
                foreach (string controlName in fisa.SelColbList)
                {
                    int rowCount = infoDataGridView1.Rows.Count;
                    rowCount = rowCount - 1;
                    for (int i = 0; i < ict.Components.Count; i++)
                    {
                        if (ict.Components[i] is JQDataGrid)
                        {
                            string strName = ((JQDataGrid)ict.Components[i]).ID;
                            string strDes = ((JQDataGrid)ict.Components[i]).DataMember;
                            string strType = "JQDataGrid";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == strName)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }

                            foreach (JQToolItem item in (ict.Components[i] as JQDataGrid).TooItems)
                            {
                                if (item.Icon == "icon-add" || item.Icon == "icon-edit" || item.Icon == "icon-remove"
                                    || item.Icon == "icon-save" || item.Icon == "icon-undo")
                                    continue;
                                strName = ((JQDataGrid)ict.Components[i]).ID;
                                strName = item.Text + "_" + strName;
                                strDes = strName;
                                strType = "JQDataGrid-ToolItem";
                                flag = false;
                                for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                    if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                        flag = true;
                                if (flag != true && controlName == strName)
                                {
                                    infoDataGridView1.Rows.Add(1);
                                    infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                                    infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                    infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                    infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                    rowCount++;
                                }
                            }
                        }
                        else if (ict.Components[i] is System.Web.UI.WebControls.Panel)
                        {
                            string strName = ((System.Web.UI.WebControls.Panel)ict.Components[i]).ID;
                            if (controlName != strName)
                                continue;
                            string strDes = "";
                            string strType = "Panel";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == strName)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is System.Web.UI.WebControls.Button)
                        {
                            string strName = ((System.Web.UI.WebControls.Button)ict.Components[i]).ID;
                            if (controlName != strName)
                                continue;
                            string strDes = ((System.Web.UI.WebControls.Button)ict.Components[i]).Text;
                            string strType = "Button";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true && controlName == strName)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is System.Web.UI.WebControls.ImageButton)
                        {
                            string strName = ((System.Web.UI.WebControls.ImageButton)ict.Components[i]).ID;
                            if (controlName != strName)
                                continue;
                            string strDes = "";
                            string strType = "ImageButton";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is System.Web.UI.WebControls.LinkButton)
                        {
                            string strName = ((System.Web.UI.WebControls.LinkButton)ict.Components[i]).ID;
                            if (controlName != strName)
                                continue;
                            string strDes = ((System.Web.UI.WebControls.LinkButton)ict.Components[i]).Text;
                            string strType = "LinkButton";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is System.Web.UI.WebControls.HyperLink)
                        {
                            string strName = ((System.Web.UI.WebControls.HyperLink)ict.Components[i]).ID;
                            if (controlName != strName)
                                continue;
                            string strDes = ((System.Web.UI.WebControls.HyperLink)ict.Components[i]).Text;
                            string strType = "LinkButton";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
                        else if (ict.Components[i] is JQSecColumns)
                        {
                            string strName = ((JQSecColumns)ict.Components[i]).ID;
                            if (controlName != strName)
                                continue;
                            string strDes = "";
                            string strType = "JQSecColumns";
                            bool flag = false;
                            for (int j = 0; j < infoDataGridView1.Rows.Count; j++)
                                if (infoDataGridView1.Rows[j].Cells["ControlName"].Value != null && strName == infoDataGridView1.Rows[j].Cells["ControlName"].Value.ToString())
                                    flag = true;
                            if (flag != true)
                            {
                                infoDataGridView1.Rows.Add(1);
                                infoDataGridView1.Rows[rowCount].Cells["MenuID"].Value = jqSecurity.MenuID;
                                infoDataGridView1.Rows[rowCount].Cells["ControlName"].Value = strName;
                                infoDataGridView1.Rows[rowCount].Cells["Descriptions"].Value = strDes;
                                infoDataGridView1.Rows[rowCount].Cells["Type"].Value = strType;
                                rowCount++;
                            }
                        }
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