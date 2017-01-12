using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Srvtools;
using Infolight.EasilyReportTools.Tools;

namespace Infolight.EasilyReportTools.UI
{
    /// <summary>
    /// The form display durning the output process
    /// </summary>
    public partial class fmProgress : Form
    {
        IReportExport iReportExport;
        IReport report;
        Thread td = null;
        private bool execSuccess;
        /// <summary>
        /// The contructor function of frmProgress
        /// </summary>
        /// <param name="title">The title of the form</param>
        /// <param name="reportExport">The reportExport component to use</param>
        public fmProgress(string title, IReportExport reportExport, IReport rpt)
        {
            InitializeComponent();
            
            iReportExport = reportExport;
            td = new Thread(new ThreadStart(Begin));
            execSuccess = false;
            this.report = rpt;
        }

        private void frmProgress_Load(object sender, EventArgs e)
        {
            #region setup lanuage
            btnCancel.Text = ERptMultiLanguage.GetLanValue("btCancel");
            #endregion

            td.Start();
        }

        public void Begin()
        {
            try
            {
                //SaveFileDialog();
                
                iReportExport.FileName = this.report.FilePath;
                if (iReportExport.FileName != null && iReportExport.FileName != "")
                {
                    //this.Visible = true;
                    ////this.Show();
                    //this.labelInfo.Text = ProcessTagInfo.ProcessStart;
                    iReportExport.Export();

                    HideButton();
                }
            }
            catch (ThreadAbortException) { }
            catch (Exception e)
            {
                ShowMessage(e.Message, MsgMode.Error);

                //HideButton();

                this.Close();
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.labelInfo.Text = iReportExport.TagInfo;
            if (iReportExport.ProgressCount > 0)
            {
                this.progressBar.Value = iReportExport.ProgressInfo * 100 / iReportExport.ProgressCount;

                if (iReportExport.ProgressCount == iReportExport.ProgressInfo && td.ThreadState == ThreadState.Stopped)
                {
                    execSuccess = true;
                }
            }
            else
            {
                if (iReportExport.ProgressCount == iReportExport.ProgressInfo && td.ThreadState == ThreadState.Stopped)
                {
                    execSuccess = true;
                }
            }

            if (td != null && td.ThreadState == ThreadState.Stopped)
            {
                timer.Enabled = false;

                if (execSuccess)
                {
                    ShowMessage(ExportMsgInfo.ExportSuccess, MsgMode.Success);

                    Thread.Sleep(100);

                    if (report.GetType().Name == ComponentInfo.EasilyReport)
                    {
                        switch (((EasilyReport)report).OutputMode)
                        {
                            case OutputModeType.None:
                                break;
                            case OutputModeType.Launch:
                                //System.Diagnostics.Process.Start("excel.exe", "\"" + this.report.FilePath + "\"");
                                System.Diagnostics.Process.Start(this.report.FilePath);
                                break;
                            case OutputModeType.Email:
                                fmEmail fm = new fmEmail(this.report);
                                fm.ShowDialog();
                                break;
                        }
                    }

                    this.Close();
                }
            }
        }

        private delegate void SaveFileDialogMethod();
        public void SaveFileDialog()
        {
            SaveFileDialogMethod call = delegate()
            {
                SaveFileDialog saveFileDialog;
                saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Excel|*.xls|Excel2007|*.xlsx";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    this.iReportExport.FileName = saveFileDialog.FileName;
                }
            };
            this.Invoke(call);
        }

        private delegate void ShowMessageMethod(string message, MsgMode msgMode);

        private void ShowMessage(string message, MsgMode msgMode)
        {
            ShowMessageMethod call = delegate(string mess, MsgMode msgModeDelegate)
            {
                switch (msgModeDelegate)
                {
                    case MsgMode.Success:
                        if (MessageBox.Show(this, mess, TitleMsgInfo.Success, MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            td.Abort();
                        }
                        break;
                    case MsgMode.Error:
                        if (MessageBox.Show(this, mess, TitleMsgInfo.Error, MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                        {
                            td.Abort();
                        }
                        break;
                    case MsgMode.Warning:
                        if (MessageBox.Show(this, mess, TitleMsgInfo.Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK)
                        {
                            //td.Abort();
                            this.Close();
                        }
                        break;
                }
                
            };
            this.Invoke(call, new object[] { message, msgMode });
        }

        private enum MsgMode
        {
            Success,
            Error,
            Warning
        }

        private delegate void HideButtonMethod();
        public void HideButton()
        {
            HideButtonMethod call = delegate()
            {
                btnCancel.Visible = false;
            };
            this.Invoke(call);
        }

        private void frmProgress_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (td.ThreadState == ThreadState.AbortRequested)
            //{
            //    td.Abort();
            //    td = null;
            //    return;
            //}
            
            //if (td != null && td.ThreadState != ThreadState.Stopped)
            //{
            //    DialogResult dr = MessageBox.Show(string.Format("{0} " + ProcessTagInfo.ProcessAbortRequest, this.Text)
            //            , TitleMsgInfo.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            //    if (dr == DialogResult.No)
            //    {
            //        e.Cancel = true;
            //        return;
            //    }
            //    else
            //    {
            //        td.Abort();
            //        td = null;
            //    }
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (td != null && td.ThreadState != ThreadState.Stopped)
            {
                td.Suspend();

                DialogResult dr = MessageBox.Show(string.Format("{0} " + ProcessTagInfo.ProcessAbortRequest, this.Text)
                        , TitleMsgInfo.Warning, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    td.Resume();
                }
                else
                {
                    timer.Stop();
                    td.Resume();
                    td.Abort();
                    this.Close();
                }
            }
        }    
    }
}