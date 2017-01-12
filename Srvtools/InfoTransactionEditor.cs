using System;

using System.Reflection;
using System.ComponentModel;

using System.ComponentModel.Design;

using System.Windows.Forms;



namespace Srvtools
{
	public class InfoTransactionEditor : ComponentDesigner
	{
		private InfoTransactionEditorDialog editor = null;

		public InfoTransactionEditor()
		{
            DesignerVerb editVerb = new DesignerVerb("Edit", new EventHandler(OnEdit));
            this.Verbs.Add(editVerb);
		}

		public override void DoDefaultAction()
		{		
			if (editor == null)
			{
				editor = new InfoTransactionEditorDialog(this.Component as InfoTransaction, 
					this.GetService(typeof(IDesignerHost)) as IDesignerHost);
			}

			editor.ShowDialog();
		}

        public void OnEdit(object sender, EventArgs e)
        {
            if (editor == null)
            {
                editor = new InfoTransactionEditorDialog(this.Component as InfoTransaction,
                    this.GetService(typeof(IDesignerHost)) as IDesignerHost);
            }

            editor.ShowDialog();
        }

        public Form ShowForm()
        {
            if (editor == null)
            {
                editor = new InfoTransactionEditorDialog(this.Component as InfoTransaction,
                    this.GetService(typeof(IDesignerHost)) as IDesignerHost);
            }

            editor.Show();
            return editor;
        }

	}
}
