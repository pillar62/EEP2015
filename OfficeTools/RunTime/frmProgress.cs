using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using Srvtools;

namespace OfficeTools.RunTime
{
    /// <summary>
    /// The form display durning the output process
    /// </summary>
    public partial class frmProgress : Form
    {
        IAutomation ia;
        Thread td = null;
        public OfficePlate.OutputModeType modetype;
        /// <summary>
        /// The contructor function of frmProgress
        /// </summary>
        /// <param name="title">The title of the form</param>
        /// <param name="automation">The automation component to use</param>
        /// <param name="tp">The type of output</param>
        public frmProgress(string title, IAutomation automation, OfficePlate.OutputModeType tp)
        {
            InitializeComponent();
            
            modetype = tp;
            ia = automation;
            td = new Thread(new ThreadStart(Begin));
        }

        private void frmProgress_Load(object sender, EventArgs e)
        {
            #region setup lanuage
            int lang = (int)CliSysMegLag.GetClientLanguage();
            this.btnCancel.Text = OfficeTools.Properties.Resources.btnCancel.Split(',')[lang]; 
            #endregion

            td.Start();
        }

        public void Begin()
        {
            try
            {
                ia.Run();
                HideButton();
            }
            catch (ThreadAbortException) { }
            catch (Exception e)
            {
                ShowMessage(e.Message);
                HideButton();
            }
            finally
            {
                (ia as OfficeAutomation).Plate.OnAfterOutput(new EventArgs());
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.labelInfo.Text = ia.TagInfo;
            if (ia.ProgressCount > 0)
            {
                this.progressBar.Value = ia.ProgressInfo * 100 / ia.ProgressCount;
            }
            if (td.ThreadState == ThreadState.Stopped)
            {
                timer.Enabled = false;
                this.Close();
            }
        }

        private delegate void ShowMessageMethod(string message);

        public void ShowMessage(string message)
        {
            ShowMessageMethod call = delegate(string mess)
            {
                MessageBox.Show(this, mess, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            };
            this.Invoke(call, new object[]{message});
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
            if (td != null && td.ThreadState != ThreadState.Stopped)
            {
                DialogResult dr = MessageBox.Show(string.Format("{0} has not completed, \ndo you really want to abort it?",this.Text)
                    ,"Warning",MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    td.Abort();
                    td = null;
                }
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }    
    }
}