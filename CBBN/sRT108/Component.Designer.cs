namespace sRT108
{
    partial class Component
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.HBADSLCMTY = new Srvtools.InfoCommand(this.components);
            this.ucHBADSLCMTY = new Srvtools.UpdateComponent(this.components);
            this.View_HBADSLCMTY = new Srvtools.InfoCommand(this.components);
            this.RT108 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HBADSLCMTY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_HBADSLCMTY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT108)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // HBADSLCMTY
            // 
            this.HBADSLCMTY.CacheConnection = false;
            this.HBADSLCMTY.CommandText = "SELECT dbo.[HBADSLCMTY].* FROM dbo.[HBADSLCMTY]";
            this.HBADSLCMTY.CommandTimeout = 30;
            this.HBADSLCMTY.CommandType = System.Data.CommandType.Text;
            this.HBADSLCMTY.DynamicTableName = false;
            this.HBADSLCMTY.EEPAlias = null;
            this.HBADSLCMTY.EncodingAfter = null;
            this.HBADSLCMTY.EncodingBefore = "Windows-1252";
            this.HBADSLCMTY.EncodingConvert = null;
            this.HBADSLCMTY.InfoConnection = this.InfoConnection1;
            this.HBADSLCMTY.MultiSetWhere = false;
            this.HBADSLCMTY.Name = "HBADSLCMTY";
            this.HBADSLCMTY.NotificationAutoEnlist = false;
            this.HBADSLCMTY.SecExcept = null;
            this.HBADSLCMTY.SecFieldName = null;
            this.HBADSLCMTY.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.HBADSLCMTY.SelectPaging = false;
            this.HBADSLCMTY.SelectTop = 0;
            this.HBADSLCMTY.SiteControl = false;
            this.HBADSLCMTY.SiteFieldName = null;
            this.HBADSLCMTY.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucHBADSLCMTY
            // 
            this.ucHBADSLCMTY.AutoTrans = true;
            this.ucHBADSLCMTY.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "comq1";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "LINEQ1";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "comn";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "addr";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "comcnt";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "usercnt";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "comtype";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "T1APPLYDAT";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "comsource";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "groupnc";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "leader";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "comagree";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "contract";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "signetdat";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "significantCNT";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "AREANC";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "RCOMDROP";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "CTYContact";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "CTYcontactTel";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "IPADDR";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "LINETEL";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "CANCELDAT";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr1);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr2);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr3);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr4);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr5);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr6);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr7);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr8);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr9);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr10);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr11);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr12);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr13);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr14);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr15);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr16);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr17);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr18);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr19);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr20);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr21);
            this.ucHBADSLCMTY.FieldAttrs.Add(fieldAttr22);
            this.ucHBADSLCMTY.LogInfo = null;
            this.ucHBADSLCMTY.Name = "ucHBADSLCMTY";
            this.ucHBADSLCMTY.RowAffectsCheck = true;
            this.ucHBADSLCMTY.SelectCmd = this.HBADSLCMTY;
            this.ucHBADSLCMTY.SelectCmdForUpdate = null;
            this.ucHBADSLCMTY.SendSQLCmd = true;
            this.ucHBADSLCMTY.ServerModify = true;
            this.ucHBADSLCMTY.ServerModifyGetMax = false;
            this.ucHBADSLCMTY.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucHBADSLCMTY.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucHBADSLCMTY.UseTranscationScope = false;
            this.ucHBADSLCMTY.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_HBADSLCMTY
            // 
            this.View_HBADSLCMTY.CacheConnection = false;
            this.View_HBADSLCMTY.CommandText = "SELECT * FROM dbo.[HBADSLCMTY]";
            this.View_HBADSLCMTY.CommandTimeout = 30;
            this.View_HBADSLCMTY.CommandType = System.Data.CommandType.Text;
            this.View_HBADSLCMTY.DynamicTableName = false;
            this.View_HBADSLCMTY.EEPAlias = null;
            this.View_HBADSLCMTY.EncodingAfter = null;
            this.View_HBADSLCMTY.EncodingBefore = "Windows-1252";
            this.View_HBADSLCMTY.EncodingConvert = null;
            this.View_HBADSLCMTY.InfoConnection = this.InfoConnection1;
            this.View_HBADSLCMTY.MultiSetWhere = false;
            this.View_HBADSLCMTY.Name = "View_HBADSLCMTY";
            this.View_HBADSLCMTY.NotificationAutoEnlist = false;
            this.View_HBADSLCMTY.SecExcept = null;
            this.View_HBADSLCMTY.SecFieldName = null;
            this.View_HBADSLCMTY.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_HBADSLCMTY.SelectPaging = false;
            this.View_HBADSLCMTY.SelectTop = 0;
            this.View_HBADSLCMTY.SiteControl = false;
            this.View_HBADSLCMTY.SiteFieldName = null;
            this.View_HBADSLCMTY.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // RT108
            // 
            this.RT108.CacheConnection = false;
            this.RT108.CommandText = resources.GetString("RT108.CommandText");
            this.RT108.CommandTimeout = 30;
            this.RT108.CommandType = System.Data.CommandType.Text;
            this.RT108.DynamicTableName = false;
            this.RT108.EEPAlias = null;
            this.RT108.EncodingAfter = null;
            this.RT108.EncodingBefore = "Windows-1252";
            this.RT108.EncodingConvert = null;
            this.RT108.InfoConnection = this.InfoConnection1;
            this.RT108.MultiSetWhere = false;
            this.RT108.Name = "RT108";
            this.RT108.NotificationAutoEnlist = false;
            this.RT108.SecExcept = null;
            this.RT108.SecFieldName = null;
            this.RT108.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RT108.SelectPaging = false;
            this.RT108.SelectTop = 0;
            this.RT108.SiteControl = false;
            this.RT108.SiteFieldName = null;
            this.RT108.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HBADSLCMTY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_HBADSLCMTY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT108)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand HBADSLCMTY;
        private Srvtools.UpdateComponent ucHBADSLCMTY;
        private Srvtools.InfoCommand View_HBADSLCMTY;
        private Srvtools.InfoCommand RT108;
    }
}
