/*  filename:       QueryColumn.cs 
 *  version:        3.1
 *  lastedittime:   10:21 9/5/2006 
 *  remark:         
 *  1.  add coldef in designmode                        at 16:57 8/5/2006
 *  2.  correct the mistake of caption display          at 17:28 8/5/2006
 *  3.  change the messagebox and some default settings at 10:21 9/5/2006 
 */
using System;
using System.Collections;
using System.Text;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;
using System.Reflection;
using System.Runtime.Remoting;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.Drawing;
using System.Collections.Generic;
using System.IO;

namespace Srvtools
{
    public class QueryColumns : InfoOwnerCollectionItem,IGetValues
    {
        #region Constructor

        public QueryColumns()
            : this("clientquery", true, "", "", 120)
        {

        }
        
        public QueryColumns(string name, bool newline, string column, string caption, int width)
        {
            _name = name;
            _newline = newline;
            _column = column;
            _caption = caption;
            _width = width;
            _columntype = "ClientQueryTextBoxColumn";
            _condition = "And";
            _operator = "=";
            _textalign = "Left";
            _defaultvalue = "";
            _text = "";
            _DefaultMode = DefaultModeType.Initial;
        }

        #endregion

        #region Properties

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

        private bool _newline;
        public bool NewLine
        {
            get
            {
                return _newline;
            }
            set
            {
                _newline = value;
            }
        }
        
        private string _column;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string Column
        {
            get
            {
                return _column;
            }
           set
           {
                 _column = value;
                 if (this.Owner != null )
                 {
                     if (((ClientQuery)this.Owner).Site == null)
                     {
                         this.Caption = GetHeaderText(_column);
                     }
                     else if (((ClientQuery)this.Owner).Site.DesignMode)
                     {
                         this.Caption = GetHeaderText(_column);
                     }
                 }
            }
        }

        //public enum columntype
        //{
        //    ClientQueryTextBoxColumn,
        //    ClientQueryComboBoxColumn,
        //    ClientQueryRefValColumn,
        //    ClientQueryCalendarColumn
        //}
        //private columntype _columntype;
        private string _columntype;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("ClientQueryTextBoxColumn")]
        public string ColumnType
        {
            get
            {
                return _columntype;
            }
            set
            {
                _columntype = value;

                if (_columntype != "ClientQueryComboBoxColumn" && _columntype != "ClientQueryRefValColumn")
                {
                    _refval = null;
                }                   
            }
        }

        private string _caption;
        public string Caption
        {
            get
            {
                return _caption;
            }
            set
            {
                if (value != null && value != "")
                {
                    _caption = value;
                    _name = _caption;
                } 
                else
                {
                    if (_column != null && _column != "")
                    {
                        _caption = _column;
                        _name = _column;
                    }
                    else
                    {
                        _name = "clientquery";
                    }
                }
            }
        }

