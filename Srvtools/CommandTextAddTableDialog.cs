using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Srvtools
{
    public partial class CommandTextAddTableDialog : Form
    {
        private List<String> _selectedTablesList = new List<string>();

        public CommandTextAddTableDialog(List<String> tablesList, List<String> tablesCaptionList)
        {
            InitializeComponent();
            //ListTables.DataSource = tablesList;
            ListView LV = ListTables;
            LV.Items.Clear();

            if (tablesList.Count > 0)
            {
                LV.BeginUpdate();
                for (int I = 0; I < tablesList.Count; I++)
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Text = tablesList[I].ToString();
                    //lvi.SubItems[""] = "";
                    LV.Items.Add(lvi);
                    String tableName = lvi.Text;
                    if (tableName.Contains("."))
                        tableName = tableName.Split('.')[1];
                    lvi.SubItems.Add(Values(tablesCaptionList, tableName));
                    //lvi.Selected = lvi.Text.CompareTo(FTableName) == 0;
                }
                LV.EndUpdate();
            }

            LV.Sort();
        }

        private string Values(List<String> tablesCaptionList, string ParamName)
        {
            string Result = "", TempName, S;
            int I, J;
            for (I = 0; I < tablesCaptionList.Count; I++) //???
            {
                S = tablesCaptionList[I].ToString();
                for (J = 0; J < S.Length; J++)
                {
                    if (S[J].ToString() == "=")
                    {
                        TempName = S.Remove(J);
                        if (string.Compare(ParamName, TempName) == 0)
                        {
                            if (J < S.Length - 1)
                            {
                                Result = S.Remove(0, J + 1);
                            }
                            break;
                        }
                    }
                }
            }
            return Result;
        }


        public List<String> SelectedTables
        { 
            get { return _selectedTablesList; } 
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ListTables.SelectedItems == null)
            { }
            else
            {
                foreach (ListViewItem i in ListTables.SelectedItems)
                {
                    _selectedTablesList.Add(i.Text);
                }
            }
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ListTables_DoubleClick(object sender, EventArgs e)
        {
            btnOK_Click(btnOK, null);
        }
    }
}