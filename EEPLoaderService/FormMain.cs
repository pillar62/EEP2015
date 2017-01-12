using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Net.Sockets;
using EEPSetUpLibrary;
using System.Net;
using System.Threading;
using EEPLoaderService.Properties;
using System.Runtime.Remoting;
using Srvtools;
using System.Reflection;

namespace EEPLoaderService
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            int port = Settings.Default.Port;
            toolStripTextBoxPort.Text = port.ToString();
            try
            {
                System.Runtime.Remoting.Channels.Http.HttpChannel channel = new System.Runtime.Remoting.Channels.Http.HttpChannel(port);
                System.Runtime.Remoting.Channels.ChannelServices.RegisterChannel(channel, false);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(LoaderObject)
                    , "LoaderObject.rem", System.Runtime.Remoting.WellKnownObjectMode.Singleton);
                RemotingConfiguration.RegisterWellKnownServiceType(typeof(ListenerService)
                   , "Srvtools.rem", System.Runtime.Remoting.WellKnownObjectMode.Singleton);

                try
                {
                    Assembly assembly = Assembly.LoadFrom(string.Format(@"{0}\{1}.dll", Application.StartupPath, "EFWCFModule"));
                    var start = assembly.GetType("EFWCFModule.ServiceProvider").GetMethod("StartListernerService", new Type[] { typeof(Type), typeof(Type[]) });
                    start.Invoke(null, new object[] { assembly.GetType("EFWCFModule.EFService"), new Type[] { assembly.GetType("EFWCFModule.IEFService") } });
                    //formwcfserver = ShowForm("WCFServer", "WCFServer.FormMain");
                }
                catch
                {
                   

                }

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            //Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //IPEndPoint ip = new IPEndPoint(IPAddress.Any, Config.ServerLoaderPort);
            //try
            //{
            //    server.Bind(ip);
            //    server.Listen(10);
            //    server.BeginAccept(new AsyncCallback(Connect), server);
            //}
            //catch (SocketException es)//端口被占用
            //{
            //    if (es.ErrorCode == 10048)
            //    {
            //        MessageBox.Show("Port of ServerLoader is being used now\r\nPlease change the port in the options");
            //    }
            //}
        }

        private Form ShowForm(string assemblyName, string typeName)
        {
            Assembly assembly = Assembly.LoadFrom(string.Format(@"{0}\{1}.dll", Application.StartupPath, assemblyName));
            Form form = (Form)assembly.CreateInstance(typeName);
            //form.WindowState = FormWindowState.Minimized;
            form.Show(this);
            form.Hide();
            return form;
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
           
        }


        private void toolStripTextBoxPort_TextChanged(object sender, EventArgs e)
        {
            if (toolStripTextBoxPort.Text.Length > 0)
            {
                try
                {
                    Convert.ToInt32(toolStripTextBoxPort.Text);
                }
                catch
                {
                    toolStripTextBoxPort.Text = Config.ServerLoaderPort.ToString();
                }
            }
        }

        private void menuStrip_MenuActivate(object sender, EventArgs e)
        {
            if (toolStripTextBoxPort.Text.Length == 0)
            {
                toolStripTextBoxPort.Text = Config.ServerLoaderPort.ToString();
            }
        }

        private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
                notifyIcon.ShowBalloonTip(3000, "Info", "EEPServerLoader is still active", ToolTipIcon.Info);
                e.Cancel = true;
            }
            else
            {
                Settings.Default.Port = Convert.ToInt32(toolStripTextBoxPort.Text);
                Settings.Default.Save();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.BringToFront();
            this.WindowState = FormWindowState.Normal;
        }
    }
}