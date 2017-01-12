using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Data;

namespace Srvtools
{
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(InfoTranslate), "Resources.InfoTranslate.png")]
    public class InfoTranslate : InfoBaseComp
    {
        public InfoTranslate()
        {
            _whereitem = new TranslateWhereItemCollection(this, typeof(TranslateWhereItem));
            _refReturnFields = new TranslateRefReturnFieldsCollection(this, typeof(TranslateRefReturnFields));
        }

        public InfoTranslate(IContainer container)
            : this()
        {
            container.Add(this);
        }

        private InfoBindingSource _bindingsource;
        [AttributeProvider(typeof(IListSource))]
        [RefreshProperties(RefreshProperties.All)]
        [Category("Infolight"),
        Description("The InfoBindingSource which the control is bound to")]
        public InfoBindingSource BindingSource
        {
            get
            {
                return _bindingsource;
            }
            set
            {
                if (value != _bindingsource)
                {
                    _bindingsource = value;
                }
            }
        }

        private TranslateWhereItemCollection _whereitem;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Infolight"),
        Description("The columns which InfoTranslate is applied to")]
        public TranslateWhereItemCollection WhereItem
        {
            get
            {
                return _whereitem;
            }
            set
            {
                _whereitem = value;
            }
        }

        private TranslateRefReturnFieldsCollection _refReturnFields;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Category("Infolight"),
        Description("The columns which InfoTranslate is applied to")]
        public TranslateRefReturnFieldsCollection RefReturnFields
        {
            get
            {
                return _refReturnFields;
            }
            set
            {
                _refReturnFields = value;
            }
        }

        private object _text;
        [Browsable(false)]
        public object Text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        //private string designname;
        //[Browsable(false)]
        //public string DesignName
        //{
        //    get
        //    {
        //        return designname;
        //    }
        //    set
        //    {
        //        designname = value;
        //    }
        //}

        public void SetWhere()
        {
            string wherestring = "";
            string strCondition = "";
            string strOperator = "";
            if (this.Text == null)
            {
                return;
            }
            ArrayList querytext = (ArrayList)this.Text;
            int textcount = querytext.Count;
            int whereitemcount = this.WhereItem.Count;
            if (this.BindingSource != null && this.BindingSource.DataSource != null)
            {
                InfoDataSet ids = (this.BindingSource.DataSource as InfoDataSet);
                string sqlcmd = DBUtils.GetCommandText(this.BindingSource);

                for (int i = 0; i < textcount && i < whereitemcount; i++)
                {
                    strCondition = ((TranslateWhereItem)this.WhereItem[i]).Condition;
                    strOperator = ((TranslateWhereItem)this.WhereItem[i]).Operator;
                    if (wherestring == "")
                    {
                        strCondition = "";
                    }

                    if (querytext[i].ToString() != string.Empty)
                    {
                        Type type = ids.RealDataSet.Tables[this.BindingSource.DataMember].Columns[((TranslateWhereItem)this.WhereItem[i]).ColumnName].DataType;
                        if (((TranslateWhereItem)this.WhereItem[i]).Operator != "%" && ((TranslateWhereItem)this.WhereItem[i]).Operator != "%%")
                        {
                            if (type == typeof(uint) || type == typeof(UInt16) || type == typeof(UInt32)
                               || type == typeof(UInt64) || type == typeof(int) || type == typeof(Int16)
                               || type == typeof(Int32) || type == typeof(Int64) || type == typeof(Single)
                               || type == typeof(Double) || type == typeof(Decimal))
                            {
                                wherestring += " " + strCondition + CliUtils.GetTableNameForColumn(sqlcmd, ((TranslateWhereItem)this.WhereItem[i]).ColumnName)
                                    + strOperator + " " + querytext[i].ToString();
                            }
                            else
                            {
                                wherestring += " " + strCondition + CliUtils.GetTableNameForColumn(sqlcmd, ((TranslateWhereItem)this.WhereItem[i]).ColumnName)
                                    + strOperator + " '" + querytext[i].ToString().Replace("'", "''") + "'";
                            }
                        }
                        else
                        {
                            if (strOperator == "%")
                            {
                                wherestring += " " + strCondition + CliUtils.GetTableNameForColumn(sqlcmd, ((TranslateWhereItem)this.WhereItem[i]).ColumnName)
                                    + "like '" + querytext[i].ToString().Replace("'", "''") + "%'";
                            }
                            if (strOperator == "%%")
                            {
                                wherestring += " " + strCondition + CliUtils.GetTableNameForColumn(sqlcmd, ((TranslateWhereItem)this.WhereItem[i]).ColumnName)
                                    + "like '%" + querytext[i].ToString().Replace("'", "''") + "%'";
                            }
                        }
                    }
                }
                if (wherestring != "")
                {
                    ids.SetWhere(wherestring);
                }
            }
        }
    }

    public class TranslateWhereItemCollection : InfoOwnerCollection
    {
        public TranslateWhereItemCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(TranslateWhereItem))
        {

        }
        public new TranslateWhereItem this[int index]
        {
            get
            {
                return (TranslateWhereItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is TranslateWhereItem)
                    {
                        //原来的Collection设置为0
                        ((TranslateWhereItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((TranslateWhereItem)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class TranslateWhereItem : InfoOwnerCollectionItem, IGetValues
    {
        public TranslateWhereItem()
        {
            _columnname = "";
            _operator = "=";
            _condition = "And";
        }

        private string _name;
        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _columnname;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ColumnName
        {
            get
            {
                return _columnname;
            }
            set
            {
                _columnname = value;
                if (value != null && value != "")
                {
                    _name = value;
                }
            }
        }

        private string _operator;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string Operator
        {
            get
            {
                return _operator;
            }
            set
            {
                _operator = value;
            }
        }

        private string _condition;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string Condition
        {
            get
            {
                return _condition;
            }
            set
            {
                _condition = value;
            }
        }

        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is InfoTranslate)
            {
                if (string.Compare(sKind, "columnname", true) == 0)//IgnoreCase
                {
                    InfoTranslate itl = this.Owner as InfoTranslate;
                    if (itl.BindingSource != null && itl.BindingSource.DataSource != null)
                    {
                        if (itl.BindingSource.DataMember != null && itl.BindingSource.DataMember != "")
                        {
                            int colNum = ((InfoDataSet)itl.BindingSource.DataSource).RealDataSet.Tables[itl.BindingSource.DataMember].Columns.Count;
                            for (int i = 0; i < colNum; i++)
                            {
                                values.Add(((InfoDataSet)itl.BindingSource.DataSource).RealDataSet.Tables[itl.BindingSource.DataMember].Columns[i].ColumnName);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("select a BindingSource first.");
                    }
                }
                else if (string.Compare(sKind, "operator", true) == 0)//IgnoreCase
                {
                    values.Add("=");
                    values.Add("!=");
                    values.Add(">");
                    values.Add("<");
                    values.Add(">=");
                    values.Add("<=");
                    values.Add("%");
                    values.Add("%%");
                }
                else if (string.Compare(sKind, "condition", true) == 0)//IgnoreCase
                {
                    values.Add("And");
                    values.Add("Or");
                }
            }
            if (values.Count > 0)
            {
                int i = values.Count;
                retList = new string[i];
                for (int j = 0; j < i; j++)
                {
                    retList[j] = values[j];
                }
            }
            return retList;
        }
        #endregion
    }

    public class TranslateRefReturnFieldsCollection : InfoOwnerCollection
    {
        public TranslateRefReturnFieldsCollection(Object aOwner, Type aItemType)
            : base(aOwner, typeof(TranslateRefReturnFields))
        {

        }
        public new TranslateRefReturnFields this[int index]
        {
            get
            {
                return (TranslateRefReturnFields)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                {
                    if (value is TranslateRefReturnFields)
                    {
                        //原来的Collection设置为0
                        ((TranslateRefReturnFields)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((TranslateRefReturnFields)InnerList[index]).Collection = this;
                    }
                }
            }
        }
    }

    public class TranslateRefReturnFields : InfoOwnerCollectionItem, IGetValues
    {
        public TranslateRefReturnFields()
        {
            _columnname = "";
        }

        private string _name;
        public override string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _columnname;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string ColumnName
        {
            get
            {
                return _columnname;
            }
            set
            {
                _columnname = value;
                if (value != null && value != "")
                {
                    _name = value;
                }
            }
        }

        private string _displaycolumnname;
        [Editor(typeof(FieldNameEditor), typeof(UITypeEditor))]
        public string DisplayColumnName
        {
            get
            {
                return _displaycolumnname;
            }
            set
            {
                _displaycolumnname = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if (this.Owner is InfoTranslate)
            {
                if (string.Compare(sKind, "columnname", true) == 0 || string.Compare(sKind, "displaycolumnname", true) == 0)//IgnoreCase
                {
                    InfoTranslate itl = this.Owner as InfoTranslate;
                    if (itl.BindingSource != null && itl.BindingSource.DataSource != null)
                    {
                        if (itl.BindingSource.DataMember != null && itl.BindingSource.DataMember != "")
                        {
                            int colNum = ((InfoDataSet)itl.BindingSource.DataSource).RealDataSet.Tables[itl.BindingSource.DataMember].Columns.Count;
                            for (int i = 0; i < colNum; i++)
                            {
                                values.Add(((InfoDataSet)itl.BindingSource.DataSource).RealDataSet.Tables[itl.BindingSource.DataMember].Columns[i].ColumnName);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("select a BindingSource first.");
                    }
                }
            }
            if (values.Count > 0)
            {
                int i = values.Count;
                retList = new string[i];
                for (int j = 0; j < i; j++)
                {
                    retList[j] = values[j];
                }
            }
            return retList;
        }
    }

}
