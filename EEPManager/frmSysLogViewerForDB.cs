using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;
using Microsoft.Win32;
using System.IO;
using System.Threading;

namespace EEPManager
{
    public partial class frmSysLogViewerForDB : Form
    {
        public frmSysLogViewerForDB()
        {
            InitializeComponent();
        }

        private bool bAbort = false;
        private DataTable tabFileLog = null;
        private DataTable tabDBLog = null;
        private DataTable tabSqlLog = null;

        private void frmSysLogViewerForDB_Load(object sender, EventArgs e)
        {
            DataTable tabUsers = CliUtils.ExecuteSql("GLModule", "cmdDDUse", "select USERID, USERNAME from USERS", true, CliUtils.fCurrentProject).Tables[0];
            DataRow userRow = tabUsers.NewRow();
            userRow["USERID"] = "All";
            userRow["USERNAME"] = "All";
            tabUsers.Rows.InsertAt(userRow, 0);

            this.loadFileLog(tabUsers);
            this.loadDBLog(tabUsers);
            this.loadSqlLog(tabUsers);
        }

        private void loadFileLog(DataTable tabUsers)
        {
            this.tView.ExpandAll();
            setSearchCompEnabled(false);

            this.cmbUserId.DataSource = tabUsers;
            this.cmbUserId.ValueMember = "USERID";
            this.cmbUserId.DisplayMember = "USERNAME";

            this.cmbLogStyle.SelectedIndex = 0;
            this.cmbLogType.SelectedIndex = 0;
        }

        private void loadDBLog(DataTable tabUsers)
        {
            this.tViewDB.ExpandAll();
            setDBSearchCompEnabled(false);

            this.cmbDBUserId.DataSource = tabUsers;
            this.cmbDBUserId.ValueMember = "USERID";
            this.cmbDBUserId.DisplayMember = "USERNAME";

            this.cmbDBLogStyle.SelectedIndex = 0;
            this.cmbDBLogType.SelectedIndex = 0;
        }

        private void loadSqlLog(DataTable tabUsers)
        {
            this.tViewSql.ExpandAll();
            setSqlSearchCompEnabled(false);

            this.cmbSqlUserId.DataSource = tabUsers;
            this.cmbSqlUserId.ValueMember = "USERID";
            this.cmbSqlUserId.DisplayMember = "USERNAME";

            this.cmbSqlLogType.SelectedIndex = 0;
        }

        private void setSearchCompEnabled(bool enabled)
        {
            this.panFileSearch.Enabled = enabled;
        }

        private void setDBSearchCompEnabled(bool enabled)
        {
            this.panel5.Enabled = enabled;
        }

        private void setSqlSearchCompEnabled(bool enabled)
        {
            this.panel8.Enabled = enabled;
        }

        private void FileLogTable(string startDate, string endDate, bool exportError)
        {
            tabFileLog = new DataTable();
            object[] myRet = CliUtils.CallMethod("GLModule", "GetLogDatas", new object[] { false, startDate, endDate, exportError });
            if (myRet[0].ToString() == "0")
            {
                tabFileLog = myRet[1] as DataTable;
            }
            tvs(this.tView.SelectedNode);
        }