        private string _operator;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("=")]
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

        //public enum condition
        //{
        //    And,
        //    Or
        //}
        //private condition _condition;
        private string _condition;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("And")]
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
        
        private InfoRefVal _refval;
        public InfoRefVal InfoRefVal
        {
            get
            {
                return _refval;
            }
            set
            {
                if (_columntype != "ClientQueryComboBoxColumn" && _columntype != "ClientQueryRefValColumn" && _columntype != "ClientQueryRefButtonColumn" && value != null)
                {
                    MessageBox.Show("InfoRefval can be set only when\ncolumntype is combobox & refval.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _refval = value;
                }
            }
        }

        private string _externalRefVal;
        [Editor(typeof(ExternalRefvalEditor), typeof(UITypeEditor))] 
        public string ExternalRefVal
        {
            get
            {
                return _externalRefVal;
            }
            set
            {
                if (_columntype != "ClientQueryRefValColumn" && value != null)
                {
                    MessageBox.Show("InfoRefval can be set only when\ncolumntype is refval.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _externalRefVal = value;
                }
            }
        }

        private Panel _refButtonPanel;
        public Panel InfoRefButtonPanel
        {
            get
            {
                return _refButtonPanel;
            }
            set
            {
                if (_columntype != "ClientQueryRefButtonColumn" && value != null)
                {
                    MessageBox.Show("InfoRefButtonPanel can be set only when\ncolumntype is rebutton.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _refButtonPanel = value;
                }
            }
        }

        private bool _refButtonAutoPanel = false;
        [DefaultValue(false)]
        public bool InfoRefButtonAutoPanel
        {
            get
            {
                return _refButtonAutoPanel;
            }
            set
            {
                if (_columntype != "ClientQueryRefButtonColumn" && value != false)
                {
                    MessageBox.Show("InfoRefButtonAutoPanel can be set only when\ncolumntype is rebutton.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _refButtonAutoPanel = false;
                }
                else
                {
                    _refButtonAutoPanel = value;
                }
            }
        }

        private InfoDateTimeBox _infoDateTimeBox;

        public InfoDateTimeBox InfoDateTimeBox
        {
            get
            {
                return _infoDateTimeBox;
            }
            set
            {
                if (_columntype != "ClientQueryDateTimeBoxColumn" && value != null)
                {
                    MessageBox.Show("InfoDateTimeBox can be set only when\ncolumntype is datetimebox.", "notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    _infoDateTimeBox = value;
                }
            }
        }
	

        private string _defaultvalue;
        public string DefaultValue
        {
            get
            {
                return _defaultvalue;
            }
            set
            {
                _defaultvalue = value;
            }
        }

        public enum DefaultModeType
        {
            Initial,
            Focused
        }

        private DefaultModeType _DefaultMode;

        public DefaultModeType DefaultMode
        {
            get { return _DefaultMode; }
            set { _DefaultMode = value; }
        }
	

        private string _text;
        [Browsable(false)]
        public string Text
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

        //textalign
        //public enum textalign
        //{
        //    Left,
        //    Centor,
        //    Right
        //}
        //private textalign _textalign;
        private string _textalign;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [DefaultValue("Left")]
        public string TextAlign
        {
            get
            {
                return _textalign;
            }
            set
            {
                _textalign = value;
            }
        }

        private int _width;
        public int Width
        {
            get
            {
                return _width;
            }
            set
            {
                _width = value;
            }
        }

        private bool visible = true;

        public bool Visible
        {
            get { return visible; }
            set { visible = value; }
        }

        private bool isNvarChar;

        public bool IsNvarChar
        {
            get { return isNvarChar; }
            set { isNvarChar = value; }
        }

        #endregion

        #region method

        private string GetHeaderText(string ColName)
        {
            DataSet ds = ((QueryColumnsCollection)this.Collection).DsForDD;
            if (ds.Tables.Count == 0)
            {
                ((QueryColumnsCollection)this.Collection).DsForDD = DBUtils.GetDataDictionary(((ClientQuery)this.Owner).BindingSource, true);
                ds = ((QueryColumnsCollection)this.Collection).DsForDD;
            }
          
            string strHeaderText = "";
            if (ds != null && ds.Tables.Count > 0)
            {
                int i = ds.Tables[0].Rows.Count;
                for (int j = 0; j < i; j++)
                {
                    if (string.Compare(ds.Tables[0].Rows[j]["FIELD_NAME"].ToString(), ColName, true) == 0)//IgnoreCase
                    {
                        strHeaderText = ds.Tables[0].Rows[j]["CAPTION"].ToString();
                    }
                }
            }
            return strHeaderText;
        }


        public void setcolumn(string colname)
        {
            this._column = colname;
        
        }
      
        #endregion


        #region IGetValues Members

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            List<string> values = new List<string>();
            if(this.Owner is ClientQuery)
            {
                if (string.Compare(sKind, "operator", true) == 0)//IgnoreCase
                { 
                    values.Add("=");
                    values.Add("!=");
                    values.Add(">");
                    values.Add("<");
                    values.Add(">=");
                    values.Add("<=");
                    values.Add("%");
                    values.Add("%%");
                    values.Add("in");
                }
                else if (string.Compare(sKind, "condition", true) == 0)//IgnoreCase
                {
                    values.Add("And");
                    values.Add("Or");              
                }
                else if (string.Compare(sKind, "columntype", true) == 0)//IgnoreCase
                { 
                    values.Add("ClientQueryTextBoxColumn");
                    values.Add("ClientQueryComboBoxColumn");
                    values.Add("ClientQueryCheckBoxColumn");
                    values.Add("ClientQueryRefValColumn");
                    values.Add("ClientQueryCalendarColumn");
                    values.Add("ClientQueryRefButtonColumn");
                    values.Add("ClientQueryDateTimeBoxColumn");
                }
                else if (string.Compare(sKind, "textalign", true) == 0)//IgnoreCase
                { 
                    values.Add("Left");
                    values.Add("Center");
                    values.Add("Right");
                }
                else if (string.Compare(sKind, "column", true) == 0)//IgnoreCase
                {
                    ClientQuery cq = this.Owner as ClientQuery;
                    if (cq.BindingSource != null && cq.BindingSource.DataSource != null)
                    {

                        int colNum = ((InfoDataSet)cq.BindingSource.DataSource).RealDataSet.Tables[cq.BindingSource.DataMember].Columns.Count;

                        for (int i = 0; i < colNum; i++)
                        {
                            values.Add(((InfoDataSet)cq.BindingSource.DataSource).RealDataSet.Tables[cq.BindingSource.DataMember].Columns[i].ColumnName);
                        }

                   
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

        #endregion
    }
}