using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.Design;
using System.Windows.Forms;

namespace EFServerTools.Design.EFTransactionDesign
{
    public class EFTransactionEditor : ComponentDesigner
    {
        private EFTransactionEditorDialog editor = null;

        public EFTransactionEditor()
        {
            DesignerVerb editVerb = new DesignerVerb("Edit", new EventHandler(OnEdit));
            this.Verbs.Add(editVerb);
        }

        public override void DoDefaultAction()
        {
            if (editor == null)
            {
                editor = new EFTransactionEditorDialog(this.Component as EFTransaction,
                    this.GetService(typeof(IDesignerHost)) as IDesignerHost);
            }

            editor.ShowDialog();
        }

        public void OnEdit(object sender, EventArgs e)
        {
            if (editor == null)
            {
                editor = new EFTransactionEditorDialog(this.Component as EFTransaction,
                    this.GetService(typeof(IDesignerHost)) as IDesignerHost);
            }

            editor.ShowDialog();
        }

        public Form ShowForm()
        {
            if (editor == null)
            {
                editor = new EFTransactionEditorDialog(this.Component as EFTransaction,
                    this.GetService(typeof(IDesignerHost)) as IDesignerHost);
            }

            editor.Show();
            return editor;
        }

    }
}
