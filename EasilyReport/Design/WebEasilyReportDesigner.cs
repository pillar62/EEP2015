using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.Design;
using System.ComponentModel.Design;
using Infolight.EasilyReportTools.UI;
using System.ComponentModel;
using System.Windows.Forms;

namespace Infolight.EasilyReportTools.Design
{
    public class WebEasilyReportDesigner: DataSourceDesigner
    {
        #region Variable Definition
        private IDesignerHost designerHost;
        private fmEasilyReportDesigner fmERptDesigner;
        private DesignerActionListCollection actionLists;
        #endregion
        
        public WebEasilyReportDesigner()
        {
            DesignerVerb createVerb = new DesignerVerb("Open Design Form...", new EventHandler(OnOpen));
            this.Verbs.Add(createVerb);
        }

        public void OnOpen(object sender, EventArgs e)
        {
            try
            {
                if (designerHost == null)
                {
                    designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                }
                fmERptDesigner = new fmEasilyReportDesigner(this.Component as WebEasilyReport, designerHost);
                fmERptDesigner.ShowDialog();

                TypeDescriptor.GetProperties(typeof(WebEasilyReport))["HeaderFont"].SetValue(this.Component, (this.Component as WebEasilyReport).HeaderFont.Clone());
                TypeDescriptor.GetProperties(typeof(WebEasilyReport))["FieldFont"].SetValue(this.Component, (this.Component as WebEasilyReport).FieldFont.Clone());
                TypeDescriptor.GetProperties(typeof(WebEasilyReport))["FooterFont"].SetValue(this.Component, (this.Component as WebEasilyReport).FooterFont.Clone());
                fmERptDesigner = null;
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                actionLists = base.ActionLists;

                if (actionLists != null)
                    actionLists.Add(new WebEasilyReportActionList(this.Component));

                return actionLists;
            }
        }

        public class WebEasilyReportActionList : DesignerActionList
        {
            private WebEasilyReport webRpt;
            private fmEasilyReportDesigner fmERptDesigner;
            private IDesignerHost designerHost;

            public WebEasilyReportActionList(IComponent component)
                : base(component)
            {
                webRpt = component as WebEasilyReport;
            }

            public void OnOpen()
            {
                try
                {
                    if (designerHost == null)
                    {
                        designerHost = this.GetService(typeof(IDesignerHost)) as IDesignerHost;
                    }

                    if (fmERptDesigner == null)
                    {
                        fmERptDesigner = new fmEasilyReportDesigner(webRpt, designerHost);
                    }

                    fmERptDesigner.ShowDialog();
                    TypeDescriptor.GetProperties(typeof(WebEasilyReport))["HeaderFont"].SetValue(this.Component, (this.Component as WebEasilyReport).HeaderFont.Clone());
                    TypeDescriptor.GetProperties(typeof(WebEasilyReport))["FieldFont"].SetValue(this.Component, (this.Component as WebEasilyReport).FieldFont.Clone());
                    TypeDescriptor.GetProperties(typeof(WebEasilyReport))["FooterFont"].SetValue(this.Component, (this.Component as WebEasilyReport).FooterFont.Clone());
                    fmERptDesigner = null;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            public override DesignerActionItemCollection GetSortedActionItems()
            {
                DesignerActionItemCollection items = new DesignerActionItemCollection();
                items.Add(new DesignerActionMethodItem(this, "OnOpen", "Open Design Form...", "UseDesign", true));
                return items;
            }
        }

    }
}
