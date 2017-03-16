namespace sRT304
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
            Srvtools.KeyItem keyItem1 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
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
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
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
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem7 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem8 = new Srvtools.KeyItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            Srvtools.KeyItem keyItem9 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem10 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem11 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem12 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTBillSeednetReckon = new Srvtools.InfoCommand(this.components);
            this.ucRTBillSeednetReckon = new Srvtools.UpdateComponent(this.components);
            this.RTBillSeednetTrade = new Srvtools.InfoCommand(this.components);
            this.ucRTBillSeednetTrade = new Srvtools.UpdateComponent(this.components);
            this.View_RTBillSeednetReckon = new Srvtools.InfoCommand(this.components);
            this.View_RTBillSeednetTrade = new Srvtools.InfoCommand(this.components);
            this.cmRT304 = new Srvtools.InfoCommand(this.components);
            this.cmRT3041 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTBillSeednetReckon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTBillSeednetTrade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTBillSeednetReckon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTBillSeednetTrade)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT304)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT3041)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTBillSeednetReckon
            // 
            this.RTBillSeednetReckon.CacheConnection = false;
            this.RTBillSeednetReckon.CommandText = "SELECT dbo.[RTBillSeednetReckon].* FROM dbo.[RTBillSeednetReckon]";
            this.RTBillSeednetReckon.CommandTimeout = 30;
            this.RTBillSeednetReckon.CommandType = System.Data.CommandType.Text;
            this.RTBillSeednetReckon.DynamicTableName = false;
            this.RTBillSeednetReckon.EEPAlias = null;
            this.RTBillSeednetReckon.EncodingAfter = null;
            this.RTBillSeednetReckon.EncodingBefore = "Windows-1252";
            this.RTBillSeednetReckon.EncodingConvert = null;
            this.RTBillSeednetReckon.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "CSNOTICEID";
            keyItem2.KeyName = "CSCUSID";
            this.RTBillSeednetReckon.KeyFields.Add(keyItem1);
            this.RTBillSeednetReckon.KeyFields.Add(keyItem2);
            this.RTBillSeednetReckon.MultiSetWhere = false;
            this.RTBillSeednetReckon.Name = "RTBillSeednetReckon";
            this.RTBillSeednetReckon.NotificationAutoEnlist = false;
            this.RTBillSeednetReckon.SecExcept = null;
            this.RTBillSeednetReckon.SecFieldName = null;
            this.RTBillSeednetReckon.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTBillSeednetReckon.SelectPaging = false;
            this.RTBillSeednetReckon.SelectTop = 0;
            this.RTBillSeednetReckon.SiteControl = false;
            this.RTBillSeednetReckon.SiteFieldName = null;
            this.RTBillSeednetReckon.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTBillSeednetReckon
            // 
            this.ucRTBillSeednetReckon.AutoTrans = true;
            this.ucRTBillSeednetReckon.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "SERNO";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "VENDORID";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "VENDORNC";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "CSNOTICEID";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "CSCUSID";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "CUSNC";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "TYM";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "BILLAMT";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "PAYAMT";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "PAYDAT";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "PAYSTORE";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "ABATEDAT";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "RCVDAT";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "CLOSEDAT";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "SOURCEFILE";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "SOURCEDAT";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr1);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr2);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr3);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr4);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr5);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr6);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr7);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr8);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr9);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr10);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr11);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr12);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr13);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr14);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr15);
            this.ucRTBillSeednetReckon.FieldAttrs.Add(fieldAttr16);
            this.ucRTBillSeednetReckon.LogInfo = null;
            this.ucRTBillSeednetReckon.Name = "ucRTBillSeednetReckon";
            this.ucRTBillSeednetReckon.RowAffectsCheck = true;
            this.ucRTBillSeednetReckon.SelectCmd = this.RTBillSeednetReckon;
            this.ucRTBillSeednetReckon.SelectCmdForUpdate = null;
            this.ucRTBillSeednetReckon.SendSQLCmd = true;
            this.ucRTBillSeednetReckon.ServerModify = true;
            this.ucRTBillSeednetReckon.ServerModifyGetMax = false;
            this.ucRTBillSeednetReckon.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTBillSeednetReckon.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTBillSeednetReckon.UseTranscationScope = false;
            this.ucRTBillSeednetReckon.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // RTBillSeednetTrade
            // 
            this.RTBillSeednetTrade.CacheConnection = false;
            this.RTBillSeednetTrade.CommandText = "SELECT dbo.[RTBillSeednetTrade].* FROM dbo.[RTBillSeednetTrade]";
            this.RTBillSeednetTrade.CommandTimeout = 30;
            this.RTBillSeednetTrade.CommandType = System.Data.CommandType.Text;
            this.RTBillSeednetTrade.DynamicTableName = false;
            this.RTBillSeednetTrade.EEPAlias = null;
            this.RTBillSeednetTrade.EncodingAfter = null;
            this.RTBillSeednetTrade.EncodingBefore = "Windows-1252";
            this.RTBillSeednetTrade.EncodingConvert = null;
            this.RTBillSeednetTrade.InfoConnection = this.InfoConnection1;
            keyItem3.KeyName = "CSNOTICEID";
            keyItem4.KeyName = "CSCUSID";
            this.RTBillSeednetTrade.KeyFields.Add(keyItem3);
            this.RTBillSeednetTrade.KeyFields.Add(keyItem4);
            this.RTBillSeednetTrade.MultiSetWhere = false;
            this.RTBillSeednetTrade.Name = "RTBillSeednetTrade";
            this.RTBillSeednetTrade.NotificationAutoEnlist = false;
            this.RTBillSeednetTrade.SecExcept = null;
            this.RTBillSeednetTrade.SecFieldName = null;
            this.RTBillSeednetTrade.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTBillSeednetTrade.SelectPaging = false;
            this.RTBillSeednetTrade.SelectTop = 0;
            this.RTBillSeednetTrade.SiteControl = false;
            this.RTBillSeednetTrade.SiteFieldName = null;
            this.RTBillSeednetTrade.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTBillSeednetTrade
            // 
            this.ucRTBillSeednetTrade.AutoTrans = true;
            this.ucRTBillSeednetTrade.ExceptJoin = false;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "PAYDUEDAT";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "CSNOTICEID";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "ACCOUNTYM";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "AMT";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "CUSNC";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "CSCUSID";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            fieldAttr23.CharSetNull = false;
            fieldAttr23.CheckNull = false;
            fieldAttr23.DataField = "TEL";
            fieldAttr23.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr23.DefaultValue = null;
            fieldAttr23.TrimLength = 0;
            fieldAttr23.UpdateEnable = true;
            fieldAttr23.WhereMode = true;
            fieldAttr24.CharSetNull = false;
            fieldAttr24.CheckNull = false;
            fieldAttr24.DataField = "MEMO";
            fieldAttr24.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr24.DefaultValue = null;
            fieldAttr24.TrimLength = 0;
            fieldAttr24.UpdateEnable = true;
            fieldAttr24.WhereMode = true;
            fieldAttr25.CharSetNull = false;
            fieldAttr25.CheckNull = false;
            fieldAttr25.DataField = "CSPAYDAT";
            fieldAttr25.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr25.DefaultValue = null;
            fieldAttr25.TrimLength = 0;
            fieldAttr25.UpdateEnable = true;
            fieldAttr25.WhereMode = true;
            fieldAttr26.CharSetNull = false;
            fieldAttr26.CheckNull = false;
            fieldAttr26.DataField = "CSNAME";
            fieldAttr26.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr26.DefaultValue = null;
            fieldAttr26.TrimLength = 0;
            fieldAttr26.UpdateEnable = true;
            fieldAttr26.WhereMode = true;
            fieldAttr27.CharSetNull = false;
            fieldAttr27.CheckNull = false;
            fieldAttr27.DataField = "CSSEEDNETDAT";
            fieldAttr27.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr27.DefaultValue = null;
            fieldAttr27.TrimLength = 0;
            fieldAttr27.UpdateEnable = true;
            fieldAttr27.WhereMode = true;
            fieldAttr28.CharSetNull = false;
            fieldAttr28.CheckNull = false;
            fieldAttr28.DataField = "SOURCEFILE";
            fieldAttr28.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr28.DefaultValue = null;
            fieldAttr28.TrimLength = 0;
            fieldAttr28.UpdateEnable = true;
            fieldAttr28.WhereMode = true;
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr17);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr18);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr19);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr20);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr21);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr22);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr23);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr24);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr25);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr26);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr27);
            this.ucRTBillSeednetTrade.FieldAttrs.Add(fieldAttr28);
            this.ucRTBillSeednetTrade.LogInfo = null;
            this.ucRTBillSeednetTrade.Name = "ucRTBillSeednetTrade";
            this.ucRTBillSeednetTrade.RowAffectsCheck = true;
            this.ucRTBillSeednetTrade.SelectCmd = this.RTBillSeednetTrade;
            this.ucRTBillSeednetTrade.SelectCmdForUpdate = null;
            this.ucRTBillSeednetTrade.SendSQLCmd = true;
            this.ucRTBillSeednetTrade.ServerModify = true;
            this.ucRTBillSeednetTrade.ServerModifyGetMax = false;
            this.ucRTBillSeednetTrade.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTBillSeednetTrade.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTBillSeednetTrade.UseTranscationScope = false;
            this.ucRTBillSeednetTrade.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTBillSeednetReckon
            // 
            this.View_RTBillSeednetReckon.CacheConnection = false;
            this.View_RTBillSeednetReckon.CommandText = "SELECT * FROM dbo.[RTBillSeednetReckon]";
            this.View_RTBillSeednetReckon.CommandTimeout = 30;
            this.View_RTBillSeednetReckon.CommandType = System.Data.CommandType.Text;
            this.View_RTBillSeednetReckon.DynamicTableName = false;
            this.View_RTBillSeednetReckon.EEPAlias = null;
            this.View_RTBillSeednetReckon.EncodingAfter = null;
            this.View_RTBillSeednetReckon.EncodingBefore = "Windows-1252";
            this.View_RTBillSeednetReckon.EncodingConvert = null;
            this.View_RTBillSeednetReckon.InfoConnection = this.InfoConnection1;
            keyItem5.KeyName = "CSNOTICEID";
            keyItem6.KeyName = "CSCUSID";
            this.View_RTBillSeednetReckon.KeyFields.Add(keyItem5);
            this.View_RTBillSeednetReckon.KeyFields.Add(keyItem6);
            this.View_RTBillSeednetReckon.MultiSetWhere = false;
            this.View_RTBillSeednetReckon.Name = "View_RTBillSeednetReckon";
            this.View_RTBillSeednetReckon.NotificationAutoEnlist = false;
            this.View_RTBillSeednetReckon.SecExcept = null;
            this.View_RTBillSeednetReckon.SecFieldName = null;
            this.View_RTBillSeednetReckon.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTBillSeednetReckon.SelectPaging = false;
            this.View_RTBillSeednetReckon.SelectTop = 0;
            this.View_RTBillSeednetReckon.SiteControl = false;
            this.View_RTBillSeednetReckon.SiteFieldName = null;
            this.View_RTBillSeednetReckon.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // View_RTBillSeednetTrade
            // 
            this.View_RTBillSeednetTrade.CacheConnection = false;
            this.View_RTBillSeednetTrade.CommandText = "SELECT * FROM dbo.[RTBillSeednetTrade]";
            this.View_RTBillSeednetTrade.CommandTimeout = 30;
            this.View_RTBillSeednetTrade.CommandType = System.Data.CommandType.Text;
            this.View_RTBillSeednetTrade.DynamicTableName = false;
            this.View_RTBillSeednetTrade.EEPAlias = null;
            this.View_RTBillSeednetTrade.EncodingAfter = null;
            this.View_RTBillSeednetTrade.EncodingBefore = "Windows-1252";
            this.View_RTBillSeednetTrade.EncodingConvert = null;
            this.View_RTBillSeednetTrade.InfoConnection = this.InfoConnection1;
            keyItem7.KeyName = "CSNOTICEID";
            keyItem8.KeyName = "CSCUSID";
            this.View_RTBillSeednetTrade.KeyFields.Add(keyItem7);
            this.View_RTBillSeednetTrade.KeyFields.Add(keyItem8);
            this.View_RTBillSeednetTrade.MultiSetWhere = false;
            this.View_RTBillSeednetTrade.Name = "View_RTBillSeednetTrade";
            this.View_RTBillSeednetTrade.NotificationAutoEnlist = false;
            this.View_RTBillSeednetTrade.SecExcept = null;
            this.View_RTBillSeednetTrade.SecFieldName = null;
            this.View_RTBillSeednetTrade.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTBillSeednetTrade.SelectPaging = false;
            this.View_RTBillSeednetTrade.SelectTop = 0;
            this.View_RTBillSeednetTrade.SiteControl = false;
            this.View_RTBillSeednetTrade.SiteFieldName = null;
            this.View_RTBillSeednetTrade.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmRT304
            // 
            this.cmRT304.CacheConnection = false;
            this.cmRT304.CommandText = resources.GetString("cmRT304.CommandText");
            this.cmRT304.CommandTimeout = 30;
            this.cmRT304.CommandType = System.Data.CommandType.Text;
            this.cmRT304.DynamicTableName = false;
            this.cmRT304.EEPAlias = null;
            this.cmRT304.EncodingAfter = null;
            this.cmRT304.EncodingBefore = "Windows-1252";
            this.cmRT304.EncodingConvert = null;
            this.cmRT304.InfoConnection = this.InfoConnection1;
            keyItem9.KeyName = "csnoticeid";
            keyItem10.KeyName = "cscusid";
            this.cmRT304.KeyFields.Add(keyItem9);
            this.cmRT304.KeyFields.Add(keyItem10);
            this.cmRT304.MultiSetWhere = false;
            this.cmRT304.Name = "cmRT304";
            this.cmRT304.NotificationAutoEnlist = false;
            this.cmRT304.SecExcept = null;
            this.cmRT304.SecFieldName = null;
            this.cmRT304.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmRT304.SelectPaging = false;
            this.cmRT304.SelectTop = 0;
            this.cmRT304.SiteControl = false;
            this.cmRT304.SiteFieldName = null;
            this.cmRT304.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmRT3041
            // 
            this.cmRT3041.CacheConnection = false;
            this.cmRT3041.CommandText = resources.GetString("cmRT3041.CommandText");
            this.cmRT3041.CommandTimeout = 30;
            this.cmRT3041.CommandType = System.Data.CommandType.Text;
            this.cmRT3041.DynamicTableName = false;
            this.cmRT3041.EEPAlias = null;
            this.cmRT3041.EncodingAfter = null;
            this.cmRT3041.EncodingBefore = "Windows-1252";
            this.cmRT3041.EncodingConvert = null;
            this.cmRT3041.InfoConnection = this.InfoConnection1;
            keyItem11.KeyName = "csnoticeid";
            keyItem12.KeyName = "cscusid";
            this.cmRT3041.KeyFields.Add(keyItem11);
            this.cmRT3041.KeyFields.Add(keyItem12);
            this.cmRT3041.MultiSetWhere = false;
            this.cmRT3041.Name = "cmRT3041";
            this.cmRT3041.NotificationAutoEnlist = false;
            this.cmRT3041.SecExcept = null;
            this.cmRT3041.SecFieldName = null;
            this.cmRT3041.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmRT3041.SelectPaging = false;
            this.cmRT3041.SelectTop = 0;
            this.cmRT3041.SiteControl = false;
            this.cmRT3041.SiteFieldName = null;
            this.cmRT3041.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTBillSeednetReckon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTBillSeednetTrade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTBillSeednetReckon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTBillSeednetTrade)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT304)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT3041)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTBillSeednetReckon;
        private Srvtools.UpdateComponent ucRTBillSeednetReckon;
        private Srvtools.InfoCommand RTBillSeednetTrade;
        private Srvtools.UpdateComponent ucRTBillSeednetTrade;
        private Srvtools.InfoCommand View_RTBillSeednetReckon;
        private Srvtools.InfoCommand View_RTBillSeednetTrade;
        private Srvtools.InfoCommand cmRT304;
        private Srvtools.InfoCommand cmRT3041;
    }
}
