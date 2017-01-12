using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel.Design;
using System.Reflection;
using Srvtools;

namespace OfficeTools.Design
{
    /// <summary>
    /// The class of frmValue
    /// </summary>
    public partial class frmValue : Form
    {
        private IOfficePlate op;
        private List<IComponent> listcomponent;
        /// <summary>
        /// Create a new instance of frmValue
        /// </summary>
        /// <param name="value">The expression value to edit</param>
        /// <param name="officeplate">The officeplate to edit</param>
        /// <param name="icon">The container of the component</param>
        public frmValue(string value, IOfficePlate officeplate, List<IComponent> list)
        {
            InitializeComponent();
            tbScript.Text = value;
            op = officeplate;
            listcomponent = list;
        }

        private void frmValue_Load(object sender, EventArgs e)
        {
            #region setup language
            int lang = (int)CliSysMegLag.GetClientLanguage();
            this.Text = "Expression" + OfficeTools.Properties.Resources.editor.Split(',')[lang];
            this.btnOK.Text = OfficeTools.Properties.Resources.btnOK.Split(',')[lang];
            this.btnCancel.Text = OfficeTools.Properties.Resources.btnCancel.Split(',')[lang]; 
            #endregion

            for (int i = 0; i < op.DataSource.Count; i++)
            {
                AddDataSourceItem(op.DataSource[i]);
            }

            for (int i = 0; i < listcomponent.Count; i++)
            {
                if (listcomponent[i] is Control|| listcomponent[i] is System.Web.UI.Control)
                {
                    AddControlItem(listcomponent[i]);
                }
            }
        }

        private void AddDataSourceItem(DataSourceItem di)
        { 
            string caption = di.Caption;
            string datamemeber = di.DataMember;
            Image imgvalue = OfficeTools.Properties.Resources.value;

            if (caption.Length > 0)
            {
                ToolStripMenuItem tmdataset = new ToolStripMenuItem(caption, imgvalue);
                if (di.GetTable().Contains(di.DataMember))
                {
                    for (int i = 0; i < ((DataView)di.GetTable()[di.DataMember]).Table.Columns.Count; i++)
                    {
                        string column = (((DataView)di.GetTable()[di.DataMember]).Table.Columns[i]).ColumnName;
                        ToolStripMenuItem tmcolumn = new ToolStripMenuItem(column, imgvalue);
                        tmcolumn.Tag = column;
                        tmcolumn.Click += new EventHandler(stripMenuItem_Click);
                        tmdataset.DropDownItems.Add(tmcolumn);
                    }
                }
                this.dataSourceItemToolStripMenuItem.DropDownItems.Add(tmdataset);
            }
        }

        private void AddControlItem(IComponent cp)
        {
            if (cp != null)
            {
                string caption = cp.Site.Name;
                Image imgvalue = OfficeTools.Properties.Resources.value;
                Image imgprop = OfficeTools.Properties.Resources.property;

                ToolStripMenuItem tmcontrol = new ToolStripMenuItem(caption, imgvalue);

                Type tp = cp.GetType();
                PropertyInfo[] pi = tp.GetProperties(BindingFlags.Instance | BindingFlags.Public);
                for (int i = 0; i < pi.Length; i++)
                {
                    Type tpprop = pi[i].PropertyType;
                    if (tpprop == typeof(int) || tpprop == typeof(double) || tpprop == typeof(float)
                        /*|| tpprop == typeof(bool)*/ || tpprop == typeof(string))
                    {
                        string prop = pi[i].Name;
                        ToolStripMenuItem tmprop = new ToolStripMenuItem(prop, imgprop);
                        tmprop.Tag = caption + "." + pi[i].Name;
                        tmprop.Click += new EventHandler(stripMenuItem_Click);
                        tmcontrol.DropDownItems.Add(tmprop);
                    }
                    else if (tpprop.GetInterface("ICollection") != null)
                    {
                        string prop = pi[i].Name;
                        ToolStripMenuItem tmprop = new ToolStripMenuItem(prop, imgprop);
                        ToolStripMenuItem tmcount = new ToolStripMenuItem("Count", imgprop);
                        tmcount.Tag = caption + "." + pi[i].Name + ".Count";
                        tmcount.Click += new EventHandler(stripMenuItem_Click);
                        tmprop.DropDownItems.Add(tmcount);
                        tmcontrol.DropDownItems.Add(tmprop);
                    }
                }
                this.controlItemToolStripMenuItem.DropDownItems.Add(tmcontrol);
            }
        }

        private void stripMenuItem_Click(object sender, EventArgs e)
        {
            string script = ((ToolStripMenuItem)sender).Tag.ToString();
            InsertScript(script);
        }

        private void InsertScript(string script)
        {
            string text = tbScript.Text;
            int position = tbScript.SelectionStart;
            int length = tbScript.SelectionLength;

            string textpart1 = string.Empty;
            string textpart2 = string.Empty;
            if (position > 0)
            {
                textpart1 = text.Substring(0, position);
            }
            if(position + length < text.Length)
            {
                textpart2 = text.Substring(position + length);
            }

            tbScript.Text = textpart1 + script + textpart2;

            tbScript.SelectionStart = position + script.Length;
        }
    }
}