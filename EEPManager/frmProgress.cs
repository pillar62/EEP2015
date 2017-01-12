using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Collections;
using Srvtools;

namespace EEPManager
{
    public partial class frmProgress : Form
    {
        Thread td = null;
        string IP = string.Empty;
        int Port = 0;
        string Message = string.Empty;
        public frmProgress(string serverip, int serverport, string message)
        {
            InitializeComponent();
            IP = serverip;
            Port = serverport;
            Message = message;
            labelInfo.Text = message;
            if (true)
            {
                td = new Thread(new ThreadStart(GetRemoteModule));
            }

            td.Start();
        }


        public EEPRemoteModule Module = null;

        private void GetRemoteModule()
        {
            EEPRemoteModule module = Activator.GetObject(typeof(EEPRemoteModule)
                     , string.Format("http://{0}:{1}/InfoRemoteModule.rem", IP, Port)) as EEPRemoteModule;
            try
            {
                module.ToString();
                Module = module;
            }
            catch (ThreadAbortException)
            { 
            
            }
            catch (Exception e)
            {
                ListenerService service = Activator.GetObject(typeof(ListenerService)
                     , string.Format("http://{0}:{1}/Srvtools.rem", IP, 8000)) as ListenerService;
                try
                {
                    if (service.StartServer())
                    {
                        GetRemoteModule();
                        return;
                    }
                }
                catch { }
                MessageBox.Show(e.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            if (td != null && td.ThreadState != ThreadState.Stopped)
            {
                td.Abort();
                this.DialogResult = DialogResult.Cancel;
                this.Close();
            }
        }

        int count = 0;
        private void timer_Tick(object sender, EventArgs e)
        {
            if (td != null && td.ThreadState == ThreadState.Stopped)
            {
                this.DialogResult = DialogResult.OK;
                timer.Enabled = false;
                this.Close();
            }
            else
            {
                StringBuilder sBuilder = new StringBuilder();
                sBuilder.Append(Message);
                sBuilder.Append('.', (count % 3) + 1);
                labelInfo.Text = sBuilder.ToString();
                count++;
            }
        }
    }
}