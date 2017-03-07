namespace sRT105
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
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem7 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem8 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem9 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem10 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem11 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem12 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem13 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem14 = new Srvtools.KeyItem();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTLessorAVSCase = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCase = new Srvtools.UpdateComponent(this.components);
            this.View_RTLessorAVSCase = new Srvtools.InfoCommand(this.components);
            this.V_RTLessorAVSCustFaqH = new Srvtools.InfoCommand(this.components);
            this.V_RTLessorAVSCustCont = new Srvtools.InfoCommand(this.components);
            this.V_RTLessorAVSCustDrop = new Srvtools.InfoCommand(this.components);
            this.V_RTLessorAVSCustReturn = new Srvtools.InfoCommand(this.components);
            this.V_RTLessorAVSCustAR = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCase)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RTLessorAVSCustFaqH)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RTLessorAVSCustCont)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RTLessorAVSCustDrop)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RTLessorAVSCustReturn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RTLessorAVSCustAR)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTLessorAVSCase
            // 
            this.RTLessorAVSCase.CacheConnection = false;
            this.RTLessorAVSCase.CommandText = "SELECT dbo.[RTLessorAVSCase].* FROM dbo.[RTLessorAVSCase]";
            this.RTLessorAVSCase.CommandTimeout = 30;
            this.RTLessorAVSCase.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCase.DynamicTableName = false;
            this.RTLessorAVSCase.EEPAlias = null;
            this.RTLessorAVSCase.EncodingAfter = null;
            this.RTLessorAVSCase.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCase.EncodingConvert = null;
            this.RTLessorAVSCase.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "CASEID";
            this.RTLessorAVSCase.KeyFields.Add(keyItem1);
            this.RTLessorAVSCase.MultiSetWhere = false;
            this.RTLessorAVSCase.Name = "RTLessorAVSCase";
            this.RTLessorAVSCase.NotificationAutoEnlist = false;
            this.RTLessorAVSCase.SecExcept = null;
            this.RTLessorAVSCase.SecFieldName = null;
            this.RTLessorAVSCase.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCase.SelectPaging = false;
            this.RTLessorAVSCase.SelectTop = 0;
            this.RTLessorAVSCase.SiteControl = false;
            this.RTLessorAVSCase.SiteFieldName = null;
            this.RTLessorAVSCase.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCase
            // 
            this.ucRTLessorAVSCase.AutoTrans = true;
            this.ucRTLessorAVSCase.ExceptJoin = false;
            fieldAttr1.CharSetNull = false;
            fieldAttr1.CheckNull = false;
            fieldAttr1.DataField = "CASEID";
            fieldAttr1.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr1.DefaultValue = null;
            fieldAttr1.TrimLength = 0;
            fieldAttr1.UpdateEnable = true;
            fieldAttr1.WhereMode = true;
            fieldAttr2.CharSetNull = false;
            fieldAttr2.CheckNull = false;
            fieldAttr2.DataField = "CASENAME";
            fieldAttr2.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr2.DefaultValue = null;
            fieldAttr2.TrimLength = 0;
            fieldAttr2.UpdateEnable = true;
            fieldAttr2.WhereMode = true;
            fieldAttr3.CharSetNull = false;
            fieldAttr3.CheckNull = false;
            fieldAttr3.DataField = "STARTDAT";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "ENDDAT";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "CRTUSR";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "MEMO";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "EUSR";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "EDAT";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "UUSR";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "UDAT";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr1);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr2);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr3);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr4);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr5);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr6);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr7);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr8);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr9);
            this.ucRTLessorAVSCase.FieldAttrs.Add(fieldAttr10);
            this.ucRTLessorAVSCase.LogInfo = null;
            this.ucRTLessorAVSCase.Name = "ucRTLessorAVSCase";
            this.ucRTLessorAVSCase.RowAffectsCheck = true;
            this.ucRTLessorAVSCase.SelectCmd = this.RTLessorAVSCase;
            this.ucRTLessorAVSCase.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCase.SendSQLCmd = true;
            this.ucRTLessorAVSCase.ServerModify = true;
            this.ucRTLessorAVSCase.ServerModifyGetMax = false;
            this.ucRTLessorAVSCase.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCase.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCase.UseTranscationScope = false;
            this.ucRTLessorAVSCase.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTLessorAVSCase
            // 
            this.View_RTLessorAVSCase.CacheConnection = false;
            this.View_RTLessorAVSCase.CommandText = "SELECT * FROM dbo.[RTLessorAVSCase]";
            this.View_RTLessorAVSCase.CommandTimeout = 30;
            this.View_RTLessorAVSCase.CommandType = System.Data.CommandType.Text;
            this.View_RTLessorAVSCase.DynamicTableName = false;
            this.View_RTLessorAVSCase.EEPAlias = null;
            this.View_RTLessorAVSCase.EncodingAfter = null;
            this.View_RTLessorAVSCase.EncodingBefore = "Windows-1252";
            this.View_RTLessorAVSCase.EncodingConvert = null;
            this.View_RTLessorAVSCase.InfoConnection = this.InfoConnection1;
            keyItem2.KeyName = "CASEID";
            this.View_RTLessorAVSCase.KeyFields.Add(keyItem2);
            this.View_RTLessorAVSCase.MultiSetWhere = false;
            this.View_RTLessorAVSCase.Name = "View_RTLessorAVSCase";
            this.View_RTLessorAVSCase.NotificationAutoEnlist = false;
            this.View_RTLessorAVSCase.SecExcept = null;
            this.View_RTLessorAVSCase.SecFieldName = null;
            this.View_RTLessorAVSCase.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTLessorAVSCase.SelectPaging = false;
            this.View_RTLessorAVSCase.SelectTop = 0;
            this.View_RTLessorAVSCase.SiteControl = false;
            this.View_RTLessorAVSCase.SiteFieldName = null;
            this.View_RTLessorAVSCase.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // V_RTLessorAVSCustFaqH
            // 
            this.V_RTLessorAVSCustFaqH.CacheConnection = false;
            this.V_RTLessorAVSCustFaqH.CommandText = resources.GetString("V_RTLessorAVSCustFaqH.CommandText");
            this.V_RTLessorAVSCustFaqH.CommandTimeout = 30;
            this.V_RTLessorAVSCustFaqH.CommandType = System.Data.CommandType.Text;
            this.V_RTLessorAVSCustFaqH.DynamicTableName = false;
            this.V_RTLessorAVSCustFaqH.EEPAlias = null;
            this.V_RTLessorAVSCustFaqH.EncodingAfter = null;
            this.V_RTLessorAVSCustFaqH.EncodingBefore = "Windows-1252";
            this.V_RTLessorAVSCustFaqH.EncodingConvert = null;
            this.V_RTLessorAVSCustFaqH.InfoConnection = this.InfoConnection1;
            keyItem3.KeyName = "CUSID";
            keyItem4.KeyName = "FAQNO";
            this.V_RTLessorAVSCustFaqH.KeyFields.Add(keyItem3);
            this.V_RTLessorAVSCustFaqH.KeyFields.Add(keyItem4);
            this.V_RTLessorAVSCustFaqH.MultiSetWhere = false;
            this.V_RTLessorAVSCustFaqH.Name = "V_RTLessorAVSCustFaqH";
            this.V_RTLessorAVSCustFaqH.NotificationAutoEnlist = false;
            this.V_RTLessorAVSCustFaqH.SecExcept = null;
            this.V_RTLessorAVSCustFaqH.SecFieldName = null;
            this.V_RTLessorAVSCustFaqH.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.V_RTLessorAVSCustFaqH.SelectPaging = false;
            this.V_RTLessorAVSCustFaqH.SelectTop = 0;
            this.V_RTLessorAVSCustFaqH.SiteControl = false;
            this.V_RTLessorAVSCustFaqH.SiteFieldName = null;
            this.V_RTLessorAVSCustFaqH.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // V_RTLessorAVSCustCont
            // 
            this.V_RTLessorAVSCustCont.CacheConnection = false;
            this.V_RTLessorAVSCustCont.CommandText = resources.GetString("V_RTLessorAVSCustCont.CommandText");
            this.V_RTLessorAVSCustCont.CommandTimeout = 30;
            this.V_RTLessorAVSCustCont.CommandType = System.Data.CommandType.Text;
            this.V_RTLessorAVSCustCont.DynamicTableName = false;
            this.V_RTLessorAVSCustCont.EEPAlias = null;
            this.V_RTLessorAVSCustCont.EncodingAfter = null;
            this.V_RTLessorAVSCustCont.EncodingBefore = "Windows-1252";
            this.V_RTLessorAVSCustCont.EncodingConvert = null;
            this.V_RTLessorAVSCustCont.InfoConnection = this.InfoConnection1;
            keyItem5.KeyName = "CUSID";
            keyItem6.KeyName = "ENTRYNO";
            keyItem7.KeyName = "ENTRYNO1";
            this.V_RTLessorAVSCustCont.KeyFields.Add(keyItem5);
            this.V_RTLessorAVSCustCont.KeyFields.Add(keyItem6);
            this.V_RTLessorAVSCustCont.KeyFields.Add(keyItem7);
            this.V_RTLessorAVSCustCont.MultiSetWhere = false;
            this.V_RTLessorAVSCustCont.Name = "V_RTLessorAVSCustCont";
            this.V_RTLessorAVSCustCont.NotificationAutoEnlist = false;
            this.V_RTLessorAVSCustCont.SecExcept = null;
            this.V_RTLessorAVSCustCont.SecFieldName = null;
            this.V_RTLessorAVSCustCont.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.V_RTLessorAVSCustCont.SelectPaging = false;
            this.V_RTLessorAVSCustCont.SelectTop = 0;
            this.V_RTLessorAVSCustCont.SiteControl = false;
            this.V_RTLessorAVSCustCont.SiteFieldName = null;
            this.V_RTLessorAVSCustCont.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // V_RTLessorAVSCustDrop
            // 
            this.V_RTLessorAVSCustDrop.CacheConnection = false;
            this.V_RTLessorAVSCustDrop.CommandText = resources.GetString("V_RTLessorAVSCustDrop.CommandText");
            this.V_RTLessorAVSCustDrop.CommandTimeout = 30;
            this.V_RTLessorAVSCustDrop.CommandType = System.Data.CommandType.Text;
            this.V_RTLessorAVSCustDrop.DynamicTableName = false;
            this.V_RTLessorAVSCustDrop.EEPAlias = null;
            this.V_RTLessorAVSCustDrop.EncodingAfter = null;
            this.V_RTLessorAVSCustDrop.EncodingBefore = "Windows-1252";
            this.V_RTLessorAVSCustDrop.EncodingConvert = null;
            this.V_RTLessorAVSCustDrop.InfoConnection = this.InfoConnection1;
            keyItem8.KeyName = "CUSID";
            keyItem9.KeyName = "ENTRYNO";
            this.V_RTLessorAVSCustDrop.KeyFields.Add(keyItem8);
            this.V_RTLessorAVSCustDrop.KeyFields.Add(keyItem9);
            this.V_RTLessorAVSCustDrop.MultiSetWhere = false;
            this.V_RTLessorAVSCustDrop.Name = "V_RTLessorAVSCustDrop";
            this.V_RTLessorAVSCustDrop.NotificationAutoEnlist = false;
            this.V_RTLessorAVSCustDrop.SecExcept = null;
            this.V_RTLessorAVSCustDrop.SecFieldName = null;
            this.V_RTLessorAVSCustDrop.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.V_RTLessorAVSCustDrop.SelectPaging = false;
            this.V_RTLessorAVSCustDrop.SelectTop = 0;
            this.V_RTLessorAVSCustDrop.SiteControl = false;
            this.V_RTLessorAVSCustDrop.SiteFieldName = null;
            this.V_RTLessorAVSCustDrop.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // V_RTLessorAVSCustReturn
            // 
            this.V_RTLessorAVSCustReturn.CacheConnection = false;
            this.V_RTLessorAVSCustReturn.CommandText = resources.GetString("V_RTLessorAVSCustReturn.CommandText");
            this.V_RTLessorAVSCustReturn.CommandTimeout = 30;
            this.V_RTLessorAVSCustReturn.CommandType = System.Data.CommandType.Text;
            this.V_RTLessorAVSCustReturn.DynamicTableName = false;
            this.V_RTLessorAVSCustReturn.EEPAlias = null;
            this.V_RTLessorAVSCustReturn.EncodingAfter = null;
            this.V_RTLessorAVSCustReturn.EncodingBefore = "Windows-1252";
            this.V_RTLessorAVSCustReturn.EncodingConvert = null;
            this.V_RTLessorAVSCustReturn.InfoConnection = this.InfoConnection1;
            keyItem10.KeyName = "CUSID";
            keyItem11.KeyName = "ENTRYNO";
            keyItem12.KeyName = "ENTRYNO1";
            this.V_RTLessorAVSCustReturn.KeyFields.Add(keyItem10);
            this.V_RTLessorAVSCustReturn.KeyFields.Add(keyItem11);
            this.V_RTLessorAVSCustReturn.KeyFields.Add(keyItem12);
            this.V_RTLessorAVSCustReturn.MultiSetWhere = false;
            this.V_RTLessorAVSCustReturn.Name = "V_RTLessorAVSCustReturn";
            this.V_RTLessorAVSCustReturn.NotificationAutoEnlist = false;
            this.V_RTLessorAVSCustReturn.SecExcept = null;
            this.V_RTLessorAVSCustReturn.SecFieldName = null;
            this.V_RTLessorAVSCustReturn.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.V_RTLessorAVSCustReturn.SelectPaging = false;
            this.V_RTLessorAVSCustReturn.SelectTop = 0;
            this.V_RTLessorAVSCustReturn.SiteControl = false;
            this.V_RTLessorAVSCustReturn.SiteFieldName = null;
            this.V_RTLessorAVSCustReturn.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // V_RTLessorAVSCustAR
            // 
            this.V_RTLessorAVSCustAR.CacheConnection = false;
            this.V_RTLessorAVSCustAR.CommandText = resources.GetString("V_RTLessorAVSCustAR.CommandText");
            this.V_RTLessorAVSCustAR.CommandTimeout = 30;
            this.V_RTLessorAVSCustAR.CommandType = System.Data.CommandType.Text;
            this.V_RTLessorAVSCustAR.DynamicTableName = false;
            this.V_RTLessorAVSCustAR.EEPAlias = null;
            this.V_RTLessorAVSCustAR.EncodingAfter = null;
            this.V_RTLessorAVSCustAR.EncodingBefore = "Windows-1252";
            this.V_RTLessorAVSCustAR.EncodingConvert = null;
            this.V_RTLessorAVSCustAR.InfoConnection = this.InfoConnection1;
            keyItem13.KeyName = "CUSID";
            keyItem14.KeyName = "BATCHNO";
            this.V_RTLessorAVSCustAR.KeyFields.Add(keyItem13);
            this.V_RTLessorAVSCustAR.KeyFields.Add(keyItem14);
            this.V_RTLessorAVSCustAR.MultiSetWhere = false;
            this.V_RTLessorAVSCustAR.Name = "V_RTLessorAVSCustAR";
            this.V_RTLessorAVSCustAR.NotificationAutoEnlist = false;
            this.V_RTLessorAVSCustAR.SecExcept = null;
            this.V_RTLessorAVSCustAR.SecFieldName = null;
            this.V_RTLessorAVSCustAR.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.V_RTLessorAVSCustAR.SelectPaging = false;
            this.V_RTLessorAVSCustAR.SelectTop = 0;
            this.V_RTLessorAVSCustAR.SiteControl = false;
            this.V_RTLessorAVSCustAR.SiteFieldName = null;
            this.V_RTLessorAVSCustAR.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCase)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RTLessorAVSCustFaqH)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RTLessorAVSCustCont)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RTLessorAVSCustDrop)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RTLessorAVSCustReturn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.V_RTLessorAVSCustAR)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTLessorAVSCase;
        private Srvtools.UpdateComponent ucRTLessorAVSCase;
        private Srvtools.InfoCommand View_RTLessorAVSCase;
        private Srvtools.InfoCommand V_RTLessorAVSCustFaqH;
        private Srvtools.InfoCommand V_RTLessorAVSCustCont;
        private Srvtools.InfoCommand V_RTLessorAVSCustDrop;
        private Srvtools.InfoCommand V_RTLessorAVSCustReturn;
        private Srvtools.InfoCommand V_RTLessorAVSCustAR;
    }
}
