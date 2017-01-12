using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System;
using System.Data;

namespace Srvtools
{
    public class InfoDataGridViewRefButtonColumn : DataGridViewButtonColumn
    {
        public InfoDataGridViewRefButtonColumn()
            : base()
        {
            base.CellTemplate = new InfoDataGridViewRefButtonCell();
            ((InfoDataGridViewRefButtonCell)base.CellTemplate).RefButtonMatchs = new RefButtonMatchs(this, typeof(RefButtonMatch));

            DataGridViewCellStyle style = new DataGridViewCellStyle();
            style.BackColor = SystemColors.ButtonFace;
            style.ForeColor = SystemColors.ControlText;
            style.SelectionBackColor = SystemColors.ButtonFace;
            style.SelectionForeColor = SystemColors.ControlText;
            style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            base.DefaultCellStyle = style;
            base.Text = "R";
            base.FlatStyle = FlatStyle.System;
            base.UseColumnTextForButtonValue = true;
        }

        [Category("Infolight")]
        [TypeConverter(typeof(CollectionConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RefButtonMatchs RefButtonMatchs
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewRefButtonCell).RefButtonMatchs;
            }
            set
            {
                InfoDataGridViewRefButtonCell template = this.CellTemplate as InfoDataGridViewRefButtonCell;
                if (template != null)
                {
                    template.RefButtonMatchs = value;
                }

                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewRefButtonCell cell = row.Cells[base.Index] as InfoDataGridViewRefButtonCell;
                        if (cell != null)
                        {
                            cell.RefButtonMatchs = value;
                        }
                    }
                }
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool AutoPanel
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewRefButtonCell).AutoPanel;
            }
            set
            {
                InfoDataGridViewRefButtonCell template = this.CellTemplate as InfoDataGridViewRefButtonCell;
                if (template != null)
                {
                    template.AutoPanel = value;
                }

                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewRefButtonCell cell = row.Cells[base.Index] as InfoDataGridViewRefButtonCell;
                        if (cell != null)
                        {
                            cell.AutoPanel = value;
                        }
                    }
                }
            }
        }

        [Category("Infolight")]
        public Panel Panel
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewRefButtonCell).Panel;
            }
            set
            {
                InfoDataGridViewRefButtonCell template = this.CellTemplate as InfoDataGridViewRefButtonCell;
                if (template != null)
                {
                    template.Panel = value;
                }

                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewRefButtonCell cell = row.Cells[base.Index] as InfoDataGridViewRefButtonCell;
                        if (cell != null)
                        {
                            cell.Panel = value;
                        }
                    }
                }
            }
        }

        [Category("Infolight")]
        public InfoTranslate InfoTranslate
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewRefButtonCell).InfoTranslate;
            }
            set
            {
                InfoDataGridViewRefButtonCell template = this.CellTemplate as InfoDataGridViewRefButtonCell;
                if (template != null)
                {
                    template.InfoTranslate = value;
                }

                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewRefButtonCell cell = row.Cells[base.Index] as InfoDataGridViewRefButtonCell;
                        if (cell != null)
                        {
                            cell.InfoTranslate = value;
                        }
                    }
                }
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool MultiSelect
        {
            get
            {
                return (this.CellTemplate as InfoDataGridViewRefButtonCell).MultiSelect;
            }
            set
            {
                InfoDataGridViewRefButtonCell template = this.CellTemplate as InfoDataGridViewRefButtonCell;
                if (template != null)
                {
                    template.MultiSelect = value;
                }

                if (base.DataGridView != null)
                {
                    int rowCount = base.DataGridView.Rows.Count;
                    DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
                    for (int index = 0; index < rowCount; ++index)
                    {
                        DataGridViewRow row = rowCollection.SharedRow(index);
                        InfoDataGridViewRefButtonCell cell = row.Cells[base.Index] as InfoDataGridViewRefButtonCell;
                        if (cell != null)
                        {
                            cell.MultiSelect = value;
                        }
                    }
                }
            }
        }

        public override DataGridViewCell  CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value == null || value is InfoDataGridViewRefButtonCell)
                {
                    base.CellTemplate = value;
                }
                else
                {
                    throw new Exception("Must be a InfoDataGridViewRefButtonCell");
                }
            }
        }
    }

    public class InfoDataGridViewRefButtonCell : DataGridViewButtonCell
    {
        bool _autoPanel = true;
        Panel _panel;
        InfoTranslate _infoTranslate;
        bool _multiSelect = true;
        RefButtonMatchs _refButtonMatchs;

        [Category("Infolight")]
        [TypeConverter(typeof(CollectionConverter))]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public RefButtonMatchs RefButtonMatchs
        {
            get
            {
                return _refButtonMatchs;
            }
            set
            {
                _refButtonMatchs = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool AutoPanel
        {
            get
            {
                return _autoPanel;
            }
            set
            {
                _autoPanel = value;
            }
        }

        [Category("Infolight")]
        public Panel Panel
        {
            get
            {
                return _panel;
            }
            set
            {
                _panel = value;
            }
        }

        [Category("Infolight")]
        public InfoTranslate InfoTranslate
        {
            get
            {
                return _infoTranslate;
            }
            set
            {
                _infoTranslate = value;
            }
        }

        [Category("Infolight")]
        [DefaultValue(true)]
        public bool MultiSelect
        {
            get
            {
                return _multiSelect;
            }
            set
            {
                _multiSelect = value;
            }
        }

        public override object Clone()
        {
            InfoDataGridViewRefButtonCell cell = base.Clone() as InfoDataGridViewRefButtonCell;
            cell.RefButtonMatchs = this.RefButtonMatchs;
            cell.AutoPanel = this._autoPanel;
            cell.Panel = this._panel;
            cell.InfoTranslate = this._infoTranslate;
            cell.MultiSelect = this._multiSelect;

            return cell;
        }

        InfoRefPanel innerPanel;

        protected override void OnClick(DataGridViewCellEventArgs e)
        {
            base.OnClick(e);
            InfoDataGridViewRefButtonColumn refButtonColumn = this.OwningColumn as InfoDataGridViewRefButtonColumn;
            InfoRefButton refButton = new InfoRefButton();
            refButton.panel = this.Panel;
            refButton.infoTranslate = this.InfoTranslate;
            refButton.autoPanel = this.AutoPanel;
            refButton.multiSelect = this.MultiSelect;
            innerPanel = new InfoRefPanel(refButton);
            innerPanel.AfterOK += new EventHandler<AfterOKEventArgs>(innerPanel_AfterOK);
            innerPanel.ShowDialog();
        }

        void innerPanel_AfterOK(object sender, AfterOKEventArgs e)
        {
            if (e.Values.Count != this.RefButtonMatchs.Count)
            {
                throw new NotSupportedException("RefButtonMatchs item count must equal to InfoTranslate.RefReturnFields");
            }
            int i = 0;
            foreach (RefButtonMatch match in this.RefButtonMatchs)
            {
                this.DataGridView[match.matchColumnName, this.RowIndex].Value = e.Values[i];
                i++;
            }
        }
    }
}
