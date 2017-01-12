using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;

namespace EEPLoaderService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        private FormMain FForm1 = null;

        protected override void OnStart(string[] args)
        {
            // TODO: Add code here to start your service.
            FForm1 = new FormMain();
        }


        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            FForm1.Dispose();
        }
    }
}
