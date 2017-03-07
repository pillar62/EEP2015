using System;
using System.ComponentModel;
using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Reflection;
using Microsoft.Win32;
//using System.IO;
using Srvtools;
using System.Security;
using System.Security.Permissions;
using System.Threading;
using genieComponent;
using genieLibrary;
using Newtonsoft.Json;

namespace smOPOM02
{
    /// <summary>
    /// Summary description for Component.
    /// </summary>
    public class Component : DataModule
    {
        private ServiceManager serviceManager;
        private InfoConnection InfoConnection;
        private InfoCommand Master;
        private UpdateComponent ucMaster;
        private InfoCommand Detail;
        private UpdateComponent ucDetail;
        private InfoDataSource idsRelation;
        private InfoCommand View_Provider;
        private AutoNumber autoNumber1;
        private InfoCommand icTemp;
        private InfoCommand cmdOPO2INV5Query;
        private InfoCommand cmdBASSHIPMARK;
        private InfoTransaction tsMaster;
        private InfoCommand Report;
        private InfoCommand cmdOPO12OPO2;


        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

        public Component(System.ComponentModel.IContainer container)
        {
            ///
            /// Required for Windows.Forms Class Composition Designer support
            ///
            container.Add(this);
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        public Component()
        {
            ///
            /// This call is required by the Windows.Forms Designer.
            ///
            InitializeComponent();

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
            this.components = new System.ComponentModel.Container();
            Srvtools.Service service1 = new Srvtools.Service();
            Srvtools.Service service2 = new Srvtools.Service();
            Srvtools.Service service3 = new Srvtools.Service();
            Srvtools.Service service4 = new Srvtools.Service();
            Srvtools.Service service5 = new Srvtools.Service();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            Srvtools.KeyItem keyItem1 = new Srvtools.KeyItem();
            Srvtools.FieldAttr fieldAttr1 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr2 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr3 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr4 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr5 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr6 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr7 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr8 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr9 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr10 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr11 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr12 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr13 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr14 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr15 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr16 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr17 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr18 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr19 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr20 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr21 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr22 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr23 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr24 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr25 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr26 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr27 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr28 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr29 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr30 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr31 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr32 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr33 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr34 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr35 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr36 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr37 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr38 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr39 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr40 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr41 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr42 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr43 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr44 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr45 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr46 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr47 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr48 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr49 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr50 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr51 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr52 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr53 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr54 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr55 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr56 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr57 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr58 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr59 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr60 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr61 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr62 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr63 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr64 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr65 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr66 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr67 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr68 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr69 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr70 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr71 = new Srvtools.FieldAttr();
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.ColumnItem columnItem1 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem2 = new Srvtools.ColumnItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem7 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem8 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem9 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem10 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem11 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem12 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem13 = new Srvtools.KeyItem();
            this.serviceManager = new Srvtools.ServiceManager(this.components);
            this.InfoConnection = new Srvtools.InfoConnection(this.components);
            this.Master = new Srvtools.InfoCommand(this.components);
            this.ucMaster = new Srvtools.UpdateComponent(this.components);
            this.Detail = new Srvtools.InfoCommand(this.components);
            this.ucDetail = new Srvtools.UpdateComponent(this.components);
            this.idsRelation = new Srvtools.InfoDataSource(this.components);
            this.View_Provider = new Srvtools.InfoCommand(this.components);
            this.autoNumber1 = new Srvtools.AutoNumber(this.components);
            this.icTemp = new Srvtools.InfoCommand(this.components);
            this.cmdOPO2INV5Query = new Srvtools.InfoCommand(this.components);
            this.cmdBASSHIPMARK = new Srvtools.InfoCommand(this.components);
            this.tsMaster = new Srvtools.InfoTransaction(this.components);
            this.Report = new Srvtools.InfoCommand(this.components);
            this.cmdOPO12OPO2 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Master)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Detail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_Provider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.icTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdOPO2INV5Query)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdBASSHIPMARK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Report)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdOPO12OPO2)).BeginInit();
            // 
            // serviceManager
            // 
            service1.DelegateName = "sqlRebuild";
            service1.NonLogin = false;
            service1.ServiceName = "sqlRebuild";
            service2.DelegateName = "WriteBackToBAS01";
            service2.NonLogin = false;
            service2.ServiceName = "WriteBackToBAS01";
            service3.DelegateName = "OPOtoINVtoDB";
            service3.NonLogin = false;
            service3.ServiceName = "OPOtoINVtoDB";
            service4.DelegateName = "CheckPRODUCTID";
            service4.NonLogin = false;
            service4.ServiceName = "CheckPRODUCTID";
            service5.DelegateName = "openBOMPRODUCT";
            service5.NonLogin = false;
            service5.ServiceName = "openBOMPRODUCT";
            this.serviceManager.ServiceCollection.Add(service1);
            this.serviceManager.ServiceCollection.Add(service2);
            this.serviceManager.ServiceCollection.Add(service3);
            this.serviceManager.ServiceCollection.Add(service4);
            this.serviceManager.ServiceCollection.Add(service5);
            // 
            // InfoConnection
            // 
            this.InfoConnection.EEPAlias = "genie03";
            // 
            // Master
            // 
            this.Master.CacheConnection = false;
            this.Master.CommandText = resources.GetString("Master.CommandText");
            this.Master.CommandTimeout = 30;
            this.Master.CommandType = System.Data.CommandType.Text;
            this.Master.DynamicTableName = false;
            this.Master.EEPAlias = null;
            this.Master.EncodingAfter = null;
            this.Master.EncodingBefore = "Windows-1252";
            this.Master.EncodingConvert = null;
            this.Master.InfoConnection = this.InfoConnection;
            keyItem1.KeyName = "BILLNO";
            this.Master.KeyFields.Add(keyItem1);
            this.Master.MultiSetWhere = false;
            this.Master.Name = "Master";
            this.Master.NotificationAutoEnlist = false;
            this.Master.SecExcept = "";
            this.Master.SecFieldName = null;
            this.Master.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.Master.SelectPaging = true;
            this.Master.SelectTop = 0;
            this.Master.SiteControl = false;
            this.Master.SiteFieldName = null;
            this.Master.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // ucMaster
            // 
            this.ucMaster.AutoTrans = true;
            this.ucMaster.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "BILLDATE";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = true;
            fieldAttr2.DataField = "BILLNO";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "SDTBILLNO";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "ISSIGN";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "BILLTYPE";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "REFBILLNO";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "ONHANDDATE";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "CUSTID";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "CORPMEMO";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "DELIVERADDRESS";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "PERSONID";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "WAREID";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "DEPARTMENTID";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "CONNECTPERSON";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "CURRENCYID";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "CURRRATE";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "INVOICETYPE";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "TAXCLASS";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "TOTALAMOUNT";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "TAXAMOUNT";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "SUMQUALITY";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "SUMAMOUNT";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            fieldAttr23.CharSetNull = false;
            fieldAttr23.CheckNull = false;
            fieldAttr23.DataField = "MEMODOC";
            fieldAttr23.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr23.DefaultValue = null;
            fieldAttr23.TrimLength = 0;
            fieldAttr23.UpdateEnable = true;
            fieldAttr23.WhereMode = true;
            fieldAttr24.CharSetNull = false;
            fieldAttr24.CheckNull = false;
            fieldAttr24.DataField = "PAYMENTWAY";
            fieldAttr24.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr24.DefaultValue = null;
            fieldAttr24.TrimLength = 0;
            fieldAttr24.UpdateEnable = true;
            fieldAttr24.WhereMode = true;
            fieldAttr25.CharSetNull = false;
            fieldAttr25.CheckNull = false;
            fieldAttr25.DataField = "SHIPMENT";
            fieldAttr25.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr25.DefaultValue = null;
            fieldAttr25.TrimLength = 0;
            fieldAttr25.UpdateEnable = true;
            fieldAttr25.WhereMode = true;
            fieldAttr26.CharSetNull = false;
            fieldAttr26.CheckNull = false;
            fieldAttr26.DataField = "VALIDITYDATE";
            fieldAttr26.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr26.DefaultValue = null;
            fieldAttr26.TrimLength = 0;
            fieldAttr26.UpdateEnable = true;
            fieldAttr26.WhereMode = true;
            fieldAttr27.CharSetNull = false;
            fieldAttr27.CheckNull = false;
            fieldAttr27.DataField = "VALIDITYDOC";
            fieldAttr27.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr27.DefaultValue = null;
            fieldAttr27.TrimLength = 0;
            fieldAttr27.UpdateEnable = true;
            fieldAttr27.WhereMode = true;
            fieldAttr28.CharSetNull = false;
            fieldAttr28.CheckNull = false;
            fieldAttr28.DataField = "BILLHEADNO";
            fieldAttr28.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr28.DefaultValue = null;
            fieldAttr28.TrimLength = 0;
            fieldAttr28.UpdateEnable = true;
            fieldAttr28.WhereMode = true;
            fieldAttr29.CharSetNull = false;
            fieldAttr29.CheckNull = false;
            fieldAttr29.DataField = "BILLTAILNO";
            fieldAttr29.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr29.DefaultValue = null;
            fieldAttr29.TrimLength = 0;
            fieldAttr29.UpdateEnable = true;
            fieldAttr29.WhereMode = true;
            fieldAttr30.CharSetNull = false;
            fieldAttr30.CheckNull = false;
            fieldAttr30.DataField = "FLOWFLAG";
            fieldAttr30.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr30.DefaultValue = null;
            fieldAttr30.TrimLength = 0;
            fieldAttr30.UpdateEnable = true;
            fieldAttr30.WhereMode = true;
            fieldAttr31.CharSetNull = false;
            fieldAttr31.CheckNull = false;
            fieldAttr31.DataField = "DATAEXTEND1";
            fieldAttr31.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr31.DefaultValue = null;
            fieldAttr31.TrimLength = 0;
            fieldAttr31.UpdateEnable = true;
            fieldAttr31.WhereMode = true;
            fieldAttr32.CharSetNull = false;
            fieldAttr32.CheckNull = false;
            fieldAttr32.DataField = "DATAEXTEND2";
            fieldAttr32.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr32.DefaultValue = null;
            fieldAttr32.TrimLength = 0;
            fieldAttr32.UpdateEnable = true;
            fieldAttr32.WhereMode = true;
            fieldAttr33.CharSetNull = false;
            fieldAttr33.CheckNull = false;
            fieldAttr33.DataField = "DATAEXTEND3";
            fieldAttr33.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr33.DefaultValue = null;
            fieldAttr33.TrimLength = 0;
            fieldAttr33.UpdateEnable = true;
            fieldAttr33.WhereMode = true;
            fieldAttr34.CharSetNull = false;
            fieldAttr34.CheckNull = false;
            fieldAttr34.DataField = "DATAEXTEND4";
            fieldAttr34.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr34.DefaultValue = null;
            fieldAttr34.TrimLength = 0;
            fieldAttr34.UpdateEnable = true;
            fieldAttr34.WhereMode = true;
            fieldAttr35.CharSetNull = false;
            fieldAttr35.CheckNull = false;
            fieldAttr35.DataField = "DATAEXTEND5";
            fieldAttr35.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr35.DefaultValue = null;
            fieldAttr35.TrimLength = 0;
            fieldAttr35.UpdateEnable = true;
            fieldAttr35.WhereMode = true;
            fieldAttr36.CharSetNull = false;
            fieldAttr36.CheckNull = false;
            fieldAttr36.DataField = "DATAEXTEND6";
            fieldAttr36.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr36.DefaultValue = null;
            fieldAttr36.TrimLength = 0;
            fieldAttr36.UpdateEnable = true;
            fieldAttr36.WhereMode = true;
            fieldAttr37.CharSetNull = false;
            fieldAttr37.CheckNull = false;
            fieldAttr37.DataField = "DATAEXTEND7";
            fieldAttr37.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr37.DefaultValue = null;
            fieldAttr37.TrimLength = 0;
            fieldAttr37.UpdateEnable = true;
            fieldAttr37.WhereMode = true;
            fieldAttr38.CharSetNull = false;
            fieldAttr38.CheckNull = false;
            fieldAttr38.DataField = "DATAEXTEND8";
            fieldAttr38.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr38.DefaultValue = null;
            fieldAttr38.TrimLength = 0;
            fieldAttr38.UpdateEnable = true;
            fieldAttr38.WhereMode = true;
            fieldAttr39.CharSetNull = false;
            fieldAttr39.CheckNull = false;
            fieldAttr39.DataField = "CREATE_USER";
            fieldAttr39.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr39.DefaultValue = "_usercode";
            fieldAttr39.TrimLength = 0;
            fieldAttr39.UpdateEnable = true;
            fieldAttr39.WhereMode = true;
            fieldAttr40.CharSetNull = false;
            fieldAttr40.CheckNull = false;
            fieldAttr40.DataField = "CREATE_DATE";
            fieldAttr40.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr40.DefaultValue = "_sysdate";
            fieldAttr40.TrimLength = 0;
            fieldAttr40.UpdateEnable = true;
            fieldAttr40.WhereMode = true;
            fieldAttr41.CharSetNull = false;
            fieldAttr41.CheckNull = false;
            fieldAttr41.DataField = "UPDATE_USER";
            fieldAttr41.DefaultMode = Srvtools.DefaultModeType.Update;
            fieldAttr41.DefaultValue = "_usercode";
            fieldAttr41.TrimLength = 0;
            fieldAttr41.UpdateEnable = true;
            fieldAttr41.WhereMode = true;
            fieldAttr42.CharSetNull = false;
            fieldAttr42.CheckNull = false;
            fieldAttr42.DataField = "UPDATE_DATE";
            fieldAttr42.DefaultMode = Srvtools.DefaultModeType.Update;
            fieldAttr42.DefaultValue = "_sysdate";
            fieldAttr42.TrimLength = 0;
            fieldAttr42.UpdateEnable = true;
            fieldAttr42.WhereMode = true;
            fieldAttr43.CharSetNull = false;
            fieldAttr43.CheckNull = false;
            fieldAttr43.DataField = "SIGN_USER";
            fieldAttr43.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr43.DefaultValue = null;
            fieldAttr43.TrimLength = 0;
            fieldAttr43.UpdateEnable = true;
            fieldAttr43.WhereMode = true;
            fieldAttr44.CharSetNull = false;
            fieldAttr44.CheckNull = false;
            fieldAttr44.DataField = "MERGETYPE";
            fieldAttr44.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr44.DefaultValue = null;
            fieldAttr44.TrimLength = 0;
            fieldAttr44.UpdateEnable = true;
            fieldAttr44.WhereMode = true;
            fieldAttr45.CharSetNull = false;
            fieldAttr45.CheckNull = false;
            fieldAttr45.DataField = "BILLNO";
            fieldAttr45.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr45.DefaultValue = null;
            fieldAttr45.TrimLength = 0;
            fieldAttr45.UpdateEnable = true;
            fieldAttr45.WhereMode = true;
            fieldAttr46.CharSetNull = false;
            fieldAttr46.CheckNull = false;
            fieldAttr46.DataField = "SEQNO";
            fieldAttr46.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr46.DefaultValue = null;
            fieldAttr46.TrimLength = 0;
            fieldAttr46.UpdateEnable = true;
            fieldAttr46.WhereMode = true;
            fieldAttr47.CharSetNull = false;
            fieldAttr47.CheckNull = false;
            fieldAttr47.DataField = "WAREID";
            fieldAttr47.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr47.DefaultValue = null;
            fieldAttr47.TrimLength = 0;
            fieldAttr47.UpdateEnable = true;
            fieldAttr47.WhereMode = true;
            fieldAttr48.CharSetNull = false;
            fieldAttr48.CheckNull = false;
            fieldAttr48.DataField = "CUSTID";
            fieldAttr48.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr48.DefaultValue = null;
            fieldAttr48.TrimLength = 0;
            fieldAttr48.UpdateEnable = true;
            fieldAttr48.WhereMode = true;
            fieldAttr49.CharSetNull = false;
            fieldAttr49.CheckNull = false;
            fieldAttr49.DataField = "PRODUCTID";
            fieldAttr49.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr49.DefaultValue = null;
            fieldAttr49.TrimLength = 0;
            fieldAttr49.UpdateEnable = true;
            fieldAttr49.WhereMode = true;
            fieldAttr50.CharSetNull = false;
            fieldAttr50.CheckNull = false;
            fieldAttr50.DataField = "PRODCNAME";
            fieldAttr50.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr50.DefaultValue = null;
            fieldAttr50.TrimLength = 0;
            fieldAttr50.UpdateEnable = true;
            fieldAttr50.WhereMode = true;
            fieldAttr51.CharSetNull = false;
            fieldAttr51.CheckNull = false;
            fieldAttr51.DataField = "PRODENAME";
            fieldAttr51.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr51.DefaultValue = null;
            fieldAttr51.TrimLength = 0;
            fieldAttr51.UpdateEnable = true;
            fieldAttr51.WhereMode = true;
            fieldAttr52.CharSetNull = false;
            fieldAttr52.CheckNull = false;
            fieldAttr52.DataField = "PRODSTRUCTURE";
            fieldAttr52.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr52.DefaultValue = null;
            fieldAttr52.TrimLength = 0;
            fieldAttr52.UpdateEnable = true;
            fieldAttr52.WhereMode = true;
            fieldAttr53.CharSetNull = false;
            fieldAttr53.CheckNull = false;
            fieldAttr53.DataField = "QUANTITY";
            fieldAttr53.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr53.DefaultValue = null;
            fieldAttr53.TrimLength = 0;
            fieldAttr53.UpdateEnable = true;
            fieldAttr53.WhereMode = true;
            fieldAttr54.CharSetNull = false;
            fieldAttr54.CheckNull = false;
            fieldAttr54.DataField = "UNIT";
            fieldAttr54.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr54.DefaultValue = null;
            fieldAttr54.TrimLength = 0;
            fieldAttr54.UpdateEnable = true;
            fieldAttr54.WhereMode = true;
            fieldAttr55.CharSetNull = false;
            fieldAttr55.CheckNull = false;
            fieldAttr55.DataField = "PRICE";
            fieldAttr55.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr55.DefaultValue = null;
            fieldAttr55.TrimLength = 0;
            fieldAttr55.UpdateEnable = true;
            fieldAttr55.WhereMode = true;
            fieldAttr56.CharSetNull = false;
            fieldAttr56.CheckNull = false;
            fieldAttr56.DataField = "TAXPRICE";
            fieldAttr56.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr56.DefaultValue = null;
            fieldAttr56.TrimLength = 0;
            fieldAttr56.UpdateEnable = true;
            fieldAttr56.WhereMode = true;
            fieldAttr57.CharSetNull = false;
            fieldAttr57.CheckNull = false;
            fieldAttr57.DataField = "PRODCOST";
            fieldAttr57.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr57.DefaultValue = null;
            fieldAttr57.TrimLength = 0;
            fieldAttr57.UpdateEnable = true;
            fieldAttr57.WhereMode = true;
            fieldAttr58.CharSetNull = false;
            fieldAttr58.CheckNull = false;
            fieldAttr58.DataField = "SUBAMOUNT";
            fieldAttr58.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr58.DefaultValue = null;
            fieldAttr58.TrimLength = 0;
            fieldAttr58.UpdateEnable = true;
            fieldAttr58.WhereMode = true;
            fieldAttr59.CharSetNull = false;
            fieldAttr59.CheckNull = false;
            fieldAttr59.DataField = "TAXAMOUNT";
            fieldAttr59.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr59.DefaultValue = null;
            fieldAttr59.TrimLength = 0;
            fieldAttr59.UpdateEnable = true;
            fieldAttr59.WhereMode = true;
            fieldAttr60.CharSetNull = false;
            fieldAttr60.CheckNull = false;
            fieldAttr60.DataField = "ONHANDDATE";
            fieldAttr60.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr60.DefaultValue = null;
            fieldAttr60.TrimLength = 0;
            fieldAttr60.UpdateEnable = true;
            fieldAttr60.WhereMode = true;
            fieldAttr61.CharSetNull = false;
            fieldAttr61.CheckNull = false;
            fieldAttr61.DataField = "ASSEMBLEQTY";
            fieldAttr61.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr61.DefaultValue = null;
            fieldAttr61.TrimLength = 0;
            fieldAttr61.UpdateEnable = true;
            fieldAttr61.WhereMode = true;
            fieldAttr62.CharSetNull = false;
            fieldAttr62.CheckNull = false;
            fieldAttr62.DataField = "SOURCEBILLNO";
            fieldAttr62.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr62.DefaultValue = null;
            fieldAttr62.TrimLength = 0;
            fieldAttr62.UpdateEnable = true;
            fieldAttr62.WhereMode = true;
            fieldAttr63.CharSetNull = false;
            fieldAttr63.CheckNull = false;
            fieldAttr63.DataField = "SOURCEUNIQUENO";
            fieldAttr63.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr63.DefaultValue = null;
            fieldAttr63.TrimLength = 0;
            fieldAttr63.UpdateEnable = true;
            fieldAttr63.WhereMode = true;
            fieldAttr64.CharSetNull = false;
            fieldAttr64.CheckNull = false;
            fieldAttr64.DataField = "ISNEEDMADE";
            fieldAttr64.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr64.DefaultValue = null;
            fieldAttr64.TrimLength = 0;
            fieldAttr64.UpdateEnable = true;
            fieldAttr64.WhereMode = true;
            fieldAttr65.CharSetNull = false;
            fieldAttr65.CheckNull = false;
            fieldAttr65.DataField = "MEMOCHAR1";
            fieldAttr65.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr65.DefaultValue = null;
            fieldAttr65.TrimLength = 0;
            fieldAttr65.UpdateEnable = true;
            fieldAttr65.WhereMode = true;
            fieldAttr66.CharSetNull = false;
            fieldAttr66.CheckNull = false;
            fieldAttr66.DataField = "MEMOCHAR2";
            fieldAttr66.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr66.DefaultValue = null;
            fieldAttr66.TrimLength = 0;
            fieldAttr66.UpdateEnable = true;
            fieldAttr66.WhereMode = true;
            fieldAttr67.CharSetNull = false;
            fieldAttr67.CheckNull = false;
            fieldAttr67.DataField = "ISGIFTPROD";
            fieldAttr67.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr67.DefaultValue = null;
            fieldAttr67.TrimLength = 0;
            fieldAttr67.UpdateEnable = true;
            fieldAttr67.WhereMode = true;
            fieldAttr68.CharSetNull = false;
            fieldAttr68.CheckNull = false;
            fieldAttr68.DataField = "BOXQTY";
            fieldAttr68.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr68.DefaultValue = null;
            fieldAttr68.TrimLength = 0;
            fieldAttr68.UpdateEnable = true;
            fieldAttr68.WhereMode = true;
            fieldAttr69.CharSetNull = false;
            fieldAttr69.CheckNull = false;
            fieldAttr69.DataField = "BOXPRICE";
            fieldAttr69.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr69.DefaultValue = null;
            fieldAttr69.TrimLength = 0;
            fieldAttr69.UpdateEnable = true;
            fieldAttr69.WhereMode = true;
            fieldAttr70.CharSetNull = false;
            fieldAttr70.CheckNull = false;
            fieldAttr70.DataField = "UNIQUENO";
            fieldAttr70.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr70.DefaultValue = null;
            fieldAttr70.TrimLength = 0;
            fieldAttr70.UpdateEnable = true;
            fieldAttr70.WhereMode = true;
            fieldAttr71.CharSetNull = false;
            fieldAttr71.CheckNull = false;
            fieldAttr71.DataField = "EXTDOC";
            fieldAttr71.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr71.DefaultValue = null;
            fieldAttr71.TrimLength = 0;
            fieldAttr71.UpdateEnable = false;
            fieldAttr71.WhereMode = true;
            this.ucMaster.FieldAttrs.Add(fieldAttr1);
            this.ucMaster.FieldAttrs.Add(fieldAttr2);
            this.ucMaster.FieldAttrs.Add(fieldAttr3);
            this.ucMaster.FieldAttrs.Add(fieldAttr4);
            this.ucMaster.FieldAttrs.Add(fieldAttr5);
            this.ucMaster.FieldAttrs.Add(fieldAttr6);
            this.ucMaster.FieldAttrs.Add(fieldAttr7);
            this.ucMaster.FieldAttrs.Add(fieldAttr8);
            this.ucMaster.FieldAttrs.Add(fieldAttr9);
            this.ucMaster.FieldAttrs.Add(fieldAttr10);
            this.ucMaster.FieldAttrs.Add(fieldAttr11);
            this.ucMaster.FieldAttrs.Add(fieldAttr12);
            this.ucMaster.FieldAttrs.Add(fieldAttr13);
            this.ucMaster.FieldAttrs.Add(fieldAttr14);
            this.ucMaster.FieldAttrs.Add(fieldAttr15);
            this.ucMaster.FieldAttrs.Add(fieldAttr16);
            this.ucMaster.FieldAttrs.Add(fieldAttr17);
            this.ucMaster.FieldAttrs.Add(fieldAttr18);
            this.ucMaster.FieldAttrs.Add(fieldAttr19);
            this.ucMaster.FieldAttrs.Add(fieldAttr20);
            this.ucMaster.FieldAttrs.Add(fieldAttr21);
            this.ucMaster.FieldAttrs.Add(fieldAttr22);
            this.ucMaster.FieldAttrs.Add(fieldAttr23);
            this.ucMaster.FieldAttrs.Add(fieldAttr24);
            this.ucMaster.FieldAttrs.Add(fieldAttr25);
            this.ucMaster.FieldAttrs.Add(fieldAttr26);
            this.ucMaster.FieldAttrs.Add(fieldAttr27);
            this.ucMaster.FieldAttrs.Add(fieldAttr28);
            this.ucMaster.FieldAttrs.Add(fieldAttr29);
            this.ucMaster.FieldAttrs.Add(fieldAttr30);
            this.ucMaster.FieldAttrs.Add(fieldAttr31);
            this.ucMaster.FieldAttrs.Add(fieldAttr32);
            this.ucMaster.FieldAttrs.Add(fieldAttr33);
            this.ucMaster.FieldAttrs.Add(fieldAttr34);
            this.ucMaster.FieldAttrs.Add(fieldAttr35);
            this.ucMaster.FieldAttrs.Add(fieldAttr36);
            this.ucMaster.FieldAttrs.Add(fieldAttr37);
            this.ucMaster.FieldAttrs.Add(fieldAttr38);
            this.ucMaster.FieldAttrs.Add(fieldAttr39);
            this.ucMaster.FieldAttrs.Add(fieldAttr40);
            this.ucMaster.FieldAttrs.Add(fieldAttr41);
            this.ucMaster.FieldAttrs.Add(fieldAttr42);
            this.ucMaster.FieldAttrs.Add(fieldAttr43);
            this.ucMaster.FieldAttrs.Add(fieldAttr44);
            this.ucMaster.FieldAttrs.Add(fieldAttr45);
            this.ucMaster.FieldAttrs.Add(fieldAttr46);
            this.ucMaster.FieldAttrs.Add(fieldAttr47);
            this.ucMaster.FieldAttrs.Add(fieldAttr48);
            this.ucMaster.FieldAttrs.Add(fieldAttr49);
            this.ucMaster.FieldAttrs.Add(fieldAttr50);
            this.ucMaster.FieldAttrs.Add(fieldAttr51);
            this.ucMaster.FieldAttrs.Add(fieldAttr52);
            this.ucMaster.FieldAttrs.Add(fieldAttr53);
            this.ucMaster.FieldAttrs.Add(fieldAttr54);
            this.ucMaster.FieldAttrs.Add(fieldAttr55);
            this.ucMaster.FieldAttrs.Add(fieldAttr56);
            this.ucMaster.FieldAttrs.Add(fieldAttr57);
            this.ucMaster.FieldAttrs.Add(fieldAttr58);
            this.ucMaster.FieldAttrs.Add(fieldAttr59);
            this.ucMaster.FieldAttrs.Add(fieldAttr60);
            this.ucMaster.FieldAttrs.Add(fieldAttr61);
            this.ucMaster.FieldAttrs.Add(fieldAttr62);
            this.ucMaster.FieldAttrs.Add(fieldAttr63);
            this.ucMaster.FieldAttrs.Add(fieldAttr64);
            this.ucMaster.FieldAttrs.Add(fieldAttr65);
            this.ucMaster.FieldAttrs.Add(fieldAttr66);
            this.ucMaster.FieldAttrs.Add(fieldAttr67);
            this.ucMaster.FieldAttrs.Add(fieldAttr68);
            this.ucMaster.FieldAttrs.Add(fieldAttr69);
            this.ucMaster.FieldAttrs.Add(fieldAttr70);
            this.ucMaster.FieldAttrs.Add(fieldAttr71);
            this.ucMaster.LogInfo = null;
            this.ucMaster.Name = "ucMaster";
            this.ucMaster.RowAffectsCheck = true;
            this.ucMaster.SelectCmd = this.Master;
            this.ucMaster.SelectCmdForUpdate = null;
            this.ucMaster.SendSQLCmd = true;
            this.ucMaster.ServerModify = true;
            this.ucMaster.ServerModifyGetMax = true;
            this.ucMaster.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucMaster.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucMaster.UseTranscationScope = false;
            this.ucMaster.WhereMode = Srvtools.WhereModeType.Keyfields;
            this.ucMaster.AfterApplied += new System.EventHandler(this.ucMaster_AfterApplied);
            this.ucMaster.BeforeInsert += new Srvtools.UpdateComponentBeforeInsertEventHandler(this.ucMaster_BeforeInsert);
            this.ucMaster.AfterInsert += new Srvtools.UpdateComponentAfterInsertEventHandler(this.ucMaster_AfterInsert);
            this.ucMaster.AfterDelete += new Srvtools.UpdateComponentAfterDeleteEventHandler(this.ucMaster_AfterDelete);
            this.ucMaster.AfterModify += new Srvtools.UpdateComponentAfterModifyEventHandler(this.ucMaster_AfterModify);
            // 
            // Detail
            // 
            this.Detail.CacheConnection = false;
            this.Detail.CommandText = "SELECT OPOORDER2_D.* FROM OPOORDER2_D";
            this.Detail.CommandTimeout = 30;
            this.Detail.CommandType = System.Data.CommandType.Text;
            this.Detail.DynamicTableName = false;
            this.Detail.EEPAlias = null;
            this.Detail.EncodingAfter = null;
            this.Detail.EncodingBefore = "Windows-1252";
            this.Detail.EncodingConvert = null;
            this.Detail.InfoConnection = this.InfoConnection;
            keyItem2.KeyName = "BILLNO";
            keyItem3.KeyName = "UNIQUENO";
            this.Detail.KeyFields.Add(keyItem2);
            this.Detail.KeyFields.Add(keyItem3);
            this.Detail.MultiSetWhere = false;
            this.Detail.Name = "Detail";
            this.Detail.NotificationAutoEnlist = false;
            this.Detail.SecExcept = "";
            this.Detail.SecFieldName = null;
            this.Detail.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.Detail.SelectPaging = false;
            this.Detail.SelectTop = 0;
            this.Detail.SiteControl = false;
            this.Detail.SiteFieldName = null;
            this.Detail.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // ucDetail
            // 
            this.ucDetail.AutoTrans = true;
            this.ucDetail.ExceptJoin = false;
            this.ucDetail.LogInfo = null;
            this.ucDetail.Name = "ucDetail";
            this.ucDetail.RowAffectsCheck = true;
            this.ucDetail.SelectCmd = this.Detail;
            this.ucDetail.SelectCmdForUpdate = null;
            this.ucDetail.SendSQLCmd = true;
            this.ucDetail.ServerModify = true;
            this.ucDetail.ServerModifyGetMax = false;
            this.ucDetail.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucDetail.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucDetail.UseTranscationScope = false;
            this.ucDetail.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // idsRelation
            // 
            this.idsRelation.Detail = this.Detail;
            columnItem1.FieldName = "BILLNO";
            this.idsRelation.DetailColumns.Add(columnItem1);
            this.idsRelation.DynamicTableName = false;
            this.idsRelation.Master = this.Master;
            columnItem2.FieldName = "BILLNO";
            this.idsRelation.MasterColumns.Add(columnItem2);
            // 
            // View_Provider
            // 
            this.View_Provider.CacheConnection = false;
            this.View_Provider.CommandText = resources.GetString("View_Provider.CommandText");
            this.View_Provider.CommandTimeout = 30;
            this.View_Provider.CommandType = System.Data.CommandType.Text;
            this.View_Provider.DynamicTableName = false;
            this.View_Provider.EEPAlias = null;
            this.View_Provider.EncodingAfter = null;
            this.View_Provider.EncodingBefore = "Windows-1252";
            this.View_Provider.EncodingConvert = null;
            this.View_Provider.InfoConnection = this.InfoConnection;
            keyItem4.KeyName = "BILLNO";
            this.View_Provider.KeyFields.Add(keyItem4);
            this.View_Provider.MultiSetWhere = false;
            this.View_Provider.Name = "View_Provider";
            this.View_Provider.NotificationAutoEnlist = false;
            this.View_Provider.SecExcept = "";
            this.View_Provider.SecFieldName = null;
            this.View_Provider.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_Provider.SelectPaging = true;
            this.View_Provider.SelectTop = 0;
            this.View_Provider.SiteControl = false;
            this.View_Provider.SiteFieldName = null;
            this.View_Provider.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // autoNumber1
            // 
            this.autoNumber1.Active = true;
            this.autoNumber1.AutoNoID = "OPOM02";
            this.autoNumber1.Description = null;
            this.autoNumber1.GetFixed = "GetPreBillNo()";
            this.autoNumber1.isNumFill = false;
            this.autoNumber1.Name = "autoNumber1";
            this.autoNumber1.Number = null;
            this.autoNumber1.NumDig = 3;
            this.autoNumber1.OldVersion = false;
            this.autoNumber1.OverFlow = true;
            this.autoNumber1.StartValue = 1;
            this.autoNumber1.Step = 1;
            this.autoNumber1.TargetColumn = "BILLNO";
            this.autoNumber1.UpdateComp = this.ucMaster;
            // 
            // icTemp
            // 
            this.icTemp.CacheConnection = false;
            this.icTemp.CommandText = "";
            this.icTemp.CommandTimeout = 30;
            this.icTemp.CommandType = System.Data.CommandType.Text;
            this.icTemp.DynamicTableName = false;
            this.icTemp.EEPAlias = null;
            this.icTemp.EncodingAfter = null;
            this.icTemp.EncodingBefore = "Windows-1252";
            this.icTemp.EncodingConvert = null;
            this.icTemp.InfoConnection = this.InfoConnection;
            this.icTemp.MultiSetWhere = false;
            this.icTemp.Name = "icTemp";
            this.icTemp.NotificationAutoEnlist = false;
            this.icTemp.SecExcept = "";
            this.icTemp.SecFieldName = null;
            this.icTemp.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.icTemp.SelectPaging = true;
            this.icTemp.SelectTop = 0;
            this.icTemp.SiteControl = false;
            this.icTemp.SiteFieldName = null;
            this.icTemp.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // cmdOPO2INV5Query
            // 
            this.cmdOPO2INV5Query.CacheConnection = false;
            this.cmdOPO2INV5Query.CommandText = resources.GetString("cmdOPO2INV5Query.CommandText");
            this.cmdOPO2INV5Query.CommandTimeout = 30;
            this.cmdOPO2INV5Query.CommandType = System.Data.CommandType.Text;
            this.cmdOPO2INV5Query.DynamicTableName = false;
            this.cmdOPO2INV5Query.EEPAlias = null;
            this.cmdOPO2INV5Query.EncodingAfter = null;
            this.cmdOPO2INV5Query.EncodingBefore = "Windows-1252";
            this.cmdOPO2INV5Query.EncodingConvert = null;
            this.cmdOPO2INV5Query.InfoConnection = this.InfoConnection;
            keyItem5.KeyName = "TYPEFLAG";
            keyItem6.KeyName = "BILLNO";
            keyItem7.KeyName = "UNIQUENO";
            this.cmdOPO2INV5Query.KeyFields.Add(keyItem5);
            this.cmdOPO2INV5Query.KeyFields.Add(keyItem6);
            this.cmdOPO2INV5Query.KeyFields.Add(keyItem7);
            this.cmdOPO2INV5Query.MultiSetWhere = false;
            this.cmdOPO2INV5Query.Name = "cmdOPO2INV5Query";
            this.cmdOPO2INV5Query.NotificationAutoEnlist = false;
            this.cmdOPO2INV5Query.SecExcept = null;
            this.cmdOPO2INV5Query.SecFieldName = null;
            this.cmdOPO2INV5Query.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdOPO2INV5Query.SelectPaging = false;
            this.cmdOPO2INV5Query.SelectTop = 0;
            this.cmdOPO2INV5Query.SiteControl = false;
            this.cmdOPO2INV5Query.SiteFieldName = null;
            this.cmdOPO2INV5Query.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdBASSHIPMARK
            // 
            this.cmdBASSHIPMARK.CacheConnection = false;
            this.cmdBASSHIPMARK.CommandText = "SELECT CUSTID,SHIPMARKID,SHIPMARKNAME,MAINMARK_DOC,MAINMARK,SIDEMARK_DOC,SIDEMARK" +
    ",MEMOCHAR FROM BASSHIPMARK";
            this.cmdBASSHIPMARK.CommandTimeout = 30;
            this.cmdBASSHIPMARK.CommandType = System.Data.CommandType.Text;
            this.cmdBASSHIPMARK.DynamicTableName = false;
            this.cmdBASSHIPMARK.EEPAlias = null;
            this.cmdBASSHIPMARK.EncodingAfter = null;
            this.cmdBASSHIPMARK.EncodingBefore = "Windows-1252";
            this.cmdBASSHIPMARK.EncodingConvert = null;
            this.cmdBASSHIPMARK.InfoConnection = this.InfoConnection;
            keyItem8.KeyName = "CUSTID";
            keyItem9.KeyName = "ShipMarkID";
            this.cmdBASSHIPMARK.KeyFields.Add(keyItem8);
            this.cmdBASSHIPMARK.KeyFields.Add(keyItem9);
            this.cmdBASSHIPMARK.MultiSetWhere = false;
            this.cmdBASSHIPMARK.Name = "cmdBASSHIPMARK";
            this.cmdBASSHIPMARK.NotificationAutoEnlist = false;
            this.cmdBASSHIPMARK.SecExcept = null;
            this.cmdBASSHIPMARK.SecFieldName = null;
            this.cmdBASSHIPMARK.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdBASSHIPMARK.SelectPaging = false;
            this.cmdBASSHIPMARK.SelectTop = 0;
            this.cmdBASSHIPMARK.SiteControl = false;
            this.cmdBASSHIPMARK.SiteFieldName = null;
            this.cmdBASSHIPMARK.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // tsMaster
            // 
            this.tsMaster.Name = "tsMaster";
            this.tsMaster.UpdateComp = this.ucMaster;
            // 
            // Report
            // 
            this.Report.CacheConnection = false;
            this.Report.CommandText = resources.GetString("Report.CommandText");
            this.Report.CommandTimeout = 30;
            this.Report.CommandType = System.Data.CommandType.Text;
            this.Report.DynamicTableName = false;
            this.Report.EEPAlias = null;
            this.Report.EncodingAfter = null;
            this.Report.EncodingBefore = "Windows-1252";
            this.Report.EncodingConvert = null;
            this.Report.InfoConnection = this.InfoConnection;
            keyItem10.KeyName = "BILLNO";
            keyItem11.KeyName = "UNIQUENO";
            this.Report.KeyFields.Add(keyItem10);
            this.Report.KeyFields.Add(keyItem11);
            this.Report.MultiSetWhere = false;
            this.Report.Name = "Report";
            this.Report.NotificationAutoEnlist = false;
            this.Report.SecExcept = "";
            this.Report.SecFieldName = null;
            this.Report.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.Report.SelectPaging = true;
            this.Report.SelectTop = 0;
            this.Report.SiteControl = false;
            this.Report.SiteFieldName = null;
            this.Report.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // cmdOPO12OPO2
            // 
            this.cmdOPO12OPO2.CacheConnection = false;
            this.cmdOPO12OPO2.CommandText = "SELECT DISTINCT BILLNO,BILLDATE,CUSTID FROM VIEW_OPO12OPO2";
            this.cmdOPO12OPO2.CommandTimeout = 30;
            this.cmdOPO12OPO2.CommandType = System.Data.CommandType.Text;
            this.cmdOPO12OPO2.DynamicTableName = false;
            this.cmdOPO12OPO2.EEPAlias = null;
            this.cmdOPO12OPO2.EncodingAfter = null;
            this.cmdOPO12OPO2.EncodingBefore = "Windows-1252";
            this.cmdOPO12OPO2.EncodingConvert = null;
            this.cmdOPO12OPO2.InfoConnection = this.InfoConnection;
            keyItem12.KeyName = "BILLNO";
            keyItem13.KeyName = "UNIQUENO";
            this.cmdOPO12OPO2.KeyFields.Add(keyItem12);
            this.cmdOPO12OPO2.KeyFields.Add(keyItem13);
            this.cmdOPO12OPO2.MultiSetWhere = false;
            this.cmdOPO12OPO2.Name = "cmdOPO12OPO2";
            this.cmdOPO12OPO2.NotificationAutoEnlist = false;
            this.cmdOPO12OPO2.SecExcept = null;
            this.cmdOPO12OPO2.SecFieldName = null;
            this.cmdOPO12OPO2.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdOPO12OPO2.SelectPaging = false;
            this.cmdOPO12OPO2.SelectTop = 0;
            this.cmdOPO12OPO2.SiteControl = false;
            this.cmdOPO12OPO2.SiteFieldName = null;
            this.cmdOPO12OPO2.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Master)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Detail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_Provider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.icTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdOPO2INV5Query)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdBASSHIPMARK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Report)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdOPO12OPO2)).EndInit();

        }

        #endregion


        private string curAddKey = "";
        public string GetPreBillNo()
        {
            curAddKey = ucMaster.GetFieldCurrentValue("BILLNO").ToString();
            //取前置碼
            string sSQL = "";
            string sCode = "";

            DateTime d1 = Convert.ToDateTime(ucMaster.GetFieldCurrentValue("BILLDATE"));

            sSQL = "SELECT PARAVALUE FROM PARAMETER WHERE PARATYPE = 'OPO' AND PARAFIELD = 'OPOBILLCODE2'";
            System.Data.DataTable dtData = ExecuteSql(sSQL, ucMaster.conn, ucMaster.trans).Tables[0];
            sCode = dtData.Rows[0][0].ToString();
            dtData.Dispose();

            object[] back = genieTools.genieUtil.GetPreFixNo(d1, "OPO", sCode);

            //流水號長度
            autoNumber1.NumDig = Convert.ToInt16(back[2].ToString());

            return back[1].ToString();
        }

        //$Edit 20110426 by alex：新增前判斷是否為autoNum，issue1225
        private void ucMaster_BeforeInsert(object sender, UpdateComponentBeforeInsertEventArgs e)
        {
            //string userparal = this.GetClientInfo(ClientInfoType.UserParam1).ToString();
            //string billno = ucMaster.GetFieldCurrentValue("BILLNO").ToString();

            ////这里如果前段的预设值autoNum改变，这里也要相应修改
            //if ((curAddKey != "autoNum" && !checkKeyRepeat("OPOORDER2_M", "BILLNO", billno)) || userparal == "import")
            //    ucMaster.SetFieldValue("BILLNO", curAddKey);
        }

        //检查Key是否重复
        public bool checkKeyRepeat(string tableName, string keyName, string keyvalue)
        {
            bool flag = false;
            string sql = "select count(*) as cnt from " + tableName + " where " + keyName + " = '" + keyvalue + "'";
            System.Data.DataSet ds = this.ExecuteSql(sql, ucMaster.conn, ucMaster.trans);
            if (ds != null && Convert.ToInt32(ds.Tables[0].Rows[0][0]) > 0)
                flag = true;
            return flag;
        }

        public object[] sqlRebuild(object[] param)
        {
            genieComponent.GenieLog glog = new genieComponent.GenieLog("bOPOM02_Report", "sqlRebuild");

            System.Data.IDbConnection conn = this.AllocateConnection(this.GetClientInfo(ClientInfoType.LoginDB).ToString());
            if (conn.State == System.Data.ConnectionState.Closed)
            {
                conn.Open();
            }
            System.Data.IDbTransaction trans = conn.BeginTransaction();
            object[] ret = new object[] { 0, "Y", null };
            string sql = "";
            try
            {
                sql = Master.CommandText;
                string sqlSelect = sql.Remove(sql.IndexOf("FROM")) + " ";
                string sqlFrom = sql.Remove(0, sql.IndexOf("FROM"));
                string sqlWhere = " ", sqlOrderBy = " ";

                if (sql.IndexOf("ORDER BY") > 0)
                {
                    sqlOrderBy = " " + sqlFrom.Remove(0, sqlFrom.IndexOf("ORDER BY"));
                    sqlFrom = " " + sqlFrom.Remove(sqlFrom.IndexOf("ORDER BY")) + " ";
                }
                if (sql.IndexOf("WHERE") > 0)
                {
                    sqlWhere = " " + sqlFrom.Remove(0, sqlFrom.IndexOf("WHERE")) + " ";
                    sqlFrom = " " + sqlFrom.Remove(sqlFrom.IndexOf("WHERE")) + " ";
                }
                else
                {
                    sqlWhere = " WHERE 1=1  ";
                }

                if (param[3] != null)
                {
                    sql = sqlSelect + param[0] + sqlFrom + param[1] + sqlWhere + " AND " + param[2] + " AND " + param[3] + sqlOrderBy;
                }
                else
                {
                    sql = sqlSelect + param[0] + sqlFrom + param[1] + sqlWhere + " AND " + param[2] + sqlOrderBy;
                }

                System.Data.DataSet ds = new System.Data.DataSet();
                ds = ExecuteSql(sql, conn, trans);
                ret[1] = ds;
                trans.Commit();
            }
            catch (Exception ex)
            {
                //if Insert error, then call RollBack         
                ret[0] = 1;
                ret[1] = "SQL Failed! ";
                trans.Rollback();

                //To Log
                glog.Error("SQL Failed! /r/n SQL:" + sql + " /r/n Msg:" + ex.Message);
            }
            finally
            {
                this.ReleaseConnection(this.GetClientInfo(ClientInfoType.LoginDB).ToString(), conn);
            }
            return ret;
        }

        private void ucMaster_AfterDelete(object sender, UpdateComponentAfterDeleteEventArgs e)
        {
            new genieLibrary.genieServerFunc().gexUserLog("DEL", ucMaster, this.GetClientInfo(ClientInfoType.LoginUser).ToString(), "OPOM02");
            string AR_billno = Convert.ToString(ucMaster.GetFieldOldValue("AR_BILLNO"));
            var sql = "DELETE FROM APRBATCHPAY1_M  WHERE BILLNO = '" + AR_billno + "'";
            this.ExecuteSql("icTemp", sql, this.GetClientInfo(ClientInfoType.LoginDB).ToString(), true);

        }

        private void ucMaster_AfterInsert(object sender, UpdateComponentAfterInsertEventArgs e)
        {
            new genieLibrary.genieServerFunc().gexUserLog("ADD", ucMaster, this.GetClientInfo(ClientInfoType.LoginUser).ToString(), "OPOM02");
            string AR_billno = Convert.ToString(ucMaster.GetFieldCurrentValue("AR_BILLNO"));
            DateTime billdate = Convert.ToDateTime(ucMaster.GetFieldCurrentValue("BILLDATE"));
            string OPOBILLNO = Convert.ToString(ucMaster.GetFieldCurrentValue("BILLNO"));
            string billdate2 = billdate.ToString("yyyy/MM/dd");
            if (AR_billno == "")
            {
                var sql = "exec GEX_CREATE_AR @BILLDATE='" + billdate2 + "' ,@OPOBILLNO='" + OPOBILLNO + "'";
                this.ExecuteSql(sql, ucMaster.conn, ucMaster.trans);
            }
        }

        private void ucMaster_AfterModify(object sender, UpdateComponentAfterModifyEventArgs e)
        {
            new genieLibrary.genieServerFunc().gexUserLog("EDIT", ucMaster, this.GetClientInfo(ClientInfoType.LoginUser).ToString(), "OPOM02");
            string AR_billno = Convert.ToString(ucMaster.GetFieldCurrentValue("AR_BILLNO"));
            DateTime billdate = Convert.ToDateTime(ucMaster.GetFieldCurrentValue("BILLDATE"));
            string OPOBILLNO = Convert.ToString(ucMaster.GetFieldCurrentValue("BILLNO"));
            string billdate2 = billdate.ToString("yyyy/MM/dd");
            if (AR_billno == "")
            {
                var sql = "exec GEX_CREATE_AR @BILLDATE='" + billdate2 + "' ,@OPOBILLNO='" + OPOBILLNO + "'";
                this.ExecuteSql(sql, ucMaster.conn, ucMaster.trans);
            }
        }

        /*2016 OPOM02A 資料回寫BAS*/
        public object[] WriteBackToBAS01(object[] objParam)
        {
            object[] objRet = new object[2] { 0, "Y" };
            genieComponent.GenieLog gLog = new genieComponent.GenieLog("回寫至客戶明細");
            object[] param = objParam[0].ToString().Split('|');
            string CUSTID = (string)param[0];
            string DELIVERADDRESS = (string)param[1];
            string TEL1 = (string)param[2];
            string PERSONID = (string)param[3];
            string PAYMENTWAY = (string)param[4];
            string INVOICETYPE = (string)param[5];
            string CORPUNICODE = (string)param[6];
            try
            {
                DataSet ds;
                string sql = "";
                sql = string.Format(" update BASCUSTOMER set DELIVERADDRESS='{1}' where CUSTID ='{0}' ", CUSTID, DELIVERADDRESS);
                ds = this.ExecuteSql("icTemp", sql, this.GetClientInfo(ClientInfoType.LoginDB).ToString(), true);
            }
            catch (Exception ex)
            {
                gLog.Error("回寫至客戶明細失敗", "【ErrorMsg】:" + ex.Message + ex.StackTrace);
                objRet = new object[2] { 0, "回寫至客戶明細失敗，請查看Log!" };
            }
            return objRet;
        }
        private void ucMaster_AfterApplied(object sender, EventArgs e)
        {
            //string AR_billno = Convert.ToString(ucMaster.GetFieldCurrentValue("AR_BILLNO"));
            //DateTime billdate = Convert.ToDateTime(ucMaster.GetFieldCurrentValue("BILLDATE"));
            //string OPOBILLNO = Convert.ToString(ucMaster.GetFieldCurrentValue("BILLNO"));
            //string billdate2 = billdate.ToString("yyyy/MM/dd");
            //if (AR_billno == "")
            //{
            //    var sql = "exec GEX_CREATE_AR @BILLDATE='" + billdate2 + "' ,@OPOBILLNO='" + OPOBILLNO + "'";
            //    this.ExecuteSql(sql, ucMaster.conn, ucMaster.trans);
            //}
        }
        //批次轉出貨單
        public object[] OPOtoINVtoDB(object[] objParam)
        {
            object[] objRet = new object[2] { 0, "Y" };
            genieComponent.GenieLog gLog = new genieComponent.GenieLog("批次轉出貨單");
            object[] param = objParam[0].ToString().Split('|');
            string BILLDATE1 = (string)param[0];
            string BILLDATE2 = (string)param[1];
            string LOGINID = (string)param[2];
            try
            {
                string sql = "";
                sql = string.Format(" Exec GEX_OPO2INV '{0}','{1}','{2}' ", BILLDATE1 + " 00:00:00", BILLDATE2 + " 23:59:59", LOGINID);
                this.ExecuteSql("icTemp", sql, this.GetClientInfo(ClientInfoType.LoginDB).ToString(), true);
            }
            catch (Exception ex)
            {
                gLog.Error("批次轉出貨單失敗", "【ErrorMsg】:" + ex.Message + ex.StackTrace);
                objRet = new object[2] { 0, "批次轉出貨單失敗，請查看Log!" };
            }
            return objRet;
        }
        //檢查EXCEL導入的PRODUCTID是否存在
        public object[] CheckPRODUCTID(object[] objParam)
        {
            string PRODUCTID = objParam[0].ToString();
            object[] objRet = new object[2] { 0, "Y" };
            genieComponent.GenieLog gLog = new genieComponent.GenieLog("PRODUCTID存在檢查");
            try
            {

                string sql = "";
                sql = string.Format(" Select * from BASPRODUCT where PRODUCTID='{0}'  ", PRODUCTID);
                DataSet ds = this.ExecuteSql("icTemp", sql, this.GetClientInfo(ClientInfoType.LoginDB).ToString(), true);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    objRet[1] = ds.Tables[0].Rows[0]["PRODCNAME"].ToString(); 
                }
                else { objRet[1] = "Y"; }
            }
            catch (Exception ex)
            {
                gLog.Error("PRODUCTID存在檢查失敗", "【ErrorMsg】:" + ex.Message + ex.StackTrace);
                objRet = new object[2] { 0, "PRODUCTID存在檢查失敗，請查看Log!" };
            }
            return objRet;
        }

        public class BOMDATA
        {
            public string BILLNO { get; set; }
            public int SEQNO { get; set; }
            public string WAREID { get; set; }
            public string PRODUCTID { get; set; }
            public string PRODCNAME { get; set; }
            public string PRODENAME { get; set; }
            public string PRODSTRUCTURE { get; set; }
            public Double QUANTITY { get; set; }
            public Double PRICE { get; set; }
            public Double TAXPRICE { get; set; }
            public Double SUBAMOUNT { get; set; }
            public Double TAXAMOUNT { get; set; }
            public string MEMOCHAR1 { get; set; }
            public string MEMOCHAR2 { get; set; }
            public string RECEIVE_ADDRESS { get; set; }
            public string ISGIFTPROD { get; set; }
            public string ONHANDDATE { get; set; }
            public string ISNEEDMADE { get; set; }
            public int UNIQUENO { get; set; }
            public int BOXPRICE { get; set; }
            public string MEMOCHAR11 { get; set; }
            public string MEMOCHAR12 { get; set; }
            public string MEMOCHAR13 { get; set; }
            public string MEMOCHAR14 { get; set; }
            public string BONUS1 { get; set; }
            public string BONUS2 { get; set; }
            public string BONUS3 { get; set; }
            public string FREIGHTPAY { get; set; }
        }

        public object[] openBOMPRODUCT(object[] objParam)
        {
            object[] objRet = new object[2] { 0, "Y" };
            genieComponent.GenieLog gLog = new genieComponent.GenieLog("Excel匯入BOM展開");
            object[] param = objParam[0].ToString().Split('|');
            string PRODUCTID = (string)param[0];
            string QUANTITY = (string)param[1];
            string PRICE = (string)param[2];
            string TAXPRICE = (string)param[3];
            string SUBAMOUNT = (string)param[4];
            string TAXAMOUNT = (string)param[5];
            string MEMOCHAR1 = (string)param[6];
            string MEMOCHAR2 = (string)param[7];
            string RECEIVE_ADDRESS = (string)param[8];
            string ISGIFTPROD = (string)param[9];
            string ONHANDDATE = (string)param[10];
            string WAREID = (string)param[11];
            int SEQNO = Convert.ToInt32((string)param[12]) + 1;
            string MEMOCHAR11 = (string)param[13];
            string MEMOCHAR12 = (string)param[14];
            string MEMOCHAR13 = (string)param[15];
            string MEMOCHAR14 = (string)param[16];
            string BONUS1 = (string)param[17];
            string BONUS2 = (string)param[18];
            string BONUS3 = (string)param[19];
            string FREIGHTPAY = (string)param[20];


            try
            {
                string sql = "";
                string js = string.Empty;
                sql = string.Format("   select *  from BASPRODUCT where PRODUCTID='{0}' and PRODCATEID ='ZZ'  ", PRODUCTID);
                DataSet ds1 = this.ExecuteSql("icTemp", sql, this.GetClientInfo(ClientInfoType.LoginDB).ToString(), true);
                if (ds1.Tables[0].Rows.Count > 0)
                {
                    sql = string.Format(" EXEC GEX_GETPRICE_CAKE '{0}','{1}','{2}'  ", PRODUCTID, QUANTITY, TAXAMOUNT);
                    DataSet ds = this.ExecuteSql("icTemp", sql, this.GetClientInfo(ClientInfoType.LoginDB).ToString(), true);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        List<BOMDATA> BOMDATA = new List<BOMDATA>();
                        objRet[1] = "";
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            Double BOMTAXAMT = Convert.ToDouble(ds.Tables[0].Rows[i]["TAXAMT"].ToString());
                            Double BOMQTY = Convert.ToDouble(ds.Tables[0].Rows[i]["QTY"].ToString());
                            Double BOMPSUBAMOUNT = BOMTAXAMT * 100 / 105;
                            Double BOMTAXPRICE = BOMTAXAMT / BOMQTY;
                            Double BOMPRICE = BOMPSUBAMOUNT / BOMQTY;
                            BOMDATA.Add(new BOMDATA
                            {
                                BILLNO = "autoNum",
                                SEQNO = SEQNO + i,
                                WAREID = WAREID,
                                PRODUCTID = ds.Tables[0].Rows[i]["PRODUCTID"].ToString(),
                                PRODCNAME = ds.Tables[0].Rows[i]["PRODCNAME"].ToString(),
                                PRODENAME = ds.Tables[0].Rows[i]["PRODENAME"].ToString(),
                                PRODSTRUCTURE = ds.Tables[0].Rows[i]["PRODSTRUCTURE"].ToString(),
                                QUANTITY = BOMQTY,
                                PRICE = BOMPRICE,
                                TAXPRICE = BOMTAXPRICE,
                                SUBAMOUNT = BOMPSUBAMOUNT,
                                TAXAMOUNT = BOMTAXAMT,
                                MEMOCHAR1 = MEMOCHAR1,
                                MEMOCHAR2 = MEMOCHAR2,
                                RECEIVE_ADDRESS = RECEIVE_ADDRESS,
                                ISGIFTPROD = ISGIFTPROD,
                                ONHANDDATE = ONHANDDATE,
                                UNIQUENO = (SEQNO + i) * -1,
                                BOXPRICE = 100,
                                ISNEEDMADE = "N",
                                MEMOCHAR11 = MEMOCHAR11,
                                MEMOCHAR12 = MEMOCHAR12,
                                MEMOCHAR13 = MEMOCHAR13,
                                MEMOCHAR14 = MEMOCHAR14,
                                BONUS1 = BONUS1,
                                BONUS2 = BONUS2,
                                BONUS3 = BONUS3,
                                FREIGHTPAY = FREIGHTPAY
                            });
                        }
                        js = JsonConvert.SerializeObject(BOMDATA, Newtonsoft.Json.Formatting.Indented);//Indented縮排 將資料轉換成Json格式
                        objRet[1] = js;
                    }
                    else
                    {
                        objRet[1] = "Y";
                    }
                }
                else
                { objRet[1] = "Y"; }
            }
            catch (Exception ex)
            {
                gLog.Error("Excel匯入BOM展開", "【ErrorMsg】:" + ex.Message + ex.StackTrace);
                objRet = new object[2] { 0, "Excel匯入BOM展開，請查看Log!" };
            }
            return objRet;
        }
    }
}
