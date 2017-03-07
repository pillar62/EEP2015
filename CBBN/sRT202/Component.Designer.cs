namespace sRT202
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
            Srvtools.FieldAttr fieldAttr19 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr20 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr21 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr22 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr23 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr24 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr25 = new Srvtools.FieldAttr();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem7 = new Srvtools.KeyItem();
            Srvtools.FieldAttr fieldAttr26 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr27 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr28 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr29 = new Srvtools.FieldAttr();
            Srvtools.ColumnItem columnItem1 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem2 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem3 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem4 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem5 = new Srvtools.ColumnItem();
            Srvtools.ColumnItem columnItem6 = new Srvtools.ColumnItem();
            Srvtools.KeyItem keyItem8 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem9 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem10 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem11 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem12 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTLessorAVSCmtyLineFAQH = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCmtyLineFAQH = new Srvtools.UpdateComponent(this.components);
            this.RTLessorAVSCmtyLineFaqList = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCmtyLineFaqList = new Srvtools.UpdateComponent(this.components);
            this.idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList = new Srvtools.InfoDataSource(this.components);
            this.View_RTLessorAVSCmtyLineFAQH = new Srvtools.InfoCommand(this.components);
            this.RTCODEA9 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCmtyLineFAQH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCmtyLineFaqList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCmtyLineFAQH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTCODEA9)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTLessorAVSCmtyLineFAQH
            // 
            this.RTLessorAVSCmtyLineFAQH.CacheConnection = false;
            this.RTLessorAVSCmtyLineFAQH.CommandText = "SELECT dbo.[RTLessorAVSCmtyLineFAQH].* FROM dbo.[RTLessorAVSCmtyLineFAQH]";
            this.RTLessorAVSCmtyLineFAQH.CommandTimeout = 30;
            this.RTLessorAVSCmtyLineFAQH.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCmtyLineFAQH.DynamicTableName = false;
            this.RTLessorAVSCmtyLineFAQH.EEPAlias = null;
            this.RTLessorAVSCmtyLineFAQH.EncodingAfter = null;
            this.RTLessorAVSCmtyLineFAQH.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCmtyLineFAQH.EncodingConvert = null;
            this.RTLessorAVSCmtyLineFAQH.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "COMQ1";
            keyItem2.KeyName = "LINEQ1";
            keyItem3.KeyName = "FAQNO";
            this.RTLessorAVSCmtyLineFAQH.KeyFields.Add(keyItem1);
            this.RTLessorAVSCmtyLineFAQH.KeyFields.Add(keyItem2);
            this.RTLessorAVSCmtyLineFAQH.KeyFields.Add(keyItem3);
            this.RTLessorAVSCmtyLineFAQH.MultiSetWhere = false;
            this.RTLessorAVSCmtyLineFAQH.Name = "RTLessorAVSCmtyLineFAQH";
            this.RTLessorAVSCmtyLineFAQH.NotificationAutoEnlist = false;
            this.RTLessorAVSCmtyLineFAQH.SecExcept = null;
            this.RTLessorAVSCmtyLineFAQH.SecFieldName = null;
            this.RTLessorAVSCmtyLineFAQH.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCmtyLineFAQH.SelectPaging = false;
            this.RTLessorAVSCmtyLineFAQH.SelectTop = 0;
            this.RTLessorAVSCmtyLineFAQH.SiteControl = false;
            this.RTLessorAVSCmtyLineFAQH.SiteFieldName = null;
            this.RTLessorAVSCmtyLineFAQH.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCmtyLineFAQH
            // 
            this.ucRTLessorAVSCmtyLineFAQH.AutoTrans = true;
            this.ucRTLessorAVSCmtyLineFAQH.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "COMQ1";
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
            fieldAttr3.DataField = "FAQNO";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "RCVDAT";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "SERVICETYPE";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "CONTACTTEL";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "MOBILE";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "EMAIL";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "PRTDAT";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "FINISHDAT";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "FUSR";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "CANCELDAT";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "CANCELUSR";
            fieldAttr13.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr13.DefaultValue = null;
            fieldAttr13.TrimLength = 0;
            fieldAttr13.UpdateEnable = true;
            fieldAttr13.WhereMode = true;
            fieldAttr14.CharSetNull = false;
            fieldAttr14.CheckNull = false;
            fieldAttr14.DataField = "EUSR";
            fieldAttr14.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr14.DefaultValue = null;
            fieldAttr14.TrimLength = 0;
            fieldAttr14.UpdateEnable = true;
            fieldAttr14.WhereMode = true;
            fieldAttr15.CharSetNull = false;
            fieldAttr15.CheckNull = false;
            fieldAttr15.DataField = "EDAT";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "UUSR";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "UDAT";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            fieldAttr18.CharSetNull = false;
            fieldAttr18.CheckNull = false;
            fieldAttr18.DataField = "MEMO";
            fieldAttr18.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr18.DefaultValue = null;
            fieldAttr18.TrimLength = 0;
            fieldAttr18.UpdateEnable = true;
            fieldAttr18.WhereMode = true;
            fieldAttr19.CharSetNull = false;
            fieldAttr19.CheckNull = false;
            fieldAttr19.DataField = "SNDWORK";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "SNDUSR";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "SNDPRTNO";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "SNDCLOSEDAT";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            fieldAttr23.CharSetNull = false;
            fieldAttr23.CheckNull = false;
            fieldAttr23.DataField = "CALLBACKDAT";
            fieldAttr23.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr23.DefaultValue = null;
            fieldAttr23.TrimLength = 0;
            fieldAttr23.UpdateEnable = true;
            fieldAttr23.WhereMode = true;
            fieldAttr24.CharSetNull = false;
            fieldAttr24.CheckNull = false;
            fieldAttr24.DataField = "CALLBACKUSR";
            fieldAttr24.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr24.DefaultValue = null;
            fieldAttr24.TrimLength = 0;
            fieldAttr24.UpdateEnable = true;
            fieldAttr24.WhereMode = true;
            fieldAttr25.CharSetNull = false;
            fieldAttr25.CheckNull = false;
            fieldAttr25.DataField = "PRTUSR";
            fieldAttr25.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr25.DefaultValue = null;
            fieldAttr25.TrimLength = 0;
            fieldAttr25.UpdateEnable = true;
            fieldAttr25.WhereMode = true;
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr1);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr2);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr3);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr4);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr5);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr6);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr7);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr8);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr9);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr10);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr11);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr12);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr13);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr14);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr15);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr16);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr17);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr18);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr19);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr20);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr21);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr22);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr23);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr24);
            this.ucRTLessorAVSCmtyLineFAQH.FieldAttrs.Add(fieldAttr25);
            this.ucRTLessorAVSCmtyLineFAQH.LogInfo = null;
            this.ucRTLessorAVSCmtyLineFAQH.Name = "ucRTLessorAVSCmtyLineFAQH";
            this.ucRTLessorAVSCmtyLineFAQH.RowAffectsCheck = true;
            this.ucRTLessorAVSCmtyLineFAQH.SelectCmd = this.RTLessorAVSCmtyLineFAQH;
            this.ucRTLessorAVSCmtyLineFAQH.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCmtyLineFAQH.SendSQLCmd = true;
            this.ucRTLessorAVSCmtyLineFAQH.ServerModify = true;
            this.ucRTLessorAVSCmtyLineFAQH.ServerModifyGetMax = false;
            this.ucRTLessorAVSCmtyLineFAQH.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCmtyLineFAQH.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCmtyLineFAQH.UseTranscationScope = false;
            this.ucRTLessorAVSCmtyLineFAQH.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // RTLessorAVSCmtyLineFaqList
            // 
            this.RTLessorAVSCmtyLineFaqList.CacheConnection = false;
            this.RTLessorAVSCmtyLineFaqList.CommandText = "SELECT dbo.[RTLessorAVSCmtyLineFaqList].* FROM dbo.[RTLessorAVSCmtyLineFaqList]";
            this.RTLessorAVSCmtyLineFaqList.CommandTimeout = 30;
            this.RTLessorAVSCmtyLineFaqList.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCmtyLineFaqList.DynamicTableName = false;
            this.RTLessorAVSCmtyLineFaqList.EEPAlias = null;
            this.RTLessorAVSCmtyLineFaqList.EncodingAfter = null;
            this.RTLessorAVSCmtyLineFaqList.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCmtyLineFaqList.EncodingConvert = null;
            this.RTLessorAVSCmtyLineFaqList.InfoConnection = this.InfoConnection1;
            keyItem4.KeyName = "COMQ1";
            keyItem5.KeyName = "LINEQ1";
            keyItem6.KeyName = "FAQNO";
            keyItem7.KeyName = "FAQCOD";
            this.RTLessorAVSCmtyLineFaqList.KeyFields.Add(keyItem4);
            this.RTLessorAVSCmtyLineFaqList.KeyFields.Add(keyItem5);
            this.RTLessorAVSCmtyLineFaqList.KeyFields.Add(keyItem6);
            this.RTLessorAVSCmtyLineFaqList.KeyFields.Add(keyItem7);
            this.RTLessorAVSCmtyLineFaqList.MultiSetWhere = false;
            this.RTLessorAVSCmtyLineFaqList.Name = "RTLessorAVSCmtyLineFaqList";
            this.RTLessorAVSCmtyLineFaqList.NotificationAutoEnlist = false;
            this.RTLessorAVSCmtyLineFaqList.SecExcept = null;
            this.RTLessorAVSCmtyLineFaqList.SecFieldName = null;
            this.RTLessorAVSCmtyLineFaqList.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCmtyLineFaqList.SelectPaging = false;
            this.RTLessorAVSCmtyLineFaqList.SelectTop = 0;
            this.RTLessorAVSCmtyLineFaqList.SiteControl = false;
            this.RTLessorAVSCmtyLineFaqList.SiteFieldName = null;
            this.RTLessorAVSCmtyLineFaqList.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCmtyLineFaqList
            // 
            this.ucRTLessorAVSCmtyLineFaqList.AutoTrans = true;
            this.ucRTLessorAVSCmtyLineFaqList.ExceptJoin = false;
            fieldAttr26.CharSetNull = false;
            fieldAttr26.CheckNull = false;
            fieldAttr26.DataField = "COMQ1";
            fieldAttr26.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr26.DefaultValue = null;
            fieldAttr26.TrimLength = 0;
            fieldAttr26.UpdateEnable = true;
            fieldAttr26.WhereMode = true;
            fieldAttr27.CharSetNull = false;
            fieldAttr27.CheckNull = false;
            fieldAttr27.DataField = "LINEQ1";
            fieldAttr27.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr27.DefaultValue = null;
            fieldAttr27.TrimLength = 0;
            fieldAttr27.UpdateEnable = true;
            fieldAttr27.WhereMode = true;
            fieldAttr28.CharSetNull = false;
            fieldAttr28.CheckNull = false;
            fieldAttr28.DataField = "FAQNO";
            fieldAttr28.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr28.DefaultValue = null;
            fieldAttr28.TrimLength = 0;
            fieldAttr28.UpdateEnable = true;
            fieldAttr28.WhereMode = true;
            fieldAttr29.CharSetNull = false;
            fieldAttr29.CheckNull = false;
            fieldAttr29.DataField = "FAQCOD";
            fieldAttr29.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr29.DefaultValue = null;
            fieldAttr29.TrimLength = 0;
            fieldAttr29.UpdateEnable = true;
            fieldAttr29.WhereMode = true;
            this.ucRTLessorAVSCmtyLineFaqList.FieldAttrs.Add(fieldAttr26);
            this.ucRTLessorAVSCmtyLineFaqList.FieldAttrs.Add(fieldAttr27);
            this.ucRTLessorAVSCmtyLineFaqList.FieldAttrs.Add(fieldAttr28);
            this.ucRTLessorAVSCmtyLineFaqList.FieldAttrs.Add(fieldAttr29);
            this.ucRTLessorAVSCmtyLineFaqList.LogInfo = null;
            this.ucRTLessorAVSCmtyLineFaqList.Name = "ucRTLessorAVSCmtyLineFaqList";
            this.ucRTLessorAVSCmtyLineFaqList.RowAffectsCheck = true;
            this.ucRTLessorAVSCmtyLineFaqList.SelectCmd = this.RTLessorAVSCmtyLineFaqList;
            this.ucRTLessorAVSCmtyLineFaqList.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCmtyLineFaqList.SendSQLCmd = true;
            this.ucRTLessorAVSCmtyLineFaqList.ServerModify = true;
            this.ucRTLessorAVSCmtyLineFaqList.ServerModifyGetMax = false;
            this.ucRTLessorAVSCmtyLineFaqList.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCmtyLineFaqList.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCmtyLineFaqList.UseTranscationScope = false;
            this.ucRTLessorAVSCmtyLineFaqList.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList
            // 
            this.idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList.Detail = this.RTLessorAVSCmtyLineFaqList;
            columnItem1.FieldName = "COMQ1";
            columnItem2.FieldName = "LINEQ1";
            columnItem3.FieldName = "FAQNO";
            this.idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList.DetailColumns.Add(columnItem1);
            this.idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList.DetailColumns.Add(columnItem2);
            this.idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList.DetailColumns.Add(columnItem3);
            this.idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList.DynamicTableName = false;
            this.idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList.Master = this.RTLessorAVSCmtyLineFAQH;
            columnItem4.FieldName = "COMQ1";
            columnItem5.FieldName = "LINEQ1";
            columnItem6.FieldName = "FAQNO";
            this.idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList.MasterColumns.Add(columnItem4);
            this.idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList.MasterColumns.Add(columnItem5);
            this.idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList.MasterColumns.Add(columnItem6);
            // 
            // View_RTLessorAVSCmtyLineFAQH
            // 
            this.View_RTLessorAVSCmtyLineFAQH.CacheConnection = false;
            this.View_RTLessorAVSCmtyLineFAQH.CommandText = "SELECT * FROM dbo.[RTLessorAVSCmtyLineFAQH]";
            this.View_RTLessorAVSCmtyLineFAQH.CommandTimeout = 30;
            this.View_RTLessorAVSCmtyLineFAQH.CommandType = System.Data.CommandType.Text;
            this.View_RTLessorAVSCmtyLineFAQH.DynamicTableName = false;
            this.View_RTLessorAVSCmtyLineFAQH.EEPAlias = null;
            this.View_RTLessorAVSCmtyLineFAQH.EncodingAfter = null;
            this.View_RTLessorAVSCmtyLineFAQH.EncodingBefore = "Windows-1252";
            this.View_RTLessorAVSCmtyLineFAQH.EncodingConvert = null;
            this.View_RTLessorAVSCmtyLineFAQH.InfoConnection = this.InfoConnection1;
            keyItem8.KeyName = "COMQ1";
            keyItem9.KeyName = "LINEQ1";
            keyItem10.KeyName = "FAQNO";
            this.View_RTLessorAVSCmtyLineFAQH.KeyFields.Add(keyItem8);
            this.View_RTLessorAVSCmtyLineFAQH.KeyFields.Add(keyItem9);
            this.View_RTLessorAVSCmtyLineFAQH.KeyFields.Add(keyItem10);
            this.View_RTLessorAVSCmtyLineFAQH.MultiSetWhere = false;
            this.View_RTLessorAVSCmtyLineFAQH.Name = "View_RTLessorAVSCmtyLineFAQH";
            this.View_RTLessorAVSCmtyLineFAQH.NotificationAutoEnlist = false;
            this.View_RTLessorAVSCmtyLineFAQH.SecExcept = null;
            this.View_RTLessorAVSCmtyLineFAQH.SecFieldName = null;
            this.View_RTLessorAVSCmtyLineFAQH.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTLessorAVSCmtyLineFAQH.SelectPaging = false;
            this.View_RTLessorAVSCmtyLineFAQH.SelectTop = 0;
            this.View_RTLessorAVSCmtyLineFAQH.SiteControl = false;
            this.View_RTLessorAVSCmtyLineFAQH.SiteFieldName = null;
            this.View_RTLessorAVSCmtyLineFAQH.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // RTCODEA9
            // 
            this.RTCODEA9.CacheConnection = false;
            this.RTCODEA9.CommandText = "SELECT * FROM RTCODE\r\nWHERE KIND  = \'A9\'";
            this.RTCODEA9.CommandTimeout = 30;
            this.RTCODEA9.CommandType = System.Data.CommandType.Text;
            this.RTCODEA9.DynamicTableName = false;
            this.RTCODEA9.EEPAlias = null;
            this.RTCODEA9.EncodingAfter = null;
            this.RTCODEA9.EncodingBefore = "Windows-1252";
            this.RTCODEA9.EncodingConvert = null;
            this.RTCODEA9.InfoConnection = this.InfoConnection1;
            keyItem11.KeyName = "KIND";
            keyItem12.KeyName = "CODE";
            this.RTCODEA9.KeyFields.Add(keyItem11);
            this.RTCODEA9.KeyFields.Add(keyItem12);
            this.RTCODEA9.MultiSetWhere = false;
            this.RTCODEA9.Name = "RTCODEA9";
            this.RTCODEA9.NotificationAutoEnlist = false;
            this.RTCODEA9.SecExcept = null;
            this.RTCODEA9.SecFieldName = null;
            this.RTCODEA9.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTCODEA9.SelectPaging = false;
            this.RTCODEA9.SelectTop = 0;
            this.RTCODEA9.SiteControl = false;
            this.RTCODEA9.SiteFieldName = null;
            this.RTCODEA9.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCmtyLineFAQH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCmtyLineFaqList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCmtyLineFAQH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTCODEA9)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTLessorAVSCmtyLineFAQH;
        private Srvtools.UpdateComponent ucRTLessorAVSCmtyLineFAQH;
        private Srvtools.InfoCommand RTLessorAVSCmtyLineFaqList;
        private Srvtools.UpdateComponent ucRTLessorAVSCmtyLineFaqList;
        private Srvtools.InfoDataSource idRTLessorAVSCmtyLineFAQH_RTLessorAVSCmtyLineFaqList;
        private Srvtools.InfoCommand View_RTLessorAVSCmtyLineFAQH;
        private Srvtools.InfoCommand RTCODEA9;
    }
}
