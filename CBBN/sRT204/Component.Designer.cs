namespace sRT204
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
            Srvtools.FieldAttr fieldAttr17 = new Srvtools.FieldAttr();
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem5 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem6 = new Srvtools.KeyItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTLessorAVSCustAR = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCustAR = new Srvtools.UpdateComponent(this.components);
            this.View_RTLessorAVSCustAR = new Srvtools.InfoCommand(this.components);
            this.cmRT204 = new Srvtools.InfoCommand(this.components);
            this.RT2044 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustAR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustAR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT204)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT2044)).BeginInit();
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTLessorAVSCustAR
            // 
            this.RTLessorAVSCustAR.CacheConnection = false;
            this.RTLessorAVSCustAR.CommandText = "SELECT dbo.[RTLessorAVSCustAR].* FROM dbo.[RTLessorAVSCustAR]";
            this.RTLessorAVSCustAR.CommandTimeout = 30;
            this.RTLessorAVSCustAR.CommandType = System.Data.CommandType.Text;
            this.RTLessorAVSCustAR.DynamicTableName = false;
            this.RTLessorAVSCustAR.EEPAlias = null;
            this.RTLessorAVSCustAR.EncodingAfter = null;
            this.RTLessorAVSCustAR.EncodingBefore = "Windows-1252";
            this.RTLessorAVSCustAR.EncodingConvert = null;
            this.RTLessorAVSCustAR.InfoConnection = this.InfoConnection1;
            keyItem1.KeyName = "CUSID";
            keyItem2.KeyName = "BATCHNO";
            this.RTLessorAVSCustAR.KeyFields.Add(keyItem1);
            this.RTLessorAVSCustAR.KeyFields.Add(keyItem2);
            this.RTLessorAVSCustAR.MultiSetWhere = false;
            this.RTLessorAVSCustAR.Name = "RTLessorAVSCustAR";
            this.RTLessorAVSCustAR.NotificationAutoEnlist = false;
            this.RTLessorAVSCustAR.SecExcept = null;
            this.RTLessorAVSCustAR.SecFieldName = null;
            this.RTLessorAVSCustAR.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTLessorAVSCustAR.SelectPaging = false;
            this.RTLessorAVSCustAR.SelectTop = 0;
            this.RTLessorAVSCustAR.SiteControl = false;
            this.RTLessorAVSCustAR.SiteFieldName = null;
            this.RTLessorAVSCustAR.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTLessorAVSCustAR
            // 
            this.ucRTLessorAVSCustAR.AutoTrans = true;
            this.ucRTLessorAVSCustAR.ExceptJoin = false;
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
            fieldAttr3.DataField = "PERIOD";
            fieldAttr3.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr3.DefaultValue = null;
            fieldAttr3.TrimLength = 0;
            fieldAttr3.UpdateEnable = true;
            fieldAttr3.WhereMode = true;
            fieldAttr4.CharSetNull = false;
            fieldAttr4.CheckNull = false;
            fieldAttr4.DataField = "ARTYPE";
            fieldAttr4.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr4.DefaultValue = null;
            fieldAttr4.TrimLength = 0;
            fieldAttr4.UpdateEnable = true;
            fieldAttr4.WhereMode = true;
            fieldAttr5.CharSetNull = false;
            fieldAttr5.CheckNull = false;
            fieldAttr5.DataField = "AMT";
            fieldAttr5.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr5.DefaultValue = null;
            fieldAttr5.TrimLength = 0;
            fieldAttr5.UpdateEnable = true;
            fieldAttr5.WhereMode = true;
            fieldAttr6.CharSetNull = false;
            fieldAttr6.CheckNull = false;
            fieldAttr6.DataField = "COD1";
            fieldAttr6.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr6.DefaultValue = null;
            fieldAttr6.TrimLength = 0;
            fieldAttr6.UpdateEnable = true;
            fieldAttr6.WhereMode = true;
            fieldAttr7.CharSetNull = false;
            fieldAttr7.CheckNull = false;
            fieldAttr7.DataField = "COD2";
            fieldAttr7.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr7.DefaultValue = null;
            fieldAttr7.TrimLength = 0;
            fieldAttr7.UpdateEnable = true;
            fieldAttr7.WhereMode = true;
            fieldAttr8.CharSetNull = false;
            fieldAttr8.CheckNull = false;
            fieldAttr8.DataField = "COD3";
            fieldAttr8.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr8.DefaultValue = null;
            fieldAttr8.TrimLength = 0;
            fieldAttr8.UpdateEnable = true;
            fieldAttr8.WhereMode = true;
            fieldAttr9.CharSetNull = false;
            fieldAttr9.CheckNull = false;
            fieldAttr9.DataField = "COD4";
            fieldAttr9.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr9.DefaultValue = null;
            fieldAttr9.TrimLength = 0;
            fieldAttr9.UpdateEnable = true;
            fieldAttr9.WhereMode = true;
            fieldAttr10.CharSetNull = false;
            fieldAttr10.CheckNull = false;
            fieldAttr10.DataField = "COD5";
            fieldAttr10.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr10.DefaultValue = null;
            fieldAttr10.TrimLength = 0;
            fieldAttr10.UpdateEnable = true;
            fieldAttr10.WhereMode = true;
            fieldAttr11.CharSetNull = false;
            fieldAttr11.CheckNull = false;
            fieldAttr11.DataField = "CDAT";
            fieldAttr11.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr11.DefaultValue = null;
            fieldAttr11.TrimLength = 0;
            fieldAttr11.UpdateEnable = true;
            fieldAttr11.WhereMode = true;
            fieldAttr12.CharSetNull = false;
            fieldAttr12.CheckNull = false;
            fieldAttr12.DataField = "MDAT";
            fieldAttr12.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr12.DefaultValue = null;
            fieldAttr12.TrimLength = 0;
            fieldAttr12.UpdateEnable = true;
            fieldAttr12.WhereMode = true;
            fieldAttr13.CharSetNull = false;
            fieldAttr13.CheckNull = false;
            fieldAttr13.DataField = "MUSR";
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
            fieldAttr15.DataField = "CANCELUSR";
            fieldAttr15.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr15.DefaultValue = null;
            fieldAttr15.TrimLength = 0;
            fieldAttr15.UpdateEnable = true;
            fieldAttr15.WhereMode = true;
            fieldAttr16.CharSetNull = false;
            fieldAttr16.CheckNull = false;
            fieldAttr16.DataField = "CANCELMEMO";
            fieldAttr16.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr16.DefaultValue = null;
            fieldAttr16.TrimLength = 0;
            fieldAttr16.UpdateEnable = true;
            fieldAttr16.WhereMode = true;
            fieldAttr17.CharSetNull = false;
            fieldAttr17.CheckNull = false;
            fieldAttr17.DataField = "REALAMT";
            fieldAttr17.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr17.DefaultValue = null;
            fieldAttr17.TrimLength = 0;
            fieldAttr17.UpdateEnable = true;
            fieldAttr17.WhereMode = true;
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr1);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr2);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr3);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr4);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr5);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr6);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr7);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr8);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr9);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr10);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr11);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr12);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr13);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr14);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr15);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr16);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr17);
            this.ucRTLessorAVSCustAR.LogInfo = null;
            this.ucRTLessorAVSCustAR.Name = "ucRTLessorAVSCustAR";
            this.ucRTLessorAVSCustAR.RowAffectsCheck = true;
            this.ucRTLessorAVSCustAR.SelectCmd = this.RTLessorAVSCustAR;
            this.ucRTLessorAVSCustAR.SelectCmdForUpdate = null;
            this.ucRTLessorAVSCustAR.SendSQLCmd = true;
            this.ucRTLessorAVSCustAR.ServerModify = true;
            this.ucRTLessorAVSCustAR.ServerModifyGetMax = false;
            this.ucRTLessorAVSCustAR.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTLessorAVSCustAR.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTLessorAVSCustAR.UseTranscationScope = false;
            this.ucRTLessorAVSCustAR.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTLessorAVSCustAR
            // 
            this.View_RTLessorAVSCustAR.CacheConnection = false;
            this.View_RTLessorAVSCustAR.CommandText = "SELECT * FROM dbo.[RTLessorAVSCustAR]";
            this.View_RTLessorAVSCustAR.CommandTimeout = 30;
            this.View_RTLessorAVSCustAR.CommandType = System.Data.CommandType.Text;
            this.View_RTLessorAVSCustAR.DynamicTableName = false;
            this.View_RTLessorAVSCustAR.EEPAlias = null;
            this.View_RTLessorAVSCustAR.EncodingAfter = null;
            this.View_RTLessorAVSCustAR.EncodingBefore = "Windows-1252";
            this.View_RTLessorAVSCustAR.EncodingConvert = null;
            this.View_RTLessorAVSCustAR.InfoConnection = this.InfoConnection1;
            keyItem3.KeyName = "CUSID";
            keyItem4.KeyName = "BATCHNO";
            this.View_RTLessorAVSCustAR.KeyFields.Add(keyItem3);
            this.View_RTLessorAVSCustAR.KeyFields.Add(keyItem4);
            this.View_RTLessorAVSCustAR.MultiSetWhere = false;
            this.View_RTLessorAVSCustAR.Name = "View_RTLessorAVSCustAR";
            this.View_RTLessorAVSCustAR.NotificationAutoEnlist = false;
            this.View_RTLessorAVSCustAR.SecExcept = null;
            this.View_RTLessorAVSCustAR.SecFieldName = null;
            this.View_RTLessorAVSCustAR.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTLessorAVSCustAR.SelectPaging = false;
            this.View_RTLessorAVSCustAR.SelectTop = 0;
            this.View_RTLessorAVSCustAR.SiteControl = false;
            this.View_RTLessorAVSCustAR.SiteFieldName = null;
            this.View_RTLessorAVSCustAR.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmRT204
            // 
            this.cmRT204.CacheConnection = false;
            this.cmRT204.CommandText = "SELECT * FROM V_RT204 A\r\nORDER BY  A.CDAT desc";
            this.cmRT204.CommandTimeout = 30;
            this.cmRT204.CommandType = System.Data.CommandType.Text;
            this.cmRT204.DynamicTableName = false;
            this.cmRT204.EEPAlias = null;
            this.cmRT204.EncodingAfter = null;
            this.cmRT204.EncodingBefore = "Windows-1252";
            this.cmRT204.EncodingConvert = null;
            this.cmRT204.InfoConnection = this.InfoConnection1;
            keyItem5.KeyName = "CUSID";
            keyItem6.KeyName = "BATCHNO";
            this.cmRT204.KeyFields.Add(keyItem5);
            this.cmRT204.KeyFields.Add(keyItem6);
            this.cmRT204.MultiSetWhere = false;
            this.cmRT204.Name = "cmRT204";
            this.cmRT204.NotificationAutoEnlist = false;
            this.cmRT204.SecExcept = null;
            this.cmRT204.SecFieldName = null;
            this.cmRT204.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmRT204.SelectPaging = false;
            this.cmRT204.SelectTop = 0;
            this.cmRT204.SiteControl = false;
            this.cmRT204.SiteFieldName = null;
            this.cmRT204.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // RT2044
            // 
            this.RT2044.CacheConnection = false;
            this.RT2044.CommandText = resources.GetString("RT2044.CommandText");
            this.RT2044.CommandTimeout = 30;
            this.RT2044.CommandType = System.Data.CommandType.Text;
            this.RT2044.DynamicTableName = false;
            this.RT2044.EEPAlias = null;
            this.RT2044.EncodingAfter = null;
            this.RT2044.EncodingBefore = "Windows-1252";
            this.RT2044.EncodingConvert = null;
            this.RT2044.InfoConnection = this.InfoConnection1;
            this.RT2044.MultiSetWhere = false;
            this.RT2044.Name = "RT2044";
            this.RT2044.NotificationAutoEnlist = false;
            this.RT2044.SecExcept = null;
            this.RT2044.SecFieldName = null;
            this.RT2044.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RT2044.SelectPaging = false;
            this.RT2044.SelectTop = 0;
            this.RT2044.SiteControl = false;
            this.RT2044.SiteFieldName = null;
            this.RT2044.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustAR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustAR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT204)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT2044)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTLessorAVSCustAR;
        private Srvtools.UpdateComponent ucRTLessorAVSCustAR;
        private Srvtools.InfoCommand View_RTLessorAVSCustAR;
        private Srvtools.InfoCommand cmRT204;
        private Srvtools.InfoCommand RT2044;
    }
}
