namespace sRT305
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
            Srvtools.FieldAttr fieldAttr23 = new Srvtools.FieldAttr();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.HBAdslCmtyCust = new Srvtools.InfoCommand(this.components);
            this.ucHBAdslCmtyCust = new Srvtools.UpdateComponent(this.components);
            this.View_HBAdslCmtyCust = new Srvtools.InfoCommand(this.components);
            this.cmRT305 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HBAdslCmtyCust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_HBAdslCmtyCust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT305)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // HBAdslCmtyCust
            // 
            this.HBAdslCmtyCust.CacheConnection = false;
            this.HBAdslCmtyCust.CommandText = "SELECT dbo.[HBAdslCmtyCust].* FROM dbo.[HBAdslCmtyCust]";
            this.HBAdslCmtyCust.CommandTimeout = 30;
            this.HBAdslCmtyCust.CommandType = System.Data.CommandType.Text;
            this.HBAdslCmtyCust.DynamicTableName = false;
            this.HBAdslCmtyCust.EEPAlias = null;
            this.HBAdslCmtyCust.EncodingAfter = null;
            this.HBAdslCmtyCust.EncodingBefore = "Windows-1252";
            this.HBAdslCmtyCust.EncodingConvert = null;
            this.HBAdslCmtyCust.InfoConnection = this.InfoConnection1;
            this.HBAdslCmtyCust.MultiSetWhere = false;
            this.HBAdslCmtyCust.Name = "HBAdslCmtyCust";
            this.HBAdslCmtyCust.NotificationAutoEnlist = false;
            this.HBAdslCmtyCust.SecExcept = null;
            this.HBAdslCmtyCust.SecFieldName = null;
            this.HBAdslCmtyCust.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.HBAdslCmtyCust.SelectPaging = false;
            this.HBAdslCmtyCust.SelectTop = 0;
            this.HBAdslCmtyCust.SiteControl = false;
            this.HBAdslCmtyCust.SiteFieldName = null;
            this.HBAdslCmtyCust.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucHBAdslCmtyCust
            // 
            this.ucHBAdslCmtyCust.AutoTrans = true;
            this.ucHBAdslCmtyCust.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "belongnc";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "salesnc";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "comtype";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "comtypenc";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "comq1";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "lineq1";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "comn";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "comq";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "cusid";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "entryno";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "cusnc";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "rcvdat";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "finishdat";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "docketdat";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "dropdat";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "canceldat";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "contacttel";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "companytel";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "RADDR";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "faqcnt";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "unfaqcnt";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "SOCIALID";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            fieldAttr23.CharSetNull = false;
            fieldAttr23.CheckNull = false;
            fieldAttr23.DataField = "IP11";
            fieldAttr23.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr23.DefaultValue = null;
            fieldAttr23.TrimLength = 0;
            fieldAttr23.UpdateEnable = true;
            fieldAttr23.WhereMode = true;
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr1);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr2);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr3);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr4);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr5);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr6);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr7);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr8);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr9);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr10);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr11);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr12);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr13);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr14);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr15);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr16);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr17);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr18);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr19);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr20);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr21);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr22);
            this.ucHBAdslCmtyCust.FieldAttrs.Add(fieldAttr23);
            this.ucHBAdslCmtyCust.LogInfo = null;
            this.ucHBAdslCmtyCust.Name = "ucHBAdslCmtyCust";
            this.ucHBAdslCmtyCust.RowAffectsCheck = true;
            this.ucHBAdslCmtyCust.SelectCmd = this.HBAdslCmtyCust;
            this.ucHBAdslCmtyCust.SelectCmdForUpdate = null;
            this.ucHBAdslCmtyCust.SendSQLCmd = true;
            this.ucHBAdslCmtyCust.ServerModify = true;
            this.ucHBAdslCmtyCust.ServerModifyGetMax = false;
            this.ucHBAdslCmtyCust.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucHBAdslCmtyCust.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucHBAdslCmtyCust.UseTranscationScope = false;
            this.ucHBAdslCmtyCust.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_HBAdslCmtyCust
            // 
            this.View_HBAdslCmtyCust.CacheConnection = false;
            this.View_HBAdslCmtyCust.CommandText = "SELECT * FROM dbo.[HBAdslCmtyCust]";
            this.View_HBAdslCmtyCust.CommandTimeout = 30;
            this.View_HBAdslCmtyCust.CommandType = System.Data.CommandType.Text;
            this.View_HBAdslCmtyCust.DynamicTableName = false;
            this.View_HBAdslCmtyCust.EEPAlias = null;
            this.View_HBAdslCmtyCust.EncodingAfter = null;
            this.View_HBAdslCmtyCust.EncodingBefore = "Windows-1252";
            this.View_HBAdslCmtyCust.EncodingConvert = null;
            this.View_HBAdslCmtyCust.InfoConnection = this.InfoConnection1;
            this.View_HBAdslCmtyCust.MultiSetWhere = false;
            this.View_HBAdslCmtyCust.Name = "View_HBAdslCmtyCust";
            this.View_HBAdslCmtyCust.NotificationAutoEnlist = false;
            this.View_HBAdslCmtyCust.SecExcept = null;
            this.View_HBAdslCmtyCust.SecFieldName = null;
            this.View_HBAdslCmtyCust.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_HBAdslCmtyCust.SelectPaging = false;
            this.View_HBAdslCmtyCust.SelectTop = 0;
            this.View_HBAdslCmtyCust.SiteControl = false;
            this.View_HBAdslCmtyCust.SiteFieldName = null;
            this.View_HBAdslCmtyCust.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmRT305
            // 
            this.cmRT305.CacheConnection = false;
            this.cmRT305.CommandText = resources.GetString("cmRT305.CommandText");
            this.cmRT305.CommandTimeout = 30;
            this.cmRT305.CommandType = System.Data.CommandType.Text;
            this.cmRT305.DynamicTableName = false;
            this.cmRT305.EEPAlias = null;
            this.cmRT305.EncodingAfter = null;
            this.cmRT305.EncodingBefore = "Windows-1252";
            this.cmRT305.EncodingConvert = null;
            this.cmRT305.InfoConnection = this.InfoConnection1;
            this.cmRT305.MultiSetWhere = false;
            this.cmRT305.Name = "cmRT305";
            this.cmRT305.NotificationAutoEnlist = false;
            this.cmRT305.SecExcept = null;
            this.cmRT305.SecFieldName = null;
            this.cmRT305.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmRT305.SelectPaging = false;
            this.cmRT305.SelectTop = 0;
            this.cmRT305.SiteControl = false;
            this.cmRT305.SiteFieldName = null;
            this.cmRT305.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HBAdslCmtyCust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_HBAdslCmtyCust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT305)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand HBAdslCmtyCust;
        private Srvtools.UpdateComponent ucHBAdslCmtyCust;
        private Srvtools.InfoCommand View_HBAdslCmtyCust;
        private Srvtools.InfoCommand cmRT305;
    }
}
