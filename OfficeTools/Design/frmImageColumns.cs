using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Srvtools;

namespace OfficeTools.Design
{
    /// <summary>
    /// The form to edit datasourceimagecolumns
    /// </summary>
    public partial class frmImageColumns : Form
    {
        public DataSourceImageColumnCollections imagecollectionedit;

        private string propertytext = string.Empty;
        /// <summary>
        /// Create a new instance of frmImageColumns
        /// </summary>
        /// <param name="dc">The collection to edit</param>
        /// <param name="owner">The owner of collection</param>
        public frmImageColumns(DataSourceImageColumnCollections dc, DataSourceItem owner)
        {
            InitializeComponent();
            imagecollectionedit = new DataSourceImageColumnCollections(owner, typeof(DataSourceImageColumnItem));//生成一个copy
            foreach (DataSourceImageColumnItem di in dc)
            {
                DataSourceImageColumnItem diedit = new DataSourceImageColumnItem();
                diedit.ColumnName = di.ColumnName;
                diedit.DefaultPath = di.DefaultPath;
                imagecollectionedit.Add(diedit);
            }
        }

        private void frmImageColumns_Load(object sender, EventArgs e)
        {
            #region setup language
            int lang = (int)CliSysMegLag.GetClientLanguage();
            this.Text = "DataSourceImageColumnItem" + OfficeTools.Properties.Resources.editor.Split(',')[lang];
            this.btnAdd.Text = OfficeTools.Properties.Resources.btnAdd.Split(',')[lang];
            this.btnRemove.Text = OfficeTools.Properties.Resources.btnRemove.Split(',')[lang];
            this.btnOK.Text = OfficeTools.Properties.Resources.btnOK.Split(',')[lang];
            this.btnCancel.Text = OfficeTools.Properties.Resources.btnCancel.Split(',')[lang];
            this.labelMember.Text = OfficeTools.Properties.Resources.member.Split(',')[lang] + ":";
            propertytext = OfficeTools.Properties.Resources.properties.Split(',')[lang];
            this.labelProperties.Text = propertytext; 
            #endregion

            InitialList();
            if (listBoxImageColumns.Items.Count > 0)
            {
                listBoxImageColumns.SelectedIndex = 0;
            }
        }

        private void InitialList()
        {
            listBoxImageColumns.Items.Clear();
            foreach (DataSourceImageColumnItem di in imagecollectionedit)
            {
                if (di.Name.Length > 0)
                {
                    listBoxImageColumns.Items.Add(listBoxImageColumns.Items.Count.ToString() + "." + di.Name);
                }
                else
                {
                    listBoxImageColumns.Items.Add(listBoxImageColumns.Items.Count.ToString() + ".OfficeTools.DataSourceImageColumnItem");
                }
            }
            btnUp.Enabled = false;
            btnDown.Enabled = false;
        }

        private void listBoxImageColumns_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxImageColumns.SelectedIndex != -1)
            {
                propertyGridImageColumns.SelectedObject = imagecollectionedit[listBoxImageColumns.SelectedIndex];
                string name = listBoxImageColumns.SelectedItem.ToString();
                labelProperties.Text = name.Substring(name.IndexOf('.') + 1) + propertytext + ":";
                if (listBoxImageColumns.SelectedIndex == 0)
                {
                    btnUp.Enabled = false;
                }
                else
                {
                    btnUp.Enabled = true;
                }
                if (listBoxImageColumns.SelectedIndex == listBoxImageColumns.Items.Count - 1)
                {
                    btnDown.Enabled = false;
                }
                else
                {
                    btnDown.Enabled = true;
                }
            }
        }

        private void propertyGridImageColumns_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            if (string.Compare(e.ChangedItem.PropertyDescriptor.Name, "ColumnName", true) == 0)
            {
                int index = listBoxImageColumns.SelectedIndex;
                InitialList();
                listBoxImageColumns.SelectedIndex = index;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataSourceImageColumnItem diedit = new DataSourceImageColumnItem();
            imagecollectionedit.Add(diedit);
            InitialList();
            listBoxImageColumns.SelectedIndex = listBoxImageColumns.Items.Count - 1;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            int index = listBoxImageColumns.SelectedIndex;
            if(index != -1)
            {
                imagecollectionedit.RemoveAt(index);
                InitialList();
                listBoxImageColumns.SelectedIndex = Math.Min(index, listBoxImageColumns.Items.Count - 1);
                if (listBoxImageColumns.SelectedIndex == -1)
                { 
                    propertyGridImageColumns.SelectedObject = null;
                    labelProperties.Text = propertytext + ":";
                    btnUp.Enabled = false;
                    btnDown.Enabled = false;
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int index = listBoxImageColumns.SelectedIndex;
            DataSourceImageColumnItem diedit = imagecollectionedit[index];
            imagecollectionedit.RemoveAt(index);
            imagecollectionedit.Insert(index - 1, diedit);
            InitialList();
            listBoxImageColumns.SelectedIndex = index - 1;
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int index = listBoxImageColumns.SelectedIndex;
            DataSourceImageColumnItem diedit = imagecollectionedit[index];
            imagecollectionedit.RemoveAt(index);
            imagecollectionedit.Insert(index + 1, diedit);
            InitialList();
            listBoxImageColumns.SelectedIndex = index + 1;
        }
    }
}