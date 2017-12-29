namespace sRT401
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
            Srvtools.FieldAttr fieldAttr35 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr36 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr37 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr38 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr39 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr40 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr41 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr42 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr43 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr44 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr45 = new Srvtools.FieldAttr();
            Srvtools.FieldAttr fieldAttr46 = new Srvtools.FieldAttr();
            Srvtools.InfoParameter infoParameter2 = new Srvtools.InfoParameter();
            this.serviceManager1 = new Srvtools.ServiceManager(this.components);
            this.InfoConnection1 = new Srvtools.InfoConnection(this.components);
            this.RTInvTemp = new Srvtools.InfoCommand(this.components);
            this.ucRTInvTemp = new Srvtools.UpdateComponent(this.components);
            this.View_RTInvTemp = new Srvtools.InfoCommand(this.components);
            this.cmdRT401 = new Srvtools.InfoCommand(this.components);
            this.cmd = new Srvtools.InfoCommand(this.components);
            this.cmdRT4011 = new Srvtools.InfoCommand(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTInvTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTInvTemp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT401)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT4011)).BeginInit();
            // 
            // serviceManager1
            // 
            service2.DelegateName = "smRT401";
            service2.NonLogin = false;
            service2.ServiceName = "smRT401";
            this.serviceManager1.ServiceCollection.Add(service2);
            // 
            // InfoConnection1
            // 
            this.InfoConnection1.EEPAlias = "RTLib";
            // 
            // RTInvTemp
            // 
            this.RTInvTemp.CacheConnection = false;
            this.RTInvTemp.CommandText = "SELECT dbo.[RTInvTemp].* FROM dbo.[RTInvTemp]";
            this.RTInvTemp.CommandTimeout = 30;
            this.RTInvTemp.CommandType = System.Data.CommandType.Text;
            this.RTInvTemp.DynamicTableName = false;
            this.RTInvTemp.EEPAlias = null;
            this.RTInvTemp.EncodingAfter = null;
            this.RTInvTemp.EncodingBefore = "Windows-1252";
            this.RTInvTemp.EncodingConvert = null;
            this.RTInvTemp.InfoConnection = this.InfoConnection1;
            this.RTInvTemp.MultiSetWhere = false;
            this.RTInvTemp.Name = "RTInvTemp";
            this.RTInvTemp.NotificationAutoEnlist = false;
            this.RTInvTemp.SecExcept = null;
            this.RTInvTemp.SecFieldName = null;
            this.RTInvTemp.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.RTInvTemp.SelectPaging = false;
            this.RTInvTemp.SelectTop = 0;
            this.RTInvTemp.SiteControl = false;
            this.RTInvTemp.SiteFieldName = null;
            this.RTInvTemp.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // ucRTInvTemp
            // 
            this.ucRTInvTemp.AutoTrans = true;
            this.ucRTInvTemp.ExceptJoin = false;
            fieldAttr24.CharSetNull = false;
            fieldAttr24.CheckNull = false;
            fieldAttr24.DataField = "GROUPNC";
            fieldAttr24.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr24.DefaultValue = null;
            fieldAttr24.TrimLength = 0;
            fieldAttr24.UpdateEnable = true;
            fieldAttr24.WhereMode = true;
            fieldAttr25.CharSetNull = false;
            fieldAttr25.CheckNull = false;
            fieldAttr25.DataField = "PRODNC";
            fieldAttr25.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr25.DefaultValue = null;
            fieldAttr25.TrimLength = 0;
            fieldAttr25.UpdateEnable = true;
            fieldAttr25.WhereMode = true;
            fieldAttr26.CharSetNull = false;
            fieldAttr26.CheckNull = false;
            fieldAttr26.DataField = "QTY";
            fieldAttr26.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr26.DefaultValue = null;
            fieldAttr26.TrimLength = 0;
            fieldAttr26.UpdateEnable = true;
            fieldAttr26.WhereMode = true;
            fieldAttr27.CharSetNull = false;
            fieldAttr27.CheckNull = false;
            fieldAttr27.DataField = "UNITAMT";
            fieldAttr27.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr27.DefaultValue = null;
            fieldAttr27.TrimLength = 0;
            fieldAttr27.UpdateEnable = true;
            fieldAttr27.WhereMode = true;
            fieldAttr28.CharSetNull = false;
            fieldAttr28.CheckNull = false;
            fieldAttr28.DataField = "RCVAMT";
            fieldAttr28.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr28.DefaultValue = null;
            fieldAttr28.TrimLength = 0;
            fieldAttr28.UpdateEnable = true;
            fieldAttr28.WhereMode = true;
            fieldAttr29.CharSetNull = false;
            fieldAttr29.CheckNull = false;
            fieldAttr29.DataField = "TAXTYPE";
            fieldAttr29.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr29.DefaultValue = null;
            fieldAttr29.TrimLength = 0;
            fieldAttr29.UpdateEnable = true;
            fieldAttr29.WhereMode = true;
            fieldAttr30.CharSetNull = false;
            fieldAttr30.CheckNull = false;
            fieldAttr30.DataField = "SALEAMT";
            fieldAttr30.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr30.DefaultValue = null;
            fieldAttr30.TrimLength = 0;
            fieldAttr30.UpdateEnable = true;
            fieldAttr30.WhereMode = true;
            fieldAttr31.CharSetNull = false;
            fieldAttr31.CheckNull = false;
            fieldAttr31.DataField = "TAXAMT";
            fieldAttr31.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr31.DefaultValue = null;
            fieldAttr31.TrimLength = 0;
            fieldAttr31.UpdateEnable = true;
            fieldAttr31.WhereMode = true;
            fieldAttr32.CharSetNull = false;
            fieldAttr32.CheckNull = false;
            fieldAttr32.DataField = "INVDAT";
            fieldAttr32.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr32.DefaultValue = null;
            fieldAttr32.TrimLength = 0;
            fieldAttr32.UpdateEnable = true;
            fieldAttr32.WhereMode = true;
            fieldAttr33.CharSetNull = false;
            fieldAttr33.CheckNull = false;
            fieldAttr33.DataField = "INVNO";
            fieldAttr33.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr33.DefaultValue = null;
            fieldAttr33.TrimLength = 0;
            fieldAttr33.UpdateEnable = true;
            fieldAttr33.WhereMode = true;
            fieldAttr34.CharSetNull = false;
            fieldAttr34.CheckNull = false;
            fieldAttr34.DataField = "UNINO";
            fieldAttr34.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr34.DefaultValue = null;
            fieldAttr34.TrimLength = 0;
            fieldAttr34.UpdateEnable = true;
            fieldAttr34.WhereMode = true;
            fieldAttr35.CharSetNull = false;
            fieldAttr35.CheckNull = false;
            fieldAttr35.DataField = "INVTITLE";
            fieldAttr35.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr35.DefaultValue = null;
            fieldAttr35.TrimLength = 0;
            fieldAttr35.UpdateEnable = true;
            fieldAttr35.WhereMode = true;
            fieldAttr36.CharSetNull = false;
            fieldAttr36.CheckNull = false;
            fieldAttr36.DataField = "社區名稱";
            fieldAttr36.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr36.DefaultValue = null;
            fieldAttr36.TrimLength = 0;
            fieldAttr36.UpdateEnable = true;
            fieldAttr36.WhereMode = true;
            fieldAttr37.CharSetNull = false;
            fieldAttr37.CheckNull = false;
            fieldAttr37.DataField = "用戶名稱";
            fieldAttr37.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr37.DefaultValue = null;
            fieldAttr37.TrimLength = 0;
            fieldAttr37.UpdateEnable = true;
            fieldAttr37.WhereMode = true;
            fieldAttr38.CharSetNull = false;
            fieldAttr38.CheckNull = false;
            fieldAttr38.DataField = "地址";
            fieldAttr38.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr38.DefaultValue = null;
            fieldAttr38.TrimLength = 0;
            fieldAttr38.UpdateEnable = true;
            fieldAttr38.WhereMode = true;
            fieldAttr39.CharSetNull = false;
            fieldAttr39.CheckNull = false;
            fieldAttr39.DataField = "聯絡電話";
            fieldAttr39.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr39.DefaultValue = null;
            fieldAttr39.TrimLength = 0;
            fieldAttr39.UpdateEnable = true;
            fieldAttr39.WhereMode = true;
            fieldAttr40.CharSetNull = false;
            fieldAttr40.CheckNull = false;
            fieldAttr40.DataField = "業務施工收款日";
            fieldAttr40.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr40.DefaultValue = null;
            fieldAttr40.TrimLength = 0;
            fieldAttr40.UpdateEnable = true;
            fieldAttr40.WhereMode = true;
            fieldAttr41.CharSetNull = false;
            fieldAttr41.CheckNull = false;
            fieldAttr41.DataField = "施工人員";
            fieldAttr41.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr41.DefaultValue = null;
            fieldAttr41.TrimLength = 0;
            fieldAttr41.UpdateEnable = true;
            fieldAttr41.WhereMode = true;
            fieldAttr42.CharSetNull = false;
            fieldAttr42.CheckNull = false;
            fieldAttr42.DataField = "業務開發單位";
            fieldAttr42.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr42.DefaultValue = null;
            fieldAttr42.TrimLength = 0;
            fieldAttr42.UpdateEnable = true;
            fieldAttr42.WhereMode = true;
            fieldAttr43.CharSetNull = false;
            fieldAttr43.CheckNull = false;
            fieldAttr43.DataField = "業務開發人員";
            fieldAttr43.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr43.DefaultValue = null;
            fieldAttr43.TrimLength = 0;
            fieldAttr43.UpdateEnable = true;
            fieldAttr43.WhereMode = true;
            fieldAttr44.CharSetNull = false;
            fieldAttr44.CheckNull = false;
            fieldAttr44.DataField = "備註";
            fieldAttr44.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr44.DefaultValue = null;
            fieldAttr44.TrimLength = 0;
            fieldAttr44.UpdateEnable = true;
            fieldAttr44.WhereMode = true;
            fieldAttr45.CharSetNull = false;
            fieldAttr45.CheckNull = false;
            fieldAttr45.DataField = "BATCH";
            fieldAttr45.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr45.DefaultValue = null;
            fieldAttr45.TrimLength = 0;
            fieldAttr45.UpdateEnable = true;
            fieldAttr45.WhereMode = true;
            fieldAttr46.CharSetNull = false;
            fieldAttr46.CheckNull = false;
            fieldAttr46.DataField = "INVTYPE";
            fieldAttr46.DefaultMode = Srvtools.DefaultModeType.Insert;
            fieldAttr46.DefaultValue = null;
            fieldAttr46.TrimLength = 0;
            fieldAttr46.UpdateEnable = true;
            fieldAttr46.WhereMode = true;
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr24);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr25);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr26);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr27);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr28);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr29);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr30);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr31);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr32);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr33);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr34);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr35);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr36);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr37);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr38);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr39);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr40);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr41);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr42);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr43);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr44);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr45);
            this.ucRTInvTemp.FieldAttrs.Add(fieldAttr46);
            this.ucRTInvTemp.LogInfo = null;
            this.ucRTInvTemp.Name = "ucRTInvTemp";
            this.ucRTInvTemp.RowAffectsCheck = true;
            this.ucRTInvTemp.SelectCmd = this.RTInvTemp;
            this.ucRTInvTemp.SelectCmdForUpdate = null;
            this.ucRTInvTemp.SendSQLCmd = true;
            this.ucRTInvTemp.ServerModify = true;
            this.ucRTInvTemp.ServerModifyGetMax = false;
            this.ucRTInvTemp.TranscationScopeTimeOut = System.TimeSpan.Parse("00:02:00");
            this.ucRTInvTemp.TransIsolationLevel = System.Data.IsolationLevel.ReadCommitted;
            this.ucRTInvTemp.UseTranscationScope = false;
            this.ucRTInvTemp.WhereMode = Srvtools.WhereModeType.Keyfields;
            // 
            // View_RTInvTemp
            // 
            this.View_RTInvTemp.CacheConnection = false;
            this.View_RTInvTemp.CommandText = "SELECT * FROM dbo.[RTInvTemp]";
            this.View_RTInvTemp.CommandTimeout = 30;
            this.View_RTInvTemp.CommandType = System.Data.CommandType.Text;
            this.View_RTInvTemp.DynamicTableName = false;
            this.View_RTInvTemp.EEPAlias = null;
            this.View_RTInvTemp.EncodingAfter = null;
            this.View_RTInvTemp.EncodingBefore = "Windows-1252";
            this.View_RTInvTemp.EncodingConvert = null;
            this.View_RTInvTemp.InfoConnection = this.InfoConnection1;
            this.View_RTInvTemp.MultiSetWhere = false;
            this.View_RTInvTemp.Name = "View_RTInvTemp";
            this.View_RTInvTemp.NotificationAutoEnlist = false;
            this.View_RTInvTemp.SecExcept = null;
            this.View_RTInvTemp.SecFieldName = null;
            this.View_RTInvTemp.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.View_RTInvTemp.SelectPaging = false;
            this.View_RTInvTemp.SelectTop = 0;
            this.View_RTInvTemp.SiteControl = false;
            this.View_RTInvTemp.SiteFieldName = null;
            this.View_RTInvTemp.UpdatedRowSource = System.Data.UpdateRowSource.None;
            // 
            // cmdRT401
            // 
            this.cmdRT401.CacheConnection = false;
            this.cmdRT401.CommandText = null;
            this.cmdRT401.CommandTimeout = 30;
            this.cmdRT401.CommandType = System.Data.CommandType.Text;
            this.cmdRT401.DynamicTableName = false;
            this.cmdRT401.EEPAlias = null;
            this.cmdRT401.EncodingAfter = null;
            this.cmdRT401.EncodingBefore = "Windows-1252";
            this.cmdRT401.EncodingConvert = null;
            this.cmdRT401.InfoConnection = this.InfoConnection1;
            this.cmdRT401.MultiSetWhere = false;
            this.cmdRT401.Name = "cmdRT401";
            this.cmdRT401.NotificationAutoEnlist = false;
            this.cmdRT401.SecExcept = null;
            this.cmdRT401.SecFieldName = null;
            this.cmdRT401.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT401.SelectPaging = false;
            this.cmdRT401.SelectTop = 0;
            this.cmdRT401.SiteControl = false;
            this.cmdRT401.SiteFieldName = null;
            this.cmdRT401.UpdatedRowSource = System.Data.UpdateRowSource.None;
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
            // 
            // cmdRT4011
            // 
            this.cmdRT4011.CacheConnection = false;
            this.cmdRT4011.CommandText = "usp_RTInvoiceImport";
            this.cmdRT4011.CommandTimeout = 30;
            this.cmdRT4011.CommandType = System.Data.CommandType.StoredProcedure;
            this.cmdRT4011.DynamicTableName = false;
            this.cmdRT4011.EEPAlias = null;
            this.cmdRT4011.EncodingAfter = null;
            this.cmdRT4011.EncodingBefore = "Windows-1252";
            this.cmdRT4011.EncodingConvert = null;
            this.cmdRT4011.InfoConnection = this.InfoConnection1;
            infoParameter2.InfoDbType = Srvtools.InfoDbType.VarChar;
            infoParameter2.ParameterName = "uusr";
            infoParameter2.Precision = ((byte)(0));
            infoParameter2.Scale = ((byte)(0));
            infoParameter2.Size = 6;
            infoParameter2.SourceColumn = null;
            infoParameter2.XmlSchemaCollectionDatabase = null;
            infoParameter2.XmlSchemaCollectionName = null;
            infoParameter2.XmlSchemaCollectionOwningSchema = null;
            this.cmdRT4011.InfoParameters.Add(infoParameter2);
            this.cmdRT4011.MultiSetWhere = false;
            this.cmdRT4011.Name = "cmdRT4011";
            this.cmdRT4011.NotificationAutoEnlist = false;
            this.cmdRT4011.SecExcept = null;
            this.cmdRT4011.SecFieldName = null;
            this.cmdRT4011.SecStyle = Srvtools.SecurityStyle.ssByNone;
            this.cmdRT4011.SelectPaging = false;
            this.cmdRT4011.SelectTop = 0;
            this.cmdRT4011.SiteControl = false;
            this.cmdRT4011.SiteFieldName = null;
            this.cmdRT4011.UpdatedRowSource = System.Data.UpdateRowSource.None;
            ((System.ComponentModel.ISupportInitialize)(this.InfoConnection1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RTInvTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_RTInvTemp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT401)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmdRT4011)).EndInit();

        }

        #endregion

        private Srvtools.ServiceManager serviceManager1;
        private Srvtools.InfoConnection InfoConnection1;
        private Srvtools.InfoCommand RTInvTemp;
        private Srvtools.UpdateComponent ucRTInvTemp;
        private Srvtools.InfoCommand View_RTInvTemp;
        private Srvtools.InfoCommand cmdRT401;
        private Srvtools.InfoCommand cmd;
        private Srvtools.InfoCommand cmdRT4011;
    }
}