        private void DBLogTable(DateTime startDate, DateTime endDate, bool exportError)
        {
            DateTime sDate = startDate;
            DateTime eDate = (endDate == DateTime.MaxValue) ? endDate : endDate.AddDays(1);

            string sqlDBLog = "select CONNID,LOGSTYLE,LOGDATETIME,DOMAINID,USERID,LOGTYPE,TITLE,DESCRIPTION,COMPUTERIP,COMPUTERNAME,EXECUTIONTIME from SYSEEPLOG where (CONNID in (select CONNID from SYSEEPLOG group by CONNID having count(*) = 1)" + (exportError ? "" : " and LOGTYPE<>'2'") + " or LOGTYPE <> '2') ";

            String dbType = "";
            object[] myRet = CliUtils.CallMethod("GLModule", "GetDataBaseType", new object[] { CliUtils.fLoginDB });
            if (myRet != null && myRet[0].ToString() == "0")
            {
                dbType = myRet[1].ToString();
            }

            if (sDate != DateTime.MinValue)
            {
                switch (dbType)
                {
                    case "1":
                        sqlDBLog += " and LOGDATETIME >= '" + sDate.ToString("yyyy/MM/dd") + " " + sDate.Hour.ToString("00") + ":" + sDate.Minute.ToString("00") + ":" + sDate.Second.ToString("00") + "'";
                        break;
                    case "2":
                        sqlDBLog += " and LOGDATETIME >= '" + sDate.ToString("yyyy/MM/dd") + " " + sDate.Hour.ToString("00") + ":" + sDate.Minute.ToString("00") + ":" + sDate.Second.ToString("00") + "'";
                        break;
                    case "3":
                        sqlDBLog += " and LOGDATETIME >= to_date('" + sDate.ToString("yyyy/MM/dd") + " " + sDate.Hour.ToString("00") + ":" + sDate.Minute.ToString("00") + ":" + sDate.Second.ToString("00") + "', 'yyyy/mm/dd hh24:mi:ss')";
                        break;
                    case "4":
                        sqlDBLog += " and LOGDATETIME >= '" + sDate.ToString("yyyy/MM/dd") + " " + sDate.Hour.ToString("00") + ":" + sDate.Minute.ToString("00") + ":" + sDate.Second.ToString("00") + "'";
                        break;
                    case "5":
                        sqlDBLog += " and LOGDATETIME >= '" + sDate.ToString("yyyy/MM/dd") + " " + sDate.Hour.ToString("00") + ":" + sDate.Minute.ToString("00") + ":" + sDate.Second.ToString("00") + "'";
                        break;
                }
            }
            if (eDate != DateTime.MaxValue)
            {
                switch (dbType)
                {
                    case "1":
                        sqlDBLog += " and LOGDATETIME < '" + eDate.ToString("yyyy/MM/dd") + " " + eDate.Hour.ToString("00") + ":" + eDate.Minute.ToString("00") + ":" + eDate.Second.ToString("00") + "'";
                        break;
                    case "2":
                        sqlDBLog += " and LOGDATETIME < '" + eDate.ToString("yyyy/MM/dd") + " " + eDate.Hour.ToString("00") + ":" + eDate.Minute.ToString("00") + ":" + eDate.Second.ToString("00") + "'";
                        break;
                    case "3":
                        sqlDBLog += " and LOGDATETIME < to_date( '" + eDate.ToString("yyyy/MM/dd") + " " + eDate.Hour.ToString("00") + ":" + eDate.Minute.ToString("00") + ":" + eDate.Second.ToString("00") + "', 'yyyy/mm/dd hh24:mi:ss')";
                        break;
                    case "4":
                        sqlDBLog += " and LOGDATETIME < '" + eDate.ToString("yyyy/MM/dd") + " " + eDate.Hour.ToString("00") + ":" + eDate.Minute.ToString("00") + ":" + eDate.Second.ToString("00") + "'";
                        break;
                    case "5":
                        sqlDBLog += " and LOGDATETIME < '" + eDate.ToString("yyyy/MM/dd") + " " + eDate.Hour.ToString("00") + ":" + eDate.Minute.ToString("00") + ":" + eDate.Second.ToString("00") + "'";
                        break;
                }
            }
            object[] ret = CliUtils.CallMethod("GLModule", "GetDBLog", new object[] { sqlDBLog });
            if(ret != null && (int)ret[0] == 0)
            {
                tabDBLog = ((DataSet)ret[1]).Tables[0];
            }
            dtvs(this.tViewDB.SelectedNode);
        }

        private void SqlLogTable(string startDate, string endDate)
        {
            tabSqlLog = new DataTable();
            object[] myRet = CliUtils.CallMethod("GLModule", "GetLogDatas", new object[] { true, startDate, endDate });
            if (myRet[0].ToString() == "0")
            {
                tabSqlLog = myRet[1] as DataTable;
            }
            stvs(this.tViewSql.SelectedNode);
        }

        private void tView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tvs(e.Node);
        }

