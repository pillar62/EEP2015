using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace AjaxTools
{
    public partial class AjaxViewPreviewEditor : Form
    {
        public AjaxViewPreviewEditor(List<AjaxViewFieldLayout> layouts, string type)
        {
            InitializeComponent();
            Layouts = layouts;
            ControlType = type;
        }

        List<AjaxViewFieldLayout> Layouts = new List<AjaxViewFieldLayout>();
        string ControlType = "";

        private void AjaxViewPreviewEditor_Load(object sender, EventArgs e)
        {
            if (ControlType == "form")
            {
                Dictionary<string, int> dicLabelWidth = new Dictionary<string, int>();
                int maxWidth = 0, maxHeight = 0, maxColumns = 1, rowMaxColumns = 1, maxRows = 0, fieldWidth = 0, labelWidth = 0;
                foreach (AjaxViewFieldLayout field in Layouts)
                {
                    Label lbl = new Label();
                    lbl.Name = string.Format("lbl{0}", field.FieldName);
                    lbl.Text = string.Format("{0}:({1})", field.Header, field.FieldEditor.ToString());
                    lbl.AutoSize = true;
                    lbl.BorderStyle = BorderStyle.FixedSingle;
                    lbl.BackColor = Color.Wheat;
                    fieldWidth = field.FieldWidth;
                    labelWidth = field.LabelWidth;
                    dicLabelWidth.Add(lbl.Name, field.EditorWidth);
                    this.Controls.Add(lbl);
                    maxWidth = Math.Max(maxWidth, lbl.Width);
                    maxHeight = Math.Max(maxHeight, lbl.Height);

                    if (field.NewLine)
                    {
                        rowMaxColumns = 1;
                        maxRows += 1;
                    }
                    else
                    {
                        rowMaxColumns += 1;
                    }
                    maxColumns = Math.Max(maxColumns, rowMaxColumns);
                }

                int x = 10, y = 10, maxX = 10 + (maxWidth + 10) * (maxColumns - 1), maxY = 10 + (maxHeight + 10) * (maxRows - 1);

                bool isFirstRow = true;
                for (int i = 0; i < Layouts.Count; i++)
                {
                    Label lbl = this.Controls[string.Format("lbl{0}", Layouts[i].FieldName)] as Label;
                    if (lbl != null)
                    {
                        lbl.AutoSize = false;
                        if (Layouts[i].NewLine)
                        {
                            if (!isFirstRow)
                            {
                                x = 10;
                                y += (maxHeight + 10);
                            }
                            isFirstRow = false;
                        }
                        else
                        {
                            x += (maxWidth + 10);
                        }
                        lbl.Location = new Point(x, y);

                        //if (i == Layouts.Count - 1 || Layouts[i + 1].NewLine)
                        //{
                        //    lbl.Width = maxX + maxWidth - x;
                        //}
                        //else
                        //{
                        //    lbl.Width = maxWidth;
                        //}
                        lbl.Width = (maxWidth + 10) * (labelWidth + dicLabelWidth[lbl.Name]) / fieldWidth;
                        lbl.Height = maxHeight;
                    }
                }

                this.Width = maxX + maxWidth + 10;
                this.Height = maxY + maxHeight + 37;
            }
            else if (ControlType == "grid")
            {
                DataGridView grid = new DataGridView();
                grid.AllowUserToAddRows = false;
                grid.ReadOnly = true;
                List<string> types = new List<string>();
                foreach (AjaxViewFieldLayout field in Layouts)
                {
                    DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                    column.Name = string.Format("col{0}", field.FieldName);
                    column.HeaderText = field.Header;
                    grid.Columns.Add(column);
                    types.Add(field.FieldEditor.ToString());
                }
                grid.Rows.Add(types.ToArray());
                grid.Dock = DockStyle.Fill;
                this.Controls.Add(grid);
                this.Width = 500;
                this.Height = 150;
            }
        }
    }

    public class AjaxViewFieldLayout
    {
        string _fieldName = "";
        string _header = "";
        int _fieldWidth = 0;
        int _labelWidth = 0;
        int _editorWidth = 0;
        bool _newLine = true;
        ExtGridEditor _fieldEditor = ExtGridEditor.TextBox;

        [DefaultValue("")]
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        [DefaultValue("")]
        public string Header
        {
            get { return _header; }
            set { _header = value; }
        }

        [DefaultValue(0)]
        public int FieldWidth
        {
            get { return _fieldWidth; }
            set { _fieldWidth = value; }
        }

        [DefaultValue(0)]
        public int LabelWidth
        {
            get { return _labelWidth; }
            set { _labelWidth = value; }
        }

        [DefaultValue(0)]
        public int EditorWidth
        {
            get { return _editorWidth; }
            set { _editorWidth = value; }
        }

        [DefaultValue(true)]
        public bool NewLine
        {
            get { return _newLine; }
            set { _newLine = value; }
        }

        [DefaultValue(typeof(ExtGridEditor), "TextBox")]
        public ExtGridEditor FieldEditor
        {
            get { return _fieldEditor; }
            set { _fieldEditor = value; }
        }
    }
}
