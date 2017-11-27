namespace sRT208
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
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTFaqM = new Srvtools.InfoCommand(this.components);
            this.ucRTFaqM = new Srvtools.UpdateComponent(this.components);
            this.View_RTFaqM = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTFaqM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTFaqM)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTFaqM
            // 
            this.RTFaqM.CacheConnection = false;
            this.RTFaqM.CommandText = "SELECT A.*, B.SALESID, B.CONSIGNEE\r\nFROM RTFaqM A\r\nLEFT JOIN RTLessorAVSCmtyLine " +
    "B ON B.COMQ1=A.COMQ1 AND B.LINEQ1=A.LINEQ1 \r\nORDER BY A.CASENO";
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
            this.View_RTFaqM.CommandText = "SELECT * FROM dbo.[RTFaqM]";
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
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTFaqM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTFaqM)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTFaqM;
        private Srvtools.UpdateComponent ucRTFaqM;
        private Srvtools.InfoCommand View_RTFaqM;
    }
}
