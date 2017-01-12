using System;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Reflection;
using Microsoft.Win32;
using System.IO;
using Srvtools;
using System.Security;
using System.Security.Permissions;

namespace SMasterDetail
{
    /// <summary>
    /// Summary description for Component.
    /// </summary>
    public class Component : DataModule
    {
        private UpdateComponent ucMaster;
        private ServiceManager serviceManager;
        private InfoCommand Master;
        private InfoCommand Detail;
        private InfoDataSource idsRelation;
        private UpdateComponent ucDetail;
        private InfoConnection infoConnection;
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
            this.ucMaster = new Srvtools.UpdateComponent(this.components);
            this.Master = new Srvtools.InfoCommand(this.components);
            this.infoConnection = new Srvtools.InfoConnection();
            this.serviceManager = new Srvtools.ServiceManager(this.components);
            this.Detail = new Srvtools.InfoCommand(this.components);
            this.idsRelation = new Srvtools.InfoDataSource(this.components);
            this.ucDetail = new Srvtools.UpdateComponent(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.Master)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Detail)).BeginInit();
            // 
            // ucMaster
            // 
            this.ucMaster.AutoTrans = true;
            this.ucMaster.ExceptJoin = false;
            this.ucMaster.LogInfo = null;
            this.ucMaster.Name = "ucMaster";
            this.ucMaster.SelectCmd = this.Master;
            this.ucMaster.ServerModify = true;
            this.ucMaster.ServerModifyGetMax = false;
            this.ucMaster.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucMaster.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // Master
            // 
            this.Master.CommandText = "";
            this.Master.CommandTimeout = 30;
            this.Master.CommandType = System.Data.CommandType.Text;
            this.Master.EEPAlias = null;
            this.Master.InfoConnection = this.infoConnection;
            this.Master.Name = "Master";
            this.Master.NotificationAutoEnlist = false;
            this.Master.SecExcept = null;
            this.Master.SecFieldName = "";
            this.Master.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.Master.SelectTop = 0;
            this.Master.SiteControl = false;
            this.Master.SiteFieldName = "";
            this.Master.Transaction = null;
            this.Master.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // infoConnection
            // 
            this.infoConnection.ConnectionType = Srvtools.ConnectionType.SqlClient;
            // 
            // Detail
            // 
            this.Detail.CommandText = "";
            this.Detail.CommandTimeout = 30;
            this.Detail.CommandType = System.Data.CommandType.Text;
            this.Detail.EEPAlias = null;
            this.Detail.InfoConnection = this.infoConnection;
            this.Detail.Name = "Detail";
            this.Detail.NotificationAutoEnlist = false;
            this.Detail.SecExcept = null;
            this.Detail.SecFieldName = "";
            this.Detail.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.Detail.SelectTop = 0;
            this.Detail.SiteControl = false;
            this.Detail.SiteFieldName = "";
            this.Detail.Transaction = null;
            this.Detail.UpdatedRowSource = System.Data.UpdateRowSource.Both;
            // 
            // idsRelation
            // 
            this.idsRelation.Detail = this.Detail;
            this.idsRelation.Master = this.Master;
            // 
            // ucDetail
            // 
            this.ucDetail.AutoTrans = true;
            this.ucDetail.ExceptJoin = false;
            this.ucDetail.LogInfo = null;
            this.ucDetail.Name = "ucDetail";
            this.ucDetail.SelectCmd = this.Detail;
            this.ucDetail.ServerModify = true;
            this.ucDetail.ServerModifyGetMax = false;
            this.ucDetail.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucDetail.WhereMode = Srvtools.WhereModeType.Keyfields;
            ((System.ComponentModel.ISupportInitialize)(this.Master)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Detail)).EndInit();

        }
        #endregion
    }
}