        private void tvs(TreeNode node)
        {
            if (tabFileLog == null || node == null) return;
            DataTable tb = new DataTable();
            if (node.Name == "nEEPLog")
            {
                tb = tabLogCount();
            }
            else if (node.Name == "nAllLog")
            {
                tb = tabFileLog;
            }
            else
            {
                tb = tabFileLog.Clone();
                DataRow[] rows = null;
                switch (node.Name)
                {
                    case "nSystem":
                        rows = tabFileLog.Select("LOGSTYLE='0'");
                        break;
                    case "nAccessProvider":
                        rows = tabFileLog.Select("LOGSTYLE='1'");
                        break;
                    case "nCallMethod":
                        rows = tabFileLog.Select("LOGSTYLE='2'");
                        break;
                    case "nWarning":
                        rows = tabFileLog.Select("LOGTYPE='1'");
                        break;
                    case "nError":
                        rows = tabFileLog.Select("LOGTYPE='3'");
                        break;
                    case "nUnknown":
                        rows = tabFileLog.Select("LOGTYPE='2'");
                        break;
                    case "nUserDefine":
                        rows = tabFileLog.Select("LOGSTYLE='3'");
                        break;
                    case "nEmail":
                        rows = tabFileLog.Select("LOGSTYLE='4'");
                        break;
                }
                if (rows != null)
                {
                    foreach (DataRow row in rows)
                    {
                        tb.ImportRow(row);
                    }
                }
            }
            this.dgvFileLog.DataSource = tb;
        }

        private void tViewDB_AfterSelect(object sender, TreeViewEventArgs e)
        {
            dtvs(e.Node);
        }

        private void dtvs(TreeNode node)
        {
            if (tabDBLog == null || node == null) return;
            DataTable tb = new DataTable();
            if (node.Name == "nEEPLog")
            {
                tb = tabDBLogCount();
            }
            else if (node.Name == "nAllLog")
            {
                tb = tabDBLog;
            }
            else
            {
                tb = tabDBLog.Clone();
                DataRow[] rows = null;
                switch (node.Name)
                {
                    case "nSystem":
                        rows = tabDBLog.Select("LOGSTYLE='0'");
                        break;
                    case "nAccessProvider":
                        rows = tabDBLog.Select("LOGSTYLE='1'");
                        break;
                    case "nCallMethod":
                        rows = tabDBLog.Select("LOGSTYLE='2'");
                        break;
                    case "nWarning":
                        rows = tabDBLog.Select("LOGTYPE='1'");
                        break;
                    case "nError":
                        rows = tabDBLog.Select("LOGTYPE='3'");
                        break;
                    case "nUnknown":
                        rows = tabDBLog.Select("LOGTYPE='2'");
                        break;
                    case "nUserDefine":
                        rows = tabDBLog.Select("LOGSTYLE='3'");
                        break;
                    case "nEmail":
                        rows = tabDBLog.Select("LOGSTYLE='4'");
                        break;
                }
                if (rows != null)
                {
                    foreach (DataRow row in rows)
                    {
                        tb.ImportRow(row);
                    }
                }
            }
            this.dgvDBLog.DataSource = tb;
        }

        private void tViewSql_AfterSelect(object sender, TreeViewEventArgs e)
        {
            stvs(e.Node);
        }

        private void stvs(TreeNode node)
        {
            if (tabSqlLog == null || node == null) return;
            DataTable tb = new DataTable();
            if (node.Name == "nEEPLog")
            {
                tb = tabSqlLogCount();
            }
            else if (node.Name == "nAllLog")
            {
                tb = tabSqlLog;
            }
            else
            {
                tb = tabSqlLog.Clone();
                DataRow[] rows = null;
                switch (node.Name)
                {
                    case "nExecuteSql":
                        rows = tabSqlLog.Select("SQLLOGTYPE='0'");
                        break;
                    case "nInfoCommand":
                        rows = tabSqlLog.Select("SQLLOGTYPE='1'");
                        break;
                    case "nUpdateComp":
                        rows = tabSqlLog.Select("SQLLOGTYPE='2'");
                        break;
                }
                if (rows != null)
                {
                    foreach (DataRow row in rows)
                    {
                        tb.ImportRow(row);
                    }
                }
            }
            this.dgvSqlLog.DataSource = tb;
            if (this.dgvSqlLog.Columns.Count == 5)
            {
                this.dgvSqlLog.Columns.RemoveAt(4);
                this.dgvSqlLog.Columns[1].Width = 150;
                this.dgvSqlLog.Columns[3].Width = 200;
                Binding binding = new Binding("Text", tb, "SqlSentence");
                this.txtSqlSentence.DataBindings.Clear();
                this.txtSqlSentence.DataBindings.Add(binding);
                if (tb.Rows.Count == 0)
                    this.txtSqlSentence.Text = string.Empty;

            }
            else
            {
                this.txtSqlSentence.Text = string.Empty;
            }
        }

