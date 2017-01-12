using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel.Design;
using EFWCFModule;
using EFBase.Design;

namespace EFServerTools.Design.EFTransactionDesign
{
    public partial class EFTransactionEditorTransFieldsDialog : Form
    {
        #region Variables
        Transaction transaction = null;
        Transaction transPrivateCopy = null;
        IUpdateComponent uctran = null;
        string strDesTable = "";
        string strSrcTable = "";
        #endregion

        #region Constructor
        public EFTransactionEditorTransFieldsDialog(Transaction trans, IUpdateComponent uc, string destablename, string srctablename)
        {
            transaction = trans;
            uctran = uc;
            strDesTable = destablename;
            strSrcTable = srctablename;
            InitializeComponent();
        }
        #endregion

        #region Events
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void InfoTransactionEditorTransFieldsDialog_Load(object sender, EventArgs e)
        {
            // Copy the editing Transaction
            //
            SetTransactionPrivateCopy();

            // Set up UI
            SetUpUI();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Refresh(false);
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

        private void lbRelation_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.lbDesField.SelectedIndex = this.lbRelation.SelectedIndex;
            this.lbScrField.SelectedIndex = this.lbRelation.SelectedIndex;

            int index = this.lbRelation.SelectedIndex;
            if (index != -1)
            {
                this.txtSrcGetValue.Text = this.transPrivateCopy.TransFields[index].SrcGetValue;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int index = this.lbRelation.SelectedIndex;
            if (index != -1)
            {
                this.lbRelation.Items.RemoveAt(index);
                this.lbDesField.Items.RemoveAt(index);
                this.lbScrField.Items.RemoveAt(index);
                this.transPrivateCopy.TransFields.RemoveAt(index);
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
            this.transaction.TransFields = this.transPrivateCopy.TransFields;
        }
        #endregion

        #region Methods
        private void SetUpUI()
        {
            // Set up ListBox 
            foreach (TransField field in transPrivateCopy.TransFields)
            {
                this.lbDesField.Items.Add(field.DesField);
                this.lbScrField.Items.Add(field.SrcField);

                switch (field.UpdateMode)
                {
                    case UpdateMode.Decrease:
                        this.lbRelation.Items.Add(field.DesField + " - " + field.SrcField);
                        break;
                    case UpdateMode.Disable:
                        this.lbRelation.Items.Add(field.DesField + " X " + field.SrcField);
                        break;
                    case UpdateMode.Increase:
                        this.lbRelation.Items.Add(field.DesField + " + " + field.SrcField);
                        break;
                    case UpdateMode.Replace:
                        this.lbRelation.Items.Add(field.DesField + " = " + field.SrcField);
                        break;
                    case UpdateMode.WriteBack:
                        this.lbRelation.Items.Add(field.DesField + " <- " + field.SrcField);
                        break;
                }
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

            EFTransactionTransFieldsAddDialog ittfad = new EFTransactionTransFieldsAddDialog();

            if (strSrcTable != string.Empty)
            {
                if (uctran != null && uctran.Command != null)
                {
                    var provider = new MetadataProvider(DTE.CurrentDirectory, uctran.Command.MetadataFile);
                    ittfad.cmbSrcField.Items.AddRange(provider.GetPropertyNames(uctran.Command.ContextName, provider.GetEntitySetType(uctran.Command.ContextName, uctran.Command.EntitySetName)).ToArray());
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
                    ittfad.cmbDesField.Items.Add(strColumn);
                }
            }

            foreach (var item in Enum.GetNames(typeof(UpdateMode)))
            {
                ittfad.cbxUpdateMode.Items.Add(item);
            }

            if (update)
            {
                ittfad.cmbDesField.Text = this.lbDesField.Items[this.lbRelation.SelectedIndex].ToString();
                ittfad.cmbSrcField.Text = this.lbScrField.Items[this.lbRelation.SelectedIndex].ToString();
                ittfad.txtSrcGetValue.Text = this.txtSrcGetValue.Text;
                ittfad.cbxUpdateMode.SelectedIndex = Convert.ToInt32(this.transPrivateCopy.TransFields[this.lbRelation.SelectedIndex].UpdateMode);
            }
            else
            {
                ittfad.cbxUpdateMode.SelectedIndex = 2;
            }

            if (ittfad.ShowDialog() == DialogResult.OK)
            {
                TransField tf = new TransField();
                tf.DesField = ittfad.cmbDesField.Text;
                tf.SrcField = ittfad.cmbSrcField.Text;
                tf.SrcGetValue = ittfad.txtSrcGetValue.Text;

                tf.UpdateMode = (UpdateMode)Enum.GetValues(typeof(UpdateMode)).GetValue(ittfad.cbxUpdateMode.SelectedIndex);

                if (update)
                {
                    this.transPrivateCopy.TransFields[this.lbRelation.SelectedIndex] = tf;
                    this.lbDesField.Items[this.lbRelation.SelectedIndex] = tf.DesField;
                    this.lbScrField.Items[this.lbRelation.SelectedIndex] = tf.SrcField;

                    this.lbRelation.Items[this.lbRelation.SelectedIndex] = string.Format(GetFormat(tf.UpdateMode), tf.DesField, tf.SrcField);
                }
                else
                {
                    this.transPrivateCopy.TransFields.Add(tf);
                    this.lbDesField.Items.Add(tf.DesField);
                    this.lbScrField.Items.Add(tf.SrcField);

                    this.lbRelation.Items.Add(string.Format(GetFormat(tf.UpdateMode), tf.DesField, tf.SrcField));
                }
            }
        }

        private string GetFormat(UpdateMode mode)
        {
            string format = string.Empty;
            format = "{0} " + GetSign(mode) + " {1}";
            return format;
        }

        private string GetSign(UpdateMode mode)
        {
            string sign = string.Empty;

            switch (mode)
            {
                case UpdateMode.Increase:
                    sign = "+";
                    break;
                case UpdateMode.Decrease:
                    sign = "-";
                    break;
                case UpdateMode.Replace:
                    sign = "=";
                    break;
                case UpdateMode.WriteBack:
                    sign = "<-";
                    break;
                case UpdateMode.Disable:
                    sign = "X";
                    break;
            }

            return sign;
        }
        #endregion
    }
}