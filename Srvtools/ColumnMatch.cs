using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Reflection;

namespace Srvtools
{
    public class ColumnMatch : InfoOwnerCollectionItem, IGetValues
    {
        public ColumnMatch()
            : this("", "", "")
        {
        }

        public ColumnMatch(string srcField, string strGetValue, string destField)
        {
            _SrcField = srcField;
            _SrcGetValue = strGetValue;
            _DestField = destField;
        }

        private string _SrcField;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string SrcField
        {
            get
            {
                return _SrcField;
            }
            set
            {
                _SrcField = value;
            }
        }

        private string _SrcGetValue;
        public string SrcGetValue
        {
            get
            {
                return _SrcGetValue;
            }
            set
            {
                _SrcGetValue = value;
            }
        }

        private string _DestField;
        [Editor(typeof(FieldNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string DestField
        {
            get
            {
                return _DestField;
            }
            set
            {
                _DestField = value;
            }
        }

        public override string Name
        {
            get { return _DestField; }
            set { _DestField = value; }
        }

        public override string ToString()
        {
            return _DestField;
        }

        #region IGetValues
        List<Control> ctrlList = new List<Control>();
        private void GetAllControls(Control.ControlCollection controlCollection)
        {
            foreach (Control ctrl in controlCollection)
            {
                ctrlList.Add(ctrl);
                if (ctrl.Controls.Count > 0)
                {
                    GetAllControls(ctrl.Controls);
                }
            }
        }

        public string[] GetValues(string sKind)
        {
            string[] retList = null;
            if (string.Compare(sKind, "srcfield", true) == 0)
            {
                if (this.Owner is InfoRefVal)
                {
                    if (((InfoRefVal)this.Owner).SelectCommand == null || ((InfoRefVal)this.Owner).SelectCommand == "")
                    {
                        if (((InfoRefVal)this.Owner).GetDataSource() != null)
                        {
                            InfoDataSet objDataSource = (InfoDataSet)((InfoRefVal)this.Owner).GetDataSource();
                            string strTabName = ((InfoRefVal)this.Owner).GetDataMember();
                            int i = objDataSource.RealDataSet.Tables[strTabName].Columns.Count;
                            string[] arrItems = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                arrItems[j] = objDataSource.GetRealDataSet().Tables[strTabName].Columns[j].ColumnName;
                            }
                            retList = arrItems;
                        }
                    }
                    else
                    {
                        if (((InfoRefVal)this.Owner).InnerBs != null && ((InfoRefVal)this.Owner).InnerBs.DataSource != null
                            && ((InfoDataSet)((InfoRefVal)this.Owner).InnerBs.DataSource).RealDataSet.Tables.Count > 0)
                        {
                            DataColumnCollection dcc = ((InfoDataSet)((InfoRefVal)this.Owner).InnerBs.DataSource).RealDataSet.Tables[0].Columns;
                            int i = dcc.Count;
                            string[] arrItems = new string[i];
                            for (int j = 0; j < i; j++)
                            {
                                arrItems[j] = dcc[j].ColumnName;
                            }
                            retList = arrItems;
                        }
                    }
                }
            }
            else if (string.Compare(sKind, "destfield", true) == 0)//IgnoreCase
            {
                object objDataSource = null;
                string strTabName = "";
                if (this.Owner is InfoRefVal)
                {
                    ctrlList.Clear();
                    ComponentCollection cc = ((InfoRefVal)this.Owner).Container.Components;
                    for (int j = 0; j < cc.Count; j++)
                    {
                        if (cc[j] is Form)
                        {
                            GetAllControls(((Form)cc[j]).Controls);
                        }
                    }
                    int m = ctrlList.Count;
                    for (int n = 0; n < m; n++)
                    {
                        /*if (ctrlList[n] is InfoTextBox)
                        {
                            InfoTextBox infoTxt = (InfoTextBox)ctrlList[n];
                            if (infoTxt.RefVal == (InfoRefVal)this.Owner)
                            {
                                objDataSource = infoTxt.GetDataSource();
                                strTabName = infoTxt.GetBindingMember();
                                break;
                            }
                        }*/
                        if (ctrlList[n] is InfoRefvalBox)
                        {
                            InfoRefvalBox infoRefBox = (InfoRefvalBox)ctrlList[n];
                            if (infoRefBox.RefVal == (InfoRefVal)this.Owner 
                                && infoRefBox.TextBoxBindingSource != null
                                && infoRefBox.TextBoxBindingSource is InfoBindingSource)
                            {
                                objDataSource = ((InfoBindingSource)infoRefBox.TextBoxBindingSource).GetDataSource();
                                strTabName = ((InfoBindingSource)infoRefBox.TextBoxBindingSource).DataMember;
                                break;
                            }
                        }
                        if (ctrlList[n] is InfoDataGridView)
                        {
                            InfoDataGridView dgv = (InfoDataGridView)ctrlList[n];
                            int x = dgv.Columns.Count;
                            bool bRefExisted = false;
                            for (int y = 0; y < x; y++)
                            {
                                if (dgv.Columns[y] is InfoDataGridViewRefValColumn)
                                {
                                    if (((InfoDataGridViewRefValColumn)dgv.Columns[y]).RefValue == (InfoRefVal)this.Owner)
                                    {
                                        objDataSource = dgv.GetDataSource();
                                        strTabName = dgv.GetDataTable();
                                        bRefExisted = true;
                                        break;
                                    }
                                }
                            }
                            if (bRefExisted)
                                break;
                        }
                    }
                }
                if (objDataSource != null && strTabName != "")
                {
                    int i = ((InfoDataSet)objDataSource).RealDataSet.Tables[strTabName].Columns.Count;
                    string[] arrItems = new string[i];
                    for (int j = 0; j < i; j++)
                    {
                        arrItems[j] = ((InfoDataSet)objDataSource).RealDataSet.Tables[strTabName].Columns[j].ColumnName;
                    }
                    retList = arrItems;
                }
            }
            return retList;
        }
        #endregion
    }
}