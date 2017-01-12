using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using System.Reflection;

using System.Data;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Data.Odbc;
using System.Data.OracleClient;
#if MySql
using MySql.Data.MySqlClient;
#endif

namespace Srvtools
{
    /// <summary>
    /// Summary description for InfoDataSource.
    /// </summary>
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(InfoDataSource), "Resources.InfoDataSource.ico")]
    public class InfoDataSource : InfoBaseComp, IInfoDataSource
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.Container components = null;

        public InfoDataSource(System.ComponentModel.IContainer container) 
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);
            InitializeComponent();
            _MasterColumns = new ColumnItems(this, typeof(ColumnItem));
            _DetailColumns = new ColumnItems(this, typeof(ColumnItem));

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public InfoDataSource()
        {
            ///
            /// This call is required by the Windows.Forms Designer.
            ///
            InitializeComponent();
            _MasterColumns = new ColumnItems(this, typeof(ColumnItem));
            _DetailColumns = new ColumnItems(this, typeof(ColumnItem));
            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }


        #region Component Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
        }
        #endregion

        #region master...
        private IDbCommand fMaster;
        [Category("Infolight"),
       Description("IDbCommand of master table")]
        public IDbCommand Master 
        {
			get
			{
				return fMaster;
			}
			set
			{
				fMaster = value;
			}
        }

        private ColumnItems _MasterColumns = null;
        [Category("Infolight"),
        Description("Specifies columns in master table associated with coloumns of detail table")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColumnItems MasterColumns
        {
            get
            {
                return _MasterColumns;
            }
            set
            {
                _MasterColumns = value;
            }
        }

        private ColumnItems _DetailColumns = null;
        [Category("Infolight"),
        Description("Specifies columns in detail table associated with coloumns of master table")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public ColumnItems DetailColumns
        {
            get
            {
                return _DetailColumns;
            }
            set
            {
                _DetailColumns = value;
            }
        }

        private bool _DynamicTableName = false;
        [Category("Infolight"),
        Description("")]
        public bool DynamicTableName
        {
            get
            {
                return _DynamicTableName;
            }
            set
            {
                _DynamicTableName = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string MasterColumn
        {
            get
            {
                string sRet = "";

                for (int i = 0; i < MasterColumns.Count; i++)
                {
                    if (i != MasterColumns.Count - 1)
                        sRet = sRet + ((ColumnItem)(MasterColumns[i])).FieldName + ";";
                    else
                        sRet = sRet + ((ColumnItem)(MasterColumns[i])).FieldName;
                }
                return sRet;
            }
        }
        #endregion

        #region detail...
        private IDbCommand fDetail;
        [Category("Infolight"),
        Description("IDbCommand of detail table")]
        public IDbCommand Detail
        {
            get
            {
                return fDetail;
            }
            set
            {
                fDetail = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string DetailColumn
        {
            get
            {
                string sRet = "";

                for (int i = 0; i < DetailColumns.Count; i++)
                {
                    if (i != DetailColumns.Count - 1)
                        sRet = sRet + ((ColumnItem)(DetailColumns[i])).FieldName + ";";
                    else
                        sRet = sRet + ((ColumnItem)(DetailColumns[i])).FieldName;
                }
                return sRet;
            }
        }
        #endregion

        public IDbCommand GetMaster()
        {
            return (IDbCommand)Master;
        }

        public IDbCommand GetDetail()
        {
            return (IDbCommand)Detail;
        }

        public string GetMasterColumn()
        {
            return MasterColumn;
        }

        public string GetDetailColumn()
        {
            return DetailColumn;
        }
    }

    public class ColumnItems : InfoOwnerCollection
    {
        public ColumnItems(Component aOwner, Type aItemType)
            : base(aOwner, typeof(ColumnItem))
        {

        }

        new public ColumnItem this[int index]
        {
            get
            {
                return (ColumnItem)InnerList[index];
            }
            set
            {
                if (index > -1 && index < Count)
                    if (value is ColumnItem)
                    {
                        //原来的Collection设置为0
                        ((ColumnItem)InnerList[index]).Collection = null;
                        InnerList[index] = value;
                        //Collection设置为this
                        ((ColumnItem)InnerList[index]).Collection = this;
                    }

            }
        }
    }

    public class ColumnItem : InfoOwnerCollectionItem, IGetValues
    {
        private string fFieldName = "";
        [Category("Infolight"),
        Description("Remote Name of InfolightDataSet"),
        Editor(typeof(PropertyDropDownEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string FieldName
        {
            get
            {
                return fFieldName;
            }
            set
            {
                fFieldName = value;
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Name
        {
            get
            {
                return FieldName;
            }

            set
            {
                FieldName = value;
            }
        }

        public string[] GetValues(string sKind)
        {
            if (sKind.Equals("FieldName"))
            {
                IInfoDataSource myDS = (IInfoDataSource)Owner;
                InfoCommand myCmd = null;
                if (Collection == ((InfoDataSource)myDS).MasterColumns)
                    myCmd = (InfoCommand)(myDS.GetMaster());
                else
                    myCmd = (InfoCommand)(myDS.GetDetail());
                return myCmd.GetFields();
            }
            else
                return new string[] { };
        }
    }
}
