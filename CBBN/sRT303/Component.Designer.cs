namespace sRT303
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
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
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
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            Srvtools.KeyItem keyItem7 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem8 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem9 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem10 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem11 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem12 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem13 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem14 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem15 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTLessorAVSCustARDTL = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCustARDTL = new Srvtools.UpdateComponent(this.components);
            this.View_RTLessorAVSCustARDTL = new Srvtools.InfoCommand(this.components);
            this.cmRT303 = new Srvtools.InfoCommand(this.components);
            this.cmRT3031 = new Srvtools.InfoCommand(this.components);
            this.cmRT3032 = new Srvtools.InfoCommand(this.components);
            this.cmRT3033 = new Srvtools.InfoCommand(this.components);
            this.cmRT30301 = new Srvtools.InfoCommand(this.components);
            this.cmRT30302 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustARDTL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustARDTL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT303)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT3031)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT3032)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT3033)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT30301)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT30302)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTLessorAVSCustARDTL
            // 
            this.RTLessorAVSCustARDTL.CacheConnection = false;
            this.RTLessorAVSCustARDTL.CommandText = "SELECT dbo.[RTLessorAVSCustARDTL].* FROM dbo.[RTLessorAVSCustARDTL]";
            this.RTLessorAVSCustARDTL.CommandTimeout = 30;
            this.RTLessorAVSCustARDTL.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCustARDTL.DynamicTableName = false;
            this.RTLessorAVSCustARDTL.EEPAlias = null;
            this.RTLessorAVSCustARDTL.EncodingAfter = null;
            this.RTLessorAVSCustARDTL.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCustARDTL.EncodingConvert = null;
            this.RTLessorAVSCustARDTL.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "CUSID";
            keyItem2.KeyName = "BATCHNO";
            keyItem3.KeyName = "SEQ";
            this.RTLessorAVSCustARDTL.KeyFields.Add(keyItem1);
            this.RTLessorAVSCustARDTL.KeyFields.Add(keyItem2);
            this.RTLessorAVSCustARDTL.KeyFields.Add(keyItem3);
            this.RTLessorAVSCustARDTL.MultiSetWhere = false;
            this.RTLessorAVSCustARDTL.Name = "RTLessorAVSCustARDTL";
            this.RTLessorAVSCustARDTL.NotificationAutoEnlist = false;
            this.RTLessorAVSCustARDTL.SecExcept = null;
            this.RTLessorAVSCustARDTL.SecFieldName = null;
            this.RTLessorAVSCustARDTL.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCustARDTL.SelectPaging = false;
            this.RTLessorAVSCustARDTL.SelectTop = 0;
            this.RTLessorAVSCustARDTL.SiteControl = false;
            this.RTLessorAVSCustARDTL.SiteFieldName = null;
            this.RTLessorAVSCustARDTL.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCustARDTL
            // 
            this.ucRTLessorAVSCustARDTL.AutoTrans = true;
            this.ucRTLessorAVSCustARDTL.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "CUSID";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "BATCHNO";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "SEQ";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "SYY";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "SMM";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "TYY";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "TMM";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "ITEMNC";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "PORM";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "AMT";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "REALAMT";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "CDAT";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "MDAT";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "CANCELDAT";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "CANCELMEMO";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "CANCELUSR";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "L14";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "L23";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr1);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr2);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr3);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr4);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr5);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr6);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr7);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr8);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr9);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr10);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr11);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr12);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr13);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr14);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr15);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr16);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr17);
            this.ucRTLessorAVSCustARDTL.FieldAttrs.Add(fieldAttr18);
            this.ucRTLessorAVSCustARDTL.LogInfo = null;
            this.ucRTLessorAVSCustARDTL.Name = "ucRTLessorAVSCustARDTL";
            this.ucRTLessorAVSCustARDTL.RowAffectsCheck = true;
            this.ucRTLessorAVSCustARDTL.SelectCmd = this.RTLessorAVSCustARDTL;
            this.ucRTLessorAVSCustARDTL.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCustARDTL.SendSQLCmd = true;
            this.ucRTLessorAVSCustARDTL.ServerModify = true;
            this.ucRTLessorAVSCustARDTL.ServerModifyGetMax = false;
            this.ucRTLessorAVSCustARDTL.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCustARDTL.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCustARDTL.UseTranscationScope = false;
            this.ucRTLessorAVSCustARDTL.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTLessorAVSCustARDTL
            // 
            this.View_RTLessorAVSCustARDTL.CacheConnection = false;
            this.View_RTLessorAVSCustARDTL.CommandText = "SELECT * FROM dbo.[RTLessorAVSCustARDTL]";
            this.View_RTLessorAVSCustARDTL.CommandTimeout = 30;
            this.View_RTLessorAVSCustARDTL.CommandType = System.Data.CommandType.Text;
            this.View_RTLessorAVSCustARDTL.DynamicTableName = false;
            this.View_RTLessorAVSCustARDTL.EEPAlias = null;
            this.View_RTLessorAVSCustARDTL.EncodingAfter = null;
            this.View_RTLessorAVSCustARDTL.EncodingBefore = "Windows-1252";
            this.View_RTLessorAVSCustARDTL.EncodingConvert = null;
            this.View_RTLessorAVSCustARDTL.InfoConnection = this.InfoConnection1;
            keyItem4.KeyName = "CUSID";
            keyItem5.KeyName = "BATCHNO";
            keyItem6.KeyName = "SEQ";
            this.View_RTLessorAVSCustARDTL.KeyFields.Add(keyItem4);
            this.View_RTLessorAVSCustARDTL.KeyFields.Add(keyItem5);
            this.View_RTLessorAVSCustARDTL.KeyFields.Add(keyItem6);
            this.View_RTLessorAVSCustARDTL.MultiSetWhere = false;
            this.View_RTLessorAVSCustARDTL.Name = "View_RTLessorAVSCustARDTL";
            this.View_RTLessorAVSCustARDTL.NotificationAutoEnlist = false;
            this.View_RTLessorAVSCustARDTL.SecExcept = null;
            this.View_RTLessorAVSCustARDTL.SecFieldName = null;
            this.View_RTLessorAVSCustARDTL.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTLessorAVSCustARDTL.SelectPaging = false;
            this.View_RTLessorAVSCustARDTL.SelectTop = 0;
            this.View_RTLessorAVSCustARDTL.SiteControl = false;
            this.View_RTLessorAVSCustARDTL.SiteFieldName = null;
            this.View_RTLessorAVSCustARDTL.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmRT303
            // 
            this.cmRT303.CacheConnection = false;
            this.cmRT303.CommandText = resources.GetString("cmRT303.CommandText");
            this.cmRT303.CommandTimeout = 30;
            this.cmRT303.CommandType = System.Data.CommandType.Text;
            this.cmRT303.DynamicTableName = false;
            this.cmRT303.EEPAlias = null;
            this.cmRT303.EncodingAfter = null;
            this.cmRT303.EncodingBefore = "Windows-1252";
            this.cmRT303.EncodingConvert = null;
            this.cmRT303.InfoConnection = this.InfoConnection1;
            this.cmRT303.MultiSetWhere = false;
            this.cmRT303.Name = "cmRT303";
            this.cmRT303.NotificationAutoEnlist = false;
            this.cmRT303.SecExcept = null;
            this.cmRT303.SecFieldName = null;
            this.cmRT303.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmRT303.SelectPaging = false;
            this.cmRT303.SelectTop = 0;
            this.cmRT303.SiteControl = false;
            this.cmRT303.SiteFieldName = null;
            this.cmRT303.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmRT3031
            // 
            this.cmRT3031.CacheConnection = false;
            this.cmRT3031.CommandText = resources.GetString("cmRT3031.CommandText");
            this.cmRT3031.CommandTimeout = 30;
            this.cmRT3031.CommandType = System.Data.CommandType.Text;
            this.cmRT3031.DynamicTableName = false;
            this.cmRT3031.EEPAlias = null;
            this.cmRT3031.EncodingAfter = null;
            this.cmRT3031.EncodingBefore = "Windows-1252";
            this.cmRT3031.EncodingConvert = null;
            this.cmRT3031.InfoConnection = this.InfoConnection1;
            keyItem7.KeyName = "CUSID";
            keyItem8.KeyName = "BATCHNO";
            keyItem9.KeyName = "SEQ";
            this.cmRT3031.KeyFields.Add(keyItem7);
            this.cmRT3031.KeyFields.Add(keyItem8);
            this.cmRT3031.KeyFields.Add(keyItem9);
            this.cmRT3031.MultiSetWhere = false;
            this.cmRT3031.Name = "cmRT3031";
            this.cmRT3031.NotificationAutoEnlist = false;
            this.cmRT3031.SecExcept = null;
            this.cmRT3031.SecFieldName = null;
            this.cmRT3031.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmRT3031.SelectPaging = false;
            this.cmRT3031.SelectTop = 0;
            this.cmRT3031.SiteControl = false;
            this.cmRT3031.SiteFieldName = null;
            this.cmRT3031.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmRT3032
            // 
            this.cmRT3032.CacheConnection = false;
            this.cmRT3032.CommandText = resources.GetString("cmRT3032.CommandText");
            this.cmRT3032.CommandTimeout = 30;
            this.cmRT3032.CommandType = System.Data.CommandType.Text;
            this.cmRT3032.DynamicTableName = false;
            this.cmRT3032.EEPAlias = null;
            this.cmRT3032.EncodingAfter = null;
            this.cmRT3032.EncodingBefore = "Windows-1252";
            this.cmRT3032.EncodingConvert = null;
            this.cmRT3032.InfoConnection = this.InfoConnection1;
            keyItem10.KeyName = "CUSID";
            keyItem11.KeyName = "BATCHNO";
            keyItem12.KeyName = "SEQ";
            this.cmRT3032.KeyFields.Add(keyItem10);
            this.cmRT3032.KeyFields.Add(keyItem11);
            this.cmRT3032.KeyFields.Add(keyItem12);
            this.cmRT3032.MultiSetWhere = false;
            this.cmRT3032.Name = "cmRT3032";
            this.cmRT3032.NotificationAutoEnlist = false;
            this.cmRT3032.SecExcept = null;
            this.cmRT3032.SecFieldName = null;
            this.cmRT3032.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmRT3032.SelectPaging = false;
            this.cmRT3032.SelectTop = 0;
            this.cmRT3032.SiteControl = false;
            this.cmRT3032.SiteFieldName = null;
            this.cmRT3032.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmRT3033
            // 
            this.cmRT3033.CacheConnection = false;
            this.cmRT3033.CommandText = resources.GetString("cmRT3033.CommandText");
            this.cmRT3033.CommandTimeout = 30;
            this.cmRT3033.CommandType = System.Data.CommandType.Text;
            this.cmRT3033.DynamicTableName = false;
            this.cmRT3033.EEPAlias = null;
            this.cmRT3033.EncodingAfter = null;
            this.cmRT3033.EncodingBefore = "Windows-1252";
            this.cmRT3033.EncodingConvert = null;
            this.cmRT3033.InfoConnection = this.InfoConnection1;
            keyItem13.KeyName = "CUSID";
            keyItem14.KeyName = "BATCHNO";
            keyItem15.KeyName = "SEQ";
            this.cmRT3033.KeyFields.Add(keyItem13);
            this.cmRT3033.KeyFields.Add(keyItem14);
            this.cmRT3033.KeyFields.Add(keyItem15);
            this.cmRT3033.MultiSetWhere = false;
            this.cmRT3033.Name = "cmRT3033";
            this.cmRT3033.NotificationAutoEnlist = false;
            this.cmRT3033.SecExcept = null;
            this.cmRT3033.SecFieldName = null;
            this.cmRT3033.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmRT3033.SelectPaging = false;
            this.cmRT3033.SelectTop = 0;
            this.cmRT3033.SiteControl = false;
            this.cmRT3033.SiteFieldName = null;
            this.cmRT3033.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmRT30301
            // 
            this.cmRT30301.CacheConnection = false;
            this.cmRT30301.CommandText = resources.GetString("cmRT30301.CommandText");
            this.cmRT30301.CommandTimeout = 30;
            this.cmRT30301.CommandType = System.Data.CommandType.Text;
            this.cmRT30301.DynamicTableName = false;
            this.cmRT30301.EEPAlias = null;
            this.cmRT30301.EncodingAfter = null;
            this.cmRT30301.EncodingBefore = "Windows-1252";
            this.cmRT30301.EncodingConvert = null;
            this.cmRT30301.InfoConnection = this.InfoConnection1;
            this.cmRT30301.MultiSetWhere = false;
            this.cmRT30301.Name = "cmRT30301";
            this.cmRT30301.NotificationAutoEnlist = false;
            this.cmRT30301.SecExcept = null;
            this.cmRT30301.SecFieldName = null;
            this.cmRT30301.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmRT30301.SelectPaging = false;
            this.cmRT30301.SelectTop = 0;
            this.cmRT30301.SiteControl = false;
            this.cmRT30301.SiteFieldName = null;
            this.cmRT30301.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmRT30302
            // 
            this.cmRT30302.CacheConnection = false;
            this.cmRT30302.CommandText = resources.GetString("cmRT30302.CommandText");
            this.cmRT30302.CommandTimeout = 30;
            this.cmRT30302.CommandType = System.Data.CommandType.Text;
            this.cmRT30302.DynamicTableName = false;
            this.cmRT30302.EEPAlias = null;
            this.cmRT30302.EncodingAfter = null;
            this.cmRT30302.EncodingBefore = "Windows-1252";
            this.cmRT30302.EncodingConvert = null;
            this.cmRT30302.InfoConnection = this.InfoConnection1;
            this.cmRT30302.MultiSetWhere = false;
            this.cmRT30302.Name = "cmRT30302";
            this.cmRT30302.NotificationAutoEnlist = false;
            this.cmRT30302.SecExcept = null;
            this.cmRT30302.SecFieldName = null;
            this.cmRT30302.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmRT30302.SelectPaging = false;
            this.cmRT30302.SelectTop = 0;
            this.cmRT30302.SiteControl = false;
            this.cmRT30302.SiteFieldName = null;
            this.cmRT30302.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustARDTL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustARDTL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT303)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT3031)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT3032)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT3033)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT30301)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT30302)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTLessorAVSCustARDTL;
        private Srvtools.UpdateComponent ucRTLessorAVSCustARDTL;
        private Srvtools.InfoCommand View_RTLessorAVSCustARDTL;
        private Srvtools.InfoCommand cmRT303;
        private Srvtools.InfoCommand cmRT3031;
        private Srvtools.InfoCommand cmRT3032;
        private Srvtools.InfoCommand cmRT3033;
        private Srvtools.InfoCommand cmRT30301;
        private Srvtools.InfoCommand cmRT30302;
    }
}
