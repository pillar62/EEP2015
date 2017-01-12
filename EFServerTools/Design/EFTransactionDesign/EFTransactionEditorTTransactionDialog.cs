using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Reflection;
using EFWCFModule;
using EFBase;
using EFBase.Design;

namespace EFServerTools.Design.EFTransactionDesign
{
	public partial class EFTransactionEditorTTransactionDialog : Form
    {
        #region Variables
        Transaction transaction = null;
		Transaction transPrivateCopy = null;
		IDesignerHost DesignerHost;
        IUpdateComponent uctran = null;
        string SrcTableName = "";
        #endregion

        #region Constructor
        public EFTransactionEditorTTransactionDialog(Transaction trans, IDesignerHost host, string srctablename)
		{
			transaction = trans;
			DesignerHost = host;
            uctran = ((EFTransaction)((IEFProperty)this.transaction).ParentProperty.Component).UpdateComponent;
			InitializeComponent();
            this.SrcTableName = srctablename;
        
		}
        #endregion

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void InfoTransactionEditorTTransactionDialog_Load(object sender, EventArgs e)
		{
			// Copy the editing Transaction
			//
			SetTransactionPrivateCopy();

			// Set up UI
			SetUpUI();
		}

		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this.cbxAutoNumber.Text != "")
			{
				transaction.AutoNumber = this.DesignerHost.Container.Components[this.cbxAutoNumber.Text] as EFAutoNumber;
			}
			else
			{
				transaction.AutoNumber = null;
			}

			transaction.TransFields = transPrivateCopy.TransFields;

			transaction.TransKeyFields = transPrivateCopy.TransKeyFields;

            transaction.TransMode = (TransMode)Enum.GetValues(typeof(TransMode)).GetValue(this.cbxTransMode.SelectedIndex);

			transaction.TransTableName = this.cmbTransTableName.Text;

            transaction.WhenInsert = cbInsert.Checked;
            transaction.WhenUpdate = cbUpdate.Checked;
            transaction.WhenDelete = cbDelete.Checked;
		}

		private void btnTransKeyFields_Click(object sender, EventArgs e)
		{
			EFTransactionEditorKeyFieldsDialog itekfd =
                new EFTransactionEditorKeyFieldsDialog(this.transPrivateCopy, this.uctran, this.cmbTransTableName.Text,SrcTableName);
			itekfd.ShowDialog();
		}

		private void btnTransFields_Click(object sender, EventArgs e)
		{
			EFTransactionEditorTransFieldsDialog itetfd =
				new EFTransactionEditorTransFieldsDialog(this.transPrivateCopy,this.uctran,this.cmbTransTableName.Text,SrcTableName);
			itetfd.ShowDialog();
        }
        #endregion

        #region Methods
        private void SetUpUI()
        {
            // cbxTransMode
            foreach (var item in Enum.GetNames(typeof(TransMode)))
            {
                this.cbxTransMode.Items.Add(item);
            }

            //cmbTransTableName
            if (uctran.Command != null)
            {
                var provider = new MetadataProvider(DTE.CurrentDirectory, uctran.Command.MetadataFile);

                List<string> lstTable = provider.GetEntitySetNames(uctran.Command.ContextName);
                foreach (string table in lstTable)
                {
                    cmbTransTableName.Items.Add(table);
                }

            }

            // cbxAutoNumber
            this.cbxAutoNumber.Items.Add("");
            foreach (IComponent comp in DesignerHost.Container.Components)
            {
                if (comp is EFAutoNumber)
                {
#warning 不一定会用Site。先跳过，日后再补。
                    //this.cbxAutoNumber.Items.Add(comp.Site.Name);
                }
            }

            this.txtTransStep.Text = this.transPrivateCopy.TransStep.ToString();

            this.cmbTransTableName.Text = this.transPrivateCopy.TransTableName;

            if (this.transPrivateCopy.AutoNumber != null
                && this.transPrivateCopy.AutoNumber.Site != null)
            {
                this.cbxAutoNumber.Text = this.transPrivateCopy.AutoNumber.Site.Name;
            }
            else
            {
                this.cbxAutoNumber.Text = "";
            }

            this.cbxTransMode.SelectedIndex = Convert.ToInt32(this.transPrivateCopy.TransMode);

            this.cbInsert.Checked = this.transPrivateCopy.WhenInsert;
            this.cbUpdate.Checked = this.transPrivateCopy.WhenUpdate;
            this.cbDelete.Checked = this.transPrivateCopy.WhenDelete;
        }

        private void SetTransactionPrivateCopy()
        {
            transPrivateCopy = transaction.Copy();
        }
        #endregion
    }
}