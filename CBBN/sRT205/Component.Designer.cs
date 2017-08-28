namespace sRT205
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
            Srvtools.Service service1 = new Srvtools.Service();
            Srvtools.Service service2 = new Srvtools.Service();
            Srvtools.Service service3 = new Srvtools.Service();
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
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTFaqM = new Srvtools.InfoCommand(this.components);
            this.ucRTFaqM = new Srvtools.UpdateComponent(this.components);
            this.View_RTFaqM = new Srvtools.InfoCommand(this.components);
            this.RT205 = new Srvtools.InfoCommand(this.components);
            this.RTFaqAdd = new Srvtools.InfoCommand(this.components);
            this.ucRTFaqAdd = new Srvtools.UpdateComponent(this.components);
            this.autoNumber1 = new Srvtools.AutoNumber(this.components);
            this.V_RT205 = new Srvtools.InfoCommand(this.components);
            this.V_RT2051 = new Srvtools.InfoCommand(this.components);
            this.V_RT2052 = new Srvtools.InfoCommand(this.components);
            this.cmd = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTFaqM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTFaqM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT205)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTFaqAdd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RT205)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RT2051)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RT2052)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmd)).BeginInit();
            // 
            // serviceManager1
            // 
            service1.DelegateName = "smRT2055";
            service1.NonLogin = false;
            service1.ServiceName = "smRT2055";
            service2.DelegateName = "smRT2056";
            service2.NonLogin = false;
            service2.ServiceName = "smRT2056";
            service3.DelegateName = "smRT2057";
            service3.NonLogin = false;
            service3.ServiceName = "smRT2057";
            this.serviceManager1.ServiceCollection.Add(service1);
            this.serviceManager1.ServiceCollection.Add(service2);
            this.serviceManager1.ServiceCollection.Add(service3);
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTFaqM
            // 
            this.RTFaqM.CacheConnection = false;
            this.RTFaqM.CommandText = "SELECT A.*, B.* \r\nFROM RTFaqM A\r\nLEFT JOIN V_RT205 B ON B.COMQ1=A.COMQ1 AND B.CUS" +
    "ID=A.CUSID AND ISNULL(B.LINEQ1, \'\')= ISNULL(A.LINEQ1, \'\')";
            this.RTFaqM.CommandTimeout = 30;
            this.RTFaqM.CommandType = System.Data.CommandType.Text;
            this.RTFaqM.DynamicTableName = false;
            this.RTFaqM.EEPAlias = null;
            this.RTFaqM.EncodingAfter = null;
            this.RTFaqM.EncodingBefore = "Windows-1252";
            this.RTFaqM.EncodingConvert = null;
            this.RTFaqM.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "CASENO";
            this.RTFaqM.KeyFields.Add(keyItem1);
            this.RTFaqM.MultiSetWhere = false;
            this.RTFaqM.Name = "RTFaqM";
            this.RTFaqM.NotificationAutoEnlist = false;
            this.RTFaqM.SecExcept = null;
            this.RTFaqM.SecFieldName = null;
            this.RTFaqM.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTFaqM.SelectPaging = false;
            this.RTFaqM.SelectTop = 0;
            this.RTFaqM.SiteControl = false;
            this.RTFaqM.SiteFieldName = null;
            this.RTFaqM.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTFaqM
            // 
            this.ucRTFaqM.AutoTrans = true;
            this.ucRTFaqM.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "CASENO";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "COMTYPE";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "COMQ1";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "LINEQ1";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "CUSID";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "ENTRYNO";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "FAQMAN";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "TEL";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "MOBILE";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "FAQREASON";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "IOBOUND";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "MEMO";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "RCVUSR";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "RCVDAT";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "CLOSEUSR";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "CLOSEDAT";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "UUSR";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "UDAT";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "CANCELUSR";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "CANCELDAT";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "ASKCASE";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "CUSTSRC";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr1);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr2);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr3);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr4);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr5);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr6);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr7);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr8);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr9);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr10);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr11);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr12);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr13);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr14);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr15);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr16);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr17);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr18);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr19);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr20);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr21);
            this.ucRTFaqM.FieldAttrs.Add(fieldAttr22);
            this.ucRTFaqM.LogInfo = null;
            this.ucRTFaqM.Name = "ucRTFaqM";
            this.ucRTFaqM.RowAffectsCheck = true;
            this.ucRTFaqM.SelectCmd = this.RTFaqM;
            this.ucRTFaqM.SelectCmdForUpdate = null;
            this.ucRTFaqM.SendSQLCmd = true;
            this.ucRTFaqM.ServerModify = true;
            this.ucRTFaqM.ServerModifyGetMax = false;
            this.ucRTFaqM.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTFaqM.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTFaqM.UseTranscationScope = false;
            this.ucRTFaqM.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTFaqM
            // 
            this.View_RTFaqM.CacheConnection = false;
            this.View_RTFaqM.CommandText = "SELECT A.*, B.* \r\nFROM RTFaqM A\r\nLEFT JOIN V_RT205 B ON B.COMQ1=A.COMQ1 AND B.CUS" +
    "ID=A.CUSID AND ISNULL(B.LINEQ1, \'\')= ISNULL(A.LINEQ1, \'\')";
            this.View_RTFaqM.CommandTimeout = 30;
            this.View_RTFaqM.CommandType = System.Data.CommandType.Text;
            this.View_RTFaqM.DynamicTableName = false;
            this.View_RTFaqM.EEPAlias = null;
            this.View_RTFaqM.EncodingAfter = null;
            this.View_RTFaqM.EncodingBefore = "Windows-1252";
            this.View_RTFaqM.EncodingConvert = null;
            this.View_RTFaqM.InfoConnection = this.InfoConnection1;
            keyItem2.KeyName = "CASENO";
            this.View_RTFaqM.KeyFields.Add(keyItem2);
            this.View_RTFaqM.MultiSetWhere = false;
            this.View_RTFaqM.Name = "View_RTFaqM";
            this.View_RTFaqM.NotificationAutoEnlist = false;
            this.View_RTFaqM.SecExcept = null;
            this.View_RTFaqM.SecFieldName = null;
            this.View_RTFaqM.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTFaqM.SelectPaging = false;
            this.View_RTFaqM.SelectTop = 0;
            this.View_RTFaqM.SiteControl = false;
            this.View_RTFaqM.SiteFieldName = null;
            this.View_RTFaqM.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // RT205
            // 
            this.RT205.CacheConnection = false;
            this.RT205.CommandText = resources.GetString("RT205.CommandText");
            this.RT205.CommandTimeout = 30;
            this.RT205.CommandType = System.Data.CommandType.Text;
            this.RT205.DynamicTableName = false;
            this.RT205.EEPAlias = null;
            this.RT205.EncodingAfter = null;
            this.RT205.EncodingBefore = "Windows-1252";
            this.RT205.EncodingConvert = null;
            this.RT205.InfoConnection = this.InfoConnection1;
            keyItem3.KeyName = "caseno";
            this.RT205.KeyFields.Add(keyItem3);
            this.RT205.MultiSetWhere = false;
            this.RT205.Name = "RT205";
            this.RT205.NotificationAutoEnlist = false;
            this.RT205.SecExcept = null;
            this.RT205.SecFieldName = null;
            this.RT205.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RT205.SelectPaging = false;
            this.RT205.SelectTop = 0;
            this.RT205.SiteControl = false;
            this.RT205.SiteFieldName = null;
            this.RT205.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // RTFaqAdd
            // 
            this.RTFaqAdd.CacheConnection = false;
            this.RTFaqAdd.CommandText = "SELECT * FROM RTFaqAdd";
            this.RTFaqAdd.CommandTimeout = 30;
            this.RTFaqAdd.CommandType = System.Data.CommandType.Text;
            this.RTFaqAdd.DynamicTableName = false;
            this.RTFaqAdd.EEPAlias = null;
            this.RTFaqAdd.EncodingAfter = null;
            this.RTFaqAdd.EncodingBefore = "Windows-1252";
            this.RTFaqAdd.EncodingConvert = null;
            this.RTFaqAdd.InfoConnection = this.InfoConnection1;
            keyItem4.KeyName = "CASENO";
            keyItem5.KeyName = "ENTRYNO";
            this.RTFaqAdd.KeyFields.Add(keyItem4);
            this.RTFaqAdd.KeyFields.Add(keyItem5);
            this.RTFaqAdd.MultiSetWhere = false;
            this.RTFaqAdd.Name = "RTFaqAdd";
            this.RTFaqAdd.NotificationAutoEnlist = false;
            this.RTFaqAdd.SecExcept = null;
            this.RTFaqAdd.SecFieldName = null;
            this.RTFaqAdd.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTFaqAdd.SelectPaging = false;
            this.RTFaqAdd.SelectTop = 0;
            this.RTFaqAdd.SiteControl = false;
            this.RTFaqAdd.SiteFieldName = null;
            this.RTFaqAdd.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTFaqAdd
            // 
            this.ucRTFaqAdd.AutoTrans = true;
            this.ucRTFaqAdd.ExceptJoin = false;
            this.ucRTFaqAdd.LogInfo = null;
            this.ucRTFaqAdd.Name = "ucRTFaqAdd";
            this.ucRTFaqAdd.RowAffectsCheck = true;
            this.ucRTFaqAdd.SelectCmd = this.RTFaqAdd;
            this.ucRTFaqAdd.SelectCmdForUpdate = null;
            this.ucRTFaqAdd.SendSQLCmd = true;
            this.ucRTFaqAdd.ServerModify = true;
            this.ucRTFaqAdd.ServerModifyGetMax = false;
            this.ucRTFaqAdd.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTFaqAdd.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTFaqAdd.UseTranscationScope = false;
            this.ucRTFaqAdd.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // autoNumber1
            // 
            this.autoNumber1.Active = true;
            this.autoNumber1.AutoNoID = "aRT205";
            this.autoNumber1.Description = null;
            this.autoNumber1.GetFixed = "getFix()";
            this.autoNumber1.isNumFill = false;
            this.autoNumber1.Name = "autoNumber1";
            this.autoNumber1.Number = null;
            this.autoNumber1.NumDig = 3;
            this.autoNumber1.OldVersion = false;
            this.autoNumber1.OverFlow = true;
            this.autoNumber1.StartValue = 1;
            this.autoNumber1.Step = 1;
            this.autoNumber1.TargetColumn = "CASENO";
            this.autoNumber1.UpdateComp = this.ucRTFaqM;
            // 
            // V_RT205
            // 
            this.V_RT205.CacheConnection = false;
            this.V_RT205.CommandText = "SELECT *\r\nFROM  V_RT205 ";
            this.V_RT205.CommandTimeout = 30;
            this.V_RT205.CommandType = System.Data.CommandType.Text;
            this.V_RT205.DynamicTableName = false;
            this.V_RT205.EEPAlias = null;
            this.V_RT205.EncodingAfter = null;
            this.V_RT205.EncodingBefore = "Windows-1252";
            this.V_RT205.EncodingConvert = null;
            this.V_RT205.InfoConnection = this.InfoConnection1;
            this.V_RT205.MultiSetWhere = false;
            this.V_RT205.Name = "V_RT205";
            this.V_RT205.NotificationAutoEnlist = false;
            this.V_RT205.SecExcept = null;
            this.V_RT205.SecFieldName = null;
            this.V_RT205.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.V_RT205.SelectPaging = false;
            this.V_RT205.SelectTop = 0;
            this.V_RT205.SiteControl = false;
            this.V_RT205.SiteFieldName = null;
            this.V_RT205.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // V_RT2051
            // 
            this.V_RT2051.CacheConnection = false;
            this.V_RT2051.CommandText = "SELECT DISTINCT comtype, COMQ1, COMN\r\nFROM  V_RT205 ";
            this.V_RT2051.CommandTimeout = 30;
            this.V_RT2051.CommandType = System.Data.CommandType.Text;
            this.V_RT2051.DynamicTableName = false;
            this.V_RT2051.EEPAlias = null;
            this.V_RT2051.EncodingAfter = null;
            this.V_RT2051.EncodingBefore = "Windows-1252";
            this.V_RT2051.EncodingConvert = null;
            this.V_RT2051.InfoConnection = this.InfoConnection1;
            this.V_RT2051.MultiSetWhere = false;
            this.V_RT2051.Name = "V_RT2051";
            this.V_RT2051.NotificationAutoEnlist = false;
            this.V_RT2051.SecExcept = null;
            this.V_RT2051.SecFieldName = null;
            this.V_RT2051.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.V_RT2051.SelectPaging = false;
            this.V_RT2051.SelectTop = 0;
            this.V_RT2051.SiteControl = false;
            this.V_RT2051.SiteFieldName = null;
            this.V_RT2051.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // V_RT2052
            // 
            this.V_RT2052.CacheConnection = false;
            this.V_RT2052.CommandText = "SELECT DISTINCT comtype, COMQ1, LINEQ1\r\nFROM  V_RT205 ";
            this.V_RT2052.CommandTimeout = 30;
            this.V_RT2052.CommandType = System.Data.CommandType.Text;
            this.V_RT2052.DynamicTableName = false;
            this.V_RT2052.EEPAlias = null;
            this.V_RT2052.EncodingAfter = null;
            this.V_RT2052.EncodingBefore = "Windows-1252";
            this.V_RT2052.EncodingConvert = null;
            this.V_RT2052.InfoConnection = this.InfoConnection1;
            this.V_RT2052.MultiSetWhere = false;
            this.V_RT2052.Name = "V_RT2052";
            this.V_RT2052.NotificationAutoEnlist = false;
            this.V_RT2052.SecExcept = null;
            this.V_RT2052.SecFieldName = null;
            this.V_RT2052.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.V_RT2052.SelectPaging = false;
            this.V_RT2052.SelectTop = 0;
            this.V_RT2052.SiteControl = false;
            this.V_RT2052.SiteFieldName = null;
            this.V_RT2052.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmd
            // 
            this.cmd.CacheConnection = false;
            this.cmd.CommandText = "";
            this.cmd.CommandTimeout = 30;
            this.cmd.CommandType = System.Data.CommandType.Text;
            this.cmd.DynamicTableName = false;
            this.cmd.EEPAlias = null;
            this.cmd.EncodingAfter = null;
            this.cmd.EncodingBefore = "Windows-1252";
            this.cmd.EncodingConvert = null;
            this.cmd.InfoConnection = this.InfoConnection1;
            this.cmd.MultiSetWhere = false;
            this.cmd.Name = "cmd";
            this.cmd.NotificationAutoEnlist = false;
            this.cmd.SecExcept = null;
            this.cmd.SecFieldName = null;
            this.cmd.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmd.SelectPaging = false;
            this.cmd.SelectTop = 0;
            this.cmd.SiteControl = false;
            this.cmd.SiteFieldName = null;
            this.cmd.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTFaqM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTFaqM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT205)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTFaqAdd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RT205)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RT2051)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RT2052)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmd)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTFaqM;
        private Srvtools.UpdateComponent ucRTFaqM;
        private Srvtools.InfoCommand View_RTFaqM;
        private Srvtools.InfoCommand RT205;
        private Srvtools.InfoCommand RTFaqAdd;
        private Srvtools.UpdateComponent ucRTFaqAdd;
        private Srvtools.AutoNumber autoNumber1;
        private Srvtools.InfoCommand V_RT205;
        private Srvtools.InfoCommand V_RT2051;
        private Srvtools.InfoCommand V_RT2052;
        private Srvtools.InfoCommand cmd;
    }
}
