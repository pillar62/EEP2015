using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using Srvtools;

namespace EEPManager
{
    public partial class SolutionDefineForm : Form
    {
        public SolutionDefineForm()
        {
            InitializeComponent();
        }

        ArrayList tempOld = new ArrayList();
        private void SolutionDefineForm_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < solutionInfo.RealDataSet.Tables[0].Rows.Count; i++)
                tempOld.Add(solutionInfo.RealDataSet.Tables[0].Rows[i]["ITEMTYPE"]);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (solutionInfo.RealDataSet.Tables[0].GetChanges(DataRowState.Modified) != null)
            {
                int lenth = solutionInfo.RealDataSet.Tables[0].GetChanges(DataRowState.Modified).Rows.Count;
                DataRow[] tempNew = new DataRow[lenth];
                object[] itemType = new object[1];
                for (int i = 0; i < lenth; i++)
                {
                    int x = -1;
                    tempNew[i] = solutionInfo.RealDataSet.Tables[0].GetChanges(DataRowState.Modified).Rows[i];

                    for (int j = 0; j < solutionInfo.RealDataSet.Tables[0].Rows.Count; j++)
                        if (solutionInfo.RealDataSet.Tables[0].Rows[j]["ITEMTYPE"] == tempNew[i]["ITEMTYPE"])
                        {
                            x = solutionInfo.RealDataSet.Tables[0].Rows.IndexOf(solutionInfo.RealDataSet.Tables[0].Rows[j]);
                            break;
                        }
                    if (x != -1)
                    {
                        itemType[0] = tempOld[x] + ";" + solutionInfo.RealDataSet.Tables[0].Rows[x]["ITEMTYPE"];
                        object[] myRet = CliUtils.CallMethod("GLModule", "UpdateMenuTable", itemType);
                    }
                }
            }
            solutionInfo.ApplyUpdates();
            tempOld.Clear();
            for (int i = 0; i < solutionInfo.RealDataSet.Tables[0].Rows.Count; i++)
                tempOld.Add(solutionInfo.RealDataSet.Tables[0].Rows[i]["ITEMTYPE"]);
            MessageBox.Show(this, "Save success", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}