        private DataTable tabLogCount()
        {
            DataTable tabLogCount = new DataTable();
            DataColumn colLogGroup = new DataColumn("LOGGROUP", typeof(string));
            DataColumn colLogCount = new DataColumn("LOGCOUNT", typeof(int));
            tabLogCount.Columns.AddRange(new DataColumn[] { colLogGroup, colLogCount });

            DataRow rowAllLog = tabLogCount.NewRow();
            rowAllLog["LOGGROUP"] = "All Log";
            rowAllLog["LOGCOUNT"] = tabFileLog.Rows.Count;
            tabLogCount.Rows.Add(rowAllLog);

            DataRow rowSystem = tabLogCount.NewRow();
            rowSystem["LOGGROUP"] = "System";
            rowSystem["LOGCOUNT"] = tabFileLog.Select("LOGSTYLE='0'").Length;
            tabLogCount.Rows.Add(rowSystem);

            DataRow rowAccessProvider = tabLogCount.NewRow();
            rowAccessProvider["LOGGROUP"] = "Access Provider";
            rowAccessProvider["LOGCOUNT"] = tabFileLog.Select("LOGSTYLE='1'").Length;
            tabLogCount.Rows.Add(rowAccessProvider);

            DataRow rowCallMethod = tabLogCount.NewRow();
            rowCallMethod["LOGGROUP"] = "Call Method";
            rowCallMethod["LOGCOUNT"] = tabFileLog.Select("LOGSTYLE='2'").Length;
            tabLogCount.Rows.Add(rowCallMethod);

            DataRow rowWarning = tabLogCount.NewRow();
            rowWarning["LOGGROUP"] = "Warning";
            rowWarning["LOGCOUNT"] = tabFileLog.Select("LOGTYPE='1'").Length;
            tabLogCount.Rows.Add(rowWarning);

            DataRow rowError = tabLogCount.NewRow();
            rowError["LOGGROUP"] = "Error";
            rowError["LOGCOUNT"] = tabFileLog.Select("LOGTYPE='3'").Length;
            tabLogCount.Rows.Add(rowError);

            DataRow rowUnknown = tabLogCount.NewRow();
            rowUnknown["LOGGROUP"] = "Unknown";
            rowUnknown["LOGCOUNT"] = tabFileLog.Select("LOGTYPE='2'").Length;
            tabLogCount.Rows.Add(rowUnknown);

            DataRow rowUserDefine = tabLogCount.NewRow();
            rowUserDefine["LOGGROUP"] = "UserDefine";
            rowUserDefine["LOGCOUNT"] = tabFileLog.Select("LOGSTYLE='3'").Length;
            tabLogCount.Rows.Add(rowUserDefine);

            return tabLogCount;
        }

