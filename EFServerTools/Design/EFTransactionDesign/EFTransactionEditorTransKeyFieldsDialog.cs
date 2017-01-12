using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using EFWCFModule;
using EFBase.Design;

namespace EFServerTools.Design.EFTransactionDesign
{
    public partial class EFTransactionEditorKeyFieldsDialog : Form
    {
        #region Variables
        Transaction transaction = null;
        Transaction transPrivateCopy = null;
        IUpdateComponent uctran = null;
        string strDesTable = "";
        string strSrcTable = "";
        #endregion

        #region Constructor
        public EFTransactionEditorKeyFieldsDialog(Transaction trans, IUpdateComponent uc, string destablename, string srctablename)
        {
            transaction = trans;
            uctran = uc;
            strDesTable = destablename;
            strSrcTable = srctablename;
            InitializeComponent();
        }
        #endregion

        #region Events
        private void InfoTransactionEditorKeyFieldsDialog_Load(object sender, EventArgs e)
        {
            // Copy the editing Transaction
            //
            SetTransactionPrivateCopy();

            // SetUpUI
            SetUpUI();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void lbRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = this.lbRelation.SelectedIndex;
            this.lbDesField.SelectedIndex = index;
            this.lbScrField.SelectedIndex = index;
            if (index != -1)
            {
                cbxWhereMode.SelectedIndex = Convert.ToInt32(transPrivateCopy.TransKeyFields[index].WhereMode);
                
                this.txtSrcGetValue.Text =
                    this.transPrivateCopy.TransKeyFields[index].SrcGetValue;
            }
            else
            {
                this.cbxWhereMode.SelectedIndex = -1;
                this.txtSrcGetValue.Clear();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Refresh(false);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = this.lbRelation.SelectedIndex;
            if (index != -1)
            {
                this.lbRelation.Items.RemoveAt(index);
                this.lbDesField.Items.RemoveAt(index);
                this.lbScrField.Items.RemoveAt(index);
                this.transPrivateCopy.TransKeyFields.RemoveAt(index);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (this.lbRelation.SelectedIndex != -1)
            {
                Refresh(true);
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            transaction.TransKeyFields = this.transPrivateCopy.TransKeyFields;
        }

        private void lbDesField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lbRelation.SelectedIndex = this.lbDesField.SelectedIndex;
            this.lbScrField.SelectedIndex = this.lbDesField.SelectedIndex;
        }

        private void lbScrField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lbRelation.SelectedIndex = this.lbScrField.SelectedIndex;
            this.lbDesField.SelectedIndex = this.lbScrField.SelectedIndex;
        }
        #endregion

        #region Methods
        private void SetUpUI()
        {
            // Set up ComboBox - cbxWhereMode
            foreach (var item in Enum.GetNames(typeof(WhereMode)))
            {
                cbxWhereMode.Items.Add(item);
            }

            // Set up ListBox 
            foreach (TransKeyField keyField in transPrivateCopy.TransKeyFields)
            {
                this.lbDesField.Items.Add(keyField.DesField);
                this.lbScrField.Items.Add(keyField.SrcField);
                this.lbRelation.Items.Add(keyField.DesField + " = " + keyField.SrcField);
            }
        }

        private void SetTransactionPrivateCopy()
        {
            transPrivateCopy = transaction.Copy();
        }

        private void Refresh(bool update)
        {
            List<string> lstSrcColumn = new List<string>();
            List<string> lstDesColumn = new List<string>();

            EFTransactionTransKeyFieldsAddDialog ittkfad = new EFTransactionTransKeyFieldsAddDialog();

            if (strSrcTable != string.Empty)
            {
                if (uctran != null && uctran.Command != null)
                {
                    var provider = new MetadataProvider(DTE.CurrentDirectory, uctran.Command.MetadataFile);
                    ittkfad.cmbSrcField.Items.AddRange(provider.GetPropertyNames(uctran.Command.ContextName, provider.GetEntitySetType(uctran.Command.ContextName, uctran.Command.EntitySetName)).ToArray());
                }
            }

            if (strDesTable != string.Empty)
            {
                if (uctran != null && uctran.Command != null)
                {
                    var provider = new MetadataProvider(DTE.CurrentDirectory, uctran.Command.MetadataFile);
                    lstDesColumn = provider.GetPropertyNames(uctran.Command.ContextName, provider.GetEntitySetType(uctran.Command.ContextName, strDesTable));
                }

                if (lstDesColumn.Count == 0)
                {
                    MessageBox.Show(string.Format("Transtable :{0} \ndoesn't exist", strDesTable));
                    return;
                }

                foreach (string strColumn in lstDesColumn)
                {
                    ittkfad.cmbDesField.Items.Add(strColumn);
                }
            }

            foreach (var item in Enum.GetNames(typeof(WhereMode)))
            {
                ittkfad.cbxWhereMode.Items.Add(item);
            }

            if (update)
            {
                ittkfad.cmbDesField.Text = this.lbDesField.Items[this.lbRelation.SelectedIndex].ToString();
                ittkfad.cmbSrcField.Text = this.lbScrField.Items[this.lbRelation.SelectedIndex].ToString();
                ittkfad.cbxWhereMode.Text = this.cbxWhereMode.Text;
                ittkfad.txtSrcGetValue.Text = this.txtSrcGetValue.Text;
            }
            else
            {
                ittkfad.cbxWhereMode.SelectedIndex = 2;
            }


            if (ittkfad.ShowDialog() == DialogResult.OK)
            {
                TransKeyField tkf = new TransKeyField();
                tkf.DesField = ittkfad.cmbDesField.Text;
                tkf.SrcField = ittkfad.cmbSrcField.Text;
                tkf.SrcGetValue = ittkfad.txtSrcGetValue.Text;

                tkf.WhereMode = (WhereMode)Enum.GetValues(typeof(WhereMode)).GetValue(ittkfad.cbxWhereMode.SelectedIndex);

                if (update)
                {
                    this.transPrivateCopy.TransKeyFields[this.lbRelation.SelectedIndex] = tkf;
                    this.lbDesField.Items[this.lbRelation.SelectedIndex] = tkf.DesField;
                    this.lbScrField.Items[this.lbRelation.SelectedIndex] = tkf.SrcField;
                    this.lbRelation.Items[this.lbRelation.SelectedIndex] = tkf.DesField + " = " + tkf.SrcField;
                }
                else
                {
                    this.transPrivateCopy.TransKeyFields.Add(tkf);
                    this.lbDesField.Items.Add(tkf.DesField);
                    this.lbScrField.Items.Add(tkf.SrcField);
                    this.lbRelation.Items.Add(tkf.DesField + " = " + tkf.SrcField);
                }
            }
        }
        #endregion
    }
}

