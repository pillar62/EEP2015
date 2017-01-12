using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace EEPSetUpLibrary.Server
{
    public partial class frmSolution : Form
    {
        public frmSolution()
        {
            InitializeComponent();
        }

        private void frmSolution_Load(object sender, EventArgs e)
        {
             XmlDocument xml = new XmlDocument();
             try
             {
                 xml.Load(Config.SolutionFile);
                 SolutionCollection solutions = new SolutionCollection(xml, "Solutions");
                 if(solutions.Count > 0)
                 {
                     dataGridViewSolution.Rows.Add(solutions.Count);
                     for (int i = 0; i < solutions.Count; i++)
                     {
                         Solution sol = solutions[i];
                         dataGridViewSolution.Rows[i].Cells["ColumnName"].Value = sol.Name;
                         dataGridViewSolution.Rows[i].Cells["ColumnText"].Value = sol.Text;
                         dataGridViewSolution.Rows[i].Cells["ColumnIP"].Value = sol.IP;
                         dataGridViewSolution.Rows[i].Cells["ColumnPort"].Value = sol.Port;
                         dataGridViewSolution.Rows[i].Cells["ColumnLoginDataBase"].Value = sol.LoginDataBase;
                         dataGridViewSolution.Rows[i].Cells["ColumnLoginSolution"].Value = sol.LoginSolution;
                         dataGridViewSolution.Rows[i].Cells["ColumnLanguage"].Value = sol.Language.ToString();
                     }
                 }
             }
             catch { }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            SolutionCollection solutions = new SolutionCollection();
            for (int i = 0; i < dataGridViewSolution.Rows.Count; i++)
            {
                if (!dataGridViewSolution.Rows[i].IsNewRow)
                {
                    DataGridViewRow row = dataGridViewSolution.Rows[i];
                    string name = (string)GetCellValue("ColumnName", row);
                    string text = (string)GetCellValue("ColumnText", row);
                    string ip = (string)GetCellValue("ColumnIP", row);
                    int port = (int)GetCellValue("ColumnPort", row);
                    if (name != null && text != null && ip != null)
                    {
                        string logindatabase = (string)row.Cells["ColumnLoginDataBase"].FormattedValue;
                        string loginsolution = (string)row.Cells["ColumnLoginSolution"].FormattedValue;
                        Solution.LanguageType language 
                            = (Solution.LanguageType)Enum.Parse(typeof(Solution.LanguageType), (string)row.Cells["ColumnLanguage"].FormattedValue);
                        Solution sol = new Solution(name, text, ip,port, 8989, logindatabase, loginsolution, language);
                        solutions.Add(sol);
                    }
                    else
                    {
                        DialogResult = DialogResult.None;
                        return;
                    }
                }
            }
            XmlDocument xml = solutions.Save();
            try
            {
                xml.Save(string.Format("{0}\\{1}", Application.StartupPath, Config.SolutionFile));
            }
            catch
            {
                MessageBox.Show(this, "Can not save solution.xml, check it's property not readonly","Error"
                    , MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.None;
            }
        }

        private object GetCellValue(string columnname, DataGridViewRow row)
        {
            if (row.Cells[columnname].Value == null || row.Cells[columnname].ToString().Trim().Length == 0)
            {
                MessageBox.Show(this, string.Format("{0} of solution could not be empty", dataGridViewSolution.Columns[columnname].HeaderText)
                       , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            else
            {
                return row.Cells[columnname].Value;
            }
        }

        private void dataGridViewSolution_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (e.RowIndex > 1)
            {
                if (dataGridViewSolution.Rows[e.RowIndex - 1].Cells["ColumnIP"].Value == null)
                {
                    dataGridViewSolution.Rows[e.RowIndex - 1].Cells["ColumnIP"].Value = dataGridViewSolution.Rows[e.RowIndex - 2].Cells["ColumnIP"].Value;
                }
                if (dataGridViewSolution.Rows[e.RowIndex - 1].Cells["ColumnPort"].Value == null)
                {
                    dataGridViewSolution.Rows[e.RowIndex - 1].Cells["ColumnPort"].Value = dataGridViewSolution.Rows[e.RowIndex - 2].Cells["ColumnPort"].Value;
                }
                if (dataGridViewSolution.Rows[e.RowIndex - 1].Cells["ColumnLanguage"].Value == null)
                {
                    dataGridViewSolution.Rows[e.RowIndex - 1].Cells["ColumnLanguage"].Value = "ENG";
                }
            }
            else if(e.RowIndex == 1)
            {
                if (dataGridViewSolution.Rows[e.RowIndex - 1].Cells["ColumnPort"].Value == null)
                {
                    dataGridViewSolution.Rows[e.RowIndex - 1].Cells["ColumnPort"].Value = Config.ServerPort;
                }
                if (dataGridViewSolution.Rows[e.RowIndex - 1].Cells["ColumnLanguage"].Value == null)
                {
                    dataGridViewSolution.Rows[e.RowIndex - 1].Cells["ColumnLanguage"].Value = "ENG";
                }
            }
        }

        private void dataGridViewSolution_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == dataGridViewSolution.Columns.IndexOf(ColumnName)
                || e.ColumnIndex == dataGridViewSolution.Columns.IndexOf(ColumnText)
                || e.ColumnIndex == dataGridViewSolution.Columns.IndexOf(ColumnIP)
                || e.ColumnIndex == dataGridViewSolution.Columns.IndexOf(ColumnPort))
            {
                if (e.FormattedValue == null || e.FormattedValue.ToString().Trim().Length == 0)
                {
                    MessageBox.Show(this, string.Format("{0} of solution could not be empty", dataGridViewSolution.Columns[e.ColumnIndex].HeaderText)
                        , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
        }
    }
}