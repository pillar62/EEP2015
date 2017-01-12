using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Design;

namespace Srvtools
{
	#region InfoDataGridViewComboBoxColumn
	public class InfoDataGridViewComboBoxColumn : DataGridViewComboBoxColumn
	{
		public InfoDataGridViewComboBoxColumn()
			: base()
		{
			base.CellTemplate = new InfoDataGridViewComboBoxCell();
		}

        [Category("Appearance")]
        public DataGridViewCellStyle HeaderCellStyle
        {
            get { return this.HeaderCell.Style; }
            set { this.HeaderCell.Style = value; }
        }

		[Category("Data")]
        public InfoRefVal RefValue
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new Exception("CellTemplate can not be null.");
				}

				return (this.CellTemplate as InfoDataGridViewComboBoxCell).RefValue;
			}
			set
			{
				if (this.CellTemplate == null)
				{
					throw new Exception("CellTemplate can not be null.");
				}

				InfoDataGridViewComboBoxCell template = this.CellTemplate as InfoDataGridViewComboBoxCell;
				if (template != null)
				{
					template.RefValue = value;
				}

				if (base.DataGridView != null)
				{
					int rowCount = base.DataGridView.Rows.Count;
					DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
					for (int index = 0; index < rowCount; ++index)
					{
						DataGridViewRow row = rowCollection.SharedRow(index);
						InfoDataGridViewComboBoxCell cell = row.Cells[base.Index] as InfoDataGridViewComboBoxCell;
						if (cell != null)
						{
							cell.RefValue = value;
						}
					}
				}
			}
		}

        [Browsable(false)]
        public new object DataSource
        {
            get { return base.DataSource; }
            set { base.DataSource = value; }
        }

        [Browsable(false)]
        public new string DisplayMember
        {
            get { return base.DisplayMember; }
            set { base.DisplayMember = value; }
        }

        [Browsable(false)]
        public new string ValueMember
        {
            get { return base.ValueMember; }
            set { base.ValueMember = value; }
        }

        [Browsable(false)]
        public string EditingDisplayMember
		{
			get
			{
				if (this.CellTemplate == null)
				{
					throw new Exception("CellTemplate can not be null.");
				}

				return (this.CellTemplate as InfoDataGridViewComboBoxCell).EditingDisplayMember;
			}
			set
			{
				if (this.CellTemplate == null)
				{
					throw new Exception("CellTemplate can not be null.");
				}

				InfoDataGridViewComboBoxCell template = this.CellTemplate as InfoDataGridViewComboBoxCell;
				if (template != null)
				{
					template.EditingDisplayMember = value;
				}

				if (base.DataGridView != null)
				{
					int rowCount = base.DataGridView.Rows.Count;
					DataGridViewRowCollection rowCollection = base.DataGridView.Rows;
					for (int index = 0; index < rowCount; ++index)
					{
						DataGridViewRow row = rowCollection.SharedRow(index);
						InfoDataGridViewComboBoxCell cell =
							row.Cells[base.Index] as InfoDataGridViewComboBoxCell;
						if (cell != null)
						{
							cell.EditingDisplayMember = value;
						}
					}
				}
			}
		}

		public override DataGridViewCell CellTemplate
		{
			get
			{
				return base.CellTemplate;
			}
			set
			{
				if (value == null || value is InfoDataGridViewComboBoxCell)
				{
					base.CellTemplate = value;
				}
				else
				{
                    throw new Exception("Must be a InfoDataGridViewComboBoxCell");
				}
			}
		}

		#region InfoDataGridViewComboBoxCell
		public class InfoDataGridViewComboBoxCell : DataGridViewComboBoxCell
		{
			public InfoDataGridViewComboBoxCell()
				: base()
			{
			}

			private InfoRefVal m_refValue = null;
			public InfoRefVal RefValue
			{
				get
				{
					return m_refValue;
				}
				set
				{
					m_refValue = value;
				}
			}

            override public string DisplayMember
			{
				get
				{
					if (this.RefValue == null)
					{
						return base.DisplayMember;
					}
					else
					{
						return this.RefValue.DisplayMember;
					}
				}
				set
				{
					base.DisplayMember = value;
				}
			}

            override public string ValueMember
			{
				get
				{
					if (this.RefValue == null)
					{
						return base.ValueMember;
					}
					else
					{
						return this.RefValue.ValueMember;
					}
				}
				set
				{
					base.ValueMember = value;
				}
			}

            override public object DataSource
			{
				get
				{
					if (this.RefValue == null)
					{
						return base.DataSource;
					}
					else
					{
						return this.RefValue.DataSource;
					}
				}
				set
				{
					base.DataSource = value;
				}
			}

            private string m_editinDisplayMember;
            public string EditingDisplayMember
			{
				get
				{
					if (this.RefValue == null)
					{
						return this.m_editinDisplayMember;
					}
					else
					{
						return this.RefValue.EditingDisplayMember;
					}
				}
				set
				{
					this.m_editinDisplayMember = value;
				}
			}

			public override Type EditType
			{
				get
				{
					return typeof(InfoDataGridViewComboBoxCellEditingControl);
				}
			}

			public override object Clone()
			{
				InfoDataGridViewComboBoxCell cell = base.Clone() as InfoDataGridViewComboBoxCell;
				cell.m_refValue = this.m_refValue;
				cell.m_editinDisplayMember = this.m_editinDisplayMember;

				(cell as InfoDataGridViewComboBoxCell).DataSource = base.DataSource;
				(cell as InfoDataGridViewComboBoxCell).ValueMember = base.ValueMember;
				(cell as InfoDataGridViewComboBoxCell).DisplayMember = base.DisplayMember;

				return cell;
			}
		}
		#endregion InfoDataGridViewComboBoxCell

		#region InfoDataGridViewComboBoxCellEditingControl
		private class InfoDataGridViewComboBoxCellEditingControl : DataGridViewComboBoxEditingControl
		{
			public InfoDataGridViewComboBoxCellEditingControl()
				: base()
			{
			}

			private DataColumn column = null;
			private DataTable table = null;
			private string prevDisplayMember = null;
			private InfoDataGridViewComboBoxCell cell = null;
            private object selectedValue = null;
			private string tableName = null;
			private string expression = null;
			private bool columnExists = false;

            private void InternalDataError(object sender, DataGridViewDataErrorEventArgs e)
            {
            }

            protected override void WndProc(ref Message m)
            {
                if (m.Msg == 0x020A && this.cell.RefValue.whereItem.Count > 0)//can not set filter when mouse wheel
                {
                    return;
                }
                base.WndProc(ref m);
            }

			protected override void OnDropDown(EventArgs e)
			{
				base.OnDropDown(e);

				if (/*(expression != null && expression.Trim() != "")
					&&*/ (cell.DisplayMember != null && cell.DisplayMember.Trim() != "")
					&& (cell.EditingDisplayMember != null && cell.EditingDisplayMember.Trim() != ""))
				{
                    if (this.cell.DataSource is InfoDataSet)
                    {
                        if (cell.DisplayMember.IndexOf('.') != -1)
                        {
                            tableName = cell.DisplayMember.Substring(0, cell.DisplayMember.IndexOf('.'));
                            table = (this.cell.DataSource as InfoDataSet).RealDataSet.Tables[tableName];
                        }
                    }
                    else if (this.cell.DataSource is InfoBindingSource)
                    {
                        tableName = ((InfoBindingSource)cell.DataSource).DataMember;
                        table = ((InfoDataSet)((InfoBindingSource)cell.DataSource).DataSource).RealDataSet.Tables[tableName];
                    }
                    if (table == null)
                    {
                        return;
                    }
                    //object originalValue = this.selectedValue;
                    this.selectedValue = this.SelectedValue;

                    #region DoSetWhere
                    string strFilter = string.Empty;
                    foreach (WhereItem wi in this.cell.RefValue.whereItem)
                    {
                        Type type = table.Columns[wi.FieldName].DataType;
                        string sWhereValue = this.cell.RefValue.GetValue(wi.Value);
                        if (wi.GetSign() != "like begin with value" && wi.GetSign() != "like with value")
                        {
                            if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32)
                            || type == typeof(UInt64) || type == typeof(int) || type == typeof(Int16)
                            || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single)
                            || type == typeof(Double) || type == typeof(Decimal))
                            {
                                strFilter += wi.FieldName + wi.GetSign() + sWhereValue + " and ";
                            }
                            else
                            {
                                strFilter += wi.FieldName + wi.GetSign() + "'" + sWhereValue + "' and ";
                            }
                        }
                        else
                        {
                            if (wi.GetSign() == "like begin with value")
                            {
                                strFilter += wi.FieldName + " like '" + sWhereValue + "%' and ";
                            }
                            if (wi.GetSign() == "like with value")
                            {
                                strFilter += wi.FieldName + " like '%" + sWhereValue + "%' and ";
                            }
                        }
                    }
                    if (strFilter != string.Empty)
                    {
                        strFilter = strFilter.Substring(0, strFilter.LastIndexOf(" and "));
                    }
                    InfoRefVal refVal = this.cell.RefValue;
                    if (refVal != null)
                    {
                        InfoDataSet dsAllData = (InfoDataSet)refVal.GetDataSource();
                        if (refVal.SelectAlias != null && refVal.SelectCommand != null && refVal.SelectAlias != "" && refVal.SelectCommand != "")
                        {
                            string strCommand = CliUtils.InsertWhere(refVal.SelectCommand, strFilter);//No need tolower
                            dsAllData.Execute(strCommand, true);
                        }
                        else
                        {
                            dsAllData.SetWhere(strFilter);
                        }
                    }
                    #endregion

                    //this.selectedValue = originalValue;

                    if (!string.IsNullOrEmpty(expression))
                    {
                        if (table.Columns[expression] == null)
                        {
                            column = table.Columns.Add();
                            column.Expression = expression;
                            column.ColumnName = expression;
                        }
                        else
                        {
                            columnExists = true;
                        }

                        this.EditingControlDataGridView.DataError += new DataGridViewDataErrorEventHandler(InternalDataError);
                        this.prevDisplayMember = cell.DisplayMember;
                        try
                        {
                            //selectedValue = this.SelectedValue;
                            if (cell.DataSource is InfoDataSet)
                            {
                                cell.DisplayMember = tableName + "." + expression;
                            }
                            else if (cell.DataSource is InfoBindingSource)
                            {
                                cell.DisplayMember = expression;
                            }
                            //this.SelectedValue = selectedValue;
                        }
                        finally
                        {
                            this.EditingControlDataGridView.DataError -= InternalDataError;
                        }
                    }
                    if (selectedValue != null)
                    {
                        this.SelectedValue = selectedValue;
                    }
                    else
                    {
                        SelectedIndex = -1;
                    }
				}
			}

			protected override void OnDropDownClosed(EventArgs e)
			{
				base.OnDropDownClosed(e);

				if (/*(expression != null && expression.Trim() != "")
					&&*/ (cell.DisplayMember != null && cell.DisplayMember.Trim() != "")
					&& (cell.EditingDisplayMember != null && cell.EditingDisplayMember.Trim() != ""))
				{
                    selectedValue = this.SelectedValue;
                    InfoRefVal refVal = this.cell.RefValue;
                    if (refVal != null)
                    {
                        InfoDataSet dsAllData = (InfoDataSet)refVal.GetDataSource();
                        if (refVal.SelectAlias != null && refVal.SelectCommand != null && refVal.SelectAlias != "" && refVal.SelectCommand != "")
                        {
                            dsAllData.Execute(refVal.SelectCommand, true);
                        }
                        else
                        {
                            dsAllData.ClearWhere();
                        }
                    }
                    if (!string.IsNullOrEmpty(expression))
                    {
                        //selectedValue = this.SelectedValue;
                        this.cell.DisplayMember = prevDisplayMember;
                        //this.SelectedValue = selectedValue;
                        if (!columnExists && table != null && column != null)
                        {
                            table.Columns.Remove(column);
                            column = null;
                        }
                    }
                    if (selectedValue != null)
                    {
                        this.SelectedValue = selectedValue;
                    }
                    else
                    {
                        SelectedIndex = -1;
                    }

                   
				}
			}

			protected override void OnEnter(EventArgs e)
			{
				base.OnEnter(e);

				cell = this.EditingControlDataGridView.CurrentCell as InfoDataGridViewComboBoxCell;
                if (cell.DisplayMember != null
                   && cell.ValueMember != null
                   && cell.DisplayMember != ""
                   && cell.ValueMember != "")
                {
                    if (cell.DisplayMember != cell.ValueMember)
                    {
                        string strValueMember = cell.ValueMember;
                        string strDisplayMember = cell.DisplayMember;
                        if (cell.RefValue.DataSource is InfoDataSet)
                        {
                            expression = strValueMember.Substring(strValueMember.IndexOf('.') + 1) + " + '(' +"
                                         + strDisplayMember.Substring(strDisplayMember.IndexOf('.') + 1) + " + ')'";
                        }
                        else if(cell.RefValue.DataSource is InfoBindingSource)
                        {
                            expression = strValueMember + " + '(' +" + strDisplayMember + " + ')'";
                        }
                    }
                    else
                    {
                        expression = "";
                    }
                }

                //if ((expression != null && expression.Trim() != "")
                //    && (cell.DisplayMember != null && cell.DisplayMember.Trim() != "")
                //    && (cell.EditingDisplayMember != null && cell.EditingDisplayMember.Trim() != ""))
                //{
					//prevDisplayMember = cell.DisplayMember;
                   // selectedValue = this.SelectedValue;
                   //// cell.DisplayMember = cell.EditingDisplayMember;
                   // try
                   // {
                   //     this.SelectedValue = selectedValue;
                   // }
                   // catch
                   // {
                   //     this.SelectedIndex = -1;
                   // }
                //}
			}

            //protected override void OnLeave(EventArgs e)
            //{
            //    base.OnLeave(e);

            //    if ((expression != null && expression.Trim() != "")
            //        && (cell.DisplayMember != null && cell.DisplayMember.Trim() != "")
            //        && (cell.EditingDisplayMember != null && cell.EditingDisplayMember.Trim() != ""))
            //    {
            //        selectedValue = this.SelectedValue;
            //        cell.DisplayMember = prevDisplayMember;
            //        this.SelectedValue = selectedValue;
            //    }
            //}
		}
		#endregion InfoDataGridViewComboBoxCellEditingControl
	}
	#endregion InfoDataGridViewComboBoxColumn
}