        private DataTable tabDBLogCount()
        {
            DataTable tabLogCount = new DataTable();
            DataColumn colLogGroup = new DataColumn("LOGGROUP", typeof(string));
            DataColumn colLogCount = new DataColumn("LOGCOUNT", typeof(int));
            tabLogCount.Columns.AddRange(new DataColumn[] { colLogGroup, colLogCount });

            DataRow rowAllLog = tabLogCount.NewRow();
            rowAllLog["LOGGROUP"] = "All Log";
            rowAllLog["LOGCOUNT"] = tabDBLog.Rows.Count;
            tabLogCount.Rows.Add(rowAllLog);

            DataRow rowSystem = tabLogCount.NewRow();
            rowSystem["LOGGROUP"] = "System";
            rowSystem["LOGCOUNT"] = tabDBLog.Select("LOGSTYLE='0'").Length;
            tabLogCount.Rows.Add(rowSystem);

            DataRow rowAccessProvider = tabLogCount.NewRow();
            rowAccessProvider["LOGGROUP"] = "Access Provider";
            rowAccessProvider["LOGCOUNT"] = tabDBLog.Select("LOGSTYLE='1'").Length;
            tabLogCount.Rows.Add(rowAccessProvider);

            DataRow rowCallMethod = tabLogCount.NewRow();
            rowCallMethod["LOGGROUP"] = "Call Method";
            rowCallMethod["LOGCOUNT"] = tabDBLog.Select("LOGSTYLE='2'").Length;
            tabLogCount.Rows.Add(rowCallMethod);

            DataRow rowWarning = tabLogCount.NewRow();
            rowWarning["LOGGROUP"] = "Warning";
            rowWarning["LOGCOUNT"] = tabDBLog.Select("LOGTYPE='1'").Length;
            tabLogCount.Rows.Add(rowWarning);

            DataRow rowError = tabLogCount.NewRow();
            rowError["LOGGROUP"] = "Error";
            rowError["LOGCOUNT"] = tabDBLog.Select("LOGTYPE='3'").Length;
            tabLogCount.Rows.Add(rowError);

            DataRow rowUnknown = tabLogCount.NewRow();
            rowUnknown["LOGGROUP"] = "Unknown";
            rowUnknown["LOGCOUNT"] = tabDBLog.Select("LOGTYPE='2'").Length;
            tabLogCount.Rows.Add(rowUnknown);

            DataRow rowUserDefine = tabLogCount.NewRow();
            rowUserDefine["LOGGROUP"] = "UserDefine";
            rowUserDefine["LOGCOUNT"] = tabDBLog.Select("LOGSTYLE='3'").Length;
            tabLogCount.Rows.Add(rowUserDefine);

            return tabLogCount;
        }

        private DataTable tabSqlLogCount()
        {
            DataTable tabLogCount = new DataTable();
            DataColumn colLogGroup = new DataColumn("LOGGROUP", typeof(string));
            DataColumn colLogCount = new DataColumn("LOGCOUNT", typeof(int));
            tabLogCount.Columns.AddRange(new DataColumn[] { colLogGroup, colLogCount });

            DataRow rowAllLog = tabLogCount.NewRow();
            rowAllLog["LOGGROUP"] = "All Log";
            rowAllLog["LOGCOUNT"] = tabSqlLog.Rows.Count;
            tabLogCount.Rows.Add(rowAllLog);

            DataRow rowExecuteSql = tabLogCount.NewRow();
            rowExecuteSql["LOGGROUP"] = "ExecuteSql";
            rowExecuteSql["LOGCOUNT"] = tabSqlLog.Select("SQLLOGTYPE='0'").Length;
            tabLogCount.Rows.Add(rowExecuteSql);

            DataRow rowInfoCommand = tabLogCount.NewRow();
            rowInfoCommand["LOGGROUP"] = "InfoCommand";
            rowInfoCommand["LOGCOUNT"] = tabSqlLog.Select("SQLLOGTYPE='1'").Length;
            tabLogCount.Rows.Add(rowInfoCommand);

            DataRow rowUpdateComp = tabLogCount.NewRow();
            rowUpdateComp["LOGGROUP"] = "UpdateComp";
            rowUpdateComp["LOGCOUNT"] = tabSqlLog.Select("SQLLOGTYPE='2'").Length;
            tabLogCount.Rows.Add(rowUpdateComp);

            return tabLogCount;
        }

