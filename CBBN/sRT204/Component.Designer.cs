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
            Srvtools.Service service2 = new Srvtools.Service();
            Srvtools.KeyItem keyItem7 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem8 = new Srvtools.KeyItem();
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
            Srvtools.FieldAttr fieldAttr29 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr30 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr31 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr32 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr33 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr34 = new Srvtools.FieldAttr();
            Srvtools.KeyItem keyItem1 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem2 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem3 = new Srvtools.KeyItem();
            Srvtools.KeyItem keyItem4 = new Srvtools.KeyItem();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Component));
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTLessorAVSCustAR = new Srvtools.InfoCommand(this.components);
            this.ucRTLessorAVSCustAR = new Srvtools.UpdateComponent(this.components);
            this.View_RTLessorAVSCustAR = new Srvtools.InfoCommand(this.components);
            this.cmRT204 = new Srvtools.InfoCommand(this.components);
            this.RT2044 = new Srvtools.InfoCommand(this.components);
            this.cmd = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustAR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustAR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT204)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT2044)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmd)).BeginInit();
            // 
            // serviceManager1
            // 
            service2.DelegateName = "smRT2045";
            service2.NonLogin = false;
            service2.ServiceName = "smRT2045";
            this.serviceManager1.ServiceCollection.Add(service2);
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
            keyItem7.KeyName = "CUSID";
            keyItem8.KeyName = "BATCHNO";
            this.RTLessorAVSCustAR.KeyFields.Add(keyItem7);
            this.RTLessorAVSCustAR.KeyFields.Add(keyItem8);
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
            fieldAttr19.DataField = "BATCHNO";
            fieldAttr19.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr19.DefaultValue = null;
            fieldAttr19.TrimLength = 0;
            fieldAttr19.UpdateEnable = true;
            fieldAttr19.WhereMode = true;
            fieldAttr20.CharSetNull = false;
            fieldAttr20.CheckNull = false;
            fieldAttr20.DataField = "PERIOD";
            fieldAttr20.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr20.DefaultValue = null;
            fieldAttr20.TrimLength = 0;
            fieldAttr20.UpdateEnable = true;
            fieldAttr20.WhereMode = true;
            fieldAttr21.CharSetNull = false;
            fieldAttr21.CheckNull = false;
            fieldAttr21.DataField = "ARTYPE";
            fieldAttr21.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr21.DefaultValue = null;
            fieldAttr21.TrimLength = 0;
            fieldAttr21.UpdateEnable = true;
            fieldAttr21.WhereMode = true;
            fieldAttr22.CharSetNull = false;
            fieldAttr22.CheckNull = false;
            fieldAttr22.DataField = "AMT";
            fieldAttr22.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr22.DefaultValue = null;
            fieldAttr22.TrimLength = 0;
            fieldAttr22.UpdateEnable = true;
            fieldAttr22.WhereMode = true;
            fieldAttr23.CharSetNull = false;
            fieldAttr23.CheckNull = false;
            fieldAttr23.DataField = "COD1";
            fieldAttr23.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr23.DefaultValue = null;
            fieldAttr23.TrimLength = 0;
            fieldAttr23.UpdateEnable = true;
            fieldAttr23.WhereMode = true;
            fieldAttr24.CharSetNull = false;
            fieldAttr24.CheckNull = false;
            fieldAttr24.DataField = "COD2";
            fieldAttr24.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr24.DefaultValue = null;
            fieldAttr24.TrimLength = 0;
            fieldAttr24.UpdateEnable = true;
            fieldAttr24.WhereMode = true;
            fieldAttr25.CharSetNull = false;
            fieldAttr25.CheckNull = false;
            fieldAttr25.DataField = "COD3";
            fieldAttr25.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr25.DefaultValue = null;
            fieldAttr25.TrimLength = 0;
            fieldAttr25.UpdateEnable = true;
            fieldAttr25.WhereMode = true;
            fieldAttr26.CharSetNull = false;
            fieldAttr26.CheckNull = false;
            fieldAttr26.DataField = "COD4";
            fieldAttr26.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr26.DefaultValue = null;
            fieldAttr26.TrimLength = 0;
            fieldAttr26.UpdateEnable = true;
            fieldAttr26.WhereMode = true;
            fieldAttr27.CharSetNull = false;
            fieldAttr27.CheckNull = false;
            fieldAttr27.DataField = "COD5";
            fieldAttr27.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr27.DefaultValue = null;
            fieldAttr27.TrimLength = 0;
            fieldAttr27.UpdateEnable = true;
            fieldAttr27.WhereMode = true;
            fieldAttr28.CharSetNull = false;
            fieldAttr28.CheckNull = false;
            fieldAttr28.DataField = "CDAT";
            fieldAttr28.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr28.DefaultValue = null;
            fieldAttr28.TrimLength = 0;
            fieldAttr28.UpdateEnable = true;
            fieldAttr28.WhereMode = true;
            fieldAttr29.CharSetNull = false;
            fieldAttr29.CheckNull = false;
            fieldAttr29.DataField = "MDAT";
            fieldAttr29.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr29.DefaultValue = null;
            fieldAttr29.TrimLength = 0;
            fieldAttr29.UpdateEnable = true;
            fieldAttr29.WhereMode = true;
            fieldAttr30.CharSetNull = false;
            fieldAttr30.CheckNull = false;
            fieldAttr30.DataField = "MUSR";
            fieldAttr30.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr30.DefaultValue = null;
            fieldAttr30.TrimLength = 0;
            fieldAttr30.UpdateEnable = true;
            fieldAttr30.WhereMode = true;
            fieldAttr31.CharSetNull = false;
            fieldAttr31.CheckNull = false;
            fieldAttr31.DataField = "CANCELDAT";
            fieldAttr31.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr31.DefaultValue = null;
            fieldAttr31.TrimLength = 0;
            fieldAttr31.UpdateEnable = true;
            fieldAttr31.WhereMode = true;
            fieldAttr32.CharSetNull = false;
            fieldAttr32.CheckNull = false;
            fieldAttr32.DataField = "CANCELUSR";
            fieldAttr32.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr32.DefaultValue = null;
            fieldAttr32.TrimLength = 0;
            fieldAttr32.UpdateEnable = true;
            fieldAttr32.WhereMode = true;
            fieldAttr33.CharSetNull = false;
            fieldAttr33.CheckNull = false;
            fieldAttr33.DataField = "CANCELMEMO";
            fieldAttr33.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr33.DefaultValue = null;
            fieldAttr33.TrimLength = 0;
            fieldAttr33.UpdateEnable = true;
            fieldAttr33.WhereMode = true;
            fieldAttr34.CharSetNull = false;
            fieldAttr34.CheckNull = false;
            fieldAttr34.DataField = "REALAMT";
            fieldAttr34.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr34.DefaultValue = null;
            fieldAttr34.TrimLength = 0;
            fieldAttr34.UpdateEnable = true;
            fieldAttr34.WhereMode = true;
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr18);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr19);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr20);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr21);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr22);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr23);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr24);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr25);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr26);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr27);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr28);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr29);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr30);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr31);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr32);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr33);
            this.ucRTLessorAVSCustAR.FieldAttrs.Add(fieldAttr34);
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
            keyItem1.KeyName = "CUSID";
            keyItem2.KeyName = "BATCHNO";
            this.View_RTLessorAVSCustAR.KeyFields.Add(keyItem1);
            this.View_RTLessorAVSCustAR.KeyFields.Add(keyItem2);
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
            keyItem3.KeyName = "CUSID";
            keyItem4.KeyName = "BATCHNO";
            this.cmRT204.KeyFields.Add(keyItem3);
            this.cmRT204.KeyFields.Add(keyItem4);
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
            ((System.ComponentModel.ISupportInitialize)(this.RTLessorAVSCustAR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTLessorAVSCustAR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmRT204)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RT2044)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmd)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTLessorAVSCustAR;
        private Srvtools.UpdateComponent ucRTLessorAVSCustAR;
        private Srvtools.InfoCommand View_RTLessorAVSCustAR;
        private Srvtools.InfoCommand cmRT204;
        private Srvtools.InfoCommand RT2044;
        private Srvtools.InfoCommand cmd;
    }
}
