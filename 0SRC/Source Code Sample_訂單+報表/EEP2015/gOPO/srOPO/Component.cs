using System;
using System.ComponentModel;
using System.Collections;
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
using Newtonsoft;
using Newtonsoft.Json;

namespace srOPO
{
    /// <summary>
    /// Summary description for Component.
    /// </summary>
    public class Component : DataModule
    {
        private ServiceManager serviceManager;
        private InfoConnection InfoConnection1;
        private InfoCommand GEXRPT_BOMR01;
        private InfoCommand VIRTUAL_BOMR01;
        private InfoCommand GEXRPT_BOMR02;
        private InfoCommand VIRTUAL_BOMR02;
        private InfoCommand VIEW_RPT_OPOR01;
        private InfoCommand VIEW_RPT_OPOR02;
        private InfoCommand VIEW_RPT_OPOR03;
        private InfoCommand VIEW_RPT_OPOR04;
        private InfoCommand VIEW_RPT_OPOR05;
        private InfoCommand VIEW_RPT_OPOR07;
        private InfoCommand VIEW_RPT_OPOR09;
        private InfoCommand GEXRPT_OPOR0A;
        private InfoCommand VIRTUAL_OPOR0A;
        private InfoCommand GEXRPT_OPOR0B;
        private InfoCommand VIRTUAL_OPOR0B;
        private InfoCommand GEXRPT_OPOR10;
        private InfoCommand VIRTUAL_OPOR10;
        private InfoCommand VIEW_RPT_OPOR11;
        private InfoCommand VIEW_RPT_OPOR12;
        private InfoCommand VIEW_RPT_OPOR13;
        private InfoCommand VIEW_RPT_OPOR14;
        private InfoCommand VIEW_RPT_OPOR16;
        private InfoCommand GEXRPT_OPOR21;
        private InfoCommand VIRTUAL_OPOR21;
        private InfoCommand GEXRPT_OPOR22;
        private InfoCommand VIRTUAL_OPOR22;

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
            this.serviceManager = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            Srvtools.InfoParameter infoPara_GEXRPT_BOMR011 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_BOMR012 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_BOMR013 = new Srvtools.InfoParameter();
            Srvtools.Service srv_GEXRPT_BOMR01 = new Srvtools.Service();
            Srvtools.Service srv_GEXRPT_BOMR01_JS = new Srvtools.Service();

        Srvtools.InfoParameter infoPara_GEXRPT_BOMR021 = new Srvtools.InfoParameter();
            Srvtools.Service srv_GEXRPT_BOMR02 = new Srvtools.Service();
            Srvtools.Service srv_GEXRPT_BOMR02_JS = new Srvtools.Service();

        Srvtools.InfoParameter infoPara_GEXRPT_OPOR0A1 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0A2 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0A3 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0A4 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0A5 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0A6 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0A7 = new Srvtools.InfoParameter();
            Srvtools.Service srv_GEXRPT_OPOR0A = new Srvtools.Service();
            Srvtools.Service srv_GEXRPT_OPOR0A_JS = new Srvtools.Service();

        Srvtools.InfoParameter infoPara_GEXRPT_OPOR0B1 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0B2 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0B3 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0B4 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0B5 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR0B6 = new Srvtools.InfoParameter();
            Srvtools.Service srv_GEXRPT_OPOR0B = new Srvtools.Service();
            Srvtools.Service srv_GEXRPT_OPOR0B_JS = new Srvtools.Service();

        Srvtools.InfoParameter infoPara_GEXRPT_OPOR101 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR102 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR103 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR104 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR105 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR106 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR107 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR108 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR109 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR1010 = new Srvtools.InfoParameter();
            Srvtools.Service srv_GEXRPT_OPOR10 = new Srvtools.Service();
            Srvtools.Service srv_GEXRPT_OPOR10_JS = new Srvtools.Service();

        Srvtools.InfoParameter infoPara_GEXRPT_OPOR211 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR212 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR213 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR214 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR215 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR216 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR217 = new Srvtools.InfoParameter();
            Srvtools.Service srv_GEXRPT_OPOR21 = new Srvtools.Service();
            Srvtools.Service srv_GEXRPT_OPOR21_JS = new Srvtools.Service();

        Srvtools.InfoParameter infoPara_GEXRPT_OPOR221 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR222 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR223 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR224 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR225 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR226 = new Srvtools.InfoParameter();
            Srvtools.InfoParameter infoPara_GEXRPT_OPOR227 = new Srvtools.InfoParameter();
            Srvtools.Service srv_GEXRPT_OPOR22 = new Srvtools.Service();
            Srvtools.Service srv_GEXRPT_OPOR22_JS = new Srvtools.Service();


            // New Report SP Para Add HERE
            this.GEXRPT_BOMR01 = new Srvtools.InfoCommand(this.components);
            this.VIRTUAL_BOMR01 = new Srvtools.InfoCommand(this.components);
            this.GEXRPT_BOMR02 = new Srvtools.InfoCommand(this.components);
            this.VIRTUAL_BOMR02 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR01 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR02 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR03 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR04 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR05 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR07 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR09 = new Srvtools.InfoCommand(this.components);
            this.GEXRPT_OPOR0A = new Srvtools.InfoCommand(this.components);
            this.VIRTUAL_OPOR0A = new Srvtools.InfoCommand(this.components);
            this.GEXRPT_OPOR0B = new Srvtools.InfoCommand(this.components);
            this.VIRTUAL_OPOR0B = new Srvtools.InfoCommand(this.components);
            this.GEXRPT_OPOR10 = new Srvtools.InfoCommand(this.components);
            this.VIRTUAL_OPOR10 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR11 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR12 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR13 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR14 = new Srvtools.InfoCommand(this.components);
            this.VIEW_RPT_OPOR16 = new Srvtools.InfoCommand(this.components);
            this.GEXRPT_OPOR21 = new Srvtools.InfoCommand(this.components);
            this.VIRTUAL_OPOR21 = new Srvtools.InfoCommand(this.components);
            this.GEXRPT_OPOR22 = new Srvtools.InfoCommand(this.components);
            this.VIRTUAL_OPOR22 = new Srvtools.InfoCommand(this.components);