        #region Search
        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataTable tb = tabFileLog.Clone();
            string userid = this.cmbUserId.SelectedValue.ToString();
            string logStyle = (this.cmbLogStyle.SelectedIndex - 1).ToString();
            string logType = (this.cmbLogType.SelectedIndex - 1).ToString();
            string filter = "";
            if (userid != null && userid != "" && userid != "All")
            {
                filter += "USERID='" + userid + "' and ";
            }
            if (logStyle != null && logStyle != "" && logStyle != "-1")
            {
                filter += "LOGSTYLE='" + logStyle + "' and ";
            }
            if (logType != null && logType != "" && logType != "-1")
            {
                filter += "LOGTYPE='" + logType + "' and ";
            }
            if (filter.EndsWith(" and "))
                filter = filter.Substring(0, filter.LastIndexOf(" and "));
            //string from = DateTime.MinValue.ToString();
            //string to = DateTime.MaxValue.ToString();
            //if (this.dtpFrom.Checked)
            //{
            //    from = this.dtpFrom.Value.Date.ToString();
            //}
            //if (this.dtpTo.Checked)
            //{
            //    to = this.dtpTo.Value.Date.AddDays(1).AddSeconds(-1).ToString();
            //}
            //filter += "LOGDATETIME>'" + from + "' and LOGDATETIME<'" + to + "'";
            DataRow[] rows = tabFileLog.Select(filter);
            foreach (DataRow row in rows)
            {
                tb.ImportRow(row);
            }
            this.dgvFileLog.DataSource = tb;
        }

        private void btnDBSearch_Click(object sender, EventArgs e)
        {
            DataTable tb = tabDBLog.Clone();
            string userid = this.cmbDBUserId.SelectedValue.ToString();
            string logStyle = (this.cmbDBLogStyle.SelectedIndex - 1).ToString();
            string logType = (this.cmbDBLogType.SelectedIndex - 1).ToString();
            string filter = "";
            if (userid != null && userid != "" && userid != "All")
            {
                filter += "USERID='" + userid + "' and ";
            }
            if (logStyle != null && logStyle != "" && logStyle != "-1")
            {
                filter += "LOGSTYLE='" + logStyle + "' and ";
            }
            if (logType != null && logType != "" && logType != "-1")
            {
                filter += "LOGTYPE='" + logType + "' and ";
            }
            //string from = DateTime.MinValue.ToString();
            //string to = DateTime.MaxValue.ToString();
            //if (this.dtpFrom.Checked)
            //{
            //    from = this.dtpDBFrom.Value.Date.ToString();
            //}
            //if (this.dtpTo.Checked)
            //{
            //    to = this.dtpDBTo.Value.Date.AddDays(1).AddSeconds(-1).ToString();
            //}
            //filter += "LOGDATETIME>'" + from + "' and LOGDATETIME<'" + to + "'";

            if (filter.EndsWith("and "))
                filter = filter.Remove(filter.LastIndexOf("and "));
            DataRow[] rows = tabDBLog.Select(filter);
            foreach (DataRow row in rows)
            {
                tb.ImportRow(row);
            }
            this.dgvDBLog.DataSource = tb;
        }

        private void btnSqlSearch_Click(object sender, EventArgs e)
        {
            DataTable tb = tabSqlLog.Clone();
            string userid = this.cmbSqlUserId.SelectedValue.ToString();
            string logType = (this.cmbSqlLogType.SelectedIndex - 1).ToString();
            string filter = "";
            if (userid != null && userid != "" && userid != "All")
            {
                filter += "USERID='" + userid + "' and ";
            }
            if (logType != null && logType != "" && logType != "-1")
            {
                filter += "SQLLOGTYPE='" + logType + "' and ";
            }
            if (filter.EndsWith("and "))
                filter = filter.Remove(filter.LastIndexOf("and "));
            //string from = DateTime.MinValue.ToString();
            //string to = DateTime.MaxValue.ToString();
            //if (this.dtpSqlFrom.Checked)
            //{
            //    from = this.dtpSqlFrom.Value.Date.ToString();
            //}
            //if (this.dtpSqlTo.Checked)
            //{
            //    to = this.dtpSqlTo.Value.Date.AddDays(1).AddSeconds(-1).ToString();
            //}
            //filter += "LOGDATETIME>'" + from + "' and LOGDATETIME<'" + to + "'";
            DataRow[] rows = tabSqlLog.Select(filter);
            foreach (DataRow row in rows)
            {
                if (tbKeyWord.Text == String.Empty)
                    tb.ImportRow(row);
                else if (row["SQLSENTENCE"].ToString().Contains(tbKeyWord.Text.ToUpper()) || row["SQLSENTENCE"].ToString().Contains(tbKeyWord.Text.ToLower()))
                    tb.ImportRow(row);
            }
            this.dgvSqlLog.DataSource = tb;
            if (this.dgvSqlLog.Columns.Count == 5)
            {
                this.dgvSqlLog.Columns.RemoveAt(4);
                this.dgvSqlLog.Columns[1].Width = 150;
                this.dgvSqlLog.Columns[3].Width = 200;
                Binding binding = new Binding("Text", tb, "SqlSentence");
                this.txtSqlSentence.DataBindings.Clear();
                this.txtSqlSentence.DataBindings.Add(binding);
                if (tb.Rows.Count == 0)
                    this.txtSqlSentence.Text = string.Empty;
            }
            else
            {
                this.txtSqlSentence.Text = string.Empty;
            }
        }
        #endregion

        #region Read
        private void btnFileRead_Click(object sender, EventArgs e)
        {
            frmSelLogConditions frm = new frmSelLogConditions(true);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                bAbort = false;
                Thread t = new Thread(new ThreadStart(ShowProgressBar));
                t.Start();
                try
                {
                    FileLogTable(frm.StartDate, frm.EndDate, frm.ExportError);
                }
                finally
                {
                    bAbort = true;
                    t.Join();
                }
                setSearchCompEnabled(true);
            }
        }

        private void btnDBRead_Click(object sender, EventArgs e)
        {
            frmSelLogConditions frm = new frmSelLogConditions(true);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                bAbort = false;
                Thread t = new Thread(new ThreadStart(ShowProgressBar));
                t.Start();
                try
                {
                    DBLogTable(frm.StartDateValue, frm.EndDateValue, frm.ExportError);
                }
                finally
                {
                    bAbort = true;
                    t.Join();
                }
                setDBSearchCompEnabled(true);
            }
        }

        private void btnSqlFileRead_Click(object sender, EventArgs e)
        {
            frmSelLogConditions frm = new frmSelLogConditions(false);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                bAbort = false;
                Thread t = new Thread(new ThreadStart(ShowProgressBar));
                t.Start();
                try
                {
                    SqlLogTable(frm.StartDate, frm.EndDate);
                }
                finally
                {
                    bAbort = true;
                    t.Join();
                }
                setSqlSearchCompEnabled(true);
            }
        }
        #endregion

        #region CellPainting
        private void dgvFileLog_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (dgvFileLog.Columns.Count != 11 || e.RowIndex == -1)
                return;
            setFormatValues(e, this.dgvFileLog);
        }

        private void dgvDBLog_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (dgvDBLog.Columns.Count != 11 || e.RowIndex == -1)
                return;
            setFormatValues(e, this.dgvDBLog);
        }

        private void dgvSqlLog_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (dgvSqlLog.Columns.Count != 4 || e.RowIndex == -1)
                return;
            setSqlFormatValues(e, this.dgvSqlLog);
        }
        #endregion

        private void setFormatValues(DataGridViewCellPaintingEventArgs e, DataGridView dgv)
        {
            if (e.ColumnIndex == 1 || e.ColumnIndex == 5)
            {
                Rectangle newRect = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 4, e.CellBounds.Height - 4);
                using (Brush gridBrush = new SolidBrush(dgv.GridColor),
                    backColorBrush = new SolidBrush(e.CellStyle.BackColor),
                    selectionBackColorBrush = new SolidBrush(e.CellStyle.SelectionBackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        // Erase the cell.
                        if (dgv.Rows[e.RowIndex].Selected)
                        {
                            e.Graphics.FillRectangle(selectionBackColorBrush, e.CellBounds);
                        }
                        else
                        {
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                        }

                        // Draw the grid lines (only the right and bottom lines;
                        // DataGridView takes care of the others).
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom - 1);
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                            e.CellBounds.Top, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom);
                    }
                }
                using (Brush foreColorBrush = new SolidBrush(e.CellStyle.ForeColor), selectionForeColorBrush = new SolidBrush(e.CellStyle.SelectionForeColor))
                {
                    string formatValue = e.Value.ToString();
                    if (e.ColumnIndex == 1) //LogStyle
                    {
                        switch (e.Value.ToString())
                        {
                            case "0":
                                formatValue = "System";
                                break;
                            case "1":
                                formatValue = "Provider";
                                break;
                            case "2":
                                formatValue = "Method";
                                break;
                            case "3":
                                formatValue = "UserDefine";
                                break;
                            case "4":
                                formatValue = "Email";
                                break;
                        }
                    }
                    else if (e.ColumnIndex == 5) //LogType
                    {
                        switch (e.Value.ToString())
                        {
                            case "0":
                                formatValue = "Normal";
                                break;
                            case "1":
                                formatValue = "Warning";
                                break;
                            case "2":
                                formatValue = "Unknown";
                                break;
                            case "3":
                                formatValue = "Error";
                                break;
                        }
                    }
                    if (dgv.Rows[e.RowIndex].Selected)
                    {
                        e.Graphics.DrawString(formatValue, e.CellStyle.Font, selectionForeColorBrush, e.CellBounds.X + 2, e.CellBounds.Y + 2);
                    }
                    else
                    {
                        e.Graphics.DrawString(formatValue, e.CellStyle.Font, foreColorBrush, e.CellBounds.X + 2, e.CellBounds.Y + 2);
                    }
                    e.Handled = true;
                }
            }
        }

        private void setSqlFormatValues(DataGridViewCellPaintingEventArgs e, DataGridView dgv)
        {
            if (e.ColumnIndex == 2)
            {
                Rectangle newRect = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 4, e.CellBounds.Height - 4);
                using (Brush gridBrush = new SolidBrush(dgv.GridColor),
                    backColorBrush = new SolidBrush(e.CellStyle.BackColor),
                    selectionBackColorBrush = new SolidBrush(e.CellStyle.SelectionBackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush))
                    {
                        // Erase the cell.
                        if (dgv.Rows[e.RowIndex].Selected)
                        {
                            e.Graphics.FillRectangle(selectionBackColorBrush, e.CellBounds);
                        }
                        else
                        {
                            e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                        }

                        // Draw the grid lines (only the right and bottom lines;
                        // DataGridView takes care of the others).
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left,
                            e.CellBounds.Bottom - 1, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom - 1);
                        e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1,
                            e.CellBounds.Top, e.CellBounds.Right - 1,
                            e.CellBounds.Bottom);
                    }
                }
                using (Brush foreColorBrush = new SolidBrush(e.CellStyle.ForeColor), selectionForeColorBrush = new SolidBrush(e.CellStyle.SelectionForeColor))
                {
                    string formatValue = e.Value.ToString();
                    switch (e.Value.ToString())
                    {
                        case "0":
                            formatValue = "ExecuteSql";
                            break;
                        case "1":
                            formatValue = "InfoCommand";
                            break;
                        case "2":
                            formatValue = "UpdateComp";
                            break;
                    }
                    if (dgv.Rows[e.RowIndex].Selected)
                    {
                        e.Graphics.DrawString(formatValue, e.CellStyle.Font, selectionForeColorBrush, e.CellBounds.X + 2, e.CellBounds.Y + 2);
                    }
                    else
                    {
                        e.Graphics.DrawString(formatValue, e.CellStyle.Font, foreColorBrush, e.CellBounds.X + 2, e.CellBounds.Y + 2);
                    }
                    e.Handled = true;
                }
            }
        }





        private void copyConnIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(this.dgvFileLog.CurrentRow != null)
            {
                Clipboard.SetDataObject(this.dgvFileLog.CurrentRow.Cells[0].Value);
            }
        }

        private void locateConnIDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmLocateLogConnId frm = new frmLocateLogConnId();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string connId = frm.ConnId;
                foreach (DataGridViewRow row in this.dgvFileLog.Rows)
                {
                    if (row.DataBoundItem != null && row.DataBoundItem is DataRowView && ((DataRowView)row.DataBoundItem)["ConnID"].ToString() == connId)
                    {
                        this.dgvFileLog.FirstDisplayedScrollingRowIndex = row.Index;
                        row.Selected = true;
                        break;
                    }
                }
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            frmLocateLogConnId frm = new frmLocateLogConnId();
            if (frm.ShowDialog() == DialogResult.OK)
            {
                string connId = frm.ConnId;
                foreach (DataGridViewRow row in this.dgvDBLog.Rows)
                {
                    if (row.DataBoundItem != null && row.DataBoundItem is DataRowView && ((DataRowView)row.DataBoundItem)["ConnID"].ToString() == connId)
                    {
                        this.dgvDBLog.FirstDisplayedScrollingRowIndex = row.Index;
                        row.Selected = true;
                        break;
                    }
                }
            }
        }

        private void ShowProgressBar()
        {
            ProgressForm proForm = new ProgressForm();
            proForm.Show();
            while (!bAbort || proForm.progressBar1.Value < 99)
            {
                if (proForm.progressBar1.Value + 3 > 100)
                {
                    proForm.progressBar1.Value = 1;
                }
                else
                {
                    proForm.progressBar1.Value += 3;
                }
                Thread.Sleep(30);
            }
        }
    }
}