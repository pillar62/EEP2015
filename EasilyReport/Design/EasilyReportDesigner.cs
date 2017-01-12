using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using Infolight.EasilyReportTools.UI;

namespace Infolight.EasilyReportTools.Design
{
    #region EasilyReportDesigner
    internal partial class EasilyReportDesigner : ComponentDesigner
    {
        #region Variable Definition
        private IDesignerHost designerHost;
        private fmEasilyReportDesigner fmERptDesigner;
        #endregion

        //public EasilyReportDesigner()
        //{
        //    designerHost = null;
        //}

        public EasilyReportDesigner()
            : base()
        {
            DesignerVerb createVerb = new DesignerVerb("Open Design Form", new EventHandler(OnOpen));
            this.Verbs.Add(createVerb);
            
            //createVerb = new DesignerVerb("Import Table", new EventHandler(OnImport));
            //this.Verbs.Add(createVerb);
        }

        public void OnImport(object sender, EventArgs e)
        {
            MessageBox.Show("Import Success");
        }

        public void OnOpen(object sender, EventArgs e)
        {
            this.DoDefaultAction();
        }

        public override void DoDefaultAction()
        {
            try
            {
                if (designerHost == null)
                {
                    designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                }
                fmERptDesigner = new fmEasilyReportDesigner(this.Component as EasilyReport, designerHost);
                fmERptDesigner.ShowDialog();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
    #endregion

    
}