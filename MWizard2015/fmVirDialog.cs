using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace MWizard2015
{
    public partial class fmVirDialog : Form
    {
        private bool FForceClose = false;
        private uint FSleepTime;

        public fmVirDialog()
        {
            InitializeComponent();
        }

        public fmVirDialog(bool ForceClose, uint SleepTime)
        {
            InitializeComponent();
            FForceClose = ForceClose;
            FSleepTime = SleepTime;
            ShowDialog();
        }

        [DllImport("User32.Dll")]
        public static extern IntPtr PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        [DllImport("kernel32.dll")]
        public extern static void Sleep(uint msec);
        private void fmVirDialog_Shown(object sender, EventArgs e)
        {
            Sleep(FSleepTime);
            if (FForceClose)
                PostMessage(Handle, 16, 0, 0);
        }
    }
}