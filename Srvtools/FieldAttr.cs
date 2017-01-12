using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Windows.Forms.Design;
using System.Drawing.Design;
using System.Text.RegularExpressions;

using System.Data.OracleClient;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace Srvtools
{
    public class FieldAttr : InfoOwnerCollectionItem, IGetValues
    {
        private String _dataField;
        private Boolean _updateEnable;
        private Boolean _checkNull;
        private String _defaultValue;
        private DefaultModeType _defaultMode;
        private Boolean _whereMode;
        private int _TrimLength;

        public FieldAttr() : this("")
        {
            
        }

        public FieldAttr(String dataField)
        {
            _updateEnable = true;
            _whereMode = true;
            _checkNull = false;
            _dataField = dataField;
        }

        #region Properties

        [Category("Data"), Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public String DataField
        {
            get { return _dataField; }
            set { _dataField = value; }
        }

        [Category("Data")]
        public String DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }

        [Category("Design")]
        public Boolean UpdateEnable
        {
            get { return _updateEnable; }
            set { _updateEnable = value; }
        }

        [Category("Design")]
        public Boolean CheckNull
        {
            get { return _checkNull; }
            set { _checkNull = value; }
        }

        [Category("Design")]
        public DefaultModeType DefaultMode
        {
            get { return _defaultMode; }
            set { _defaultMode = value; }
        }

        [Category("Design")]
        public Boolean WhereMode
        {
            get { return _whereMode; }
            set { _whereMode = value; }
        }

        [Browsable(false)]
        public override string Name
        {
            get { return _dataField; }
            set { _dataField = value; }
        }

        [Category("Design")]
        public int TrimLength
        {
            get { return _TrimLength; }
            set { _TrimLength = value; }
        }
        [Category("Design")]
        public bool CharSetNull { get; set; }

        #endregion

        #region Method

        /// <summary>
        /// Override the ToString method.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return _dataField;
        }

        #endregion

        #region IGetValues

        public string[] GetValues(string sKind)
        {
            if (string.Compare(sKind, "datafield", true) == 0)//IgnoreCase
            {
                UpdateComponent updateComp = (UpdateComponent)Owner;

                InfoCommand myCmd = updateComp.SelectCmd;
                return myCmd.GetFields();
            }
            else
                return null;
        }

        #endregion

      }


    public enum DefaultModeType
    {
        /// <summary>
        /// Insert
        /// </summary>
        Insert = 0,

        /// <summary>
        /// Update
        /// </summary>
        Update = 1,

        /// <summary>
        /// Insert and update
        /// </summary>
        InsertAndUpdate
    }
}
