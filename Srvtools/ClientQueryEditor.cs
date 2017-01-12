using System;
using System.Reflection;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace Srvtools
{
    class clientQueryEditor : ComponentDesigner
    {
        //private frmClientQueryEditor editor = null;
        private frmClientQuery previewer = null;

        public clientQueryEditor()
        {
            DesignerVerb previewVerb = new DesignerVerb("Preview", new EventHandler(OnPreview));
            this.Verbs.Add(previewVerb);
        }

        public override void DoDefaultAction()
        {
            frmClientQueryEditor editor = new frmClientQueryEditor(this.Component as ClientQuery, this.GetService(typeof(IDesignerHost)) as IDesignerHost);
            editor.ShowDialog();
            ////editor.Dispose();
            //editor = null;
       }

        public void OnPreview(object sender, EventArgs e)
        {
            try
            {
                previewer = new frmClientQuery((ClientQuery)this.Component,true);
            }
            catch
            {
                MessageBox.Show("Preview encounter an error\ncheck ClientQuery's property");
                return;
            }
            previewer.btnCancel.Enabled = false;
            previewer.btnOk.Enabled = false;
            previewer.ShowDialog();
            previewer.Dispose();
        }
    }
}
