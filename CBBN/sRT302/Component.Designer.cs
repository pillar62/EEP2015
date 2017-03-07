namespace sRT302
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
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            Srvtools.FieldAttr fieldAttr14 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr15 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr16 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr17 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr18 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr19 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr20 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr21 = new Srvtools.FieldAttr();
            Srvtools.ColumnItem columnItem1 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem2 = new Srvtools.ColumnItem();
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTLessorAVSCustBillingPrt = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCustBillingPrt = new Srvtools.UpdateComponent(this.components);
            this.RTLessorAVSCustBillingPrtSub = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCustBillingPrtSub = new Srvtools.UpdateComponent(this.components);
            this.idRTLessorAVSCustBillingPrt_RTLessorAVSCustBillingPrtSub = new Srvtools.InfoDataSource(this.components);
            this.View_RTLessorAVSCustBillingPrt = new Srvtools.InfoCommand(this.components);
            this.RT302 = new Srvtools.InfoCommand(this.components);
            this.ucRT302 = new Srvtools.UpdateComponent(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustBillingPrt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustBillingPrtSub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustBillingPrt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT302)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTLessorAVSCustBillingPrt
            // 
            this.RTLessorAVSCustBillingPrt.CacheConnection = false;
            this.RTLessorAVSCustBillingPrt.CommandText = "SELECT dbo.[RTLessorAVSCustBillingPrt].* FROM dbo.[RTLessorAVSCustBillingPrt]";
            this.RTLessorAVSCustBillingPrt.CommandTimeout = 30;
            this.RTLessorAVSCustBillingPrt.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCustBillingPrt.DynamicTableName = false;
            this.RTLessorAVSCustBillingPrt.EEPAlias = null;
            this.RTLessorAVSCustBillingPrt.EncodingAfter = null;
            this.RTLessorAVSCustBillingPrt.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCustBillingPrt.EncodingConvert = null;
            this.RTLessorAVSCustBillingPrt.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "BATCH";
            this.RTLessorAVSCustBillingPrt.KeyFields.Add(keyItem1);
            this.RTLessorAVSCustBillingPrt.MultiSetWhere = false;
            this.RTLessorAVSCustBillingPrt.Name = "RTLessorAVSCustBillingPrt";
            this.RTLessorAVSCustBillingPrt.NotificationAutoEnlist = false;
            this.RTLessorAVSCustBillingPrt.SecExcept = null;
            this.RTLessorAVSCustBillingPrt.SecFieldName = null;
            this.RTLessorAVSCustBillingPrt.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCustBillingPrt.SelectPaging = false;
            this.RTLessorAVSCustBillingPrt.SelectTop = 0;
            this.RTLessorAVSCustBillingPrt.SiteControl = false;
            this.RTLessorAVSCustBillingPrt.SiteFieldName = null;
            this.RTLessorAVSCustBillingPrt.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCustBillingPrt
            // 
            this.ucRTLessorAVSCustBillingPrt.AutoTrans = true;
            this.ucRTLessorAVSCustBillingPrt.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "BATCH";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "DUEDATSB";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "DUEDATEB";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "DUEDATSA";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "DUEDATEA";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "CDAT";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "CUSR";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "PRTDAT";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "PRTUSR";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "BARCODOUTDAT";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "BARCODOUTUSR";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "BARCODINDAT";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "BARCODINUSR";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr1);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr2);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr3);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr4);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr5);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr6);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr7);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr8);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr9);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr10);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr11);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr12);
            this.ucRTLessorAVSCustBillingPrt.FieldAttrs.Add(fieldAttr13);
            this.ucRTLessorAVSCustBillingPrt.LogInfo = null;
            this.ucRTLessorAVSCustBillingPrt.Name = "ucRTLessorAVSCustBillingPrt";
            this.ucRTLessorAVSCustBillingPrt.RowAffectsCheck = true;
            this.ucRTLessorAVSCustBillingPrt.SelectCmd = this.RTLessorAVSCustBillingPrt;
            this.ucRTLessorAVSCustBillingPrt.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCustBillingPrt.SendSQLCmd = true;
            this.ucRTLessorAVSCustBillingPrt.ServerModify = true;
            this.ucRTLessorAVSCustBillingPrt.ServerModifyGetMax = false;
            this.ucRTLessorAVSCustBillingPrt.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCustBillingPrt.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCustBillingPrt.UseTranscationScope = false;
            this.ucRTLessorAVSCustBillingPrt.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // RTLessorAVSCustBillingPrtSub
            // 
            this.RTLessorAVSCustBillingPrtSub.CacheConnection = false;
            this.RTLessorAVSCustBillingPrtSub.CommandText = "SELECT dbo.[RTLessorAVSCustBillingPrtSub].* FROM dbo.[RTLessorAVSCustBillingPrtSu" +
    "b]";
            this.RTLessorAVSCustBillingPrtSub.CommandTimeout = 30;
            this.RTLessorAVSCustBillingPrtSub.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCustBillingPrtSub.DynamicTableName = false;
            this.RTLessorAVSCustBillingPrtSub.EEPAlias = null;
            this.RTLessorAVSCustBillingPrtSub.EncodingAfter = null;
            this.RTLessorAVSCustBillingPrtSub.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCustBillingPrtSub.EncodingConvert = null;
            this.RTLessorAVSCustBillingPrtSub.InfoConnection = this.InfoConnection1;
            keyItem2.KeyName = "NOTICEID";
            this.RTLessorAVSCustBillingPrtSub.KeyFields.Add(keyItem2);
            this.RTLessorAVSCustBillingPrtSub.MultiSetWhere = false;
            this.RTLessorAVSCustBillingPrtSub.Name = "RTLessorAVSCustBillingPrtSub";
            this.RTLessorAVSCustBillingPrtSub.NotificationAutoEnlist = false;
            this.RTLessorAVSCustBillingPrtSub.SecExcept = null;
            this.RTLessorAVSCustBillingPrtSub.SecFieldName = null;
            this.RTLessorAVSCustBillingPrtSub.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCustBillingPrtSub.SelectPaging = false;
            this.RTLessorAVSCustBillingPrtSub.SelectTop = 0;
            this.RTLessorAVSCustBillingPrtSub.SiteControl = false;
            this.RTLessorAVSCustBillingPrtSub.SiteFieldName = null;
            this.RTLessorAVSCustBillingPrtSub.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCustBillingPrtSub
            // 
            this.ucRTLessorAVSCustBillingPrtSub.AutoTrans = true;
            this.ucRTLessorAVSCustBillingPrtSub.ExceptJoin = false;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "NOTICEID";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "BATCH";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "COMQ1";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "LINEQ1";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "CUSID";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "DUEDAT";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "CASEKIND";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "PAYCYCLE";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            this.ucRTLessorAVSCustBillingPrtSub.FieldAttrs.Add(fieldAttr14);
            this.ucRTLessorAVSCustBillingPrtSub.FieldAttrs.Add(fieldAttr15);
            this.ucRTLessorAVSCustBillingPrtSub.FieldAttrs.Add(fieldAttr16);
            this.ucRTLessorAVSCustBillingPrtSub.FieldAttrs.Add(fieldAttr17);
            this.ucRTLessorAVSCustBillingPrtSub.FieldAttrs.Add(fieldAttr18);
            this.ucRTLessorAVSCustBillingPrtSub.FieldAttrs.Add(fieldAttr19);
            this.ucRTLessorAVSCustBillingPrtSub.FieldAttrs.Add(fieldAttr20);
            this.ucRTLessorAVSCustBillingPrtSub.FieldAttrs.Add(fieldAttr21);
            this.ucRTLessorAVSCustBillingPrtSub.LogInfo = null;
            this.ucRTLessorAVSCustBillingPrtSub.Name = "ucRTLessorAVSCustBillingPrtSub";
            this.ucRTLessorAVSCustBillingPrtSub.RowAffectsCheck = true;
            this.ucRTLessorAVSCustBillingPrtSub.SelectCmd = this.RTLessorAVSCustBillingPrtSub;
            this.ucRTLessorAVSCustBillingPrtSub.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCustBillingPrtSub.SendSQLCmd = true;
            this.ucRTLessorAVSCustBillingPrtSub.ServerModify = true;
            this.ucRTLessorAVSCustBillingPrtSub.ServerModifyGetMax = false;
            this.ucRTLessorAVSCustBillingPrtSub.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCustBillingPrtSub.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCustBillingPrtSub.UseTranscationScope = false;
            this.ucRTLessorAVSCustBillingPrtSub.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // idRTLessorAVSCustBillingPrt_RTLessorAVSCustBillingPrtSub
            // 
            this.idRTLessorAVSCustBillingPrt_RTLessorAVSCustBillingPrtSub.Detail = this.RTLessorAVSCustBillingPrtSub;
            columnItem1.FieldName = "BATCH";
            this.idRTLessorAVSCustBillingPrt_RTLessorAVSCustBillingPrtSub.DetailColumns.Add(columnItem1);
            this.idRTLessorAVSCustBillingPrt_RTLessorAVSCustBillingPrtSub.DynamicTableName = false;
            this.idRTLessorAVSCustBillingPrt_RTLessorAVSCustBillingPrtSub.Master = this.RTLessorAVSCustBillingPrt;
            columnItem2.FieldName = "BATCH";
            this.idRTLessorAVSCustBillingPrt_RTLessorAVSCustBillingPrtSub.MasterColumns.Add(columnItem2);
            // 
            // View_RTLessorAVSCustBillingPrt
            // 
            this.View_RTLessorAVSCustBillingPrt.CacheConnection = false;
            this.View_RTLessorAVSCustBillingPrt.CommandText = "SELECT * FROM dbo.[RTLessorAVSCustBillingPrt]";
            this.View_RTLessorAVSCustBillingPrt.CommandTimeout = 30;
            this.View_RTLessorAVSCustBillingPrt.CommandType = System.Data.CommandType.Text;
            this.View_RTLessorAVSCustBillingPrt.DynamicTableName = false;
            this.View_RTLessorAVSCustBillingPrt.EEPAlias = null;
            this.View_RTLessorAVSCustBillingPrt.EncodingAfter = null;
            this.View_RTLessorAVSCustBillingPrt.EncodingBefore = "Windows-1252";
            this.View_RTLessorAVSCustBillingPrt.EncodingConvert = null;
            this.View_RTLessorAVSCustBillingPrt.InfoConnection = this.InfoConnection1;
            keyItem3.KeyName = "BATCH";
            this.View_RTLessorAVSCustBillingPrt.KeyFields.Add(keyItem3);
            this.View_RTLessorAVSCustBillingPrt.MultiSetWhere = false;
            this.View_RTLessorAVSCustBillingPrt.Name = "View_RTLessorAVSCustBillingPrt";
            this.View_RTLessorAVSCustBillingPrt.NotificationAutoEnlist = false;
            this.View_RTLessorAVSCustBillingPrt.SecExcept = null;
            this.View_RTLessorAVSCustBillingPrt.SecFieldName = null;
            this.View_RTLessorAVSCustBillingPrt.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTLessorAVSCustBillingPrt.SelectPaging = false;
            this.View_RTLessorAVSCustBillingPrt.SelectTop = 0;
            this.View_RTLessorAVSCustBillingPrt.SiteControl = false;
            this.View_RTLessorAVSCustBillingPrt.SiteFieldName = null;
            this.View_RTLessorAVSCustBillingPrt.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // RT302
            // 
            this.RT302.CacheConnection = false;
            this.RT302.CommandText = resources.GetString("RT302.CommandText");
            this.RT302.CommandTimeout = 30;
            this.RT302.CommandType = System.Data.CommandType.Text;
            this.RT302.DynamicTableName = false;
            this.RT302.EEPAlias = null;
            this.RT302.EncodingAfter = null;
            this.RT302.EncodingBefore = "Windows-1252";
            this.RT302.EncodingConvert = null;
            this.RT302.InfoConnection = this.InfoConnection1;
            keyItem4.KeyName = "BATCH";
            this.RT302.KeyFields.Add(keyItem4);
            this.RT302.MultiSetWhere = false;
            this.RT302.Name = "RT302";
            this.RT302.NotificationAutoEnlist = false;
            this.RT302.SecExcept = null;
            this.RT302.SecFieldName = null;
            this.RT302.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RT302.SelectPaging = false;
            this.RT302.SelectTop = 0;
            this.RT302.SiteControl = false;
            this.RT302.SiteFieldName = null;
            this.RT302.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRT302
            // 
            this.ucRT302.AutoTrans = true;
            this.ucRT302.ExceptJoin = false;
            this.ucRT302.LogInfo = null;
            this.ucRT302.Name = null;
            this.ucRT302.RowAffectsCheck = true;
            this.ucRT302.SelectCmd = this.RT302;
            this.ucRT302.SelectCmdForUpdate = null;
            this.ucRT302.SendSQLCmd = true;
            this.ucRT302.ServerModify = true;
            this.ucRT302.ServerModifyGetMax = false;
            this.ucRT302.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRT302.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRT302.UseTranscationScope = false;
            this.ucRT302.WhereMode = Srvtools.WhereModeType.Keyfields;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustBillingPrt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustBillingPrtSub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustBillingPrt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT302)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTLessorAVSCustBillingPrt;
        private Srvtools.UpdateComponent ucRTLessorAVSCustBillingPrt;
        private Srvtools.InfoCommand RTLessorAVSCustBillingPrtSub;
        private Srvtools.UpdateComponent ucRTLessorAVSCustBillingPrtSub;
        private Srvtools.InfoDataSource idRTLessorAVSCustBillingPrt_RTLessorAVSCustBillingPrtSub;
        private Srvtools.InfoCommand View_RTLessorAVSCustBillingPrt;
        private Srvtools.InfoCommand RT302;
        private Srvtools.UpdateComponent ucRT302;
    }
}