            // New Report ComDef Add HERE
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_BOMR01)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_BOMR01)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_BOMR02)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_BOMR02)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR01)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR02)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR03)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR04)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR05)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR07)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR09)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_OPOR0A)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_OPOR0A)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_OPOR0B)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_OPOR0B)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_OPOR10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_OPOR10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR11)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR13)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR14)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_OPOR21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_OPOR21)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_OPOR22)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_OPOR22)).BeginInit();

            // New Report BeginInit Add HERE
            // 
            // serviceManager
            //
            srv_GEXRPT_BOMR01.DelegateName = "GetData_GEXRPT_BOMR01";
            srv_GEXRPT_BOMR01.NonLogin = false;
            srv_GEXRPT_BOMR01.ServiceName = "GetData_GEXRPT_BOMR01";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_BOMR01);
            srv_GEXRPT_BOMR01_JS.DelegateName = "GetData_GEXRPT_BOMR01_JS";
            srv_GEXRPT_BOMR01_JS.NonLogin = false;
            srv_GEXRPT_BOMR01_JS.ServiceName = "GetData_GEXRPT_BOMR01_JS";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_BOMR01_JS);
            srv_GEXRPT_BOMR02.DelegateName = "GetData_GEXRPT_BOMR02";
            srv_GEXRPT_BOMR02.NonLogin = false;
            srv_GEXRPT_BOMR02.ServiceName = "GetData_GEXRPT_BOMR02";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_BOMR02);
            srv_GEXRPT_BOMR02_JS.DelegateName = "GetData_GEXRPT_BOMR02_JS";
            srv_GEXRPT_BOMR02_JS.NonLogin = false;
            srv_GEXRPT_BOMR02_JS.ServiceName = "GetData_GEXRPT_BOMR02_JS";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_BOMR02_JS);
            srv_GEXRPT_OPOR0A.DelegateName = "GetData_GEXRPT_OPOR0A";
            srv_GEXRPT_OPOR0A.NonLogin = false;
            srv_GEXRPT_OPOR0A.ServiceName = "GetData_GEXRPT_OPOR0A";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_OPOR0A);
            srv_GEXRPT_OPOR0A_JS.DelegateName = "GetData_GEXRPT_OPOR0A_JS";
            srv_GEXRPT_OPOR0A_JS.NonLogin = false;
            srv_GEXRPT_OPOR0A_JS.ServiceName = "GetData_GEXRPT_OPOR0A_JS";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_OPOR0A_JS);
            srv_GEXRPT_OPOR0B.DelegateName = "GetData_GEXRPT_OPOR0B";
            srv_GEXRPT_OPOR0B.NonLogin = false;
            srv_GEXRPT_OPOR0B.ServiceName = "GetData_GEXRPT_OPOR0B";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_OPOR0B);
            srv_GEXRPT_OPOR0B_JS.DelegateName = "GetData_GEXRPT_OPOR0B_JS";
            srv_GEXRPT_OPOR0B_JS.NonLogin = false;
            srv_GEXRPT_OPOR0B_JS.ServiceName = "GetData_GEXRPT_OPOR0B_JS";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_OPOR0B_JS);
            srv_GEXRPT_OPOR10.DelegateName = "GetData_GEXRPT_OPOR10";
            srv_GEXRPT_OPOR10.NonLogin = false;
            srv_GEXRPT_OPOR10.ServiceName = "GetData_GEXRPT_OPOR10";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_OPOR10);
            srv_GEXRPT_OPOR10_JS.DelegateName = "GetData_GEXRPT_OPOR10_JS";
            srv_GEXRPT_OPOR10_JS.NonLogin = false;
            srv_GEXRPT_OPOR10_JS.ServiceName = "GetData_GEXRPT_OPOR10_JS";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_OPOR10_JS);
            srv_GEXRPT_OPOR21.DelegateName = "GetData_GEXRPT_OPOR21";
            srv_GEXRPT_OPOR21.NonLogin = false;
            srv_GEXRPT_OPOR21.ServiceName = "GetData_GEXRPT_OPOR21";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_OPOR21);
            srv_GEXRPT_OPOR21_JS.DelegateName = "GetData_GEXRPT_OPOR21_JS";
            srv_GEXRPT_OPOR21_JS.NonLogin = false;
            srv_GEXRPT_OPOR21_JS.ServiceName = "GetData_GEXRPT_OPOR21_JS";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_OPOR21_JS);
            srv_GEXRPT_OPOR22.DelegateName = "GetData_GEXRPT_OPOR22";
            srv_GEXRPT_OPOR22.NonLogin = false;
            srv_GEXRPT_OPOR22.ServiceName = "GetData_GEXRPT_OPOR22";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_OPOR22);
            srv_GEXRPT_OPOR22_JS.DelegateName = "GetData_GEXRPT_OPOR22_JS";
            srv_GEXRPT_OPOR22_JS.NonLogin = false;
            srv_GEXRPT_OPOR22_JS.ServiceName = "GetData_GEXRPT_OPOR22_JS";
            this.serviceManager.ServiceCollection.Add(srv_GEXRPT_OPOR22_JS);

            // New Report SP srvManager Add HERE
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "gGexDemo";
            //
            // GEXRPT_BOMR01
            // 
            this.GEXRPT_BOMR01.CacheConnection = false;
            this.GEXRPT_BOMR01.CommandText = "GEXRPT_BOMR01";
            this.GEXRPT_BOMR01.CommandTimeout = 1800;
            this.GEXRPT_BOMR01.CommandType = System.Data.CommandType.StoredProcedure;
            this.GEXRPT_BOMR01.DynamicTableName = false;
            this.GEXRPT_BOMR01.EEPAlias = "";
            this.GEXRPT_BOMR01.EncodingAfter = null;
            this.GEXRPT_BOMR01.EncodingBefore = "Windows-1252";
            this.GEXRPT_BOMR01.InfoConnection = this.InfoConnection1;
            infoPara_GEXRPT_BOMR011.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_BOMR011.ParameterName = "M_PRODID1";
            infoPara_GEXRPT_BOMR011.Precision = ((byte)(0));
            infoPara_GEXRPT_BOMR011.Scale = ((byte)(0));
            infoPara_GEXRPT_BOMR011.Size = 0;
            infoPara_GEXRPT_BOMR011.SourceColumn = null;
            infoPara_GEXRPT_BOMR011.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_BOMR011.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_BOMR011.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_BOMR012.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_BOMR012.ParameterName = "M_PRODID2";
            infoPara_GEXRPT_BOMR012.Precision = ((byte)(0));
            infoPara_GEXRPT_BOMR012.Scale = ((byte)(0));
            infoPara_GEXRPT_BOMR012.Size = 0;
            infoPara_GEXRPT_BOMR012.SourceColumn = null;
            infoPara_GEXRPT_BOMR012.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_BOMR012.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_BOMR012.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_BOMR013.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_BOMR013.ParameterName = "BOMTYPE";
            infoPara_GEXRPT_BOMR013.Precision = ((byte)(0));
            infoPara_GEXRPT_BOMR013.Scale = ((byte)(0));
            infoPara_GEXRPT_BOMR013.Size = 0;
            infoPara_GEXRPT_BOMR013.SourceColumn = null;
            infoPara_GEXRPT_BOMR013.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_BOMR013.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_BOMR013.XmlSchemaCollectionOwningSchema = null;
            this.GEXRPT_BOMR01.InfoParameters.Add(infoPara_GEXRPT_BOMR011);
            this.GEXRPT_BOMR01.InfoParameters.Add(infoPara_GEXRPT_BOMR012);
            this.GEXRPT_BOMR01.InfoParameters.Add(infoPara_GEXRPT_BOMR013);
            this.GEXRPT_BOMR01.MultiSetWhere = false;
            this.GEXRPT_BOMR01.Name = "GEXRPT_BOMR01";
            this.GEXRPT_BOMR01.NotificationAutoEnlist = false;
            this.GEXRPT_BOMR01.SecExcept = null;
            this.GEXRPT_BOMR01.SecFieldName = "";
            this.GEXRPT_BOMR01.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.GEXRPT_BOMR01.SelectPaging = false;
            this.GEXRPT_BOMR01.SelectTop = 0;
            this.GEXRPT_BOMR01.SiteControl = false;
            this.GEXRPT_BOMR01.SiteFieldName = "";
            this.GEXRPT_BOMR01.UpdatedRowSource = System.Data.UpdateRowSource.None;

            // 
            // VIRTUAL_BOMR01
            // 
            this.VIRTUAL_BOMR01.CacheConnection = false;
            this.VIRTUAL_BOMR01.CommandText = "Select  USERID as R_PRODID, USERID as R_PRODCNAME, USERID as CALCUNIT, USERID as LLC, USERID as PRODUCTID, USERID as PRODCNAME, USERID as PUNIT, RPT_SUM as QUANTITY, RPT_SUM as STANDARDQTY, RPT_SUM as LOSTRATE, USERID as ISTAIL From SYSUSER_CNT Where 1<>1";
            this.VIRTUAL_BOMR01.CommandTimeout = 1800;
            this.VIRTUAL_BOMR01.CommandType = System.Data.CommandType.Text;
            this.VIRTUAL_BOMR01.DynamicTableName = false;
            this.VIRTUAL_BOMR01.EEPAlias = null;
            this.VIRTUAL_BOMR01.EncodingAfter = null;
            this.VIRTUAL_BOMR01.EncodingBefore = "Windows-1252";
            this.VIRTUAL_BOMR01.InfoConnection = this.InfoConnection1;
            this.VIRTUAL_BOMR01.MultiSetWhere = false;
            this.VIRTUAL_BOMR01.Name = "VIRTUAL_BOMR01";
            this.VIRTUAL_BOMR01.NotificationAutoEnlist = false;
            this.VIRTUAL_BOMR01.SecExcept = null;
            this.VIRTUAL_BOMR01.SecFieldName = null;
            this.VIRTUAL_BOMR01.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIRTUAL_BOMR01.SelectPaging = false;
            this.VIRTUAL_BOMR01.SelectTop = 0;
            this.VIRTUAL_BOMR01.SiteControl = false;
            this.VIRTUAL_BOMR01.SiteFieldName = null;
            this.VIRTUAL_BOMR01.UpdatedRowSource = System.Data.UpdateRowSource.None;
        //
            // GEXRPT_BOMR02
            // 
            this.GEXRPT_BOMR02.CacheConnection = false;
            this.GEXRPT_BOMR02.CommandText = "GEXRPT_BOMR02";
            this.GEXRPT_BOMR02.CommandTimeout = 1800;
            this.GEXRPT_BOMR02.CommandType = System.Data.CommandType.StoredProcedure;
            this.GEXRPT_BOMR02.DynamicTableName = false;
            this.GEXRPT_BOMR02.EEPAlias = "";
            this.GEXRPT_BOMR02.EncodingAfter = null;
            this.GEXRPT_BOMR02.EncodingBefore = "Windows-1252";
            this.GEXRPT_BOMR02.InfoConnection = this.InfoConnection1;
            infoPara_GEXRPT_BOMR021.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_BOMR021.ParameterName = "PRODUCTID";
            infoPara_GEXRPT_BOMR021.Precision = ((byte)(0));
            infoPara_GEXRPT_BOMR021.Scale = ((byte)(0));
            infoPara_GEXRPT_BOMR021.Size = 0;
            infoPara_GEXRPT_BOMR021.SourceColumn = null;
            infoPara_GEXRPT_BOMR021.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_BOMR021.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_BOMR021.XmlSchemaCollectionOwningSchema = null;
            this.GEXRPT_BOMR02.InfoParameters.Add(infoPara_GEXRPT_BOMR021);
            this.GEXRPT_BOMR02.MultiSetWhere = false;
            this.GEXRPT_BOMR02.Name = "GEXRPT_BOMR02";
            this.GEXRPT_BOMR02.NotificationAutoEnlist = false;
            this.GEXRPT_BOMR02.SecExcept = null;
            this.GEXRPT_BOMR02.SecFieldName = "";
            this.GEXRPT_BOMR02.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.GEXRPT_BOMR02.SelectPaging = false;
            this.GEXRPT_BOMR02.SelectTop = 0;
            this.GEXRPT_BOMR02.SiteControl = false;
            this.GEXRPT_BOMR02.SiteFieldName = "";
            this.GEXRPT_BOMR02.UpdatedRowSource = System.Data.UpdateRowSource.None;

            // 
            // VIRTUAL_BOMR02
            // 
            this.VIRTUAL_BOMR02.CacheConnection = false;
            this.VIRTUAL_BOMR02.CommandText = "Select  USERID as LLC, USERID as R_PRODID, USERID as R_PRODCNAME, USERID as CALCUNIT, USERID as M_PRODID, USERID as M_PRODCNAME, USERID as MUNIT, USERID as PRODUCTID, USERID as PRODCNAME, USERID as PUNIT, RPT_SUM as QUANTITY, RPT_SUM as STANDARDQTY, RPT_SUM as LOSTRATE From SYSUSER_CNT Where 1<>1";
            this.VIRTUAL_BOMR02.CommandTimeout = 1800;
            this.VIRTUAL_BOMR02.CommandType = System.Data.CommandType.Text;
            this.VIRTUAL_BOMR02.DynamicTableName = false;
            this.VIRTUAL_BOMR02.EEPAlias = null;
            this.VIRTUAL_BOMR02.EncodingAfter = null;
            this.VIRTUAL_BOMR02.EncodingBefore = "Windows-1252";
            this.VIRTUAL_BOMR02.InfoConnection = this.InfoConnection1;
            this.VIRTUAL_BOMR02.MultiSetWhere = false;
            this.VIRTUAL_BOMR02.Name = "VIRTUAL_BOMR02";
            this.VIRTUAL_BOMR02.NotificationAutoEnlist = false;
            this.VIRTUAL_BOMR02.SecExcept = null;
            this.VIRTUAL_BOMR02.SecFieldName = null;
            this.VIRTUAL_BOMR02.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIRTUAL_BOMR02.SelectPaging = false;
            this.VIRTUAL_BOMR02.SelectTop = 0;
            this.VIRTUAL_BOMR02.SiteControl = false;
            this.VIRTUAL_BOMR02.SiteFieldName = null;
            this.VIRTUAL_BOMR02.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR01
            // 
            this.VIEW_RPT_OPOR01.CacheConnection = false;
            this.VIEW_RPT_OPOR01.CommandText = "SELECT * FROM [VIEW_RPT_OPOR01]";
            this.VIEW_RPT_OPOR01.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR01.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR01.DynamicTableName = false;
            this.VIEW_RPT_OPOR01.EEPAlias = null;
            this.VIEW_RPT_OPOR01.EncodingAfter = null;
            this.VIEW_RPT_OPOR01.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR01.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR01.MultiSetWhere = false;
            this.VIEW_RPT_OPOR01.Name = "VIEW_RPT_OPOR01";
            this.VIEW_RPT_OPOR01.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR01.SecExcept = null;
            this.VIEW_RPT_OPOR01.SecFieldName = null;
            this.VIEW_RPT_OPOR01.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR01.SelectTop = 0;
            this.VIEW_RPT_OPOR01.SiteControl = false;
            this.VIEW_RPT_OPOR01.SiteFieldName = null;
            this.VIEW_RPT_OPOR01.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR02
            // 
            this.VIEW_RPT_OPOR02.CacheConnection = false;
            this.VIEW_RPT_OPOR02.CommandText = "SELECT * FROM [VIEW_RPT_OPOR02]";
            this.VIEW_RPT_OPOR02.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR02.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR02.DynamicTableName = false;
            this.VIEW_RPT_OPOR02.EEPAlias = null;
            this.VIEW_RPT_OPOR02.EncodingAfter = null;
            this.VIEW_RPT_OPOR02.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR02.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR02.MultiSetWhere = false;
            this.VIEW_RPT_OPOR02.Name = "VIEW_RPT_OPOR02";
            this.VIEW_RPT_OPOR02.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR02.SecExcept = null;
            this.VIEW_RPT_OPOR02.SecFieldName = null;
            this.VIEW_RPT_OPOR02.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR02.SelectTop = 0;
            this.VIEW_RPT_OPOR02.SiteControl = false;
            this.VIEW_RPT_OPOR02.SiteFieldName = null;
            this.VIEW_RPT_OPOR02.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR03
            // 
            this.VIEW_RPT_OPOR03.CacheConnection = false;
            this.VIEW_RPT_OPOR03.CommandText = "SELECT * FROM [VIEW_RPT_OPOR03]";
            this.VIEW_RPT_OPOR03.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR03.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR03.DynamicTableName = false;
            this.VIEW_RPT_OPOR03.EEPAlias = null;
            this.VIEW_RPT_OPOR03.EncodingAfter = null;
            this.VIEW_RPT_OPOR03.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR03.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR03.MultiSetWhere = false;
            this.VIEW_RPT_OPOR03.Name = "VIEW_RPT_OPOR03";
            this.VIEW_RPT_OPOR03.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR03.SecExcept = null;
            this.VIEW_RPT_OPOR03.SecFieldName = null;
            this.VIEW_RPT_OPOR03.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR03.SelectTop = 0;
            this.VIEW_RPT_OPOR03.SiteControl = false;
            this.VIEW_RPT_OPOR03.SiteFieldName = null;
            this.VIEW_RPT_OPOR03.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR04
            // 
            this.VIEW_RPT_OPOR04.CacheConnection = false;
            this.VIEW_RPT_OPOR04.CommandText = "SELECT * FROM [VIEW_RPT_OPOR04]";
            this.VIEW_RPT_OPOR04.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR04.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR04.DynamicTableName = false;
            this.VIEW_RPT_OPOR04.EEPAlias = null;
            this.VIEW_RPT_OPOR04.EncodingAfter = null;
            this.VIEW_RPT_OPOR04.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR04.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR04.MultiSetWhere = false;
            this.VIEW_RPT_OPOR04.Name = "VIEW_RPT_OPOR04";
            this.VIEW_RPT_OPOR04.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR04.SecExcept = null;
            this.VIEW_RPT_OPOR04.SecFieldName = null;
            this.VIEW_RPT_OPOR04.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR04.SelectTop = 0;
            this.VIEW_RPT_OPOR04.SiteControl = false;
            this.VIEW_RPT_OPOR04.SiteFieldName = null;
            this.VIEW_RPT_OPOR04.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR05
            // 
            this.VIEW_RPT_OPOR05.CacheConnection = false;
            this.VIEW_RPT_OPOR05.CommandText = "SELECT * FROM [VIEW_RPT_OPOR05]";
            this.VIEW_RPT_OPOR05.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR05.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR05.DynamicTableName = false;
            this.VIEW_RPT_OPOR05.EEPAlias = null;
            this.VIEW_RPT_OPOR05.EncodingAfter = null;
            this.VIEW_RPT_OPOR05.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR05.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR05.MultiSetWhere = false;
            this.VIEW_RPT_OPOR05.Name = "VIEW_RPT_OPOR05";
            this.VIEW_RPT_OPOR05.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR05.SecExcept = null;
            this.VIEW_RPT_OPOR05.SecFieldName = null;
            this.VIEW_RPT_OPOR05.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR05.SelectTop = 0;
            this.VIEW_RPT_OPOR05.SiteControl = false;
            this.VIEW_RPT_OPOR05.SiteFieldName = null;
            this.VIEW_RPT_OPOR05.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR07
            // 
            this.VIEW_RPT_OPOR07.CacheConnection = false;
            this.VIEW_RPT_OPOR07.CommandText = "SELECT * FROM [VIEW_RPT_OPOR07]";
            this.VIEW_RPT_OPOR07.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR07.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR07.DynamicTableName = false;
            this.VIEW_RPT_OPOR07.EEPAlias = null;
            this.VIEW_RPT_OPOR07.EncodingAfter = null;
            this.VIEW_RPT_OPOR07.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR07.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR07.MultiSetWhere = false;
            this.VIEW_RPT_OPOR07.Name = "VIEW_RPT_OPOR07";
            this.VIEW_RPT_OPOR07.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR07.SecExcept = null;
            this.VIEW_RPT_OPOR07.SecFieldName = null;
            this.VIEW_RPT_OPOR07.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR07.SelectTop = 0;
            this.VIEW_RPT_OPOR07.SiteControl = false;
            this.VIEW_RPT_OPOR07.SiteFieldName = null;
            this.VIEW_RPT_OPOR07.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR09
            // 
            this.VIEW_RPT_OPOR09.CacheConnection = false;
            this.VIEW_RPT_OPOR09.CommandText = "SELECT * FROM [VIEW_RPT_OPOR09]";
            this.VIEW_RPT_OPOR09.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR09.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR09.DynamicTableName = false;
            this.VIEW_RPT_OPOR09.EEPAlias = null;
            this.VIEW_RPT_OPOR09.EncodingAfter = null;
            this.VIEW_RPT_OPOR09.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR09.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR09.MultiSetWhere = false;
            this.VIEW_RPT_OPOR09.Name = "VIEW_RPT_OPOR09";
            this.VIEW_RPT_OPOR09.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR09.SecExcept = null;
            this.VIEW_RPT_OPOR09.SecFieldName = null;
            this.VIEW_RPT_OPOR09.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR09.SelectTop = 0;
            this.VIEW_RPT_OPOR09.SiteControl = false;
            this.VIEW_RPT_OPOR09.SiteFieldName = null;
            this.VIEW_RPT_OPOR09.UpdatedRowSource = System.Data.UpdateRowSource.None;
        //
            // GEXRPT_OPOR0A
            // 
            this.GEXRPT_OPOR0A.CacheConnection = false;
            this.GEXRPT_OPOR0A.CommandText = "GEXRPT_OPOR0A";
            this.GEXRPT_OPOR0A.CommandTimeout = 1800;
            this.GEXRPT_OPOR0A.CommandType = System.Data.CommandType.StoredProcedure;
            this.GEXRPT_OPOR0A.DynamicTableName = false;
            this.GEXRPT_OPOR0A.EEPAlias = "";
            this.GEXRPT_OPOR0A.EncodingAfter = null;
            this.GEXRPT_OPOR0A.EncodingBefore = "Windows-1252";
            this.GEXRPT_OPOR0A.InfoConnection = this.InfoConnection1;
            infoPara_GEXRPT_OPOR0A1.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0A1.ParameterName = "ONHANDDATE1";
            infoPara_GEXRPT_OPOR0A1.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0A1.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0A1.Size = 0;
            infoPara_GEXRPT_OPOR0A1.SourceColumn = null;
            infoPara_GEXRPT_OPOR0A1.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0A1.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0A1.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0A2.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0A2.ParameterName = "ONHANDDATE2";
            infoPara_GEXRPT_OPOR0A2.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0A2.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0A2.Size = 0;
            infoPara_GEXRPT_OPOR0A2.SourceColumn = null;
            infoPara_GEXRPT_OPOR0A2.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0A2.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0A2.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0A3.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0A3.ParameterName = "PRODUCTID1";
            infoPara_GEXRPT_OPOR0A3.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0A3.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0A3.Size = 0;
            infoPara_GEXRPT_OPOR0A3.SourceColumn = null;
            infoPara_GEXRPT_OPOR0A3.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0A3.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0A3.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0A4.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0A4.ParameterName = "PRODUCTID2";
            infoPara_GEXRPT_OPOR0A4.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0A4.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0A4.Size = 0;
            infoPara_GEXRPT_OPOR0A4.SourceColumn = null;
            infoPara_GEXRPT_OPOR0A4.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0A4.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0A4.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0A5.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0A5.ParameterName = "PRODCATEID1";
            infoPara_GEXRPT_OPOR0A5.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0A5.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0A5.Size = 0;
            infoPara_GEXRPT_OPOR0A5.SourceColumn = null;
            infoPara_GEXRPT_OPOR0A5.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0A5.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0A5.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0A6.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0A6.ParameterName = "PRODCATEID2";
            infoPara_GEXRPT_OPOR0A6.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0A6.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0A6.Size = 0;
            infoPara_GEXRPT_OPOR0A6.SourceColumn = null;
            infoPara_GEXRPT_OPOR0A6.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0A6.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0A6.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0A7.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0A7.ParameterName = "LOGINID";
            infoPara_GEXRPT_OPOR0A7.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0A7.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0A7.Size = 0;
            infoPara_GEXRPT_OPOR0A7.SourceColumn = null;
            infoPara_GEXRPT_OPOR0A7.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0A7.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0A7.XmlSchemaCollectionOwningSchema = null;
            this.GEXRPT_OPOR0A.InfoParameters.Add(infoPara_GEXRPT_OPOR0A1);
            this.GEXRPT_OPOR0A.InfoParameters.Add(infoPara_GEXRPT_OPOR0A2);
            this.GEXRPT_OPOR0A.InfoParameters.Add(infoPara_GEXRPT_OPOR0A3);
            this.GEXRPT_OPOR0A.InfoParameters.Add(infoPara_GEXRPT_OPOR0A4);
            this.GEXRPT_OPOR0A.InfoParameters.Add(infoPara_GEXRPT_OPOR0A5);
            this.GEXRPT_OPOR0A.InfoParameters.Add(infoPara_GEXRPT_OPOR0A6);
            this.GEXRPT_OPOR0A.InfoParameters.Add(infoPara_GEXRPT_OPOR0A7);
            this.GEXRPT_OPOR0A.MultiSetWhere = false;
            this.GEXRPT_OPOR0A.Name = "GEXRPT_OPOR0A";
            this.GEXRPT_OPOR0A.NotificationAutoEnlist = false;
            this.GEXRPT_OPOR0A.SecExcept = null;
            this.GEXRPT_OPOR0A.SecFieldName = "";
            this.GEXRPT_OPOR0A.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.GEXRPT_OPOR0A.SelectPaging = false;
            this.GEXRPT_OPOR0A.SelectTop = 0;
            this.GEXRPT_OPOR0A.SiteControl = false;
            this.GEXRPT_OPOR0A.SiteFieldName = "";
            this.GEXRPT_OPOR0A.UpdatedRowSource = System.Data.UpdateRowSource.None;

            // 
            // VIRTUAL_OPOR0A
            // 
            this.VIRTUAL_OPOR0A.CacheConnection = false;
            this.VIRTUAL_OPOR0A.CommandText = "Select  USERID as PRODUCTID, USERID as PRODCNAME, USERID as ONHANDDATE, USERID as SUMQTY, USERID as PRODCATEID, USERID as WEEKNAME From SYSUSER_CNT Where 1<>1";
            this.VIRTUAL_OPOR0A.CommandTimeout = 1800;
            this.VIRTUAL_OPOR0A.CommandType = System.Data.CommandType.Text;
            this.VIRTUAL_OPOR0A.DynamicTableName = false;
            this.VIRTUAL_OPOR0A.EEPAlias = null;
            this.VIRTUAL_OPOR0A.EncodingAfter = null;
            this.VIRTUAL_OPOR0A.EncodingBefore = "Windows-1252";
            this.VIRTUAL_OPOR0A.InfoConnection = this.InfoConnection1;
            this.VIRTUAL_OPOR0A.MultiSetWhere = false;
            this.VIRTUAL_OPOR0A.Name = "VIRTUAL_OPOR0A";
            this.VIRTUAL_OPOR0A.NotificationAutoEnlist = false;
            this.VIRTUAL_OPOR0A.SecExcept = null;
            this.VIRTUAL_OPOR0A.SecFieldName = null;
            this.VIRTUAL_OPOR0A.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIRTUAL_OPOR0A.SelectPaging = false;
            this.VIRTUAL_OPOR0A.SelectTop = 0;
            this.VIRTUAL_OPOR0A.SiteControl = false;
            this.VIRTUAL_OPOR0A.SiteFieldName = null;
            this.VIRTUAL_OPOR0A.UpdatedRowSource = System.Data.UpdateRowSource.None;
        //
            // GEXRPT_OPOR0B
            // 
            this.GEXRPT_OPOR0B.CacheConnection = false;
            this.GEXRPT_OPOR0B.CommandText = "GEXRPT_OPOR0B";
            this.GEXRPT_OPOR0B.CommandTimeout = 1800;
            this.GEXRPT_OPOR0B.CommandType = System.Data.CommandType.StoredProcedure;
            this.GEXRPT_OPOR0B.DynamicTableName = false;
            this.GEXRPT_OPOR0B.EEPAlias = "";
            this.GEXRPT_OPOR0B.EncodingAfter = null;
            this.GEXRPT_OPOR0B.EncodingBefore = "Windows-1252";
            this.GEXRPT_OPOR0B.InfoConnection = this.InfoConnection1;
            infoPara_GEXRPT_OPOR0B1.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0B1.ParameterName = "PRODUCTID1";
            infoPara_GEXRPT_OPOR0B1.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0B1.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0B1.Size = 0;
            infoPara_GEXRPT_OPOR0B1.SourceColumn = null;
            infoPara_GEXRPT_OPOR0B1.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0B1.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0B1.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0B2.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0B2.ParameterName = "PRODUCTID2";
            infoPara_GEXRPT_OPOR0B2.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0B2.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0B2.Size = 0;
            infoPara_GEXRPT_OPOR0B2.SourceColumn = null;
            infoPara_GEXRPT_OPOR0B2.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0B2.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0B2.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0B3.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0B3.ParameterName = "PRODCATEID1";
            infoPara_GEXRPT_OPOR0B3.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0B3.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0B3.Size = 0;
            infoPara_GEXRPT_OPOR0B3.SourceColumn = null;
            infoPara_GEXRPT_OPOR0B3.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0B3.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0B3.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0B4.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0B4.ParameterName = "PRODCATEID2";
            infoPara_GEXRPT_OPOR0B4.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0B4.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0B4.Size = 0;
            infoPara_GEXRPT_OPOR0B4.SourceColumn = null;
            infoPara_GEXRPT_OPOR0B4.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0B4.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0B4.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0B5.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0B5.ParameterName = "ONHANDDATE2";
            infoPara_GEXRPT_OPOR0B5.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0B5.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0B5.Size = 0;
            infoPara_GEXRPT_OPOR0B5.SourceColumn = null;
            infoPara_GEXRPT_OPOR0B5.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0B5.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0B5.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR0B6.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR0B6.ParameterName = "LOGINID";
            infoPara_GEXRPT_OPOR0B6.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR0B6.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR0B6.Size = 0;
            infoPara_GEXRPT_OPOR0B6.SourceColumn = null;
            infoPara_GEXRPT_OPOR0B6.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR0B6.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR0B6.XmlSchemaCollectionOwningSchema = null;
            this.GEXRPT_OPOR0B.InfoParameters.Add(infoPara_GEXRPT_OPOR0B1);
            this.GEXRPT_OPOR0B.InfoParameters.Add(infoPara_GEXRPT_OPOR0B2);
            this.GEXRPT_OPOR0B.InfoParameters.Add(infoPara_GEXRPT_OPOR0B3);
            this.GEXRPT_OPOR0B.InfoParameters.Add(infoPara_GEXRPT_OPOR0B4);
            this.GEXRPT_OPOR0B.InfoParameters.Add(infoPara_GEXRPT_OPOR0B5);
            this.GEXRPT_OPOR0B.InfoParameters.Add(infoPara_GEXRPT_OPOR0B6);
            this.GEXRPT_OPOR0B.MultiSetWhere = false;
            this.GEXRPT_OPOR0B.Name = "GEXRPT_OPOR0B";
            this.GEXRPT_OPOR0B.NotificationAutoEnlist = false;
            this.GEXRPT_OPOR0B.SecExcept = null;
            this.GEXRPT_OPOR0B.SecFieldName = "";
            this.GEXRPT_OPOR0B.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.GEXRPT_OPOR0B.SelectPaging = false;
            this.GEXRPT_OPOR0B.SelectTop = 0;
            this.GEXRPT_OPOR0B.SiteControl = false;
            this.GEXRPT_OPOR0B.SiteFieldName = "";
            this.GEXRPT_OPOR0B.UpdatedRowSource = System.Data.UpdateRowSource.None;

            // 
            // VIRTUAL_OPOR0B
            // 
            this.VIRTUAL_OPOR0B.CacheConnection = false;
            this.VIRTUAL_OPOR0B.CommandText = "Select  USERID as BILLNO, USERID as DATAEXTEND3, USERID as PRODUCTID, USERID as PRODCNAME, USERID as PRODCATEID, RPT_SUM as QUANTITY, USERID as ONHANDDATE, USERID as PAYMENTWAY From SYSUSER_CNT Where 1<>1";
            this.VIRTUAL_OPOR0B.CommandTimeout = 1800;
            this.VIRTUAL_OPOR0B.CommandType = System.Data.CommandType.Text;
            this.VIRTUAL_OPOR0B.DynamicTableName = false;
            this.VIRTUAL_OPOR0B.EEPAlias = null;
            this.VIRTUAL_OPOR0B.EncodingAfter = null;
            this.VIRTUAL_OPOR0B.EncodingBefore = "Windows-1252";
            this.VIRTUAL_OPOR0B.InfoConnection = this.InfoConnection1;
            this.VIRTUAL_OPOR0B.MultiSetWhere = false;
            this.VIRTUAL_OPOR0B.Name = "VIRTUAL_OPOR0B";
            this.VIRTUAL_OPOR0B.NotificationAutoEnlist = false;
            this.VIRTUAL_OPOR0B.SecExcept = null;
            this.VIRTUAL_OPOR0B.SecFieldName = null;
            this.VIRTUAL_OPOR0B.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIRTUAL_OPOR0B.SelectPaging = false;
            this.VIRTUAL_OPOR0B.SelectTop = 0;
            this.VIRTUAL_OPOR0B.SiteControl = false;
            this.VIRTUAL_OPOR0B.SiteFieldName = null;
            this.VIRTUAL_OPOR0B.UpdatedRowSource = System.Data.UpdateRowSource.None;
        //
            // GEXRPT_OPOR10
            // 
            this.GEXRPT_OPOR10.CacheConnection = false;
            this.GEXRPT_OPOR10.CommandText = "GEXRPT_OPOR10";
            this.GEXRPT_OPOR10.CommandTimeout = 1800;
            this.GEXRPT_OPOR10.CommandType = System.Data.CommandType.StoredProcedure;
            this.GEXRPT_OPOR10.DynamicTableName = false;
            this.GEXRPT_OPOR10.EEPAlias = "";
            this.GEXRPT_OPOR10.EncodingAfter = null;
            this.GEXRPT_OPOR10.EncodingBefore = "Windows-1252";
            this.GEXRPT_OPOR10.InfoConnection = this.InfoConnection1;
            infoPara_GEXRPT_OPOR101.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR101.ParameterName = "BILLDATE1";
            infoPara_GEXRPT_OPOR101.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR101.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR101.Size = 0;
            infoPara_GEXRPT_OPOR101.SourceColumn = null;
            infoPara_GEXRPT_OPOR101.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR101.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR101.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR102.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR102.ParameterName = "BILLDATE2";
            infoPara_GEXRPT_OPOR102.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR102.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR102.Size = 0;
            infoPara_GEXRPT_OPOR102.SourceColumn = null;
            infoPara_GEXRPT_OPOR102.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR102.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR102.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR103.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR103.ParameterName = "CUSTID1";
            infoPara_GEXRPT_OPOR103.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR103.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR103.Size = 0;
            infoPara_GEXRPT_OPOR103.SourceColumn = null;
            infoPara_GEXRPT_OPOR103.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR103.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR103.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR104.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR104.ParameterName = "CUSTID2";
            infoPara_GEXRPT_OPOR104.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR104.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR104.Size = 0;
            infoPara_GEXRPT_OPOR104.SourceColumn = null;
            infoPara_GEXRPT_OPOR104.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR104.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR104.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR105.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR105.ParameterName = "PRODUCTID1";
            infoPara_GEXRPT_OPOR105.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR105.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR105.Size = 0;
            infoPara_GEXRPT_OPOR105.SourceColumn = null;
            infoPara_GEXRPT_OPOR105.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR105.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR105.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR106.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR106.ParameterName = "PRODUCTID2";
            infoPara_GEXRPT_OPOR106.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR106.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR106.Size = 0;
            infoPara_GEXRPT_OPOR106.SourceColumn = null;
            infoPara_GEXRPT_OPOR106.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR106.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR106.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR107.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR107.ParameterName = "PERSONID1";
            infoPara_GEXRPT_OPOR107.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR107.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR107.Size = 0;
            infoPara_GEXRPT_OPOR107.SourceColumn = null;
            infoPara_GEXRPT_OPOR107.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR107.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR107.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR108.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR108.ParameterName = "PERSONID2";
            infoPara_GEXRPT_OPOR108.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR108.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR108.Size = 0;
            infoPara_GEXRPT_OPOR108.SourceColumn = null;
            infoPara_GEXRPT_OPOR108.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR108.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR108.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR109.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR109.ParameterName = "ISSIGN";
            infoPara_GEXRPT_OPOR109.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR109.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR109.Size = 0;
            infoPara_GEXRPT_OPOR109.SourceColumn = null;
            infoPara_GEXRPT_OPOR109.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR109.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR109.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR1010.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR1010.ParameterName = "LOGINID";
            infoPara_GEXRPT_OPOR1010.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR1010.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR1010.Size = 0;
            infoPara_GEXRPT_OPOR1010.SourceColumn = null;
            infoPara_GEXRPT_OPOR1010.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR1010.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR1010.XmlSchemaCollectionOwningSchema = null;
            this.GEXRPT_OPOR10.InfoParameters.Add(infoPara_GEXRPT_OPOR101);
            this.GEXRPT_OPOR10.InfoParameters.Add(infoPara_GEXRPT_OPOR102);
            this.GEXRPT_OPOR10.InfoParameters.Add(infoPara_GEXRPT_OPOR103);
            this.GEXRPT_OPOR10.InfoParameters.Add(infoPara_GEXRPT_OPOR104);
            this.GEXRPT_OPOR10.InfoParameters.Add(infoPara_GEXRPT_OPOR105);
            this.GEXRPT_OPOR10.InfoParameters.Add(infoPara_GEXRPT_OPOR106);
            this.GEXRPT_OPOR10.InfoParameters.Add(infoPara_GEXRPT_OPOR107);
            this.GEXRPT_OPOR10.InfoParameters.Add(infoPara_GEXRPT_OPOR108);
            this.GEXRPT_OPOR10.InfoParameters.Add(infoPara_GEXRPT_OPOR109);
            this.GEXRPT_OPOR10.InfoParameters.Add(infoPara_GEXRPT_OPOR1010);
            this.GEXRPT_OPOR10.MultiSetWhere = false;
            this.GEXRPT_OPOR10.Name = "GEXRPT_OPOR10";
            this.GEXRPT_OPOR10.NotificationAutoEnlist = false;
            this.GEXRPT_OPOR10.SecExcept = null;
            this.GEXRPT_OPOR10.SecFieldName = "";
            this.GEXRPT_OPOR10.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.GEXRPT_OPOR10.SelectPaging = false;
            this.GEXRPT_OPOR10.SelectTop = 0;
            this.GEXRPT_OPOR10.SiteControl = false;
            this.GEXRPT_OPOR10.SiteFieldName = "";
            this.GEXRPT_OPOR10.UpdatedRowSource = System.Data.UpdateRowSource.None;

            // 
            // VIRTUAL_OPOR10
            // 
            this.VIRTUAL_OPOR10.CacheConnection = false;
            this.VIRTUAL_OPOR10.CommandText = "Select  USERID as BILLDATE, USERID as BILLNO, USERID as PRODUCTID, USERID as PRODCNAME, USERID as PRODSTRUCTURE, USERID as CUSTID, USERID as IDNO, USERID as CORPCNAME, USERID as ONHANDDATE, USERID as PERSONID, USERID as PERSONCNAME, USERID as CURRENCYID, RPT_SUM as CURRRATE, RPT_SUM as QUANTITY, RPT_SUM as PRICE, RPT_SUM as SUBAMOUNT, RPT_SUM as UNIQUENO, USERID as INVQTY, RPT_SUM as NOTOUTQTY From SYSUSER_CNT Where 1<>1";
            this.VIRTUAL_OPOR10.CommandTimeout = 1800;
            this.VIRTUAL_OPOR10.CommandType = System.Data.CommandType.Text;
            this.VIRTUAL_OPOR10.DynamicTableName = false;
            this.VIRTUAL_OPOR10.EEPAlias = null;
            this.VIRTUAL_OPOR10.EncodingAfter = null;
            this.VIRTUAL_OPOR10.EncodingBefore = "Windows-1252";
            this.VIRTUAL_OPOR10.InfoConnection = this.InfoConnection1;
            this.VIRTUAL_OPOR10.MultiSetWhere = false;
            this.VIRTUAL_OPOR10.Name = "VIRTUAL_OPOR10";
            this.VIRTUAL_OPOR10.NotificationAutoEnlist = false;
            this.VIRTUAL_OPOR10.SecExcept = null;
            this.VIRTUAL_OPOR10.SecFieldName = null;
            this.VIRTUAL_OPOR10.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIRTUAL_OPOR10.SelectPaging = false;
            this.VIRTUAL_OPOR10.SelectTop = 0;
            this.VIRTUAL_OPOR10.SiteControl = false;
            this.VIRTUAL_OPOR10.SiteFieldName = null;
            this.VIRTUAL_OPOR10.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR11
            // 
            this.VIEW_RPT_OPOR11.CacheConnection = false;
            this.VIEW_RPT_OPOR11.CommandText = "SELECT * FROM [VIEW_RPT_OPOR11]";
            this.VIEW_RPT_OPOR11.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR11.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR11.DynamicTableName = false;
            this.VIEW_RPT_OPOR11.EEPAlias = null;
            this.VIEW_RPT_OPOR11.EncodingAfter = null;
            this.VIEW_RPT_OPOR11.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR11.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR11.MultiSetWhere = false;
            this.VIEW_RPT_OPOR11.Name = "VIEW_RPT_OPOR11";
            this.VIEW_RPT_OPOR11.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR11.SecExcept = null;
            this.VIEW_RPT_OPOR11.SecFieldName = null;
            this.VIEW_RPT_OPOR11.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR11.SelectTop = 0;
            this.VIEW_RPT_OPOR11.SiteControl = false;
            this.VIEW_RPT_OPOR11.SiteFieldName = null;
            this.VIEW_RPT_OPOR11.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR12
            // 
            this.VIEW_RPT_OPOR12.CacheConnection = false;
            this.VIEW_RPT_OPOR12.CommandText = "SELECT * FROM [VIEW_RPT_OPOR12]";
            this.VIEW_RPT_OPOR12.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR12.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR12.DynamicTableName = false;
            this.VIEW_RPT_OPOR12.EEPAlias = null;
            this.VIEW_RPT_OPOR12.EncodingAfter = null;
            this.VIEW_RPT_OPOR12.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR12.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR12.MultiSetWhere = false;
            this.VIEW_RPT_OPOR12.Name = "VIEW_RPT_OPOR12";
            this.VIEW_RPT_OPOR12.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR12.SecExcept = null;
            this.VIEW_RPT_OPOR12.SecFieldName = null;
            this.VIEW_RPT_OPOR12.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR12.SelectTop = 0;
            this.VIEW_RPT_OPOR12.SiteControl = false;
            this.VIEW_RPT_OPOR12.SiteFieldName = null;
            this.VIEW_RPT_OPOR12.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR13
            // 
            this.VIEW_RPT_OPOR13.CacheConnection = false;
            this.VIEW_RPT_OPOR13.CommandText = "SELECT * FROM [VIEW_RPT_OPOR13]";
            this.VIEW_RPT_OPOR13.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR13.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR13.DynamicTableName = false;
            this.VIEW_RPT_OPOR13.EEPAlias = null;
            this.VIEW_RPT_OPOR13.EncodingAfter = null;
            this.VIEW_RPT_OPOR13.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR13.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR13.MultiSetWhere = false;
            this.VIEW_RPT_OPOR13.Name = "VIEW_RPT_OPOR13";
            this.VIEW_RPT_OPOR13.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR13.SecExcept = null;
            this.VIEW_RPT_OPOR13.SecFieldName = null;
            this.VIEW_RPT_OPOR13.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR13.SelectTop = 0;
            this.VIEW_RPT_OPOR13.SiteControl = false;
            this.VIEW_RPT_OPOR13.SiteFieldName = null;
            this.VIEW_RPT_OPOR13.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR14
            // 
            this.VIEW_RPT_OPOR14.CacheConnection = false;
            this.VIEW_RPT_OPOR14.CommandText = "SELECT * FROM [VIEW_RPT_OPOR14]";
            this.VIEW_RPT_OPOR14.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR14.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR14.DynamicTableName = false;
            this.VIEW_RPT_OPOR14.EEPAlias = null;
            this.VIEW_RPT_OPOR14.EncodingAfter = null;
            this.VIEW_RPT_OPOR14.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR14.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR14.MultiSetWhere = false;
            this.VIEW_RPT_OPOR14.Name = "VIEW_RPT_OPOR14";
            this.VIEW_RPT_OPOR14.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR14.SecExcept = null;
            this.VIEW_RPT_OPOR14.SecFieldName = null;
            this.VIEW_RPT_OPOR14.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR14.SelectTop = 0;
            this.VIEW_RPT_OPOR14.SiteControl = false;
            this.VIEW_RPT_OPOR14.SiteFieldName = null;
            this.VIEW_RPT_OPOR14.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // VIEW_RPT_OPOR16
            // 
            this.VIEW_RPT_OPOR16.CacheConnection = false;
            this.VIEW_RPT_OPOR16.CommandText = "SELECT * FROM [VIEW_RPT_OPOR16]";
            this.VIEW_RPT_OPOR16.CommandTimeout = 1800;
            this.VIEW_RPT_OPOR16.CommandType = System.Data.CommandType.Text;
            this.VIEW_RPT_OPOR16.DynamicTableName = false;
            this.VIEW_RPT_OPOR16.EEPAlias = null;
            this.VIEW_RPT_OPOR16.EncodingAfter = null;
            this.VIEW_RPT_OPOR16.EncodingBefore = "Windows-1252";
            this.VIEW_RPT_OPOR16.InfoConnection = this.InfoConnection1;
            this.VIEW_RPT_OPOR16.MultiSetWhere = false;
            this.VIEW_RPT_OPOR16.Name = "VIEW_RPT_OPOR16";
            this.VIEW_RPT_OPOR16.NotificationAutoEnlist = false;
            this.VIEW_RPT_OPOR16.SecExcept = null;
            this.VIEW_RPT_OPOR16.SecFieldName = null;
            this.VIEW_RPT_OPOR16.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIEW_RPT_OPOR16.SelectTop = 0;
            this.VIEW_RPT_OPOR16.SiteControl = false;
            this.VIEW_RPT_OPOR16.SiteFieldName = null;
            this.VIEW_RPT_OPOR16.UpdatedRowSource = System.Data.UpdateRowSource.None;
        //
            // GEXRPT_OPOR21
            // 
            this.GEXRPT_OPOR21.CacheConnection = false;
            this.GEXRPT_OPOR21.CommandText = "GEXRPT_OPOR21";
            this.GEXRPT_OPOR21.CommandTimeout = 1800;
            this.GEXRPT_OPOR21.CommandType = System.Data.CommandType.StoredProcedure;
            this.GEXRPT_OPOR21.DynamicTableName = false;
            this.GEXRPT_OPOR21.EEPAlias = "";
            this.GEXRPT_OPOR21.EncodingAfter = null;
            this.GEXRPT_OPOR21.EncodingBefore = "Windows-1252";
            this.GEXRPT_OPOR21.InfoConnection = this.InfoConnection1;
            infoPara_GEXRPT_OPOR211.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR211.ParameterName = "PRODUCTID1";
            infoPara_GEXRPT_OPOR211.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR211.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR211.Size = 0;
            infoPara_GEXRPT_OPOR211.SourceColumn = null;
            infoPara_GEXRPT_OPOR211.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR211.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR211.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR212.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR212.ParameterName = "PRODUCTID2";
            infoPara_GEXRPT_OPOR212.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR212.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR212.Size = 0;
            infoPara_GEXRPT_OPOR212.SourceColumn = null;
            infoPara_GEXRPT_OPOR212.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR212.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR212.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR213.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR213.ParameterName = "WAREID1";
            infoPara_GEXRPT_OPOR213.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR213.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR213.Size = 0;
            infoPara_GEXRPT_OPOR213.SourceColumn = null;
            infoPara_GEXRPT_OPOR213.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR213.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR213.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR214.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR214.ParameterName = "WAREID2";
            infoPara_GEXRPT_OPOR214.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR214.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR214.Size = 0;
            infoPara_GEXRPT_OPOR214.SourceColumn = null;
            infoPara_GEXRPT_OPOR214.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR214.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR214.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR215.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR215.ParameterName = "ONHANDDATE2";
            infoPara_GEXRPT_OPOR215.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR215.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR215.Size = 0;
            infoPara_GEXRPT_OPOR215.SourceColumn = null;
            infoPara_GEXRPT_OPOR215.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR215.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR215.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR216.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR216.ParameterName = "ISZERO";
            infoPara_GEXRPT_OPOR216.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR216.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR216.Size = 0;
            infoPara_GEXRPT_OPOR216.SourceColumn = null;
            infoPara_GEXRPT_OPOR216.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR216.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR216.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR217.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR217.ParameterName = "LOGINID";
            infoPara_GEXRPT_OPOR217.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR217.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR217.Size = 0;
            infoPara_GEXRPT_OPOR217.SourceColumn = null;
            infoPara_GEXRPT_OPOR217.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR217.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR217.XmlSchemaCollectionOwningSchema = null;
            this.GEXRPT_OPOR21.InfoParameters.Add(infoPara_GEXRPT_OPOR211);
            this.GEXRPT_OPOR21.InfoParameters.Add(infoPara_GEXRPT_OPOR212);
            this.GEXRPT_OPOR21.InfoParameters.Add(infoPara_GEXRPT_OPOR213);
            this.GEXRPT_OPOR21.InfoParameters.Add(infoPara_GEXRPT_OPOR214);
            this.GEXRPT_OPOR21.InfoParameters.Add(infoPara_GEXRPT_OPOR215);
            this.GEXRPT_OPOR21.InfoParameters.Add(infoPara_GEXRPT_OPOR216);
            this.GEXRPT_OPOR21.InfoParameters.Add(infoPara_GEXRPT_OPOR217);
            this.GEXRPT_OPOR21.MultiSetWhere = false;
            this.GEXRPT_OPOR21.Name = "GEXRPT_OPOR21";
            this.GEXRPT_OPOR21.NotificationAutoEnlist = false;
            this.GEXRPT_OPOR21.SecExcept = null;
            this.GEXRPT_OPOR21.SecFieldName = "";
            this.GEXRPT_OPOR21.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.GEXRPT_OPOR21.SelectPaging = false;
            this.GEXRPT_OPOR21.SelectTop = 0;
            this.GEXRPT_OPOR21.SiteControl = false;
            this.GEXRPT_OPOR21.SiteFieldName = "";
            this.GEXRPT_OPOR21.UpdatedRowSource = System.Data.UpdateRowSource.None;

            // 
            // VIRTUAL_OPOR21
            // 
            this.VIRTUAL_OPOR21.CacheConnection = false;
            this.VIRTUAL_OPOR21.CommandText = "Select  RPT_SUM as FLAG, USERID as BILLNO, USERID as PRODUCTID, USERID as PRODCNAME, USERID as PRODSTRUCTURE, USERID as ONHANDDATE, USERID as IDNAME, RPT_SUM as QUANTITY, RPT_SUM as QTY1, RPT_SUM as QTY2, RPT_SUM as QTY3, RPT_SUM as UNIQUENO From SYSUSER_CNT Where 1<>1";
            this.VIRTUAL_OPOR21.CommandTimeout = 1800;
            this.VIRTUAL_OPOR21.CommandType = System.Data.CommandType.Text;
            this.VIRTUAL_OPOR21.DynamicTableName = false;
            this.VIRTUAL_OPOR21.EEPAlias = null;
            this.VIRTUAL_OPOR21.EncodingAfter = null;
            this.VIRTUAL_OPOR21.EncodingBefore = "Windows-1252";
            this.VIRTUAL_OPOR21.InfoConnection = this.InfoConnection1;
            this.VIRTUAL_OPOR21.MultiSetWhere = false;
            this.VIRTUAL_OPOR21.Name = "VIRTUAL_OPOR21";
            this.VIRTUAL_OPOR21.NotificationAutoEnlist = false;
            this.VIRTUAL_OPOR21.SecExcept = null;
            this.VIRTUAL_OPOR21.SecFieldName = null;
            this.VIRTUAL_OPOR21.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIRTUAL_OPOR21.SelectPaging = false;
            this.VIRTUAL_OPOR21.SelectTop = 0;
            this.VIRTUAL_OPOR21.SiteControl = false;
            this.VIRTUAL_OPOR21.SiteFieldName = null;
            this.VIRTUAL_OPOR21.UpdatedRowSource = System.Data.UpdateRowSource.None;
        //
            // GEXRPT_OPOR22
            // 
            this.GEXRPT_OPOR22.CacheConnection = false;
            this.GEXRPT_OPOR22.CommandText = "GEXRPT_OPOR22";
            this.GEXRPT_OPOR22.CommandTimeout = 1800;
            this.GEXRPT_OPOR22.CommandType = System.Data.CommandType.StoredProcedure;
            this.GEXRPT_OPOR22.DynamicTableName = false;
            this.GEXRPT_OPOR22.EEPAlias = "";
            this.GEXRPT_OPOR22.EncodingAfter = null;
            this.GEXRPT_OPOR22.EncodingBefore = "Windows-1252";
            this.GEXRPT_OPOR22.InfoConnection = this.InfoConnection1;
            infoPara_GEXRPT_OPOR221.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR221.ParameterName = "PRODUCTID1";
            infoPara_GEXRPT_OPOR221.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR221.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR221.Size = 0;
            infoPara_GEXRPT_OPOR221.SourceColumn = null;
            infoPara_GEXRPT_OPOR221.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR221.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR221.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR222.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR222.ParameterName = "PRODUCTID2";
            infoPara_GEXRPT_OPOR222.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR222.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR222.Size = 0;
            infoPara_GEXRPT_OPOR222.SourceColumn = null;
            infoPara_GEXRPT_OPOR222.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR222.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR222.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR223.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR223.ParameterName = "WAREID1";
            infoPara_GEXRPT_OPOR223.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR223.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR223.Size = 0;
            infoPara_GEXRPT_OPOR223.SourceColumn = null;
            infoPara_GEXRPT_OPOR223.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR223.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR223.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR224.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR224.ParameterName = "WAREID2";
            infoPara_GEXRPT_OPOR224.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR224.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR224.Size = 0;
            infoPara_GEXRPT_OPOR224.SourceColumn = null;
            infoPara_GEXRPT_OPOR224.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR224.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR224.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR225.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR225.ParameterName = "ONHANDDATE2";
            infoPara_GEXRPT_OPOR225.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR225.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR225.Size = 0;
            infoPara_GEXRPT_OPOR225.SourceColumn = null;
            infoPara_GEXRPT_OPOR225.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR225.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR225.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR226.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR226.ParameterName = "ISZERO";
            infoPara_GEXRPT_OPOR226.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR226.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR226.Size = 0;
            infoPara_GEXRPT_OPOR226.SourceColumn = null;
            infoPara_GEXRPT_OPOR226.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR226.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR226.XmlSchemaCollectionOwningSchema = null;
            infoPara_GEXRPT_OPOR227.InfoDbType = Srvtools.InfoDbType.NChar;
            infoPara_GEXRPT_OPOR227.ParameterName = "LOGINID";
            infoPara_GEXRPT_OPOR227.Precision = ((byte)(0));
            infoPara_GEXRPT_OPOR227.Scale = ((byte)(0));
            infoPara_GEXRPT_OPOR227.Size = 0;
            infoPara_GEXRPT_OPOR227.SourceColumn = null;
            infoPara_GEXRPT_OPOR227.XmlSchemaCollectionDatabase = null;
            infoPara_GEXRPT_OPOR227.XmlSchemaCollectionName = null;
            infoPara_GEXRPT_OPOR227.XmlSchemaCollectionOwningSchema = null;
            this.GEXRPT_OPOR22.InfoParameters.Add(infoPara_GEXRPT_OPOR221);
            this.GEXRPT_OPOR22.InfoParameters.Add(infoPara_GEXRPT_OPOR222);
            this.GEXRPT_OPOR22.InfoParameters.Add(infoPara_GEXRPT_OPOR223);
            this.GEXRPT_OPOR22.InfoParameters.Add(infoPara_GEXRPT_OPOR224);
            this.GEXRPT_OPOR22.InfoParameters.Add(infoPara_GEXRPT_OPOR225);
            this.GEXRPT_OPOR22.InfoParameters.Add(infoPara_GEXRPT_OPOR226);
            this.GEXRPT_OPOR22.InfoParameters.Add(infoPara_GEXRPT_OPOR227);
            this.GEXRPT_OPOR22.MultiSetWhere = false;
            this.GEXRPT_OPOR22.Name = "GEXRPT_OPOR22";
            this.GEXRPT_OPOR22.NotificationAutoEnlist = false;
            this.GEXRPT_OPOR22.SecExcept = null;
            this.GEXRPT_OPOR22.SecFieldName = "";
            this.GEXRPT_OPOR22.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.GEXRPT_OPOR22.SelectPaging = false;
            this.GEXRPT_OPOR22.SelectTop = 0;
            this.GEXRPT_OPOR22.SiteControl = false;
            this.GEXRPT_OPOR22.SiteFieldName = "";
            this.GEXRPT_OPOR22.UpdatedRowSource = System.Data.UpdateRowSource.None;

            // 
            // VIRTUAL_OPOR22
            // 
            this.VIRTUAL_OPOR22.CacheConnection = false;
            this.VIRTUAL_OPOR22.CommandText = "Select  USERID as PRODUCTID, USERID as PRODCNAME, USERID as PRODSTRUCTURE, RPT_SUM as NOWQUANTITY, RPT_SUM as OPO_QTYOUT, RPT_SUM as OPO_QTYIN, RPT_SUM as BILLQTY From SYSUSER_CNT Where 1<>1";
            this.VIRTUAL_OPOR22.CommandTimeout = 1800;
            this.VIRTUAL_OPOR22.CommandType = System.Data.CommandType.Text;
            this.VIRTUAL_OPOR22.DynamicTableName = false;
            this.VIRTUAL_OPOR22.EEPAlias = null;
            this.VIRTUAL_OPOR22.EncodingAfter = null;
            this.VIRTUAL_OPOR22.EncodingBefore = "Windows-1252";
            this.VIRTUAL_OPOR22.InfoConnection = this.InfoConnection1;
            this.VIRTUAL_OPOR22.MultiSetWhere = false;
            this.VIRTUAL_OPOR22.Name = "VIRTUAL_OPOR22";
            this.VIRTUAL_OPOR22.NotificationAutoEnlist = false;
            this.VIRTUAL_OPOR22.SecExcept = null;
            this.VIRTUAL_OPOR22.SecFieldName = null;
            this.VIRTUAL_OPOR22.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.VIRTUAL_OPOR22.SelectPaging = false;
            this.VIRTUAL_OPOR22.SelectTop = 0;
            this.VIRTUAL_OPOR22.SiteControl = false;
            this.VIRTUAL_OPOR22.SiteFieldName = null;
            this.VIRTUAL_OPOR22.UpdatedRowSource = System.Data.UpdateRowSource.None;

            // New Report View Add HERE
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_BOMR01)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_BOMR01)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_BOMR02)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_BOMR02)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR01)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR02)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR03)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR04)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR05)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR07)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR09)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_OPOR0A)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_OPOR0A)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_OPOR0B)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_OPOR0B)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_OPOR10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_OPOR10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR11)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR13)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR14)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIEW_RPT_OPOR16)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_OPOR21)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_OPOR21)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.GEXRPT_OPOR22)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.VIRTUAL_OPOR22)).EndInit();

            // New Report EndInit Add HERE
        }

        #endregion

        System.Data.IDbConnection connection = null;
        protected System.Data.IDbConnection GetInfoconn()
        {
            System.Data.IDbConnection connection = (System.Data.IDbConnection)AllocateConnection(this.GetClientInfo(ClientInfoType.LoginDB).ToString());
            if (connection.State != System.Data.ConnectionState.Open)
            {
                connection.Open();
            }

            return connection;
        }

        protected void ReleaseConn()
        {
            if (connection != null)
            {
                connection.Close();
                ReleaseConnection(this.GetClientInfo(ClientInfoType.LoginDB).ToString(), connection);
            }
        }
        
        protected void ReleaseConn(System.Data.IDbConnection conn)
        {
            if (conn != null)
            {
                conn.Close();
                ReleaseConnection(this.GetClientInfo(ClientInfoType.LoginDB).ToString(), conn);
            }
        }

        public object[] GetData_GEXRPT_BOMR01(object[] objParam)
        {
          //
          IDbConnection conn = GetInfoconn();
          try
          {
              GEXRPT_BOMR01.Connection = conn;
              //
              GEXRPT_BOMR01.InfoParameters[0].Value = objParam[0];
              GEXRPT_BOMR01.InfoParameters[1].Value = objParam[1];
              GEXRPT_BOMR01.InfoParameters[2].Value = objParam[2];
              //
              System.Data.DataSet ds = GEXRPT_BOMR01.ExecuteDataSet();
              return new object[] { 0, ds };
          }
          finally
          {
              ReleaseConn(conn);
          }
        }

        // Call By EEP2012 JS
        public object[] GetData_GEXRPT_BOMR01_JS(object[] objParam)
        {
            try
            {
                object[] param = objParam[0].ToString().Split('|');
                object[] ret = GetData_GEXRPT_BOMR01(param);
                System.Data.DataSet ds = (System.Data.DataSet)ret[1];

                //Indented trans Data to Json
                string retStr = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);

                return new object[] { 0, retStr };
            }
            finally
            {

            }
        }
        public object[] GetData_GEXRPT_BOMR02(object[] objParam)
        {
          //
          IDbConnection conn = GetInfoconn();
          try
          {
              GEXRPT_BOMR02.Connection = conn;
              //
              GEXRPT_BOMR02.InfoParameters[0].Value = objParam[0];
              //
              System.Data.DataSet ds = GEXRPT_BOMR02.ExecuteDataSet();
              return new object[] { 0, ds };
          }
          finally
          {
              ReleaseConn(conn);
          }
        }

        // Call By EEP2012 JS
        public object[] GetData_GEXRPT_BOMR02_JS(object[] objParam)
        {
            try
            {
                object[] param = objParam[0].ToString().Split('|');
                object[] ret = GetData_GEXRPT_BOMR02(param);
                System.Data.DataSet ds = (System.Data.DataSet)ret[1];

                //Indented trans Data to Json
                string retStr = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);

                return new object[] { 0, retStr };
            }
            finally
            {

            }
        }
        public object[] GetData_GEXRPT_OPOR0A(object[] objParam)
        {
          //
          IDbConnection conn = GetInfoconn();
          try
          {
              GEXRPT_OPOR0A.Connection = conn;
              //
              GEXRPT_OPOR0A.InfoParameters[0].Value = objParam[0];
              GEXRPT_OPOR0A.InfoParameters[1].Value = objParam[1];
              GEXRPT_OPOR0A.InfoParameters[2].Value = objParam[2];
              GEXRPT_OPOR0A.InfoParameters[3].Value = objParam[3];
              GEXRPT_OPOR0A.InfoParameters[4].Value = objParam[4];
              GEXRPT_OPOR0A.InfoParameters[5].Value = objParam[5];
              GEXRPT_OPOR0A.InfoParameters[6].Value = objParam[6];
              //
              System.Data.DataSet ds = GEXRPT_OPOR0A.ExecuteDataSet();
              return new object[] { 0, ds };
          }
          finally
          {
              ReleaseConn(conn);
          }
        }

        // Call By EEP2012 JS
        public object[] GetData_GEXRPT_OPOR0A_JS(object[] objParam)
        {
            try
            {
                object[] param = objParam[0].ToString().Split('|');
                object[] ret = GetData_GEXRPT_OPOR0A(param);
                System.Data.DataSet ds = (System.Data.DataSet)ret[1];

                //Indented trans Data to Json
                string retStr = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);

                return new object[] { 0, retStr };
            }
            finally
            {

            }
        }
        public object[] GetData_GEXRPT_OPOR0B(object[] objParam)
        {
          //
          IDbConnection conn = GetInfoconn();
          try
          {
              GEXRPT_OPOR0B.Connection = conn;
              //
              GEXRPT_OPOR0B.InfoParameters[0].Value = objParam[0];
              GEXRPT_OPOR0B.InfoParameters[1].Value = objParam[1];
              GEXRPT_OPOR0B.InfoParameters[2].Value = objParam[2];
              GEXRPT_OPOR0B.InfoParameters[3].Value = objParam[3];
              GEXRPT_OPOR0B.InfoParameters[4].Value = objParam[4];
              GEXRPT_OPOR0B.InfoParameters[5].Value = objParam[5];
              //
              System.Data.DataSet ds = GEXRPT_OPOR0B.ExecuteDataSet();
              return new object[] { 0, ds };
          }
          finally
          {
              ReleaseConn(conn);
          }
        }

        // Call By EEP2012 JS
        public object[] GetData_GEXRPT_OPOR0B_JS(object[] objParam)
        {
            try
            {
                object[] param = objParam[0].ToString().Split('|');
                object[] ret = GetData_GEXRPT_OPOR0B(param);
                System.Data.DataSet ds = (System.Data.DataSet)ret[1];

                //Indented trans Data to Json
                string retStr = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);

                return new object[] { 0, retStr };
            }
            finally
            {

            }
        }
        public object[] GetData_GEXRPT_OPOR10(object[] objParam)
        {
          //
          IDbConnection conn = GetInfoconn();
          try
          {
              GEXRPT_OPOR10.Connection = conn;
              //
              GEXRPT_OPOR10.InfoParameters[0].Value = objParam[0];
              GEXRPT_OPOR10.InfoParameters[1].Value = objParam[1];
              GEXRPT_OPOR10.InfoParameters[2].Value = objParam[2];
              GEXRPT_OPOR10.InfoParameters[3].Value = objParam[3];
              GEXRPT_OPOR10.InfoParameters[4].Value = objParam[4];
              GEXRPT_OPOR10.InfoParameters[5].Value = objParam[5];
              GEXRPT_OPOR10.InfoParameters[6].Value = objParam[6];
              GEXRPT_OPOR10.InfoParameters[7].Value = objParam[7];
              GEXRPT_OPOR10.InfoParameters[8].Value = objParam[8];
              GEXRPT_OPOR10.InfoParameters[9].Value = objParam[9];
              //
              System.Data.DataSet ds = GEXRPT_OPOR10.ExecuteDataSet();
              return new object[] { 0, ds };
          }
          finally
          {
              ReleaseConn(conn);
          }
        }

        // Call By EEP2012 JS
        public object[] GetData_GEXRPT_OPOR10_JS(object[] objParam)
        {
            try
            {
                object[] param = objParam[0].ToString().Split('|');
                object[] ret = GetData_GEXRPT_OPOR10(param);
                System.Data.DataSet ds = (System.Data.DataSet)ret[1];

                //Indented trans Data to Json
                string retStr = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);

                return new object[] { 0, retStr };
            }
            finally
            {

            }
        }
        public object[] GetData_GEXRPT_OPOR21(object[] objParam)
        {
          //
          IDbConnection conn = GetInfoconn();
          try
          {
              GEXRPT_OPOR21.Connection = conn;
              //
              GEXRPT_OPOR21.InfoParameters[0].Value = objParam[0];
              GEXRPT_OPOR21.InfoParameters[1].Value = objParam[1];
              GEXRPT_OPOR21.InfoParameters[2].Value = objParam[2];
              GEXRPT_OPOR21.InfoParameters[3].Value = objParam[3];
              GEXRPT_OPOR21.InfoParameters[4].Value = objParam[4];
              GEXRPT_OPOR21.InfoParameters[5].Value = objParam[5];
              GEXRPT_OPOR21.InfoParameters[6].Value = objParam[6];
              //
              System.Data.DataSet ds = GEXRPT_OPOR21.ExecuteDataSet();
              return new object[] { 0, ds };
          }
          finally
          {
              ReleaseConn(conn);
          }
        }

        // Call By EEP2012 JS
        public object[] GetData_GEXRPT_OPOR21_JS(object[] objParam)
        {
            try
            {
                object[] param = objParam[0].ToString().Split('|');
                object[] ret = GetData_GEXRPT_OPOR21(param);
                System.Data.DataSet ds = (System.Data.DataSet)ret[1];

                //Indented trans Data to Json
                string retStr = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);

                return new object[] { 0, retStr };
            }
            finally
            {

            }
        }
        public object[] GetData_GEXRPT_OPOR22(object[] objParam)
        {
          //
          IDbConnection conn = GetInfoconn();
          try
          {
              GEXRPT_OPOR22.Connection = conn;
              //
              GEXRPT_OPOR22.InfoParameters[0].Value = objParam[0];
              GEXRPT_OPOR22.InfoParameters[1].Value = objParam[1];
              GEXRPT_OPOR22.InfoParameters[2].Value = objParam[2];
              GEXRPT_OPOR22.InfoParameters[3].Value = objParam[3];
              GEXRPT_OPOR22.InfoParameters[4].Value = objParam[4];
              GEXRPT_OPOR22.InfoParameters[5].Value = objParam[5];
              GEXRPT_OPOR22.InfoParameters[6].Value = objParam[6];
              //
              System.Data.DataSet ds = GEXRPT_OPOR22.ExecuteDataSet();
              return new object[] { 0, ds };
          }
          finally
          {
              ReleaseConn(conn);
          }
        }

        // Call By EEP2012 JS
        public object[] GetData_GEXRPT_OPOR22_JS(object[] objParam)
        {
            try
            {
                object[] param = objParam[0].ToString().Split('|');
                object[] ret = GetData_GEXRPT_OPOR22(param);
                System.Data.DataSet ds = (System.Data.DataSet)ret[1];

                //Indented trans Data to Json
                string retStr = Newtonsoft.Json.JsonConvert.SerializeObject(ds.Tables[0], Newtonsoft.Json.Formatting.Indented);

                return new object[] { 0, retStr };
            }
            finally
            {

            }
        }

        // New Report SP function Add HERE
    }
}
